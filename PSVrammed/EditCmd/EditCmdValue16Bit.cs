using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdValue16Bit : EditCmdValue
        {
            public const string SaveId = "EditCmdValue16Bit";

            public EditCmdValue16Bit(byte[] vram, Rectangle rcPixels, int value, int mask)
                : base(vram, rcPixels, BitDepth.Rgb16Bit, value, mask)
            {
            }

            public EditCmdValue16Bit(byte[] vram, Rectangle rcPixels, int value, int mask, int ignore)
                : base(vram, rcPixels, BitDepth.Rgb16Bit, value, mask, ignore)
            {
            }

            protected override void redoInner()
            {
                Rectangle rcPixels = mRcPixels;
                byte[] vram = mVram;
                int vramPtr = getVramPointer16Bit(rcPixels.Location);
                byte value0, value1;
                toBytes(mValue & ~mMask, out value0, out value1);
                byte mask0, mask1;
                toBytes(mMask, out mask0, out mask1);
                bool doIgnore = mIgnore != NoIgnore;
                byte ignore0, ignore1;
                toBytes(mIgnore, out ignore0, out ignore1);
                for (int y = rcPixels.Height; y > 0; y--, vramPtr += VramStride)
                {
                    for (int x = rcPixels.Width, vp = vramPtr; x > 0; x--, vp += 2)
                    {
                        byte vram0 = vram[vp + 0];
                        byte vram1 = vram[vp + 1];

                        //Should this value be ignored (not changed)?
                        if (doIgnore && ignore0 == vram0 && ignore1 == vram1) continue;

                        vram[vp + 0] = (byte)(value0 | (vram0 & mask0));
                        vram[vp + 1] = (byte)(value1 | (vram1 & mask1));
                    }
                }
            }

            private static void toBytes(int i, out byte b0, out byte b1)
            {
                b0 = (byte)(i >> 0);
                b1 = (byte)(i >> 8);
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                return (prevEditCmd is EditCmdValue16Bit) && base.isRepeat(prevEditCmd);
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mRcPixels, mValue, mMask, mIgnore);
            }

            public static EditCmdValue16Bit openLine(VramMngr vramMngr, FieldsReader fr)
            {
                if (fr.FieldCount < 8) //Old format without ignore value field?
                {
                    return new EditCmdValue16Bit(vramMngr.mVram, fr.readRc(), fr.readInt(), fr.readInt(), NoIgnore);
                }
                return new EditCmdValue16Bit(vramMngr.mVram, fr.readRc(), fr.readInt(), fr.readInt(), fr.readInt());
            }
        }
    }
}
