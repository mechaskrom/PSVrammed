using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private abstract class EditCmdValue : EditCmdChange
        {
            public const int NoMask = 0;
            public const int NoIgnore = -1;

            protected readonly Rectangle mRcPixels;
            protected readonly BitDepth mBitDepth;
            protected readonly int mValue;
            protected readonly int mMask;
            protected readonly int mIgnore;

            protected EditCmdValue(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value)
                : this(vram, rcPixels, bitDepth, value, NoMask, NoIgnore)
            {
            }

            protected EditCmdValue(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value, int mask)
                : this(vram, rcPixels, bitDepth, value, mask, NoIgnore)
            {
            }

            protected EditCmdValue(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value, int mask, int ignore)
                : base(vram, rcPixels, bitDepth)
            {
                mRcPixels = rcPixels;
                mBitDepth = bitDepth;
                mValue = value;
                mMask = mask;
                mIgnore = ignore;
            }

            public Rectangle RcPixels
            {
                get { return mRcPixels; }
            }

            public BitDepth BitDepth
            {
                get { return mBitDepth; }
            }

            public int Value
            {
                get { return mValue; }
            }

            public int Mask
            {
                get { return mMask; }
            }

            public int Ignore
            {
                get { return mIgnore; }
            }

            protected override void initInner()
            {
                mEcCopy.redo(); //Save old vram.
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                EditCmdValue ec = prevEditCmd as EditCmdValue;
                return ec != null && base.isRepeat(ec) &&
                    mRcPixels == ec.mRcPixels &&
                    mBitDepth == ec.mBitDepth &&
                    mValue == ec.mValue &&
                    mMask == ec.mMask &&
                    mIgnore == ec.mIgnore;
            }

            public static EditCmdValue create(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value)
            {
                return create(vram, rcPixels, bitDepth, value, NoMask, NoIgnore);
            }

            public static EditCmdValue create(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value, int mask)
            {
                return create(vram, rcPixels, bitDepth, value, mask, NoIgnore);
            }

            public static EditCmdValue create(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value, int mask, int ignore)
            {
                if (bitDepth == BitDepth.Ind4Bit) return new EditCmdValue4Bit(vram, rcPixels, value);
                if (bitDepth == BitDepth.Ind8Bit) return new EditCmdValue8Bit(vram, rcPixels, value);
                if (bitDepth == BitDepth.Rgb16Bit) return new EditCmdValue16Bit(vram, rcPixels, value, mask, ignore);
                if (bitDepth == BitDepth.Rgb24Bit) return new EditCmdValue24Bit(vram, rcPixels, value, mask);
                return null;
            }
        }
    }
}
