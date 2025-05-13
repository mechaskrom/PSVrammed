using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    partial class VramMngr
    {
        private class EditCmdPaste : EditCmdChange
        {
            public const string SaveId = "EditCmdPaste";

            private readonly VramMngr mVramMngr;
            private ISource mEcSrc;

            public interface ISource
            {
                byte[] PasteSrc { get; }
                Rectangle RcBytes { get; }
            }

            public EditCmdPaste(VramMngr vramMngr, Point ptBytes)
                : this(vramMngr, ptBytes, BitDepth.Ind8Bit)
            {
            }

            public EditCmdPaste(VramMngr vramMngr, Point ptPixels, BitDepth bitDepth)
                : base(vramMngr.mVram, new Rectangle(ptPixels, Size.Empty), bitDepth)
            {
                mVramMngr = vramMngr;
                mEcSrc = null;
            }

            protected override void initInner()
            {
                //Call this on redo so the edit undo stack is properly set up.

                ISource ecSrc = mVramMngr.mPasteSrc;
                if (ecSrc != null && ecSrc.PasteSrc != null) //A paste source exists?
                {
                    Rectangle rcBytesSrc = ecSrc.RcBytes;

                    Rectangle rcBytesDst = new Rectangle(RcBytes.Location, rcBytesSrc.Size);
                    rcBytesDst.Intersect(VramBytesRc); //Intersect dst to fit in vram.
                    //This does not wrap around to the next rows. Maybe it should? To weird?

                    //Update old vram copy if bytes rectangle changed (it was outside vram).
                    if (RcBytes != rcBytesDst)
                    {
                        mEcCopy = new EditCmdCopy(mVramMngr.mVram, rcBytesDst);
                    }
                    mEcCopy.redo(); //Save old vram.

                    mEcSrc = ecSrc;
                }

                //It's possible, but unlikely, that there is no command to use as paste
                //source e.g. when opening an edit sequence. The paste source command may
                //have been pushed out of the saved stack (a lot of edits between the
                //copy and the paste) or the sequence file was manually edited wrong.
            }

            protected override void redoInner()
            {
                if (mEcSrc != null)
                {
                    copy(mEcSrc.PasteSrc, 0, mEcSrc.RcBytes.Width,
                        mVram, getVramPointer8Bit(RcBytes.Location), VramStride, RcBytes.Size);
                }
            }

            public override bool isRepeat(EditCmd prevEditCmd)
            {
                EditCmdPaste ec = prevEditCmd as EditCmdPaste;
                return ec != null && RcBytes.Location == ec.RcBytes.Location;
            }

            protected override string saveLine()
            {
                return FieldsWriter.toLine(SaveId, RcBytes.Location);
            }

            public static EditCmdPaste openLine(VramMngr vramMngr, FieldsReader fr)
            {
                return new EditCmdPaste(vramMngr, fr.readPt());
            }
        }
    }
}
