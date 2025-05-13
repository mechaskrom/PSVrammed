using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace PSVrammed
{
    partial class VramMngr
    {
        private abstract class EditCmd
        {
            private const string SaveSignature = "psvrammed edits";
            private const string SaveVersionV1 = "v1";
            private const string SaveVersionCurrent = SaveVersionV1;

            private bool mIsInit = false;

            [Conditional("DEBUG")]
            public void checkInit()
            {
                Debug.Assert(mIsInit, "Edit command must be initialized first!");
            }

            //Edit command had no effect (don't add to stack)?
            public bool IsBlank
            {
                get
                {
                    checkInit();
                    return RcBytes.Width <= 0 || RcBytes.Height <= 0;
                }
            }

            //True if command should be hidden in the edit stack i.e. undo/redo
            //should not "stop" on it so it isn't noticeable to the user.
            public virtual bool IsHidden { get { return false; } }

            public void redo()
            {
                //Initialize command on first redo. Many commands rely on the
                //edit/undo stacks being properly set up so therefore we have
                //to do it here instead of in the constructors.
                if (!mIsInit)
                {
                    initInner();
                    mIsInit = true;
                }
                redoInner();
            }

            public void undo()
            {
                checkInit();
                undoInner();
            }

            //Area of affected bytes. May change after first redo is performed.
            public abstract Rectangle RcBytes { get; }

            protected virtual void initInner() { } //Called once at first redo.
            protected abstract void redoInner();
            protected abstract void undoInner();

            //True if considered a repeat of previous command in edit stack.
            public abstract bool isRepeat(EditCmd prevEditCmd);

            protected abstract string saveLine();

            public static void save(List<EditCmd> editCmds, string filePath)
            {
                using (TextWriter tw = new StreamWriter(filePath))
                {
                    tw.WriteLine(SaveSignature);
                    tw.WriteLine(SaveVersionCurrent);
                    foreach (EditCmd ec in editCmds)
                    {
                        tw.WriteLine(ec.saveLine());
                    }
                }
            }

            public static List<EditCmd> open(VramMngr vramMngr, string filePath)
            {
                using (TextReader tr = new StreamReader(filePath))
                {
                    string signature = tr.ReadLine();
                    if (signature != SaveSignature)
                    {
                        throw new InvalidDataException(Strings.OpenEditSignatureErrorMsg);
                    }
                    string version = tr.ReadLine();
                    if (version != SaveVersionCurrent)
                    {
                        throw new InvalidDataException(Strings.OpenEditVersionErrorMsg);
                    }

                    List<EditCmd> editCmds = new List<EditCmd>();
                    string line;
                    int lineNr = 1;
                    while ((line = tr.ReadLine()) != null)
                    {
                        try
                        {
                            EditCmd ec = openLine(vramMngr, line);
                            editCmds.Add(ec);
                            lineNr++;
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException(String.Format(
                                Strings.OpenEditReadErrorMsg, lineNr, ex.Message));
                        }
                    }
                    return editCmds;
                }
            }

            private static EditCmd openLine(VramMngr vramMngr, string line)
            {
                FieldsReader fr = new FieldsReader(line);
                string id = fr.readString();
                if (id == EditCmdValue4Bit.SaveId) return EditCmdValue4Bit.openLine(vramMngr, fr);
                if (id == EditCmdValue8Bit.SaveId) return EditCmdValue8Bit.openLine(vramMngr, fr);
                if (id == EditCmdValue16Bit.SaveId) return EditCmdValue16Bit.openLine(vramMngr, fr);
                if (id == EditCmdValue24Bit.SaveId) return EditCmdValue24Bit.openLine(vramMngr, fr);
                if (id == EditCmdInvert.SaveId) return EditCmdInvert.openLine(vramMngr, fr);
                if (id == EditCmdMath16Bit.SaveId) return EditCmdMath16Bit.openLine(vramMngr, fr);
                if (id == EditCmdCopy.SaveId) return EditCmdCopy.openLine(vramMngr, fr);
                if (id == EditCmdCut.SaveId) return EditCmdCut.openLine(vramMngr, fr);
                if (id == EditCmdPaste.SaveId) return EditCmdPaste.openLine(vramMngr, fr);
                throw new Exception(Strings.OpenEditReadErrorMsgUnknown);
            }

            protected static void copy(byte[] src, int srcPtr, int srcStride, byte[] dst, int dstPtr, int dstStride, Size szBytes)
            {
                //Copy between byte arrays.
                int length = szBytes.Width;
                for (int y = szBytes.Height; y > 0; y--, srcPtr += srcStride, dstPtr += dstStride)
                {
                    Array.Copy(src, srcPtr, dst, dstPtr, length);
                }
            }
        }
    }
}
