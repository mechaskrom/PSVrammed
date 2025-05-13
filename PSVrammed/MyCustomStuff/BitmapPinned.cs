using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MyCustomStuff
{
    //Special bitmap with direct fast access to its pixels.
    public abstract class BitmapPinnedBase : IDisposable
    {
        protected readonly Rectangle mBounds;
        protected readonly int mBitsPerPixel;
        protected readonly int mStrideBytes;
        protected readonly int mStridePixels;
        protected Bitmap mBitmap;
        protected GCHandle mPixelsHandle;
        protected bool mIsDisposed;

        protected BitmapPinnedBase(int width, int height, PixelFormat format)
        {
            mBounds = new Rectangle(0, 0, width, height);
            mBitsPerPixel = Bitmap.GetPixelFormatSize(format);
            mStrideBytes = getStrideBytes(width, mBitsPerPixel);
            mStridePixels = (mStrideBytes * 8) / mBitsPerPixel;
            System.Diagnostics.Debug.Assert(((mStridePixels * mBitsPerPixel) / 8) == mStrideBytes, "Stride error!");
        }

        public Rectangle Bounds { get { return mBounds; } }
        public Size Size { get { return mBounds.Size; } }
        public int Width { get { return mBounds.Width; } }
        public int Height { get { return mBounds.Height; } }
        public int StrideBytes { get { return mStrideBytes; } } //Bytes per line.
        public int StridePixels { get { return mStridePixels; } } //Pixels per line. Not always same as bitmap width.
        public Bitmap Bitmap { get { return mBitmap; } }
        public PixelFormat Format { get { return mBitmap.PixelFormat; } }
        public bool IsDisposed { get { return mIsDisposed; } }

        public static implicit operator Bitmap(BitmapPinnedBase bmp)
        {
            return bmp.Bitmap;
        }

        public int getBytesIndex(Point p)
        {
            return getBytesIndex(p.X, p.Y);
        }

        public int getBytesIndex(int y)
        {
            return y * mStrideBytes;
        }

        public int getBytesIndex(int x, int y) //Pixel location to byte array index.
        {
            return (y * mStrideBytes) + (x * mBitsPerPixel / 8);
        }

        public int getPixelIndex(Point p)
        {
            return getPixelIndex(p.X, p.Y);
        }

        public int getPixelIndex(int x, int y) //Pixel location to pixel array index.
        {
            return x + (y * mStridePixels);
        }

        public void setPalette(Color[] palette)
        {
            if (!mBitmap.PixelFormat.IsIndexed())
            {
                throw new InvalidOperationException("Can't set palette for a non-indexed bitmap!");
            }

            ColorPalette bmpPalette = mBitmap.Palette; //Returns a copy.
            int entries = bmpPalette.Entries.Length;
            for (int i = 0; i < entries; i++)
            {
                //Set any missing entries to black.
                bmpPalette.Entries[i] = i < palette.Length ? palette[i] : Color.Black;
            }
            mBitmap.Palette = bmpPalette; //Set changed copy as the new palette.
        }

        private static int getStrideBytes(int width, int bitsPerPixel)
        {
            //Stride needs to be rounded up to a multiple of 4 bytes (32 bits)
            //and it also needs to be a multiple of bits per pixel.
            //Figure out least common multiple between bits per pixel and 32
            //and use it to calculate stride (bytes per row).
            int least = LeastCommonMultiple(bitsPerPixel, 32);
            int stride = (((width * bitsPerPixel) + least - 1) / least) * (least / 8);
            return stride;
        }

        private static int LeastCommonMultiple(int va11, int val2)
        {
            return (va11 / GreatestCommonFactor(va11, val2)) * val2;
        }

        private static int GreatestCommonFactor(int va11, int val2)
        {
            while (val2 != 0)
            {
                int temp = val2;
                val2 = va11 % val2;
                va11 = temp;
            }
            return va11;
        }

        public void Dispose()
        {
            if (mIsDisposed) return;
            mIsDisposed = true;
            mBitmap.Dispose();
            mPixelsHandle.Free();
        }
    }

    //Bitmap with custom pixel format and direct fast access to its pixels.
    public class BitmapPinned : BitmapPinnedBase
    {
        private readonly byte[] mBytes;

        public BitmapPinned(int width, int height, PixelFormat format)
            : base(width, height, format)
        {
            mBytes = new byte[mStrideBytes * height];
            mPixelsHandle = GCHandle.Alloc(mBytes, GCHandleType.Pinned);
            mBitmap = new Bitmap(width, height, mStrideBytes, format, mPixelsHandle.AddrOfPinnedObject());
        }

        public BitmapPinned(Bitmap copyFrom)
            : this(copyFrom.Width, copyFrom.Height, copyFrom.PixelFormat)
        {
            copyBmpBytes(copyFrom);

            //Copy palette if present.
            if (copyFrom.PixelFormat.IsIndexed())
            {
                mBitmap.Palette = copyFrom.Palette;
            }
        }

        public byte[] Bytes { get { return mBytes; } }

        private void copyBmpBytes(Bitmap bmp)
        {
            copyBmpBytes1(bmp, mBytes, mStrideBytes);
            //copyBmpBytes2(bmp, mBytes, mStrideBytes, mPixelsHandle.AddrOfPinnedObject());
        }

        private static void copyBmpBytes1(Bitmap bmp, byte[] dst, int dstStride)
        {
            BitmapData bmpData = bmp.LockBits(bmp.GetRectangle(), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int srcStride = bmpData.Stride;
            long srcInd = bmpData.Scan0.ToInt64();
            int dstInd = 0;
            if (srcStride == dstStride) //Can read all pixels at once if same stride.
            {
                System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcInd, dst, dstInd, dst.Length);
            }
            else //Read per line if different stride.
            {
                int copyBytesPerRow = Math.Min(srcStride, dstStride);
                for (int y = bmpData.Height; y > 0; y--, srcInd += srcStride, dstInd += dstStride)
                {
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcInd, dst, dstInd, copyBytesPerRow);
                }
            }
            bmp.UnlockBits(bmpData);
        }

        //TODO: Remove? Test using lock bits to copy bmp bytes. Not faster than marshal copy version above.
        private static void copyBmpBytes2(Bitmap bmp, byte[] dst, int dstStride, IntPtr dstScan0)
        {
            //Manual init bmp data.
            BitmapData bmpData = new BitmapData();
            bmpData.Width = bmp.Width;
            bmpData.Height = bmp.Height;
            bmpData.PixelFormat = bmp.PixelFormat;
            bmpData.Stride = dstStride;
            bmpData.Scan0 = dstScan0;

            //Copy bmp bytes to dst scan0 pointer (as specified by bmp data).
            BitmapData result = bmp.LockBits(bmp.GetRectangle(),
                ImageLockMode.UserInputBuffer | ImageLockMode.ReadOnly,
                bmp.PixelFormat, bmpData);
            bmp.UnlockBits(result);
        }
    }

    //Special bitmap with 32bppPArgb pixel format and direct fast access to its pixels.
    public class BitmapPinned32bppPArgb : BitmapPinnedBase
    {
        private readonly Int32[] mPixels;

        public BitmapPinned32bppPArgb(int width, int height)
            : base(width, height, PixelFormat.Format32bppPArgb)
        {
            mPixels = new Int32[mStridePixels * height];
            mPixelsHandle = GCHandle.Alloc(mPixels, GCHandleType.Pinned);
            mBitmap = new Bitmap(width, height, mStrideBytes, PixelFormat.Format32bppPArgb, mPixelsHandle.AddrOfPinnedObject());
        }

        public Int32[] Pixels { get { return mPixels; } }

        public Int32 this[int index]
        {
            get { return mPixels[index]; }
            set { mPixels[index] = value; }
        }

        public Int32 this[Point pos]
        {
            get { return mPixels[getPixelIndex(pos)]; }
            set { mPixels[getPixelIndex(pos)] = value; }
        }

        public Int32 this[int x, int y]
        {
            get { return mPixels[getPixelIndex(x, y)]; }
            set { mPixels[getPixelIndex(x, y)] = value; }
        }

        public void SetPixel(int x, int y, Color color)
        {
            this[x, y] = color.ToArgb();
        }

        public Color GetPixel(int x, int y)
        {
            return Color.FromArgb(this[x, y]);
        }
    }

    //Special bitmap with 8bppIndexed pixel format and direct fast access to its pixels.
    public class BitmapPinned8bppIndexed : BitmapPinnedBase
    {
        private readonly byte[] mPixels;

        public BitmapPinned8bppIndexed(int width, int height)
            : base(width, height, PixelFormat.Format8bppIndexed)
        {
            mPixels = new byte[mStridePixels * height];
            mPixelsHandle = GCHandle.Alloc(mPixels, GCHandleType.Pinned);
            mBitmap = new Bitmap(width, height, mStrideBytes, PixelFormat.Format8bppIndexed, mPixelsHandle.AddrOfPinnedObject());
        }

        public byte[] Pixels { get { return mPixels; } }

        public byte this[int index]
        {
            get { return mPixels[index]; }
            set { mPixels[index] = value; }
        }

        public byte this[Point pos]
        {
            get { return mPixels[getPixelIndex(pos)]; }
            set { mPixels[getPixelIndex(pos)] = value; }
        }

        public byte this[int x, int y]
        {
            get { return mPixels[getPixelIndex(x, y)]; }
            set { mPixels[getPixelIndex(x, y)] = value; }
        }

        public void SetPixel(int x, int y, byte index)
        {
            this[x, y] = index;
        }

        public byte GetPixel(int x, int y)
        {
            return this[x, y];
        }
    }
}
