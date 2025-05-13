using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdValue4Bit : EditCmdValue
        {
            public const string SaveId = "EditCmdValue4Bit";

            public EditCmdValue4Bit(byte[] vram, Rectangle rcPixels, int value)
                : base(vram, rcPixels, BitDepth.Ind4Bit, value)
            {
            }

            protected override void redoInner()
            {
                Rectangle rcPixels = mRcPixels;
                byte[] vram = mVram;
                int vramPtr = getVramPointer(rcPixels.Location, mBitDepth);
                int valueLo = mValue & 0x0F;
                int valueHi = valueLo << 4;
                byte valueLoHi = (byte)(valueLo | valueHi);
                for (int y = rcPixels.Height; y > 0; y--, vramPtr += VramStride)
                {
                    int vp = vramPtr;
                    int x = rcPixels.Width;
                    if ((rcPixels.Left & 0x1) != 0) //Left location is not even?
                    {
                        //Only change right palette index in high nibble.
                        int oldLo = vram[vp] & 0x0F;
                        vram[vp] = (byte)(oldLo | valueHi);
                        x--; vp++;
                    }
                    for (; x > 1; x -= 2, vp += 1) //Change both low and high nibble in bytes.
                    {
                        vram[vp] = valueLoHi;
                    }
                    if (x > 0) //Right location was not even?
                    {
                        //Only change left palette index in low nibble.
                        int oldHi = vram[vp] & 0xF0;
                        vram[vp] = (byte)(valueLo | oldHi);
                    }
                }
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                return (prevEditCmd is EditCmdValue4Bit) && base.isRepeat(prevEditCmd);
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mRcPixels, mValue);
            }

            public static EditCmdValue4Bit openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdValue4Bit(vramMngr.mVram, fr.readRc(), fr.readInt());
            }
        }
    }
}
