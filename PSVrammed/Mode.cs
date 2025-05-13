using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyCustomStuff;

namespace PSVrammed
{
    enum Mode
    {
        Ind4BitView,
        Ind4BitComp,
        Ind4BitPalE,
        Ind8BitView,
        Ind8BitComp,
        Ind8BitPalE,
        Rgb16BitView,
        Rgb24BitView,
    }

    static class ModeExt
    {
        public static bool isView(this Mode mode)
        {
            return
                mode == Mode.Ind4BitView ||
                mode == Mode.Ind8BitView ||
                mode == Mode.Rgb16BitView ||
                mode == Mode.Rgb24BitView;
        }

        public static bool isCompare(this Mode mode)
        {
            return mode == Mode.Ind4BitComp || mode == Mode.Ind8BitComp;
        }

        public static bool isPaletteEditor(this Mode mode)
        {
            return mode == Mode.Ind4BitPalE || mode == Mode.Ind8BitPalE;
        }

        public static bool usesPalette(this Mode mode)
        {
            return
                mode == Mode.Ind4BitView ||
                mode == Mode.Ind4BitComp ||
                mode == Mode.Ind4BitPalE ||
                mode == Mode.Ind8BitView ||
                mode == Mode.Ind8BitComp ||
                mode == Mode.Ind8BitPalE;
        }

        public static string toDescr(this Mode mode)
        {
            if (mode == Mode.Ind4BitView) return Strings.Mode4BitView;
            if (mode == Mode.Ind4BitComp) return Strings.Mode4BitComp;
            if (mode == Mode.Ind4BitPalE) return Strings.Mode4BitPalE;
            if (mode == Mode.Ind8BitView) return Strings.Mode8BitView;
            if (mode == Mode.Ind8BitComp) return Strings.Mode8BitComp;
            if (mode == Mode.Ind8BitPalE) return Strings.Mode8BitPalE;
            if (mode == Mode.Rgb16BitView) return Strings.Mode16BitView;
            if (mode == Mode.Rgb24BitView) return Strings.Mode24BitView;
            throw new NotImplementedException();
        }

        public static int toBitsPerPixel(this Mode mode)
        {
            if (mode == Mode.Ind4BitView ||
                mode == Mode.Ind4BitComp ||
                mode == Mode.Ind4BitPalE) return 4;
            if (mode == Mode.Ind8BitView ||
                mode == Mode.Ind8BitComp ||
                mode == Mode.Ind8BitPalE) return 8;
            if (mode == Mode.Rgb16BitView) return 16;
            if (mode == Mode.Rgb24BitView) return 24;
            throw new NotImplementedException();
        }

        public static VramMngr.BitDepth toVramBitDepth(this Mode mode)
        {
            if (mode == Mode.Ind4BitView ||
                mode == Mode.Ind4BitComp ||
                mode == Mode.Ind4BitPalE) return VramMngr.BitDepth.Ind4Bit;
            if (mode == Mode.Ind8BitView ||
                mode == Mode.Ind8BitComp ||
                mode == Mode.Ind8BitPalE) return VramMngr.BitDepth.Ind8Bit;
            if (mode == Mode.Rgb16BitView) return VramMngr.BitDepth.Rgb16Bit;
            if (mode == Mode.Rgb24BitView) return VramMngr.BitDepth.Rgb24Bit;
            throw new NotImplementedException();
        }

        public static Mode toMode(this string s, Mode def) //Returns def value if value is invalid.
        {
            try
            {
                return (Mode)(Enum.Parse(typeof(Mode), s));
            }
            catch
            {
                return def;
            }
        }

        public static Mode toMode(this int i, Mode def) //Returns def value if value is invalid.
        {
            if (i < (int)Mode.Ind4BitView || i > (int)Mode.Rgb24BitView) return def;
            return (Mode)i;
        }
    }
}
