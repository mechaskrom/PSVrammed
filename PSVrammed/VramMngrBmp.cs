using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using MyCustomStuff;

namespace PSVrammed
{
    //Manages vram (from state file). Bitmap vram part.
    partial class VramMngr
    {
        private class VramBmp
        {
            private readonly VramMngr mVramMngr;
            private readonly BitDepth mBitDepth;
            private readonly int[] mPalette; //32 bit pargb values. 0 if not an indexed format.
            private readonly bool mShowAlpha;
            private readonly BitmapPinned32bppPArgb mBmpDraw; //Bitmap of vram content.
            private bool mUpdatePalette;
            private Rectangle mRcDirtyDraw; //Draw pixels that needs to be updated.

            //Bitmap Format32bppPArgb is the fastest when drawing (on my system at least).
            //I've tried many ways to convert vram bytes into Format32bppPArgb pixels:
            //threads, Graphics.DrawImage(), a system with dirty tiles (min 16*16 big) to
            //update and more, but just converting vram bytes directly into Format32bppPArgb
            //pixels is almost always faster and simpler. The only case where something else
            //was obviously faster was when converting a lot (like almost full vram) of 8 bit
            //indexed bytes.

            //Use an extra bitmap in Format8bppIndexed and just Array.Copy() the vram bytes
            //to it and then set the bitmap palette and then use Graphics.DrawImage() to
            //draw/convert it to the Format32bppPArgb bitmap. In extreme cases (full vram update)
            //this was about 2-3 times faster than converting vram bytes directly into
            //Format32bppPArgb. Not sure if worth the extra code though and I don't know if
            //you can rely on draw Format8bppIndexed being this fast on all systems.

            public VramBmp(VramMngr vramMngr, BitDepth bitDepth, bool showAlpha)
            {
                mVramMngr = vramMngr;
                mBitDepth = bitDepth;
                mPalette = new int[toPaletteEntries(bitDepth)];
                mShowAlpha = showAlpha;
                mBmpDraw = new BitmapPinned32bppPArgb(toPixelWidth(bitDepth), VramHeight);
                mUpdatePalette = IsIndexedFormat;
                mRcDirtyDraw = mBmpDraw.Bounds;
            }

            private bool IsIndexedFormat
            {
                get { return mPalette.Length > 0; }
            }

            private static int toPaletteEntries(BitDepth bitDepth)
            {
                if (bitDepth == BitDepth.Ind4Bit) return PaletteWidth4Bit;
                if (bitDepth == BitDepth.Ind8Bit) return PaletteWidth8Bit;
                return 0;
            }

            public void onVramChanged(Rectangle rcBytes)
            {
                Rectangle rcPixels = rcByteToPixel(rcBytes, mBitDepth);
                mRcDirtyDraw = rcPixels.UnionWithNonEmpty(mRcDirtyDraw);
            }

            public void onVramChanged(Rectangle rcBytes, bool isPaletteAffected)
            {
                onVramChanged(rcBytes);
                mUpdatePalette = mUpdatePalette || (IsIndexedFormat && isPaletteAffected);
            }

            public void onPaletteLocationChanged()
            {
                mUpdatePalette = IsIndexedFormat;
            }

            public BitmapPinned32bppPArgb getBmpDraw(Rectangle rcClip)
            {
                if (mUpdatePalette && IsIndexedFormat) //Need to update palette?
                {
                    mVramMngr.updatePalette(mPalette, mShowAlpha);
                    mRcDirtyDraw = mBmpDraw.Bounds;
                    mUpdatePalette = false;
                }

                Rectangle rcUpdatePixels = getUpdatePixelsRc(rcClip, ref mRcDirtyDraw);
                if (!rcUpdatePixels.IsEmpty) //Need to update some part of bmp draw?
                {
                    updateBmpDraw32BitPargb(rcUpdatePixels);
                }

                return mBmpDraw;
            }

