using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdValue24Bit : EditCmdValue
        {
            public const string SaveId = "EditCmdValue24Bit";

            public EditCmdValue24Bit(byte[] vram, Rectangle rcPixels, int value, int mask)
                : base(vram, rcPixels, BitDepth.Rgb24Bit, value, mask)
            {
            }

            protected override void redoInner()
            {
                Rectangle rcPixels = mRcPixels;
                byte[] vram = mVram;
                int vramPtr = getVramPointer24Bit(rcPixels.Location);
                byte value0, value1, value2;
                toBytes(mValue & ~mMask, out value0, out value1, out value2);
                byte mask0, mask1, mask2;
                toBytes(mMask, out mask0, out mask1, out mask2);
                for (int y = rcPixels.Height; y > 0; y--, vramPtr += VramStride)
                {
                    for (int x = rcPixels.Width, vp = vramPtr; x > 0; x--, vp += 3)
                    {
                        vram[vp + 0] = (byte)(value0 | (vram[vp + 0] & mask0));
                        vram[vp + 1] = (byte)(value1 | (vram[vp + 1] & mask1));
                        vram[vp + 2] = (byte)(value2 | (vram[vp + 2] & mask2));
                    }
                }
            }

            private static void toBytes(int i, out byte b0, out byte b1, out byte b2)
            {
                b0 = (byte)(i >> 0);
                b1 = (byte)(i >> 8);
                b2 = (byte)(i >> 16);
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                return (prevEditCmd is EditCmdValue24Bit) && base.isRepeat(prevEditCmd);
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mRcPixels, mValue, mMask);
            }

            public static EditCmdValue24Bit openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdValue24Bit(vramMngr.mVram, fr.readRc(), fr.readInt(), fr.readInt());
            }
        }
    }
}
