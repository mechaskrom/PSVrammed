using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdCopy : EditCmd, EditCmdPaste.ISource
        {
            public const string SaveId = "EditCmdCopy";

            private readonly byte[] mVram; //Pointer to vram.
            private byte[] mCopy; //Copy of vram bytes.
            private Rectangle mRcBytes; //Copied area.

            public EditCmdCopy(byte[] vram, Rectangle rcBytes)
                : this(vram, rcBytes, BitDepth.Ind8Bit)
            {
            }

            public EditCmdCopy(byte[] vram, Rectangle rcPixels, BitDepth bitDepth)
            {
                mVram = vram;
                mCopy = null;
                mRcBytes = rcPixelToByte(rcPixels, bitDepth);
            }

            public byte[] PasteSrc
            {
                get { return mCopy; }
            }

            public void restoreVram()
            {
                copy(mCopy, 0, mRcBytes.Width, mVram, getVramPointer8Bit(mRcBytes.Location), VramStride, mRcBytes.Size);
            }

            public override bool IsHidden
            {
                //Hide copy in stack so the user doesn't notice it when undo/redo.
                get { return true; }
            }

            public override Rectangle RcBytes
            {
                get { return mRcBytes; }
            }

            protected override void initInner()
            {
                //Only need to copy vram on the first redo.
                mCopy = new byte[mRcBytes.Width * mRcBytes.Height];
                copy(mVram, getVramPointer8Bit(mRcBytes.Location), VramStride, mCopy, 0, mRcBytes.Width, mRcBytes.Size);
            }

            protected override void redoInner()
            {
                //Do nothing. Vram is copied in initInner().
            }

            protected override void undoInner()
            {
                //Do nothing.
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                EditCmdCopy ec = prevEditCmd as EditCmdCopy;
                return ec != null && mRcBytes == ec.mRcBytes;
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mRcBytes);
            }

            public static EditCmdCopy openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdCopy(vramMngr.mVram, fr.readRc());
            }
        }
    }
}
