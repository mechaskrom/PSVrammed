using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    //Struct for working with playstation 16bit argb colors.
    //FEDCBA98 76543210 bit
    //abbbbbgg gggrrrrr Playstation 16bit argb.
    struct PsColor16Bit
    {
        private ushort mAbgr;

        public PsColor16Bit(ushort abgr)
        {
            mAbgr = abgr;
        }

        public static implicit operator ushort(PsColor16Bit color)
        {
            return color.mAbgr;
        }

        public byte R //0-31.
        {
            get { return (byte)(mAbgr & 0x1F); }
            set { mAbgr = (ushort)((mAbgr & 0xFFE0) | (value & 0x1F)); }
        }

        public byte G //0-31.
        {
            get { return (byte)((mAbgr >> 5) & 0x1F); }
            set { mAbgr = (ushort)((mAbgr & 0xFC1F) | ((value & 0x1F) << 5)); }
        }

        public byte B //0-31.
        {
            get { return (byte)((mAbgr >> 10) & 0x1F); }
            set { mAbgr = (ushort)((mAbgr & 0x83FF) | ((value & 0x1F) << 10)); }
        }

        public byte A //0-1.
        {
            get { return (byte)((mAbgr >> 15) & 0x01); }
            set { mAbgr = (ushort)((mAbgr & 0x7FFF) | ((value & 0x01) << 15)); }
        }

        public ushort Abgr
        {
            get { return mAbgr; }
            set { mAbgr = value; }
        }

        public Color Color
        {
            get { return Color.FromArgb(toFormat32bppArgb(mAbgr)); }
            set { mAbgr = fromColor(value); }
        }

        public ushort toFormat16bppRgb555()
        {
            return toFormat16bppRgb555(mAbgr);
        }

        public int toFormat24bppRgb()
        {
            return toFormat24bppRgb(mAbgr);
        }

        public int toFormat32bppArgb()
        {
            return toFormat32bppArgb(mAbgr);
        }

        public string toString()
        {
            return "ARGB=" + toStringShort();
        }

        public string toStringShort()
        {
            return String.Format("{0},{1:D2},{2:D2},{3:D2}", A, R, G, B);
        }

        public static ushort toFormat16bppRgb555(ushort abgr)
        {
            //Get playstation 16 bit color as PixelFormat.Format16bppRgb555.
            //Swap red and blue channels. Ignore alpha.
            return (ushort)(((abgr >> 10) & 0x1F) | ((abgr & 0x1F) << 10) | (abgr & 0x03E0));
        }

        public static int toFormat24bppRgb(ushort abgr)
        {
            //Get playstation 16 bit color as PixelFormat.Format24bppRgb.
            //Swap red and blue channels. Ignore alpha.
            //Extract 5 bit color channels from 16 bit ps color and "expand" them to 8 bit.
            //This matches what PixelFormat.Format16bppRgb555 gives as 8 bit values.
            int r = (abgr << 3) & 0xF8;
            int g = (abgr >> 2) & 0xF8;
            int b = (abgr >> 7) & 0xF8;
            r |= r >> 5;
            g |= g >> 5;
            b |= b >> 5;
            return (r << 16) | (g << 8) | b;
        }

        public static int toFormat32bppArgb(ushort abgr)
        {
            //Get playstation 16 bit color as PixelFormat.Format32bppArgb.
            return (byte.MaxValue << 24) | toFormat24bppRgb(abgr); //Max alpha (opaque).
        }

        public static PsColor16Bit fromRgb555(byte r, byte g, byte b)
        {
            return fromArgb1555(0, r, g, b);
        }

        public static PsColor16Bit fromArgb1555(byte a, byte r, byte g, byte b)
        {
            return new PsColor16Bit((ushort)(
                ((r & 0x1F)) |
                ((g & 0x1F) << 5) |
                ((b & 0x1F) << 10) |
                ((a & 0x01) << 15)));
        }

        public static PsColor16Bit fromRgb888(byte r, byte g, byte b)
        {
            return fromRgb555((byte)(r >> 3), (byte)(g >> 3), (byte)(b >> 3));
        }

        public static PsColor16Bit fromColor(Color color) //Alpha is ignored.
        {
            return fromRgb888(color.R, color.G, color.B);
        }
    }

    //***********************************************************************************
    //***********************************************************************************

    //Struct for working with playstation 24bit rgb colors.
    //bbbbbbbb gggggggg rrrrrrrr Playstation 24bit rgb. 3 bytes.
    struct PsColor24Bit
    {
        private int mBgr;

        public PsColor24Bit(int bgr)
        {
            mBgr = bgr;
        }

        public static implicit operator int(PsColor24Bit color)
        {
            return color.mBgr;
        }

        public byte R
        {
            get { return (byte)(mBgr >> 0); }
            set { mBgr = (mBgr & 0xFFFF00) | (value << 0); }
        }

        public byte G
        {
            get { return (byte)(mBgr >> 8); }
            set { mBgr = (mBgr & 0xFF00FF) | (value << 8); }
        }

        public byte B
        {
            get { return (byte)(mBgr >> 16); }
            set { mBgr = (mBgr & 0x00FFFF) | (value << 16); }
        }

        public int Bgr
        {
            get { return mBgr; }
            set { mBgr = value; }
        }

        public Color Color
        {
            get { return Color.FromArgb(toFormat32bppArgb(mBgr)); }
            set { mBgr = fromColor(value); }
        }

        public int toFormat24bppRgb()
        {
            return toFormat24bppRgb(mBgr);
        }

        public int toFormat32bppArgb()
        {
            return toFormat32bppArgb(mBgr);
        }

        public string toString()
        {
            return String.Format("RGB={0:D3},{1:D3},{2:D3}", R, G, B);
        }

        public static int toFormat24bppRgb(int bgr)
        {
            //Get playstation 24 bit color as PixelFormat.Format24bppRgb.
            //Swap red and blue channels. Ignore alpha.
            return ((bgr & 0x0000FF) << 16) | (bgr & 0x00FF00) | ((bgr & 0xFF0000) >> 16);
        }

        public static int toFormat32bppArgb(int bgr)
        {
            //Get playstation 24 bit color as PixelFormat.Format32bppArgb.
            return (byte.MaxValue << 24) | toFormat24bppRgb(bgr); //Max alpha (opaque).
        }

        public static PsColor24Bit fromRgb888(byte r, byte g, byte b)
        {
            return new PsColor24Bit((b << 16) | (g << 8) | r);
        }

        public static PsColor24Bit fromColor(Color color) //Alpha is ignored.
        {
            return fromRgb888(color.R, color.G, color.B);
        }
    }
}
