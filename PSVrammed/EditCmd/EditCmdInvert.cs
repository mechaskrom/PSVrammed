using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MyCustomStuff;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdInvert : EditCmd
        {
            public const string SaveId = "EditCmdInvert";

            private readonly VramMngr mVramMngr;
            private EditCmdValue mEcClear;
            private EditCmdCopy mEcCopy;

            public EditCmdInvert(VramMngr vramMngr, Rectangle rcPixels, BitDepth bitDepth, int value)
            {
                mVramMngr = vramMngr;
                mEcClear = EditCmdValue.create(mVramMngr.mVram, rcPixels, bitDepth, value);
                mEcCopy = null;
            }

            protected override void initInner()
            {
                //Call this on redo so the edit undo stack is properly set up.

                //Add commands in stack until any previous edit invert.
                List<EditCmd> prevCmds = new List<EditCmd>();
                foreach (EditCmd ec in mVramMngr.mEditUndoStack)
                {
                    if (ec is EditCmdInvert) break; //Previous edit invert found?

                    prevCmds.Add(ec);
                }

                //If no commands to invert then change the clear command to a blank one.
                //It's possible, but unlikely, that there are no commands to invert e.g.
                //when opening an edit sequence that was manually edited wrong.
                if (prevCmds.Count < 1)
                {
                    mEcClear = EditCmdValue.create(mVramMngr.mVram,
                        Rectangle.Empty, mEcClear.BitDepth, mEcClear.Value);
                }

                //Perform clear command.
                mEcClear.redo();

                //Undo previous commands to restore vram in cleared area.
                foreach (EditCmd ec in prevCmds)
                {
                    ec.undo();
                }

                //Copy vram. Use this afterwards to perform invert of marked area.
                mEcCopy = new EditCmdCopy(mVramMngr.mVram, mEcClear.RcBytes);
                mEcCopy.redo();

                //Redo previous commands to restore vram to current state.
                //This is to restore vram outside inverted area.
                foreach (EditCmd ec in prevCmds)
                {
                    ec.redo();
                }

                //This copy and redo loop solution is simple, but maybe a bit inefficient?
                //A more efficient, but more complex, solution would be to pass a
                //rectangle parameter to undo() that specifies the area of bytes to undo.
                //In this case that rectangle would be the clear command's RcBytes.
                //But I like the simplicity of the copy+redo solution.
            }

            public override Rectangle RcBytes
            {
                get { return mEcClear.RcBytes; }
            }

            protected override void redoInner()
            {
                mEcCopy.restoreVram();
            }

            protected override void undoInner()
            {
                mEcClear.undo();
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                //Can't repeat an edit invert regardless of their data.
                return prevEditCmd is EditCmdInvert;
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, mEcClear.RcPixels, mEcClear.BitDepth, mEcClear.Value);
            }

            public static EditCmdInvert openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdInvert(vramMngr, fr.readRc(), fr.readEnum<BitDepth>(), fr.readInt());
            }
        }
    }
}