            private static Rectangle getUpdatePixelsRc(Rectangle clip, ref Rectangle dirty)
            {
                //Use dirty and clip rectangle to figure out smallest area that needs to be updated.
                //This will also update the dirty rectangle by removing the update rectangle part
                //from it.

                Rectangle rc = Rectangle.Intersect(dirty, clip);
                if (rc.IsEmpty) return rc; //Dirty and clip don't overlap i.e. no update needed.

                //We can't just return the intersect of dirty and clip because if e.g. the
                //dirty rectangle contains the clip rectangle it will not shrink and the
                //same area will update again the next time. We need to make sure that the
                //update cover a whole edge/side of the dirty rectangle to avoid updates
                //that will not shrink the dirty rectangle.

                //Check which edge/side of the dirty rectangle is smallest (fastest to update).
                //Start with a edge and then check for a smaller.

                //Left edge.
                int rcMinArea = dirty.Height * (rc.Right - dirty.Left);
                Rectangle rcMin = Rectangle.FromLTRB(dirty.Left, dirty.Top, rc.Right, dirty.Bottom);
                Rectangle rcDrt = Rectangle.FromLTRB(rc.Right, dirty.Top, dirty.Right, dirty.Bottom);

                //Top edge.
                int rcArea = dirty.Width * (rc.Bottom - dirty.Top);
                if (rcArea < rcMinArea)
                {
                    rcMinArea = rcArea;
                    rcMin = Rectangle.FromLTRB(dirty.Left, dirty.Top, dirty.Right, rc.Bottom);
                    rcDrt = Rectangle.FromLTRB(dirty.Left, rc.Bottom, dirty.Right, dirty.Bottom);
                }

                //Right edge.
                rcArea = dirty.Height * (dirty.Right - rc.Left);
                if (rcArea < rcMinArea)
                {
                    rcMinArea = rcArea;
                    rcMin = Rectangle.FromLTRB(rc.Left, dirty.Top, dirty.Right, dirty.Bottom);
                    rcDrt = Rectangle.FromLTRB(dirty.Left, dirty.Top, rc.Left, dirty.Bottom);
                }

                //Bottom edge.
                rcArea = dirty.Width * (dirty.Bottom - rc.Top);
                if (rcArea < rcMinArea)
                {
                    rcMinArea = rcArea;
                    rcMin = Rectangle.FromLTRB(dirty.Left, rc.Top, dirty.Right, dirty.Bottom);
                    rcDrt = Rectangle.FromLTRB(dirty.Left, dirty.Top, dirty.Right, rc.Top);
                }

                //New dirty rectangle with update rectangle excluded from it.
                dirty = rcDrt;

                return rcMin;
            }

            private void updateBmpDraw32BitPargb(Rectangle rcPixels)
            {
                //Update draw bitmap by copying pixels specified by rectangle from vram.
                //This method is called very frequently and needs to be fast.

                //Make sure that rectangle's left and right locations are even.
                //Some copy format methods rely on this to be simpler/faster.
                int left = rcPixels.Left & ~0x1; //Left even down.
                int right = (rcPixels.Right + 1) & ~0x1; //Right even up.
                rcPixels.X = left;
                rcPixels.Width = right - left;

                byte[] src = mVramMngr.mVram;
                int[] dst = mBmpDraw.Pixels;
                int srcPtr = getVramPointer(rcPixels.Location, mBitDepth);
                int dstPtr = mBmpDraw.getPixelIndex(rcPixels.Location);
                int srcStride = VramStride;
                int dstStride = mBmpDraw.StridePixels;
                if (mBitDepth == BitDepth.Ind4Bit)
                {
                    copy4BitTo32BitPargb(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride, mPalette);
                }
                else if (mBitDepth == BitDepth.Ind8Bit)
                {
                    copy8BitTo32BitPargb(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride, mPalette);
                }
                else if (mBitDepth == BitDepth.Rgb16Bit)
                {
                    copy16BitTo32BitPargb(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride, mShowAlpha);
                }
                else //if (mBitDepth == BitDepth.Rgb24Bit)
                {
                    copy24BitTo32BitPargb(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride);
                }
            }

