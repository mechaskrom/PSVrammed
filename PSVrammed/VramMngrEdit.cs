using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    //Manages vram (from state file). Edit vram part.
    partial class VramMngr
    {
        public const int EditValueDefault4Bit = 0;
        public const int EditValueDefault8Bit = 0;
        public static readonly int EditValueDefault16Bit = PsColor16Bit.fromColor(Color.Magenta);
        public static readonly int EditValueDefault24Bit = PsColor24Bit.fromColor(Color.Magenta);
        public const int EditMaskDefault = 0;
        private const int MaskRed16Bit = 0x1F << 0;
        private const int MaskGreen16Bit = 0x1F << 5;
        private const int MaskBlue16Bit = 0x1F << 10;
        private const int MaskAlpha16Bit = 0x01 << 15;
        private const int MaskRed24Bit = 0xFF << 0;
        private const int MaskGreen24Bit = 0xFF << 8;
        private const int MaskBlue24Bit = 0xFF << 16;
        private const int EditStackMaxItems = 8000; //Max edit commands in stacks.

        private int mEditValue4Bit;
        private int mEditValue8Bit;
        private int mEditValue16Bit;
        private int mEditValue24Bit;
        private int mEditMask16Bit;
        private int mEditMask24Bit;
        private EditCmdPaste.ISource mPasteSrc = null; //Used for copy/cut and paste.
        private readonly LimitedStack<EditCmd> mEditUndoStack;
        private readonly LimitedStack<EditCmd> mEditRedoStack;

        //*******************************************************************************
        //*******************************************************************************

        #region Properties

        public bool CanEditUndo
        {
            get { return mEditUndoStack.Any((ec) => !ec.IsHidden); } //Stack has not a hidden edit?
        }

        public bool CanEditRedo
        {
            get { return mEditRedoStack.Any((ec) => !ec.IsHidden); } //Stack has not a hidden edit?
        }

        public bool CanEditValue
        {
            get { return mVrammed.IsFileOpen && TextureMarker.IsVisible; }
        }

        public bool CanEditAlpha
        {
            get { return CanEditAlphaTexture || CanEditAlphaPalette; }
        }

        public bool CanEditAlphaTexture
        {
            get { return CanEditValue && mBitDepthTexture == BitDepth.Rgb16Bit; }
        }

        public bool CanEditAlphaPalette
        {
            get { return CanEditPalette; }
        }

        public bool CanEditPalette
        {
            get { return mVrammed.IsFileOpen && PaletteMarker.IsVisible; }
        }

        public bool CanEditInvert
        {
            get { return CanEditInvertAll && TextureMarker.IsVisible; }
        }

        public bool CanEditInvertAll
        {
            get { return CanEditUndo && !(mEditUndoStack.Peek() is EditCmdInvert); }
        }

        public bool CanEditCopy
        {
            get { return mVrammed.IsFileOpen && TextureMarker.IsVisible; }
        }

        public bool CanEditCut
        {
            get { return mVrammed.IsFileOpen && TextureMarker.IsVisible; }
        }

        public bool CanEditPaste
        {
            get { return mPasteSrc != null && mVrammed.IsFileOpen && TextureMarker.IsVisible; }
        }

        private int EditValueCurrentBit
        {
            get
            {
                if (mBitDepthTexture == BitDepth.Ind4Bit) return EditValue4Bit;
                if (mBitDepthTexture == BitDepth.Ind8Bit) return EditValue8Bit;
                if (mBitDepthTexture == BitDepth.Rgb16Bit) return EditValue16Bit;
                if (mBitDepthTexture == BitDepth.Rgb24Bit) return EditValue24Bit;
                return 0;
            }
        }

        public int EditValue4Bit
        {
            get { return mEditValue4Bit; }
            set { mEditValue4Bit = value & 0x00000F; }
        }

        public int EditValue8Bit
        {
            get { return mEditValue8Bit; }
            set { mEditValue8Bit = value & 0x0000FF; }
        }

        public int EditValue16Bit
        {
            get { return mEditValue16Bit; }
            set { mEditValue16Bit = value & 0x00FFFF; }
        }

        public int EditValue24Bit
        {
            get { return mEditValue24Bit; }
            set { mEditValue24Bit = value & 0xFFFFFF; }
        }

        private int EditMaskCurrentBit
        {
            get
            {
                if (mBitDepthTexture == BitDepth.Rgb16Bit) return EditMask16Bit;
                if (mBitDepthTexture == BitDepth.Rgb24Bit) return EditMask24Bit;
                return 0;
            }
        }

        public int EditMask16Bit
        {
            get { return mEditMask16Bit; }
            set { mEditMask16Bit = value & 0x00FFFF; }
        }

        public int EditMask24Bit
        {
            get { return mEditMask24Bit; }
            set { mEditMask24Bit = value & 0xFFFFFF; }
        }

        #endregion Properties

        //*******************************************************************************
        //*******************************************************************************

        #region Convert mask

        public static int editMask16BitBoolToInt(
            bool maskAlpha, bool maskRed, bool maskGreen, bool maskBlue)
        {
            return
                (maskAlpha ? MaskAlpha16Bit : 0) |
                (maskRed ? MaskRed16Bit : 0) |
                (maskGreen ? MaskGreen16Bit : 0) |
                (maskBlue ? MaskBlue16Bit : 0);
        }

        public static void editMask16BitIntToBool(int mask,
            out bool maskAlpha, out bool maskRed, out bool maskGreen, out bool maskBlue)
        {
            maskAlpha = (mask & MaskAlpha16Bit) == MaskAlpha16Bit;
            maskRed = (mask & MaskRed16Bit) == MaskRed16Bit;
            maskGreen = (mask & MaskGreen16Bit) == MaskGreen16Bit;
            maskBlue = (mask & MaskBlue16Bit) == MaskBlue16Bit;
        }

        public static int editMask24BitBoolToInt(
            bool maskRed, bool maskGreen, bool maskBlue)
        {
            return
                (maskRed ? MaskRed24Bit : 0) |
                (maskGreen ? MaskGreen24Bit : 0) |
                (maskBlue ? MaskBlue24Bit : 0);
        }

        public static void editMask24BitIntToBool(int mask,
            out bool maskRed, out bool maskGreen, out bool maskBlue)
        {
            maskRed = (mask & MaskRed24Bit) == MaskRed24Bit;
            maskGreen = (mask & MaskGreen24Bit) == MaskGreen24Bit;
            maskBlue = (mask & MaskBlue24Bit) == MaskBlue24Bit;
        }

        #endregion Convert mask

        //*******************************************************************************
        //*******************************************************************************

        #region Edit vram

        //Perform edits at where the texture marker appears to be (visual location).
        //But use palette marker's vram indicator when editing palette entries.
        //This is to make the edit mouse tool work and also not surprise the user.

        public void editCopy()
        {
            if (CanEditCopy)
            {
                editCmdPerform(new EditCmdCopy(mVram, TextureMarker.Rectangle, BitDepthTexture));
            }
        }

        public void editCut()
        {
            if (CanEditCut)
            {
                editCmdPerform(new EditCmdCut(mVram, TextureMarker.Rectangle, BitDepthTexture, EditValueCurrentBit));
            }
        }

        public void editPaste()
        {
            if (CanEditPaste)
            {
                editCmdPerform(new EditCmdPaste(this, TextureMarker.Location, BitDepthTexture));
            }
        }

        public void editValue()
        {
            if (CanEditValue)
            {
                editCmdPerform(EditCmdValue.create(mVram,
                    TextureMarker.Rectangle, BitDepthTexture, EditValueCurrentBit, EditMaskCurrentBit));
            }
        }

        //TODO: Special edits that ignores black/zero color. Something I used to clear palettes to
        //a solid color, but keep any transparent black/zero color. Useful for ripping a "mask" in
        //a game of hi-prio graphics that covers lo-prio dito. Test this for a while and see how it works.
        //Maybe make it more user visible if it turns out to be a useful and frequently used feature?
        public void editValueIgnoreBlack() //Edit at texture marker
        {
            if (CanEditValue)
            {
                editCmdPerform(EditCmdValue.create(mVram,
                    TextureMarker.Rectangle, BitDepthTexture, EditValueCurrentBit, EditMaskCurrentBit, 0));
            }
        }
        public void editPaletteIgnoreBlack() //Edit at palette marker
        {
            editPaletteAllEntries(EditValue16Bit, EditMask16Bit, 0);
        }

        public void editAlphaSet()
        {
            editAlpha(true);
        }

        public void editAlphaClear()
        {
            editAlpha(false);
        }

        private void editAlpha(bool alphaSet)
        {
            //Edit alpha at the texture marker if possible (i.e. 16bit mode)
            //else do it at the palette marker (i.e. 4/8bit mode).
            if (CanEditAlpha)
            {
                Rectangle rcPixels = CanEditAlphaTexture ? TextureMarker.Rectangle : PaletteMarker.Rectangle;
                editAlpha(rcPixels, alphaSet);
            }
        }

        private void editAlpha(Rectangle rcPixels, bool alphaSet)
        {
            int value = alphaSet ? MaskAlpha16Bit : 0;
            int mask = editMask16BitBoolToInt(false, true, true, true);
            editCmdPerform(new EditCmdValue16Bit(mVram, rcPixels, value, mask));
        }

        public void editPaletteAllEntries(int value, int mask)
        {
            editPaletteAllEntries(value, mask, EditCmdValue.NoIgnore);
        }

        public void editPaletteAllEntries(int value, int mask, int ignore)
        {
            if (CanEditPalette)
            {
                Rectangle rcPixels = getEditPalettePixelsRc();
                editCmdPerform(new EditCmdValue16Bit(mVram, rcPixels, value, mask, ignore));
            }
        }

        public void editPaletteEntry(int index, int value, int mask)
        {
            editPaletteEntry(index, value, mask, EditCmdValue.NoIgnore);
        }

        public void editPaletteEntry(int index, int value, int mask, int ignore)
        {
            if (CanEditPalette)
            {
                Rectangle rcPixels = getEditPalettePixelsRc(index);
                editCmdPerform(new EditCmdValue16Bit(mVram, rcPixels, value, mask, ignore));
            }
        }

        private Rectangle getEditPalettePixelsRc()
        {
            return PaletteMarker.Indicator.Rectangle;
        }

        private Rectangle getEditPalettePixelsRc(int index)
        {
            Rectangle rc = getEditPalettePixelsRc();
            //Shrink rectangle to indexed entry.
            rc.X += index;
            rc.Width = 1;
            return rc;
        }

        public void editInvert()
        {
            if (CanEditInvert)
            {
                editCmdPerform(new EditCmdInvert(this, TextureMarker.Rectangle, BitDepthTexture, EditValueCurrentBit));
            }
        }

        public void editInvertAll()
        {
            if (CanEditInvertAll)
            {
                editCmdPerform(new EditCmdInvert(this, BmpTextureBounds, BitDepthTexture, EditValueCurrentBit));
            }
        }

        public void editPaletteAddSuikoden2Docks()
        {
            //TODO: This is a test/temporary function I made for Suikoden 2 for changing the water
            //at the docks in the headquarters (add 7,10,11).
            //
            //Maybe make this functionality available to the user? Like an advanced edit?
            //-Shortcut key = alt+delete?
            //-Create an advanced edit values dialog where R,G,B values and math op (add/mul) can be set? For texture edits.
            //-Make the palette editor palette entry dialog tabbed. First tab as dialog is now and
            // second tab with a similar dialog to the new advanced edit values dialog? For full palette edits.
            // Maybe not needed as the advanced texture edit can be used on palettes also. User just has
            // to change the texture marker size to 16/256*1 big.
            //
            //I'm just not sure how useful this feature really is. Is it worth the extra work/code?
            //Suikoden 2 docks is the only time I've used it so far.
            if (CanEditPalette)
            {
                Rectangle rcPixels = getEditPalettePixelsRc();
                int addR = 7;
                int addG = 10;
                int addB = 11;
                editCmdPerform(new EditCmdMath16Bit(mVram, rcPixels, addR, addG, addB, EditCmdMath16Bit.MathOp.Add));
            }
        }

        private void editCmdPerform(EditCmd editCmd)
        {
            //Perform command if it's not a repeat of the last one.
            if (!(mEditUndoStack.HasObject && editCmd.isRepeat(mEditUndoStack.Peek())))
            {
                editCmd.redo();
                if (!editCmd.IsBlank) //Command did something?
                {
                    mEditUndoStack.Push(editCmd);
                    mEditRedoStack.Clear();
                    if (!editCmd.IsHidden) //Command visible to user?
                    {
                        editVramChanged(editCmd.RcBytes);
                    }
                }

                if (editCmd is EditCmdPaste.ISource)
                {
                    mPasteSrc = (EditCmdPaste.ISource)editCmd;
                }
            }
            //else mVrammed.playSound(System.Media.SystemSounds.Beep);
        }

        public void editUndo()
        {
            if (!mEditUndoStack.HasObject) //Can't undo?
            {
                mVrammed.playSound(System.Media.SystemSounds.Beep);
                return;
            }

            Log.timeBegin();
            while (mEditUndoStack.HasObject)
            {
                EditCmd editCmd = mEditUndoStack.Pop();
                editCmd.undo();
                mEditRedoStack.Push(editCmd);
                editVramChanged(editCmd.RcBytes);
                if (!editCmd.IsHidden) //Stop undo if not a hidden command.
                {
                    break;
                }
            }
            Log.timeEnd("editUndo()");
        }

        public void editRedo()
        {
            if (!mEditRedoStack.HasObject) //Can't redo?
            {
                mVrammed.playSound(System.Media.SystemSounds.Beep);
                return;
            }

            Log.timeBegin();
            while (mEditRedoStack.HasObject)
            {
                EditCmd editCmd = mEditRedoStack.Pop();
                editCmd.redo();
                mEditUndoStack.Push(editCmd);
                editVramChanged(editCmd.RcBytes);
                if (!editCmd.IsHidden) //Stop redo if not a hidden command.
                {
                    break;
                }
            }
            Log.timeEnd("editRedo()");
        }

        private void editVramChanged(Rectangle rcBytes)
        {
            //Check if this vram change affected the currently selected palette.
            Rectangle rcPixels = rcByteToPixel(rcBytes, BitDepth.Rgb16Bit);
            Rectangle rcPalette = PaletteMarker.Indicator.Rectangle;
            rcPalette.Width = PaletteWidth4Bit;
            bool isPal4BitAffected = rcPixels.IntersectsWith(rcPalette);
            rcPalette.Width = PaletteWidth8Bit;
            bool isPal8BitAffected = isPal4BitAffected || rcPixels.IntersectsWith(rcPalette);

            mBmpVram4Bit.onVramChanged(rcBytes, isPal4BitAffected);
            mBmpVram8Bit.onVramChanged(rcBytes, isPal8BitAffected);
            mBmpVram16Bit.onVramChanged(rcBytes);
            mBmpVram24Bit.onVramChanged(rcBytes);
            mBmpVram4BitAlpha.onVramChanged(rcBytes, isPal4BitAffected);
            mBmpVram8BitAlpha.onVramChanged(rcBytes, isPal8BitAffected);
            mBmpVram16BitAlpha.onVramChanged(rcBytes);

            OnVramChanged(isPal4BitAffected || isPal8BitAffected);
        }

        public void editOpenSequence(string filePath)
        {
            try
            {
                Log.timeBegin();

                List<EditCmd> editCmds = EditCmd.open(this, filePath);
                foreach (EditCmd ec in editCmds)
                {
                    ec.redo();
                    mEditUndoStack.Push(ec);

                    if (ec is EditCmdPaste.ISource)
                    {
                        mPasteSrc = (EditCmdPaste.ISource)ec;
                    }
                }

                mEditRedoStack.Clear();
                editVramChanged(VramBytesRc);
            }
            finally
            {
                Log.timeEnd("editOpenSequence()");
            }
        }

        public void editSaveSequence(string filePath)
        {
            try
            {
                Log.timeBegin();

                List<EditCmd> editCmds = new List<EditCmd>(mEditUndoStack);
                editCmds.Reverse();

                EditCmd.save(editCmds, filePath);
            }
            finally
            {
                Log.timeEnd("editSaveSequence()");
            }
        }

        #endregion Edit vram
    }
}
