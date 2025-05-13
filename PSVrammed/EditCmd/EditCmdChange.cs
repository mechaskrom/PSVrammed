using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private abstract class EditCmdChange : EditCmd
        {
            protected readonly byte[] mVram; //Pointer to vram.
            protected EditCmdCopy mEcCopy; //Copy of affected vram bytes before edit (first redo).

            protected EditCmdChange(byte[] vram, Rectangle rcPixels, BitDepth bitDepth)
            {
                mVram = vram;
                mEcCopy = new EditCmdCopy(vram, rcPixels, bitDepth);
            }

            public EditCmdCopy EcCopy
            {
                get { return mEcCopy; }
            }

            public override Rectangle RcBytes
            {
                get { return mEcCopy.RcBytes; }
            }

            protected override void undoInner()
            {
                mEcCopy.restoreVram();
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                EditCmdChange ec = prevEditCmd as EditCmdChange;
                return ec != null && RcBytes == ec.RcBytes;
            }
        }
    }
}
