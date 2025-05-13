using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    //Manages vram (from state file). Visualize vram part.
    partial class VramMngr
    {
        //In bytes.
        public const int VramStride = 1024 * 2; //2048 bytes per "line".
        public const int VramHeight = 512; //Also in pixels.
        public const int VramSize = VramStride * VramHeight; //1048576 bytes total.
        private static readonly Rectangle VramBytesRc = new Rectangle(0, 0, VramStride, VramHeight);
        private const int VramSizeMask = VramSize - 1; //Mask vram pointers with this (0xFFFFF) to wrap around.

        //In pixels.
        public const int VramWidth4Bit = VramStride * 8 / 4; //4096.
        public const int VramWidth8Bit = VramStride * 8 / 8; //2048.
        public const int VramWidth16Bit = VramStride * 8 / 16; //1024.
        public const int VramWidth24Bit = VramStride * 8 / 24; //682 (682.666 rounded down).
        public const int PaletteHeight = 1;
        public const int PaletteWidth4Bit = 16;
        public const int PaletteWidth8Bit = 256;

        //Alpha bit blend display.
        private const int Ratio16BitAlpha = 155; //0-255. Around 155 looks best.
        private const int Ratio16BitAlphaInv = byte.MaxValue - Ratio16BitAlpha;

        //Bit depth.
        public enum BitDepth
        {
            Ind4Bit = 4, //Bits per pixel.
            Ind8Bit = 8,
            Rgb16Bit = 16,
            Rgb24Bit = 24,
        }

        public const BitDepth BitDepthCompare = BitDepth.Rgb16Bit;
        public const BitDepth BitDepthPalette = BitDepth.Rgb16Bit;

        private readonly Vrammed mVrammed;
        private byte[] mVram;
        private BitDepth mBitDepthTexture;
        private bool mShowAlphaTexture; //Flag for if alpha bit should be shown/exposed.
        private bool mShowAlphaCompare;
        private bool mShowAlphaPalette;

        //Bitmaps with visual representation of vram in different formats.
        private readonly VramBmp mBmpVram4Bit;
        private readonly VramBmp mBmpVram8Bit;
        private readonly VramBmp mBmpVram16Bit;
        private readonly VramBmp mBmpVram24Bit;
        private readonly VramBmp mBmpVram4BitAlpha;
        private readonly VramBmp mBmpVram8BitAlpha;
        private readonly VramBmp mBmpVram16BitAlpha;

        public VramMngr(Vrammed vrammed)
        {
            mVrammed = vrammed;
            mBitDepthTexture = BitDepth.Rgb16Bit;
            mShowAlphaTexture = false;
            mShowAlphaCompare = false;
            mShowAlphaPalette = false;

            mBmpVram4Bit = new VramBmp(this, BitDepth.Ind4Bit, false);
            mBmpVram8Bit = new VramBmp(this, BitDepth.Ind8Bit, false);
            mBmpVram16Bit = new VramBmp(this, BitDepth.Rgb16Bit, false);
            mBmpVram24Bit = new VramBmp(this, BitDepth.Rgb24Bit, false);
            mBmpVram4BitAlpha = new VramBmp(this, BitDepth.Ind4Bit, true);
            mBmpVram8BitAlpha = new VramBmp(this, BitDepth.Ind8Bit, true);
            mBmpVram16BitAlpha = new VramBmp(this, BitDepth.Rgb16Bit, true);

            //VramMngrEdit.
            EditValue4Bit = EditValueDefault4Bit;
            EditValue8Bit = EditValueDefault8Bit;
            EditValue16Bit = EditValueDefault16Bit;
            EditValue24Bit = EditValueDefault24Bit;
            mEditMask16Bit = EditMaskDefault;
            mEditMask24Bit = EditMaskDefault;
            mEditUndoStack = new LimitedStack<EditCmd>(EditStackMaxItems);
            mEditRedoStack = new LimitedStack<EditCmd>(EditStackMaxItems);
        }

        public void init()
        {
            mVrammed.FileChanged += Vrammed_FileChanged;
            mVrammed.ModeChanged += Vrammed_ModeChanged;
            mVrammed.PaletteIndicator.LocationChanged += PaletteIndicator_LocationChanged;
        }

        private void setVram(byte[] vram)
        {
            mVram = vram;

            mEditUndoStack.Clear();
            mEditRedoStack.Clear();

            editVramChanged(VramBytesRc);
        }

        //*******************************************************************************
        //*******************************************************************************

        #region Properties

        private VramMarkerTexture TextureMarker
        {
            get { return mVrammed.TextureMarker; }
        }

        private VramMarkerCompare CompareMarker
        {
            get { return mVrammed.CompareMarker; }
        }

        private VramMarkerPalette PaletteMarker
        {
            get { return mVrammed.PaletteMarker; }
        }

        public Rectangle BmpTextureBounds
        {
            get { return getBmpBounds(BitDepthTexture); }
        }

        public Rectangle BmpCompareBounds
        {
            get { return getBmpBounds(BitDepthCompare); }
        }

        public Rectangle BmpPaletteBounds
        {
            get { return getBmpBounds(BitDepthPalette); }
        }

        private static Rectangle getBmpBounds(BitDepth bitDepth)
        {
            return new Rectangle(0, 0, toPixelWidth(bitDepth), VramHeight);
        }

        public static int toPixelWidth(BitDepth bitDepth)
        {
            return VramStride * 8 / (int)bitDepth; ;
        }

        public static int toPaletteWidth(BitDepth bitDepth)
        {
            return bitDepth == BitDepth.Ind4Bit ? PaletteWidth4Bit : PaletteWidth8Bit;
        }

        public static bool isIndexed(BitDepth bitDepth)
        {
            return bitDepth == BitDepth.Ind4Bit || bitDepth == BitDepth.Ind8Bit;
        }

        public BitDepth BitDepthTexture
        {
            get { return mBitDepthTexture; }
        }

        public bool ShowAlphaTexture
        {
            get { return mShowAlphaTexture; }
            set { setShowAlphaTexture(value); }
        }

        public bool ShowAlphaCompare
        {
            get { return mShowAlphaCompare; }
            set { setShowAlphaCompare(value); }
        }

        public bool ShowAlphaPalette
        {
            get { return mShowAlphaPalette; }
            set { setShowAlphaPalette(value); }
        }

        private void setBitDepthTexture(BitDepth bitDepth)
        {
            if (mBitDepthTexture != bitDepth)
            {
                BitDepth oldBitDepth = mBitDepthTexture;
                mBitDepthTexture = bitDepth;
                OnBitDepthTextureChanged(oldBitDepth, bitDepth);
            }
        }

        private void setShowAlphaTexture(bool showAlpha)
        {
            if (mShowAlphaTexture != showAlpha)
            {
                mShowAlphaTexture = showAlpha;
                OnShowAlphaTextureChanged(showAlpha);
            }
        }

        private void setShowAlphaCompare(bool showAlpha)
        {
            if (mShowAlphaCompare != showAlpha)
            {
                mShowAlphaCompare = showAlpha;
                OnShowAlphaCompareChanged(showAlpha);
            }
        }

        private void setShowAlphaPalette(bool showAlpha)
        {
            if (mShowAlphaPalette != showAlpha)
            {
                mShowAlphaPalette = showAlpha;
                OnShowAlphaPaletteChanged(showAlpha);
            }
        }

        #endregion Properties

        //*******************************************************************************
        //*******************************************************************************

        #region Draw

        public void drawBmpTexture(Graphics gr)
        {
            VramBmp vb;
            if (mBitDepthTexture == BitDepth.Ind4Bit)
            {
                vb = mShowAlphaTexture ? mBmpVram4BitAlpha : mBmpVram4Bit;
            }
            else if (mBitDepthTexture == BitDepth.Ind8Bit)
            {
                vb = mShowAlphaTexture ? mBmpVram8BitAlpha : mBmpVram8Bit;
            }
            else if (mBitDepthTexture == BitDepth.Rgb16Bit)
            {
                vb = mShowAlphaTexture ? mBmpVram16BitAlpha : mBmpVram16Bit;
            }
            else if (mBitDepthTexture == BitDepth.Rgb24Bit)
            {
                vb = mBmpVram24Bit;
            }
            else throw new InvalidOperationException();
            Bitmap bmp = vb.getBmpDraw(gr.ClipBounds.ToRectangle());
            gr.DrawImage(bmp, 0, 0);
        }

        public void drawBmpCompare(Graphics gr)
        {
            VramBmp vb = mShowAlphaCompare ? mBmpVram16BitAlpha : mBmpVram16Bit;
            Bitmap bmp = vb.getBmpDraw(gr.ClipBounds.ToRectangle());
            gr.DrawImage(bmp, 0, 0);
        }

        public void drawBmpPalette(Graphics gr)
        {
            VramBmp vb = mShowAlphaPalette ? mBmpVram16BitAlpha : mBmpVram16Bit;
            Bitmap bmp = vb.getBmpDraw(gr.ClipBounds.ToRectangle());
            gr.DrawImage(bmp, 0, 0);
        }

        #endregion Draw

        //*******************************************************************************
        //*******************************************************************************

        #region Events

        public event EventChangeOldNew<VramMngr, BitDepth> BitDepthTextureChanged;
        public event EventChangeNew<VramMngr, bool> ShowAlphaTextureChanged;
        public event EventChangeNew<VramMngr, bool> ShowAlphaCompareChanged;
        public event EventChangeNew<VramMngr, bool> ShowAlphaPaletteChanged;
        public event EventChange<VramMngr, bool> VramChanged;

        private void OnBitDepthTextureChanged(BitDepth oldBitDepth, BitDepth newBitDepth)
        {
            if (BitDepthTextureChanged != null) BitDepthTextureChanged(this, oldBitDepth, newBitDepth);
        }

        private void OnShowAlphaTextureChanged(bool newShowAlpha)
        {
            if (ShowAlphaTextureChanged != null) ShowAlphaTextureChanged(this, newShowAlpha);
        }

        private void OnShowAlphaCompareChanged(bool newShowAlpha)
        {
            if (ShowAlphaCompareChanged != null) ShowAlphaCompareChanged(this, newShowAlpha);
        }

        private void OnShowAlphaPaletteChanged(bool newShowAlpha)
        {
            if (ShowAlphaPaletteChanged != null) ShowAlphaPaletteChanged(this, newShowAlpha);
        }

        private void OnVramChanged(bool isPaletteAffected)
        {
            if (VramChanged != null) VramChanged(this, isPaletteAffected);
        }

        private void Vrammed_FileChanged(Vrammed sender, StateFile oldFile, StateFile newFile)
        {
            if (newFile != null)
            {
                setVram(newFile.Vram);
            }
        }

        private void Vrammed_ModeChanged(Vrammed sender, Mode oldMode, Mode newMode)
        {
            setBitDepthTexture(newMode.toVramBitDepth());
        }

        private void PaletteIndicator_LocationChanged(VramIndicator sender, Point oldLoc, Point newLoc)
        {
            mBmpVram4Bit.onPaletteLocationChanged();
            mBmpVram8Bit.onPaletteLocationChanged();
            mBmpVram4BitAlpha.onPaletteLocationChanged();
            mBmpVram8BitAlpha.onPaletteLocationChanged();
        }

        #endregion Events

        //*******************************************************************************
        //*******************************************************************************

        #region Vram pointer from pixel location

        private static int getVramPointer(Point pixelLoc, BitDepth bitDepth)
        {
            return (pixelLoc.Y * VramStride) + (pixelLoc.X * (int)bitDepth / 8);
        }

        private static int getVramPointer4BitNibble(Point pixelLoc)
        {
            //Get pointer in nibbles (4 bits). Divide it by 2 to get a byte (8 bits) pointer.
            //Lowest bit in pointer indicates low (0) or high (1) nibble.
            int ptr = getVramPointer(pixelLoc, BitDepth.Ind4Bit);
            return (ptr * 2) + (pixelLoc.X & 0x1);
        }

        private static int getVramPointer8Bit(Point pixelLoc)
        {
            return getVramPointer(pixelLoc, BitDepth.Ind8Bit);
        }

        private static int getVramPointer16Bit(Point pixelLoc)
        {
            return getVramPointer(pixelLoc, BitDepth.Rgb16Bit);
        }

        private static int getVramPointer24Bit(Point pixelLoc)
        {
            return getVramPointer(pixelLoc, BitDepth.Rgb24Bit);
        }

        private int getPalettePointer()
        {
            return getPalettePointer(0);
        }

        private int getPalettePointer(int index)
        {
            //Because 8 bit palette marker can be outside vram we need to mask the pointer.
            return (getVramPointer16Bit(PaletteMarker.Indicator.Location) + (index * 2)) & VramSizeMask;
        }

        private int getPaletteIndex4Bit(int nibblePointer) //Pointer in nibbles.
        {
            return getPaletteIndex4Bit(mVram, nibblePointer);
        }

        private static int getPaletteIndex4Bit(byte[] src, int srcPtrNibble) //Pointer in nibbles.
        {
            //Convert nibble pointer to bytes (divide by 2) and then use lowest bit in pointer
            //to select low or high nibble in source byte.
            int indices = src[srcPtrNibble / 2]; //Read byte with the two 4 bit indices.
            int shift = (srcPtrNibble & 0x1) * 4; //Shift count, 4 if high nibble, 0 if low nibble.
            return (indices >> shift) & 0x0F;
        }

        #endregion Vram pointer from pixel location

        //*******************************************************************************
        //*******************************************************************************

        #region Pixel from vram

        public PsColor16Bit getPixel4Bit(Point pixelLoc)
        {
            int dummy;
            return getPixel4Bit(pixelLoc, out dummy);
        }

        public PsColor16Bit getPixel4Bit(Point pixelLoc, out int index)
        {
            int nibblePtr = getVramPointer4BitNibble(pixelLoc);
            index = getPaletteIndex4Bit(nibblePtr);
            return getPixelPalette(index);
        }

        public PsColor16Bit getPixel8Bit(Point pixelLoc)
        {
            int dummy;
            return getPixel8Bit(pixelLoc, out dummy);
        }

        public PsColor16Bit getPixel8Bit(Point pixelLoc, out int index)
        {
            index = mVram[getVramPointer8Bit(pixelLoc)];
            return getPixelPalette(index);
        }

        public PsColor16Bit getPixel16Bit(Point pixelLoc)
        {
            int abgr = mVram.read16Bit(getVramPointer16Bit(pixelLoc));
            return new PsColor16Bit((ushort)abgr);
        }

        public PsColor24Bit getPixel24Bit(Point pixelLoc)
        {
            int bgr = mVram.read24Bit(getVramPointer24Bit(pixelLoc));
            return new PsColor24Bit(bgr);
        }

        public PsColor16Bit getPixelPalette(int index)
        {
            int abgr = mVram.read16Bit(getPalettePointer(index));
            return new PsColor16Bit((ushort)abgr);
        }

        private static int getPixel16BitPargb(byte[] src, int srcPtr)
        {
            return getPixel16BitPargb(src, srcPtr, false);
        }

        private static int getPixel16BitPargb(byte[] src, int srcPtr, bool showAlpha)
        {
            int abgr = (src[srcPtr]) | (src[srcPtr + 1] << 8);
            int r = (abgr << 3) & 0xF8;
            int g = (abgr >> 2) & 0xF8;
            int b = (abgr >> 7) & 0xF8;

            //"Expand" 5-bit to 8-bit by using the 3 MSB as the 3 LSB.
            //r |= r >> 5;
            //g |= g >> 5;
            //b |= b >> 5;

            if (showAlpha)
            {
                //If alpha bit is set make pixel yellow by increasing red&green and decreasing blue.
                //Else if not set make pixel blue by increasing blue and decreasing red&green.
                int a = abgr >> 15;
                int blendSet = a * byte.MaxValue; //Blend ratio if alpha bit is set.
                int blendClr = byte.MaxValue - blendSet; //Blend ratio if alpha bit is clear.

                //Integer math version.
                r = ((r * Ratio16BitAlphaInv) + (blendSet * Ratio16BitAlpha)) >> 8; //Divide by 256.
                g = ((g * Ratio16BitAlphaInv) + (blendSet * Ratio16BitAlpha)) >> 8;
                b = ((b * Ratio16BitAlphaInv) + (blendClr * Ratio16BitAlpha)) >> 8;

                //Float version. A bit slower than the integer version above.
                //Here Ratio16BitAlpha would be a float between 0.0-1.0
                //and Ratio16BitAlphaInv = 1.0-Ratio16BitAlpha.
                //r = (int)((r * Ratio16BitAlphaInvF) + (blendSet * Ratio16BitAlphaF));
                //g = (int)((g * Ratio16BitAlphaInvF) + (blendSet * Ratio16BitAlphaF));
                //b = (int)((b * Ratio16BitAlphaInvF) + (blendClr * Ratio16BitAlphaF));
            }

            return (byte.MaxValue << 24) | (r << 16) | (g << 8) | b; //Max alpha (opaque).
        }

        #endregion Pixel from vram

        //*******************************************************************************
        //*******************************************************************************

        #region Rectangle conversion

        private static Rectangle rcByteToPixel(Rectangle rcByte, BitDepth bitDepthPixel)
        {
            return rcToBitDepth(rcByte, BitDepth.Ind8Bit, bitDepthPixel);
        }

        private static Rectangle rcPixelToByte(Rectangle rcPixel, BitDepth bitDepthPixel)
        {
            return rcToBitDepth(rcPixel, bitDepthPixel, BitDepth.Ind8Bit);
        }

        public static Rectangle rcToBitDepth(Rectangle rcSrc, BitDepth bitDepthSrc, BitDepth bitDepthDst)
        {
            //Convert rectangle's "pixelformat" (bits per horizontal step, rectangle.X).
            int bitsSrc = (int)bitDepthSrc;
            double bitsDst = (int)bitDepthDst;
            int left = (int)Math.Floor((rcSrc.Left * bitsSrc) / bitsDst); //Round left side down.
            int right = (int)Math.Ceiling((rcSrc.Right * bitsSrc) / bitsDst); //Round right side up.
            rcSrc.X = left;
            rcSrc.Width = right - left;
            if (bitDepthDst == BitDepth.Rgb24Bit)
            {
                //Because 24 bit mode is "missing" 2 bytes per scanline and therefore
                //has a smaller bitmap than other modes we have to make sure the
                //rectangle is inside the 24 bit bitmap when converting to 24 bit.
                rcSrc.Intersect(getBmpBounds(BitDepth.Rgb24Bit));
            }
            return rcSrc;
        }

        #endregion Rectangle conversion

        //*******************************************************************************
        //*******************************************************************************

        #region Palette update

        private void updatePalette(Bitmap bmp, int entries, bool showAlpha)
        {
            updatePalette(bmp, mVram, getPalettePointer(), entries, showAlpha);
        }

        private static void updatePalette(Bitmap bmp, byte[] vram, int vramPtr, int entries, bool showAlpha)
        {
            //Because palette marker can be outside vram we need to mask the pointer.
            ColorPalette palette = bmp.Palette;
            for (int i = 0; i < entries; i++, vramPtr = (vramPtr + 2) & VramSizeMask)
            {
                palette.Entries[i] = Color.FromArgb(getPixel16BitPargb(vram, vramPtr, showAlpha));
            }
            bmp.Palette = palette;
        }

        private void updatePalette(int[] palette, bool showAlpha)
        {
            updatePalette(palette, mVram, getPalettePointer(), showAlpha);
        }

        private static void updatePalette(int[] palette, byte[] vram, int vramPtr, bool showAlpha)
        {
            //Because palette marker can be outside vram we need to mask the pointer.
            for (int i = 0; i < palette.Length; i++, vramPtr = (vramPtr + 2) & VramSizeMask)
            {
                palette[i] = getPixel16BitPargb(vram, vramPtr, showAlpha);
            }
        }

        #endregion Palette update

        //*******************************************************************************
        //*******************************************************************************

        #region Compare update

        public void updateCompareBmp(BitmapPinned32bppPArgb bmp, bool flipH, bool flipV, float alpha, Color colorEqual)
        {
            //Compare content of texture and compare indicators and store result in bmp parameter.
            //Texture indicator can be flipped.
            //Transparency (alpha) of result and color of equal pixels can also be set.

            Rectangle tRc = mVrammed.TextureIndicator.Rectangle;
            Rectangle cRc = mVrammed.CompareIndicator.Rectangle;

            BitmapPinned32bppPArgb tBmp =
                (mBitDepthTexture == BitDepth.Ind4Bit ? mBmpVram4Bit : mBmpVram8Bit).getBmpDraw(tRc);
            BitmapPinned32bppPArgb cBmp = mBmpVram16Bit.getBmpDraw(cRc);

            int pargbEqual = colorEqual.ToPargb(alpha);
            byte a = (byte)(byte.MaxValue * alpha + 0.5f);

            int tPtr = tBmp.getPixelIndex(flipH ? tRc.Right - 1 : tRc.X, flipV ? tRc.Bottom - 1 : tRc.Y);
            int cPtr = cBmp.getPixelIndex(cRc.Location);
            int bPtr = 0;
            int tPtrAddY = tBmp.StridePixels * (flipV ? -1 : 1);
            int cPtrAddY = cBmp.StridePixels;
            int bPtrAddY = bmp.StridePixels;
            int tiAddX = flipH ? -1 : 1;
            int ciEnd = cBmp.Pixels.Length;

            for (int y = tRc.Height; y > 0; y--, tPtr += tPtrAddY, cPtr += cPtrAddY, bPtr += bPtrAddY)
            {
                int ti = tPtr, ci = cPtr, bi = bPtr;
                for (int x = tRc.Width; x > 0; x--, ti += tiAddX, ci++, bi++)
                {
                    //Because compare marker can be outside bmp we need to check its index.
                    //What's best? Skip or wrap around? Skip will leave old pixels, but that should
                    //be okay as they are outside i.e. never visible?
                    //if (ci < 0 || ci >= ciEnd) continue; //Skip if outside? Old pixel in bmp untouched.
                    int cp = cBmp[(int)(((UInt32)ci) % ciEnd)]; //Wrap around (<0 or >=length)?

                    int tp = tBmp[ti];

                    if (tp == cp) //Equal?
                    {
                        bmp[bi] = pargbEqual;
                    }
                    else //Set result as texture pixel with alpha applied.
                    {
                        int b = tp & 0xFF;
                        int g = (tp >> 8) & 0xFF;
                        int r = (tp >> 16) & 0xFF;
                        b = (int)(b * alpha + 0.5f);
                        g = (int)(g * alpha + 0.5f);
                        r = (int)(r * alpha + 0.5f);
                        bmp[bi] = (a << 24) | (r << 16) | (g << 8) | b;
                    }
                }
            }
            //That alpha multiply might look slow, but I tested a method that used palettes
            //(4 or 8 bit) to compare and store result instead and it was actually a bit slower.
            //Float multiply is fast? The palette method was a bit more complicated also and
            //4 bit palette indices are always a bit tricky to deal with.
        }

        #endregion Compare update

        //*******************************************************************************
        //*******************************************************************************

        #region Save texture

        public BitmapPinned createSaveTextureBmp(bool onlyMarked)
        {
            Rectangle rcPixels = onlyMarked ? TextureMarker.Rectangle : getBmpBounds(mBitDepthTexture);
            return VramBmp.createSaveTextureBmp(this, rcPixels, mBitDepthTexture, mShowAlphaTexture);
        }

        #endregion Save texture
    }

    static class ByteArrayExt
    {
        public static int read16Bit(this byte[] src, int srcInd)
        {
            return (src[srcInd]) | (src[srcInd + 1] << 8);
        }

        public static int read24Bit(this byte[] src, int srcInd)
        {
            return (src[srcInd]) | (src[srcInd + 1] << 8) | (src[srcInd + 2] << 16);
        }

        public static void write16Bit(this byte[] dst, int dstInd, int val)
        {
            dst[dstInd] = (byte)val;
            dst[dstInd + 1] = (byte)(val >> 8);
        }
    }
}
