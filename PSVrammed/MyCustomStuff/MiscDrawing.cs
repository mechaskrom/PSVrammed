using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

namespace MyCustomStuff
{
    public static class MiscDrawing
    {
        public static bool Contains(this Size s, Size size) //Parameter size can fit inside this size.
        {
            return s.Width >= size.Width && s.Height >= size.Height;
        }

        public static bool IsZero(this Size s) //Width or height is zero.
        {
            return s.Width == 0 || s.Height == 0;
        }

        public static Point Floor(this PointF pf)
        {
            return new Point(pf.X.Floor(), pf.Y.Floor());
        }

        public static Point Ceiling(this PointF pf)
        {
            return new Point(pf.X.Ceiling(), pf.Y.Ceiling());
        }

        public static Size Multiply(this Size s, int mulW, int mulH)
        {
            return new Size(s.Width * mulW, s.Height * mulH);
        }

        public static SizeF Multiply(this Size s, float mulW, float mulH)
        {
            return new SizeF(s.Width * mulW, s.Height * mulH);
        }

        public static Size Multiply(this Size s, Size size)
        {
            return s.Multiply(size.Width, size.Height);
        }

        public static Point Subtract(this Point p, Point point)
        {
            return new Point(p.X - point.X, p.Y - point.Y);
        }

        public static Point Divide(this Point p, int divX, int divY)
        {
            return new Point(p.X / divX, p.Y / divY);
        }

        public static Point Clamp(this Point p, int minX, int minY, int maxX, int maxY)
        {
            return new Point(p.X.Clamp(minX, maxX), p.Y.Clamp(minY, maxY));
        }

        public static Point Clamp(this Point p, Size max)
        {
            return p.Clamp(0, 0, max.Width - 1, max.Height - 1);
        }

        public static Size Clamp(this Size s, int minW, int maxW, int minH, int maxH)
        {
            return new Size(s.Width.Clamp(minW, maxW), s.Height.Clamp(minH, maxH));
        }

        public static Size Clamp(this Size s, Size min, Size max)
        {
            return s.Clamp(min.Width, max.Width, min.Height, max.Height);
        }

        public static Point Snap(this Point p, int multipleX, int multipleY)
        {
            return new Point(p.X.Snap(multipleX), p.Y.Snap(multipleY));
        }

        public static Point SnapDown(this Point p, int multipleX, int multipleY)
        {
            return new Point(p.X.SnapDown(multipleX), p.Y.SnapDown(multipleY));
        }

        public static Size SnapDown(this Size s, Size size)
        {
            return s.SnapDown(size.Width, size.Height);
        }

        public static Size SnapDown(this Size s, int multipleWidth, int multipleHeight)
        {
            return new Size(s.Width.SnapDown(multipleWidth), s.Height.SnapDown(multipleHeight));
        }

        public static Point SnapUp(this Point p, int multipleX, int multipleY)
        {
            return new Point(p.X.SnapUp(multipleX), p.Y.SnapUp(multipleY));
        }

        public static Rectangle Snap(this Rectangle r, int multipleX, int multipleY)
        {
            return Rectangle.FromLTRB(r.X.SnapDown(multipleX), r.Y.SnapDown(multipleY),
                r.Right.SnapUp(multipleX), r.Bottom.SnapUp(multipleY));
        }

        public static Point OffsetRet(this Point p, int dx, int dy)
        {
            p.Offset(dx, dy);
            return p;
        }

        public static Point OffsetRet(this Point p, Point point)
        {
            return p.OffsetRet(point.X, point.Y);
        }

        public static Rectangle Union(this Point p, Point point)
        {
            return new Rectangle(Math.Min(p.X, point.X), Math.Min(p.Y, point.Y), Math.Abs(p.X - point.X), Math.Abs(p.Y - point.Y));
        }

        public static bool IntersectsWithNonEmpty(this Rectangle r, Rectangle rc)
        {
            return (rc.Width > 0 || rc.Height > 0) && r.IntersectsWith(rc);
        }

        public static Rectangle UnionWithNonEmpty(this Rectangle r, Rectangle rc)
        {
            return (rc.Width > 0 || rc.Height > 0) ? Rectangle.Union(r, rc) : r;
        }

        public static Rectangle InflateRet(this Rectangle r, int x, int y)
        {
            r.Inflate(x, y);
            return r;
        }

        public static Rectangle OffsetRet(this Rectangle r, Point p)
        {
            return r.OffsetRet(p.X, p.Y);
        }

        public static Rectangle OffsetRet(this Rectangle r, int x, int y)
        {
            r.Offset(x, y);
            return r;
        }

        public static Rectangle ToRectangle(this RectangleF r)
        {
            return Rectangle.FromLTRB(r.Left.Floor(), r.Top.Floor(), r.Right.Ceiling(), r.Bottom.Ceiling());
        }

        public static Point GetLeftTopPoint(this Rectangle r)
        {
            return r.Location;
        }

        public static Point GetRightTopPoint(this Rectangle r)
        {
            return new Point(r.Right, r.Top);
        }