            private static void copy4BitTo32BitPargb(Rectangle rcPixels, byte[] src, int[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride, int[] pargbs)
            {
                for (int y = rcPixels.Height; y > 0; y--, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    for (int x = rcPixels.Width; x > 0; x -= 2, si += 1, di += 2)
                    {
                        int indices = src[si]; //Two palette indices per byte.
                        dst[di + 0] = pargbs[indices & 0x0F]; //Left palette index in low nibble.
                        dst[di + 1] = pargbs[indices >> 4]; //Right palette index in high nibble.
                    }
                }
            }

            private static void copy8BitTo32BitPargb(Rectangle rcPixels, byte[] src, int[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride, int[] pargbs)
            {
                for (int y = rcPixels.Height; y > 0; y--, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    for (int x = rcPixels.Width; x > 0; x -= 2, si += 2, di += 2)
                    {
                        int index1 = src[si + 0];
                        int index2 = src[si + 1];
                        dst[di + 0] = pargbs[index1];
                        dst[di + 1] = pargbs[index2];
                    }
                    //Because pixel rectangle must have even left and right locations (i.e. width
                    //must be even) we can do two pixels per loop here (a bit of loop unrolling).
                    //Seems to be a little bit faster for 8 bit copy at least.
                }
            }

            private static void copy16BitTo32BitPargb(Rectangle rcPixels, byte[] src, int[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride, bool showAlpha)
            {
                for (int y = rcPixels.Height; y > 0; y--, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    for (int x = rcPixels.Width; x > 0; x -= 1, si += 2, di += 1)
                    {
                        dst[di] = getPixel16BitPargb(src, si, showAlpha);
                    }
                }
            }

            private static void copy24BitTo32BitPargb(Rectangle rcPixels, byte[] src, int[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride)
            {
                for (int y = rcPixels.Height; y > 0; y--, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    for (int x = rcPixels.Width; x > 0; x -= 1, si += 3, di += 1)
                    {
                        byte r = src[si + 0];
                        byte g = src[si + 1];
                        byte b = src[si + 2];
                        dst[di] = (byte.MaxValue << 24) | (r << 16) | (g << 8) | b; //Max alpha (opaque).
                    }
                }
            }

            public static BitmapPinned createSaveTextureBmp(VramMngr vramMngr, Rectangle rcPixels, BitDepth bitDepth, bool showAlpha)
            {
                Log.timeBegin();

                BitmapPinned bmp = new BitmapPinned(rcPixels.Width, rcPixels.Height,
                    isIndexed(bitDepth) ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb);

                byte[] src = vramMngr.mVram;
                byte[] dst = bmp.Bytes;
                int srcPtr = getVramPointer(rcPixels.Location, bitDepth);
                int dstPtr = 0;
                int srcStride = VramStride;
                int dstStride = bmp.StrideBytes;
                if (bitDepth == BitDepth.Ind4Bit)
                {
                    copy4BitTo8BitIndexed(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride);
                    vramMngr.updatePalette(bmp, PaletteWidth4Bit, showAlpha);
                }
                else if (bitDepth == BitDepth.Ind8Bit)
                {
                    copy8BitTo8BitIndexed(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride);
                    vramMngr.updatePalette(bmp, PaletteWidth8Bit, showAlpha);
                }
                else if (bitDepth == BitDepth.Rgb16Bit)
                {
                    copy16BitTo24BitRgb(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride, showAlpha);
                }
                else if (bitDepth == BitDepth.Rgb24Bit)
                {
                    copy24BitTo24BitRgb(rcPixels, src, dst, srcPtr, dstPtr, srcStride, dstStride);
                }
                else throw new InvalidOperationException();

                Log.timeEnd(bitDepth + " createSaveTextureBmp()");
                return bmp;
            }

            private static void copy4BitTo8BitIndexed(Rectangle rcPixels, byte[] src, byte[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride)
            {
                //Convert psx 4 bit indexes to bitmap PixelFormat.Format8bppIndexed.
                // 76543210 bit
                // rrrrllll psx (left and right pixel index)
                // iiiiiiii Format8bppIndexed
                for (int y = rcPixels.Height; y > 0; y--, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    int x = rcPixels.Width;
                    if ((rcPixels.Left & 0x1) != 0) //Left location is not even?
                    {
                        int indices = src[si];
                        dst[di] = (byte)(indices >> 4); //Right palette index in high nibble.
                        x--; si++; di++;
                    }
                    for (; x > 1; x -= 2, si += 1, di += 2)
                    {
                        int indices = src[si]; //Two palette indices per byte.
                        dst[di + 0] = (byte)(indices & 0x0F); //Left palette index in low nibble.
                        dst[di + 1] = (byte)(indices >> 4); //Right palette index in high nibble.
                    }
                    if (x > 0) //Right location was not even?
                    {
                        int indices = src[si];
                        dst[di] = (byte)(indices & 0x0F); //Left palette index in low nibble.
                    }
                }
            }

            private static void copy8BitTo8BitIndexed(Rectangle rcPixels, byte[] src, byte[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride)
            {
                //Convert psx 8 bit indexes to bitmap PixelFormat.Format8bppIndexed.
                // 76543210 bit
                // iiiiiiii psx
                // iiiiiiii Format8bppIndexed
                // Differences: None.
                for (int y = 0; y < rcPixels.Height; y++, srcPtr += srcStride, dstPtr += dstStride)
                {
                    Array.Copy(src, srcPtr, dst, dstPtr, rcPixels.Width);
                }
            }

            private static void copy16BitTo24BitRgb(Rectangle rcPixels, byte[] src, byte[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride, bool showAlpha)
            {
                //Convert psx 16 bit rgbs to Bitmap PixelFormat.Format24bppRgb.
                // 76543210 FEDCBA98 76543210 bit
                //          -bbbbbgg gggrrrrr psx
                // rrrrrrrr gggggggg bbbbbbbb Format24bppRgb
                for (int y = 0; y < rcPixels.Height; y++, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    for (int x = 0; x < rcPixels.Width; x += 1, si += 2, di += 3)
                    {
                        int pargb = getPixel16BitPargb(src, si, showAlpha);
                        dst[di + 0] = (byte)(pargb);
                        dst[di + 1] = (byte)(pargb >> 8);
                        dst[di + 2] = (byte)(pargb >> 16);
                    }
                }
            }

            private static void copy24BitTo24BitRgb(Rectangle rcPixels, byte[] src, byte[] dst,
                int srcPtr, int dstPtr, int srcStride, int dstStride)
            {
                //Convert psx 24 bit rgbs to Bitmap PixelFormat.Format24bppRgb.
                // 210 byte (3 bytes = 2 pixels)
                // BGR psx
                // RGB Format24bppRgb
                // Differences: Blue and red channels are swapped.
                for (int y = 0; y < rcPixels.Height; y++, srcPtr += srcStride, dstPtr += dstStride)
                {
                    int si = srcPtr, di = dstPtr;
                    for (int x = 0; x < rcPixels.Width; x += 1, si += 3, di += 3)
                    {
                        dst[di + 0] = src[si + 2]; //Swap blue and red.
                        dst[di + 1] = src[si + 1];
                        dst[di + 2] = src[si + 0];
                    }
                }
            }
        }
    }
}
