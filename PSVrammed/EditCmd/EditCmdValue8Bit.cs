using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdValue8Bit : EditCmdValue
        {
            public const string SaveId = "EditCmdValue8Bit";

            public EditCmdValue8Bit(byte[] vram, Rectangle rcPixels, int value)
                : base(vram, rcPixels, BitDepth.Ind8Bit, value)
            {
            }

            protected override void redoInner()
            {
                Rectangle rcPixels = mRcPixels;
                byte[] vram = mVram;
                int vramPtr = getVramPointer8Bit(rcPixels.Location);
                byte value0 = (byte)mValue;
                for (int y = rcPixels.Height; y > 0; y--, vramPtr += VramStride)
                {
                    for (int x = rcPixels.Width, vp = vramPtr; x > 0; x--, vp++)
                    {
                        vram[vp] = value0;
                    }
                }
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                return (prevEditCmd is EditCmdValue8Bit) && base.isRepeat(prevEditCmd);
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mRcPixels, mValue);
            }

            public static EditCmdValue8Bit openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdValue8Bit(vramMngr.mVram, fr.readRc(), fr.readInt());
            }
        }
    }
}