        public static Point GetLeftBottomPoint(this Rectangle r)
        {
            return new Point(r.Left, r.Bottom);
        }

        public static Point GetRightBottomPoint(this Rectangle r)
        {
            return new Point(r.Right, r.Bottom);
        }

        public static Point GetCenterPoint(this Rectangle r)
        {
            return new Point(r.X + (r.Width / 2), r.Y + (r.Height / 2));
        }

        [DebuggerStepThrough]
        public static Rectangle GetRectangle(this Bitmap bmp)
        {
            return new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public static Bitmap Copy(this Bitmap bmp)
        {
            return bmp.Copy(bmp.GetRectangle(), bmp.PixelFormat);
        }

        public static Bitmap Copy(this Bitmap bmp, Rectangle rc)
        {
            return bmp.Copy(rc, bmp.PixelFormat);
        }

        public static Bitmap Copy(this Bitmap bmp, PixelFormat format)
        {
            return bmp.Copy(bmp.GetRectangle(), format);
        }

        public static Bitmap Copy(this Bitmap bmp, Rectangle rc, PixelFormat format)
        {
            return bmp.Copy(rc, format, Color.Empty, byte.MaxValue);
        }

        public static Bitmap Copy(this Bitmap bmp, Rectangle rc, Color colorKey)
        {
            return bmp.Copy(rc, bmp.PixelFormat, colorKey, byte.MaxValue);
        }

        public static Bitmap Copy(this Bitmap bmp, Color colorKey)
        {
            return bmp.Copy(colorKey, byte.MaxValue);
        }

        public static Bitmap Copy(this Bitmap bmp, int opacity)
        {
            return bmp.Copy(Color.Empty, opacity);
        }

        public static Bitmap Copy(this Bitmap bmp, Color colorKey, int opacity)
        {
            return bmp.Copy(bmp.GetRectangle(), bmp.PixelFormat, colorKey, opacity);
        }

        public static Bitmap Copy(this Bitmap bmp, Rectangle rc, PixelFormat format, Color colorKey, int opacity)
        {
            if (!bmp.GetRectangle().Contains(rc))
            {
                throw new ArgumentException("Rectangle is outside bitmap!");
            }
            if (opacity < byte.MinValue || opacity > byte.MaxValue)
            {
                throw new ArgumentException(String.Format("Opacity value isn't valid ({0}-{1})!", byte.MinValue, byte.MaxValue));
            }

            //Try to select best way to copy bitmap with specified parameters.
            Bitmap copy;
            if (colorKey.IsEmpty && opacity == byte.MaxValue)
            {
                //Clone copy is fastest if no alpha changes.
                copy = cloneCopy(bmp, rc, format);
            }
            else if (format == PixelFormat.Format32bppPArgb)
            {
                //Draw copy can be faster sometimes (windows only?).
                copy = drawCopy(bmp, rc, format, colorKey, opacity);
                //copy = alphaCopy(bmp, rc, format, colorKey, opacity);
            }
            else
            {
                //Best compromise between different formats and systems.
                copy = alphaCopy(bmp, rc, format, colorKey, opacity);
            }

            Debug.Assert(copy.PixelFormat == format, "Pixel format wrong in copied bitmap!");
            Debug.Assert(copy.HorizontalResolution == bmp.HorizontalResolution &&
                copy.VerticalResolution == bmp.VerticalResolution, "DPI changed in copied bitmap!");
            return copy;
        }

        private static Bitmap cloneCopy(Bitmap bmpSrc, Rectangle rc, PixelFormat format)
        {
            //Prefer clone, but fallback to lock copy if better. Both methods seem to work similarly.

            //Clone is slow if bitmap was created from a stream i.e. not a MemoryBmp (and/or ReadOnly?)
            //and will keep the underlying stream/file locked which probably isn't wanted.
            bool isStreamedBmp =
            !bmpSrc.IsImageFormat(ImageFormat.MemoryBmp) || bmpSrc.IsFlags(ImageFlags.ReadOnly);
            //LockBits cannot convert indexed format to an indexed format with fewer bits.
            bool isReducedIndexed =
                (format & bmpSrc.PixelFormat).IsIndexed() && format.BitsPerPixel() < bmpSrc.PixelFormat.BitsPerPixel();

            Bitmap copy;
            if (!isStreamedBmp || isReducedIndexed)
            {
                copy = bmpSrc.Clone(rc, format);
                copy.SetDpi(bmpSrc); //DPI not cloned.
            }
            else
            {
                copy = new Bitmap(rc.Width, rc.Height, format);
                copy.SetDpi(bmpSrc);
                if ((format & bmpSrc.PixelFormat).IsIndexed())
                {
                    copy.Palette = bmpSrc.Palette; //Returns a copy and not a ref.
                }
                //LockBits creates and points to a buffer with format specified. Extra buffer created on conversions?
                BitmapData dataSrc = bmpSrc.LockBits(rc, ImageLockMode.ReadOnly, format); //Converts format.
                BitmapData dataDst = copy.LockBits(copy.GetRectangle(), ImageLockMode.WriteOnly, format);
                int length = (int)Math.Ceiling(rc.Width * format.BitsPerPixel() / 8.0f); //Round up?
                byte[] buffer = new byte[length];
                long srcInd = dataSrc.Scan0.ToInt64(), dstInd = dataDst.Scan0.ToInt64();
                long strideSrc = dataSrc.Stride, strideDst = dataDst.Stride;
                //Copy lines is a bit faster than one full copy. Because of smaller buffer allocated?
                for (int y = 0; y < rc.Height; y++, srcInd += strideSrc, dstInd += strideDst)
                {
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcInd, buffer, 0, buffer.Length);
                    System.Runtime.InteropServices.Marshal.Copy(buffer, 0, (IntPtr)dstInd, buffer.Length);
                }
                bmpSrc.UnlockBits(dataSrc);
                copy.UnlockBits(dataDst);

                //LockBits per line is actually both faster and a lot slower with some formats, but doubtful
                //if this can be relied on. It is usually considered a costly operation?
                //LockBits will use extra memory when converting format. If converting very big bitmaps it is
                //maybe a good idea to only lock 1/2 or 1/3 of the height at a time to reduce memory usage?
            }
            return copy;

            //Formats with problems:
            //Format1bppIndexed -> Output wrong or error on linux.
            //Format4bppIndexed -> Error on linux.
            //Format16bppGrayScale -> Error on windows.
            //Format16bppRgb555, Format16bppRgb565, Format16bppArgb1555 -> Saved as 32 bit (24 bit enough), otherwise fine.
            //Format48bppRgb, Format64bppArgb -> Output wrong.
            //Format64bppPArgb -> Output wrong on windows. Error on linux.
        }

