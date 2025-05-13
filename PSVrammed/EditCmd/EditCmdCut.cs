using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdCut : EditCmd, EditCmdPaste.ISource
        {
            public const string SaveId = "EditCmdCut";

            //Cut is just a value edit with the copy of the old vram exposed as a paste source.
            private readonly EditCmdValue mEcValue;

            public EditCmdCut(byte[] vram, Rectangle rcPixels, BitDepth bitDepth, int value)
            {
                mEcValue = EditCmdValue.create(vram, rcPixels, bitDepth, value);
            }

            public byte[] PasteSrc
            {
                get { return mEcValue.EcCopy.PasteSrc; }
            }

            public override Rectangle RcBytes
            {
                get { return mEcValue.RcBytes; }
            }

            protected override void redoInner()
            {
                mEcValue.redo();
            }

            protected override void undoInner()
            {
                mEcValue.undo();
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                EditCmdCut ec = prevEditCmd as EditCmdCut;
                return ec != null && mEcValue.isRepeat(ec.mEcValue);
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mEcValue.RcPixels, mEcValue.BitDepth, mEcValue.Value);
            }

            public static EditCmdCut openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdCut(vramMngr.mVram, fr.readRc(), fr.readEnum<BitDepth>(), fr.readInt());
            }
        }
    }
}