        private static Bitmap drawCopy(Bitmap bmpSrc, Rectangle rc, PixelFormat format, Color colorKey, int opacity)
        {
            if (colorKey.IsEmpty && opacity == byte.MaxValue) //No alpha changes?
            {
                return drawCopy(bmpSrc, rc, format, null);
            }

            using (ImageAttributes ia = new ImageAttributes())
            {
                if (opacity != byte.MaxValue)
                {
                    ColorMatrix cm = new ColorMatrix();
                    cm.Matrix33 = opacity / (float)(byte.MaxValue);
                    ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                }
                if (!colorKey.IsEmpty)
                {
                    ia.SetColorKey(colorKey, colorKey, ColorAdjustType.Bitmap);
                }
                return drawCopy(bmpSrc, rc, format, ia);
            }
        }

        private static Bitmap drawCopy(Bitmap bmpSrc, Rectangle rc, PixelFormat format, ImageAttributes ia)
        {
            //Draw cannot handle indexed formats or Format16bppArgb1555 (windows only?).
            //Unpredictable performance from really fast to really slow depending on format and system.
            //Fast if source format is indexed or copy format is Format32bppPArgb on windows at least.
            //Not sure if alpha is handled correctly. Seems to set color to 0 if alpha is 0 even if
            //not a premultiplied alpha format? Maybe draw use Format32bppPArgb internally and
            //therefore always convert values to/from it?

            Bitmap copy = new Bitmap(rc.Width, rc.Height, format);
            copy.SetDpi(bmpSrc);
            using (Graphics gr = Graphics.FromImage(copy)) //Error if indexed format or Format16bppArgb1555.
            {
                gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy; //Does nothing?
                if (ia == null)
                {
                    gr.DrawImage(bmpSrc, 0, 0, rc, GraphicsUnit.Pixel);
                }
                else
                {
                    gr.DrawImageAttr(bmpSrc, 0, 0, rc, GraphicsUnit.Pixel, ia);
                }
            }
            return copy;
        }

        private static Bitmap alphaCopy(Bitmap bmpSrc, Rectangle rc, PixelFormat format, Color colorKey, int opacity)
        {
            //Return copy of bitmap and also changes alpha on pixels.
            //If non-empty key matches color then alpha is set to 0 else it's set to opacity if it isn't null.
            //Opacity parameter is used as a factor when calculating new alpha: alphaNew = alphaOld * opacity / 255.
            //I.e. if opacity is 255 then alpha is not changed, but if opacity is 128 then alpha is halved, and so on.

            //Draw bitmap with ImageAttributes is usually a fast and easy way to change alpha, but drawing has
            //unpredictable performance on different systems, formats, and doesn't support indexed formats.
            //This method is slower (up to 2x) than fastest drawing, but still a lot faster than slowest drawing
            //though. Alpha is handled more correctly also?

            if (colorKey.IsEmpty && opacity == byte.MaxValue) //No alpha changes?
            {
                return cloneCopy(bmpSrc, rc, format);
            }

            float af = opacity / (float)(byte.MaxValue);
            Bitmap copy;
            if (format.IsIndexed()) //Change alpha in palette colors.
            {
                //Not sure if color key makes sense for indexed format? It must match the color
                //in the palette and not in the source bitmap. Hard to know what source colors
                //get converted to and which colors are in the palette?
                int ck = colorKey.IsEmpty ? -1 : colorKey.ToArgb() & 0xFFFFFF;
                copy = cloneCopy(bmpSrc, rc, format);
                ColorPalette palette = copy.Palette;
                Color[] colors = palette.Entries;
                for (int i = 0; i < colors.Length; i++)
                {
                    Color v = colors[i];
                    int c = v.ToArgb() & 0xFFFFFF; //Value without alpha.
                    //Set alpha if specified. 0 if color matches key.
                    int a = c == ck ? 0 : (int)(v.A * af + 0.5f);
                    colors[i] = Color.FromArgb(a, v);
                }
                copy.Palette = palette;
            }
            else if (format == PixelFormat.Format16bppArgb1555 ||
                format == PixelFormat.Format32bppArgb || format == PixelFormat.Format32bppPArgb)
            {
                copy = new Bitmap(rc.Width, rc.Height, format);
                copy.SetDpi(bmpSrc);
                //It's easier and faster to work with formats without premultiplied alpha.
                BitmapData dataSrc = bmpSrc.LockBits(rc, ImageLockMode.ReadOnly,
                    format == PixelFormat.Format32bppPArgb ? PixelFormat.Format32bppArgb : format); //Converts format.
                BitmapData dataDst = copy.LockBits(copy.GetRectangle(), ImageLockMode.WriteOnly, dataSrc.PixelFormat);
                long srcInd = dataSrc.Scan0.ToInt64(), dstInd = dataDst.Scan0.ToInt64();
                long strideSrc = dataSrc.Stride, strideDst = dataDst.Stride;
                //Copy lines is a bit faster than one full copy. Because of smaller buffer allocated?
                if (dataSrc.PixelFormat == PixelFormat.Format16bppArgb1555)
                {
                    int ck = colorKey.IsEmpty ? -1 : colorKey.ToFormat16bppArgb1555() & 0x7FFF;
                    //int am = ((opacity << 8) & 0x8000) | 0x7FFF; //Alpha bit mask.
                    int am = opacity > byte.MaxValue / 2 ? 0xFFFF : 0x7FFF; //Alpha bit mask.
                    short[] buffer = new short[rc.Width];
                    for (int y = 0; y < rc.Height; y++, srcInd += strideSrc, dstInd += strideDst)
                    {
                        System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcInd, buffer, 0, buffer.Length);
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            int v = buffer[i];
                            int c = v & 0x7FFF; //Value without alpha. c == v if alpha already is 0.
                            buffer[i] = (short)(c == v || c == ck ? c : v & am);
                        }
                        System.Runtime.InteropServices.Marshal.Copy(buffer, 0, (IntPtr)dstInd, buffer.Length);
                    }
                }
                else //PixelFormat.Format32bppArgb
                {
                    int ck = colorKey.IsEmpty ? -1 : colorKey.ToArgb() & 0xFFFFFF;
                    int[] buffer = new int[rc.Width];
                    bool noAf = opacity == byte.MaxValue; //No need to recalculate alpha?
                    for (int y = 0; y < rc.Height; y++, srcInd += strideSrc, dstInd += strideDst)
                    {
                        System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcInd, buffer, 0, buffer.Length);
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            uint v = (uint)buffer[i];
                            uint c = v & 0xFFFFFF; //Value without alpha. c == v if alpha already is 0.
                            buffer[i] = (int)(c == v || c == ck ? c //Alpha is 0?
                                : (noAf ? v : c | ((uint)((v >> 24) * af + 0.5f) << 24))); //Recalculate alpha?

                        }
                        System.Runtime.InteropServices.Marshal.Copy(buffer, 0, (IntPtr)dstInd, buffer.Length);
                    }
                }
                bmpSrc.UnlockBits(dataSrc);
                copy.UnlockBits(dataDst);
            }
            else if (format == PixelFormat.Format64bppArgb || format == PixelFormat.Format64bppPArgb)
            {
                throw new NotSupportedException(); //TODO: Add support? How to pass in 64bit color key and opacity?
            }
            else //Format has no alpha channel.
            {
                //TODO: Error or ignore and just return a copy?
                //throw new ArgumentException("Cannot apply color key or opacity to format with no alpha channel!");
                copy = cloneCopy(bmpSrc, rc, format);
            }
            return copy;
        }

        public static Bitmap openImage(string filepath) //Open file without keeping it locked afterwards.
        {
            //Testing some ways to open a file without locking it. Keep it here mostly for future references?

            //Tested on two 24bit png files: 1344*1632 (1 027 516 Bytes), 6392*4792 (14 637 472 Bytes).
            //-Clone and new is fastest, but keeps the file locked.
            //-Memory stream is also fast, but uses more memory (size of the file). The stream is kept
            // alive along the bitmap so should only be used if higher memory usage is not an issue.
            //-BitmapDirect is interesting, but I'm not sure it is 100% correct, and harder to use
            // because you need(?) to keep a ref to it so its pinned buffer can be disposed later.
            // Just disposing the bitmap ref from it is not enough.
            //-Flip bitmap twice is a cute trick. Slower than copy, but simple.
            //-My copy extension method seems to be the best overall for now. Not much slower than
            // memory stream or BitmapDirect and without their drawbacks.

            //return new Bitmap(filepath); //Locked! time = 00:00:00.0261523, time = 00:00:00.2934980

            //byte[] buffer = File.ReadAllBytes(filepath);
            //MemoryStream stream = new MemoryStream(buffer); //Stream i.e. buffer is kept alive. Dispose/Close does nothing?
            //return new Bitmap(stream); //time = 00:00:00.0271109, time = 00:00:00.3123441

            //Bitmap copy = new Bitmap(filepath);
            //copy.RotateFlip(RotateFlipType.RotateNoneFlipXY); //Releases lock.
            //copy.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            //return copy; //time = 00:00:00.0430107, time = 00:00:00.4826100

            using (Bitmap bmpFile = new Bitmap(filepath))
            {
                //return new Bitmap(bmpFile); //time = 00:00:00.0556160, time = 00:00:00.6888009

                //return (Bitmap)bmpFile.Clone(); //Locked! time = 00:00:00.0262075, time = 00:00:00.2905568

                return bmpFile.Copy(); //time = 00:00:00.0353598, time = 00:00:00.3934300

                //return drawCopy(bmpFile, bmpFile.GetRectangle(), bmpFile.PixelFormat, null); //time = 00:00:00.0439738, time = 00:00:00.5240042

                //return new BitmapDirect(bmpFile).Bitmap; //time = 00:00:00.0338588, time = 00:00:00.3811015
            }
        }

        public static byte[] getPixels(this Bitmap bmp)
        {
            //Returns bitmap's pixels as a byte array.
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int bitsPerPixel = Bitmap.GetPixelFormatSize(bmpData.PixelFormat);
            int bmpStride = bmpData.Stride;
            int arrStride = ((bmpData.Width * bitsPerPixel) + 7) / 8; //Round up.
            byte[] pixels = new byte[arrStride * bmpData.Height];
            long bmpInd = bmpData.Scan0.ToInt64();
            int arrInd = 0;
            if (bmpStride == arrStride) //Can read all pixels at once if same stride.
            {
                System.Runtime.InteropServices.Marshal.Copy((IntPtr)bmpInd, pixels, arrInd, pixels.Length);
            }
            else //Read per line if different stride.
            {
                for (int y = 0; y < bmpData.Height; y++, bmpInd += bmpStride, arrInd += arrStride)
                {
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)bmpInd, pixels, arrInd, arrStride);
                }
            }
            bmp.UnlockBits(bmpData);
            return pixels;
        }

        public static void SetDpi(this Bitmap bmp, Bitmap source)
        {
            bmp.SetResolution(source.HorizontalResolution, source.VerticalResolution);
        }

        public static bool IsIndexed(this PixelFormat format)
        {
            return (format & PixelFormat.Indexed) != 0;
        }

        public static bool HasAlpha(this PixelFormat format)
        {
            return (format & (PixelFormat.Alpha | PixelFormat.PAlpha)) != 0;
        }

        public static int BitsPerPixel(this PixelFormat format)
        {
            return Bitmap.GetPixelFormatSize(format);
        }

        public static ImageFormat GetImageFormat(this Image image)
        {
            ImageFormat imageFormat = image.RawFormat;
            if (imageFormat.Equals(ImageFormat.Bmp)) return ImageFormat.Bmp;
            if (imageFormat.Equals(ImageFormat.Emf)) return ImageFormat.Emf;
            if (imageFormat.Equals(ImageFormat.Exif)) return ImageFormat.Exif;
            if (imageFormat.Equals(ImageFormat.Gif)) return ImageFormat.Gif;
            if (imageFormat.Equals(ImageFormat.Icon)) return ImageFormat.Icon;
            if (imageFormat.Equals(ImageFormat.Jpeg)) return ImageFormat.Jpeg;
            if (imageFormat.Equals(ImageFormat.Png)) return ImageFormat.Png;
            if (imageFormat.Equals(ImageFormat.MemoryBmp)) return ImageFormat.MemoryBmp;
            if (imageFormat.Equals(ImageFormat.Tiff)) return ImageFormat.Tiff;
            if (imageFormat.Equals(ImageFormat.Wmf)) return ImageFormat.Wmf;
            throw new NotImplementedException();
        }

        public static bool IsImageFormat(this Image image, ImageFormat imageFormat)
        {
            return image.RawFormat.Equals(imageFormat);
        }

        public static bool IsFlags(this Image image, ImageFlags flags)
        {
            return (image.Flags & (int)flags) != 0;
        }

        public static Pen createPen(Color color, float[] dashPattern)
        {
            Pen pen = new Pen(color);
            if (dashPattern != null)
            {
                pen.DashPattern = dashPattern;
            }
            return pen;
        }

        public static TextureBrush createDashedBrush(Color color1, Color color2, int dashWidth, bool isHorizontal)
        {
            Bitmap bmp = isHorizontal ? new Bitmap(dashWidth * 2, 1) : new Bitmap(1, dashWidth * 2);
            using (SolidBrush brush1 = new SolidBrush(color1), brush2 = new SolidBrush(color2))
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                if (isHorizontal)
                {
                    gr.FillRectangle(brush1, 0, 0, dashWidth, 1);
                    gr.FillRectangle(brush2, dashWidth, 0, dashWidth, 1);
                }
                else
                {
                    gr.FillRectangle(brush1, 0, 0, 1, dashWidth);
                    gr.FillRectangle(brush2, 0, dashWidth, 1, dashWidth);
                }
            }
            return new TextureBrush(bmp, System.Drawing.Drawing2D.WrapMode.Tile);
        }

        public static TextureBrush createCheckeredBrush(Color color1, Color color2, int squareWidth)
        {
            Bitmap bmp = new Bitmap(squareWidth * 2, squareWidth * 2);
            using (SolidBrush brush1 = new SolidBrush(color1), brush2 = new SolidBrush(color2))
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.FillRectangle(brush1, 0, 0, squareWidth, squareWidth);
                gr.FillRectangle(brush1, squareWidth, squareWidth, squareWidth, squareWidth);
                gr.FillRectangle(brush2, squareWidth, 0, squareWidth, squareWidth);
                gr.FillRectangle(brush2, 0, squareWidth, squareWidth, squareWidth);
            }
            return new TextureBrush(bmp, System.Drawing.Drawing2D.WrapMode.Tile);
        }

        public static TextureBrush createDiagonalBrush(Color color1, Color color2, int lineWidth)
        {
            Bitmap bmp = new Bitmap(lineWidth * 2, lineWidth * 2);
            using (SolidBrush brush1 = new SolidBrush(color1), brush2 = new SolidBrush(color2))
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0 - y; x < bmp.Width; x += 2 * lineWidth) //Color 1 in row y.
                    {
                        gr.FillRectangle(brush1, x, y, lineWidth, 1); //Outside fill will be clipped.
                    }
                    for (int x = lineWidth - y; x < bmp.Width; x += 2 * lineWidth) //Color 2 in row y.
                    {
                        gr.FillRectangle(brush2, x, y, lineWidth, 1); //Outside fill will be clipped.
                    }
                }
            }
            return new TextureBrush(bmp, System.Drawing.Drawing2D.WrapMode.Tile);
        }

        public static void DrawLineHor(this Graphics gr, Brush brush, int x, int y, int width)
        {
            gr.DrawLineHor(brush, x, y, width, 1);
        }

        public static void DrawLineHor(this Graphics gr, Brush brush, int x, int y, int width, int lineWidth)
        {
            gr.FillRectangle(brush, x, y, width, lineWidth);
        }

        public static void DrawLineVer(this Graphics gr, Brush brush, int x, int y, int height)
        {
            gr.DrawLineVer(brush, x, y, height, 1);
        }

        public static void DrawLineVer(this Graphics gr, Brush brush, int x, int y, int height, int lineWidth)
        {
            gr.FillRectangle(brush, x, y, lineWidth, height);
        }

        public static void DrawRectangleFast(this Graphics gr, Brush brush, Rectangle rc)
        {
            gr.DrawRectangleFast(brush, rc, 1);
        }

        public static void DrawRectangleFast(this Graphics gr, Brush brush, Rectangle rc, int lineWidth)
        {
            gr.DrawRectangleFast(brush, brush, rc, lineWidth);
        }

        public static void DrawRectangleFast(this Graphics gr, Brush brushHor, Brush brushVer, Rectangle rc)
        {
            gr.DrawRectangleFast(brushHor, brushVer, rc, 1);
        }

        public static void DrawRectangleFast(this Graphics gr, Brush brushHor, Brush brushVer, Rectangle rc, int lineWidth)
        {
            if (rc.Width > 0 && rc.Height > 0)
            {
                //Faster than Graphics.DrawRectangle in my tests at least.
                gr.DrawLineHor(brushHor, rc.X, rc.Y, rc.Width, lineWidth);
                gr.DrawLineHor(brushHor, rc.X, rc.Y + rc.Height - lineWidth, rc.Width, lineWidth);
                gr.DrawLineVer(brushVer, rc.X, rc.Y + lineWidth, rc.Height - (2 * lineWidth), lineWidth);
                gr.DrawLineVer(brushVer, rc.X + rc.Width - lineWidth, rc.Y + lineWidth, rc.Height - (2 * lineWidth), lineWidth);
            }
        }

        public static void DrawRectangleFast(this Graphics gr, Brush brush, Rectangle rc, int lineWidth,
            bool doTop, bool doBot, bool doLef, bool doRig)
        {
            gr.DrawRectangleFast(brush, brush, rc, lineWidth, doTop, doBot, doLef, doRig);
        }

        public static void DrawRectangleFast(this Graphics gr, Brush brushHor, Brush brushVer, Rectangle rc, int lineWidth,
            bool doTop, bool doBot, bool doLef, bool doRig)
        {
            if (rc.Width > 0 && rc.Height > 0)
            {
                //Faster than Graphics.DrawRectangle in my tests at least.
                if (doTop) gr.DrawLineHor(brushHor, rc.X, rc.Y, rc.Width, lineWidth);
                if (doBot) gr.DrawLineHor(brushHor, rc.X, rc.Y + rc.Height - lineWidth, rc.Width, lineWidth);
                if (doLef) gr.DrawLineVer(brushVer, rc.X, rc.Y + lineWidth, rc.Height - (2 * lineWidth), lineWidth);
                if (doRig) gr.DrawLineVer(brushVer, rc.X + rc.Width - lineWidth, rc.Y + lineWidth, rc.Height - (2 * lineWidth), lineWidth);
            }
        }

        public static void DrawRectangleDashed(this Graphics gr, SolidBrush brush, Rectangle rc, int lineWidth, int dashWidth)
        {
            int lineWidthDec = 1 - lineWidth; //A common value used in the loops below.

            List<Rectangle> rcs = new List<Rectangle>();
            int i = 0;
            for (int x = lineWidthDec; x < (rc.Width - lineWidthDec); x++, i++)
            {
                if ((i & dashWidth) != dashWidth) rcs.Add(new Rectangle(x, lineWidthDec, 1, lineWidth));
            }
            i--;
            for (int y = lineWidthDec; y < (rc.Height - lineWidthDec); y++, i++)
            {
                if ((i & dashWidth) != dashWidth) rcs.Add(new Rectangle(rc.Width - 1, y, lineWidth, 1));
            }
            i--;
            for (int x = rc.Width - lineWidthDec - 1; x >= lineWidthDec; x--, i++)
            {
                if ((i & dashWidth) != dashWidth) rcs.Add(new Rectangle(x, rc.Height - 1, 1, lineWidth));
            }
            i--;
            for (int y = rc.Height - lineWidthDec - 1; y >= lineWidthDec; y--, i++)
            {
                if ((i & dashWidth) != dashWidth) rcs.Add(new Rectangle(lineWidthDec, y, lineWidth, 1));
            }

            //Using fill rectangles once seems to be a bit faster than calling fill rectangle many times.
            System.Drawing.Drawing2D.Matrix m = gr.Transform;
            gr.TranslateTransform(rc.X, rc.Y);
            gr.FillRectangles(brush, rcs.ToArray());
            gr.Transform = m;
        }

        public static void DrawCross(this Graphics gr, Pen pen, Rectangle rc)
        {
            //Draws an 'X' inside specified rectangle.
            gr.DrawLine(pen, rc.Left, rc.Top, rc.Right - 1, rc.Bottom - 1);
            gr.DrawLine(pen, rc.Left, rc.Bottom - 1, rc.Right - 1, rc.Top);
        }

        public static void DrawImageAttr(this Graphics gr, Image image,
            int x, int y, Rectangle src, GraphicsUnit unit, ImageAttributes attr)
        {
            gr.DrawImageAttr(image, new Rectangle(x, y, src.Width, src.Height), src, unit, attr);
        }

        public static void DrawImageAttr(this Graphics gr, Image image,
            Rectangle dst, Rectangle src, GraphicsUnit unit, ImageAttributes attr)
        {
            gr.DrawImage(image, dst, src.X, src.Y, src.Width, src.Height, unit, attr);
        }

        public static void DrawGrid(this Graphics gr, Brush brush, Rectangle rc, Size cellSize)
        {
            for (int gy = rc.Y; gy < rc.Height; gy += cellSize.Height)
            {
                gr.DrawLineHor(brush, rc.X, gy, rc.Width, 1);
            }
            for (int gx = rc.X; gx < rc.Width; gx += cellSize.Width)
            {
                gr.DrawLineVer(brush, gx, rc.Y, rc.Height, 1);
            }
        }

        public static void DrawGrid(this Graphics gr, Brush brush, Rectangle rc, SizeF cellSize)
        {
            for (float gy = rc.Y; gy < rc.Height; gy += cellSize.Height)
            {
                gr.DrawLineHor(brush, rc.X, (int)gy, rc.Width, 1);
            }
            for (float gx = rc.X; gx < rc.Width; gx += cellSize.Width)
            {
                gr.DrawLineVer(brush, (int)gx, rc.Y, rc.Height, 1);
            }
        }

        public static void DrawGrid(this Graphics gr, Pen pen, Rectangle rc, Size cellSize)
        {
            for (int gy = rc.Y; gy < rc.Height; gy += cellSize.Height)
            {
                gr.DrawLine(pen, rc.X, gy, rc.Right, gy);
            }
            for (int gx = rc.X; gx < rc.Width; gx += cellSize.Width)
            {
                gr.DrawLine(pen, gx, rc.Y, gx, rc.Bottom);
            }
        }

        public static void DrawGrid(this Graphics gr, Pen pen, Rectangle rc, SizeF cellSize)
        {
            for (float gy = rc.Y; gy < rc.Height; gy += cellSize.Height)
            {
                gr.DrawLine(pen, rc.X, gy, rc.Right, gy);
            }
            for (float gx = rc.X; gx < rc.Width; gx += cellSize.Width)
            {
                gr.DrawLine(pen, gx, rc.Y, gx, rc.Bottom);
            }
        }

        public static Color RemoveAlpha(this Color color)
        {
            return color.IsEmpty ? color : Color.FromArgb(color.R, color.G, color.B);
        }

        public static Color ToPremultipliedAlpha(this Color color)
        {
            return color.ToPremultipliedAlpha(color.A / 255.0f);
        }

        public static Color ToPremultipliedAlpha(this Color color, float alphaFactor) //Alpha factor 0-1.
        {
            //Sets a new alpha value in color based on alpha factor parameter.
            return Color.FromArgb(
                (int)(byte.MaxValue * alphaFactor + 0.5f),
                (int)(color.R * alphaFactor + 0.5f),
                (int)(color.G * alphaFactor + 0.5f),
                (int)(color.B * alphaFactor + 0.5f));
        }

        public static Color ToNonPremultipliedAlpha(this Color color)
        {
            float alphaFactor = 255.0f / color.A;
            return Color.FromArgb(
                (int)(color.A * alphaFactor + 0.5f),
                (int)(color.R * alphaFactor + 0.5f),
                (int)(color.G * alphaFactor + 0.5f),
                (int)(color.B * alphaFactor + 0.5f));
        }

        public static int ToPargb(this Color color)
        {
            return color.ToPremultipliedAlpha().ToArgb();
        }

        public static int ToPargb(this Color color, float alphaFactor) //Alpha factor 0-1.
        {
            return color.ToPremultipliedAlpha(alphaFactor).ToArgb();
        }

        public static string ToRgbHex(this Color color)
        {
            return color.IsEmpty ? string.Empty : (color.ToArgb() & 0xFFFFFF).ToString("X6");
        }

        public static string ToArgbHex(this Color color)
        {
            return color.IsEmpty ? string.Empty : (color.ToArgb() & 0xFFFFFFFF).ToString("X8");
        }

        public static ushort ToFormat16bppArgb1555(this Color color)
        {
            uint c = (uint)color.ToArgb();
            return (ushort)(((c >> 16) & 0x8000) | ((c >> 9) & 0x7C00) | ((c >> 6) & 0x03E0) | ((c >> 3) & 0x001F));
        }

        public static Color Invert(this Color color)
        {
            return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
        }

        public static bool compareBitmaps(Rectangle rc, Bitmap bmp1, Bitmap bmp2)
        {
            string dummy;
            return compareBitmaps(rc, bmp1, bmp2, out dummy);
        }

        public static bool compareBitmaps(Rectangle rc, Bitmap bmp1, Bitmap bmp2, out string msg)
        {
            if (bmp1.Size != bmp2.Size || bmp1.PixelFormat != bmp2.PixelFormat)
            {
                msg = "Bitmaps have different size or pixelformat.";
                return false;
            }

            if (!bmp1.GetRectangle().Contains(rc))
            {
                throw new ArgumentException("Rectangle is outside bitmap!");
            }

            BitmapData data1 = bmp1.LockBits(rc, ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData data2 = bmp2.LockBits(rc, ImageLockMode.ReadOnly, bmp2.PixelFormat);
            try
            {
                //int bytesPerRow = Math.Min(data1.Stride, data2.Stride); //Stride can contain extra unequal garbage data?
                int bytesPerRow = ((rc.Width * bmp1.PixelFormat.BitsPerPixel()) + 7) / 8; //Round up.

                byte[] buffer1 = new byte[bytesPerRow], buffer2 = new byte[bytesPerRow];
                long ind1 = data1.Scan0.ToInt64(), ind2 = data2.Scan0.ToInt64();
                long stride1 = data1.Stride, stride2 = data2.Stride;
                for (int y = 0; y < rc.Height; y++, ind1 += stride1, ind2 += stride2)
                {
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)ind1, buffer1, 0, bytesPerRow);
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)ind2, buffer2, 0, bytesPerRow);
                    for (int i = 0; i < bytesPerRow; i++)
                    {
                        if (buffer1[i] != buffer2[i])
                        {
                            msg = "Bitmaps have different pixel values.";
                            return false;
                        }
                    }
                }
            }
            finally
            {
                bmp1.UnlockBits(data1);
                bmp2.UnlockBits(data2);
            }
            msg = "Bitmaps have equal pixel values.";
            return true;
        }

        public static PixelFormat getFormsPixelFormat(IntPtr handle)
        {
            //Used in form like MiscDrawing.GetFormsPixelFormat(this.Handle);
            using (Bitmap bmp = new Bitmap(8, 8, Graphics.FromHwnd(handle)))
            {
                return bmp.PixelFormat;
            }
        }
    }
}
