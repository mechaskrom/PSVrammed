using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Text;
using System.Linq;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    partial class FormMain
    {
        private CmdHandler mCmdHandler;

        private void initCmdHandler()
        {
            Log.timeBegin();

            mCmdHandler = new CmdHandler();
            //mCmdHandler = new CmdHandler(new StringBuilder()); //Enable save key table print out.

            bool canRepeat = true; //Just to make code below easier to read.

            //File menu.
            mCmdHandler.addCmd("Open file",
                mVrammed.openStateDialog, mainMenuFileOpen, Keys.O | Keys.Control);
            mCmdHandler.addCmd("Reopen file",
                mVrammed.reopenState, mainMenuFileReopen, Keys.R | Keys.Control);
            mCmdHandler.addCmd("Save file",
                mVrammed.saveState, mainMenuFileSave, Keys.S | Keys.Control);
            mCmdHandler.addCmd("Save file as",
                mVrammed.saveStateDialog, mainMenuFileSaveAs, Keys.S | Keys.Control | Keys.Shift);
            mCmdHandler.addCmd("Save texture",
                mVrammed.saveTextureDialog, mainMenuFileSaveTexture, Keys.T | Keys.Control);
            mCmdHandler.addCmd("Save texture all",
                mVrammed.saveTextureAllDialog, mainMenuFileSaveTextureAll, Keys.T | Keys.Control | Keys.Shift);
            mCmdHandler.addCmd("Open edit sequence",
                mVrammed.openEditDialog, mainMenuFileOpenEdit, Keys.Q | Keys.Control);
            mCmdHandler.addCmd("Save edit sequence",
                mVrammed.saveEditDialog, mainMenuFileSaveEdit, Keys.U | Keys.Control);
            mCmdHandler.addCmd("Show file info dialog",
                mVrammed.showFileInfoDialog, mainMenuFileInfo);
            mCmdHandler.addCmd("Show about dialog",
                () => new DialogAbout().ShowDialog(), mainMenuFileAbout);
            mCmdHandler.addCmd("Exit program",
                Application.Exit, mainMenuFileExit);

            //Edit menu.
            mCmdHandler.addCmd("Undo edit",
                mVrammed.VramMngr.editUndo, mainMenuEditUndo, canRepeat, Keys.Z | Keys.Control);
            mCmdHandler.addCmd("Redo edit",
                mVrammed.VramMngr.editRedo, mainMenuEditRedo, canRepeat, Keys.Y | Keys.Control);
            mCmdHandler.addCmd("Edit set value",
                mVrammed.VramMngr.editValue, mainMenuEditSetValue, Keys.Delete);
            mCmdHandler.addCmd("Edit set value ignore black",
                mVrammed.VramMngr.editValueIgnoreBlack, Keys.Delete | Keys.Shift);
            mCmdHandler.addCmd("Edit clear alpha",
                mVrammed.VramMngr.editAlphaClear, mainMenuEditClearAlpha, Keys.P);
            mCmdHandler.addCmd("Edit set alpha",
                mVrammed.VramMngr.editAlphaSet, mainMenuEditSetAlpha, Keys.P | Keys.Shift);
            mCmdHandler.addCmd("Edit invert",
                mVrammed.VramMngr.editInvert, mainMenuEditInvert, Keys.I);
            mCmdHandler.addCmd("Edit invert all",
                mVrammed.VramMngr.editInvertAll, mainMenuEditInvertAll, Keys.I | Keys.Shift);
            mCmdHandler.addCmd("Edit copy",
                mVrammed.VramMngr.editCopy, mainMenuEditCopy, Keys.C | Keys.Control);
            mCmdHandler.addCmd("Edit cut",
                mVrammed.VramMngr.editCut, mainMenuEditCut, Keys.X | Keys.Control);
            mCmdHandler.addCmd("Edit paste",
                mVrammed.VramMngr.editPaste, mainMenuEditPaste, Keys.V | Keys.Control);
            mCmdHandler.addCmd("Edit copy texture to clipboard",
                mVrammed.copyTextureClipboard, mainMenuEditTextureClipboard, Keys.T);
            mCmdHandler.addCmd("Edit copy texture all to clipboard",
                mVrammed.copyTextureAllClipboard, mainMenuEditTextureAllClipboard, Keys.T | Keys.Shift);
            mCmdHandler.addCmd("Edit palette entry",
                mVrammed.PaletteEditor.editEntryOpen, mainMenuEditPaletteEntry, Keys.Enter);
            mCmdHandler.addCmd("Repeat edit palette entry",
                mVrammed.PaletteEditor.editEntryRepeat, mainMenuEditPaletteEntryRepeat, Keys.Delete | Keys.Control);
            mCmdHandler.addCmd("Edit palette all entries ignore black",
                mVrammed.VramMngr.editPaletteIgnoreBlack, Keys.Delete | Keys.Control | Keys.Shift);

            //Mode menu.
            mCmdHandler.addCmd("Switch mode to 4 bit view",
                () => mVrammed.setMode(Mode.Ind4BitView), mainMenuMode4bitView, Keys.F1);
            mCmdHandler.addCmd("Switch mode to 4 bit compare",
                () => mVrammed.setMode(Mode.Ind4BitComp), mainMenuMode4bitComp, Keys.F1 | Keys.Shift);
            mCmdHandler.addCmd("Switch mode to 4 bit palette editor",
                () => mVrammed.setMode(Mode.Ind4BitPalE), mainMenuMode4bitPale, Keys.F1 | Keys.Control);
            mCmdHandler.addCmd("Switch mode to 8 bit view",
                () => mVrammed.setMode(Mode.Ind8BitView), mainMenuMode8bitView, Keys.F2);
            mCmdHandler.addCmd("Switch mode to 8 bit compare",
                () => mVrammed.setMode(Mode.Ind8BitComp), mainMenuMode8bitComp, Keys.F2 | Keys.Shift);
            mCmdHandler.addCmd("Switch mode to 8 bit palette editor",
                () => mVrammed.setMode(Mode.Ind8BitPalE), mainMenuMode8bitPale, Keys.F2 | Keys.Control);
            mCmdHandler.addCmd("Switch mode to 16 bit view",
                () => mVrammed.setMode(Mode.Rgb16BitView), mainMenuMode16bitView, Keys.F3);
            mCmdHandler.addCmd("Switch mode to 24 bit view",
                () => mVrammed.setMode(Mode.Rgb24BitView), mainMenuMode24bitView, Keys.F4);

            //Tool menu.
            mCmdHandler.addCmd("Switch tool to none",
                () => mVrammed.setTool(Tool.None), mainMenuToolNone, Keys.F8);
            mCmdHandler.addCmd("Switch tool to zoom",
                () => mVrammed.setTool(Tool.Zoom), mainMenuToolZoom, Keys.F9);
            mCmdHandler.addCmd("Switch tool to situate",
                () => mVrammed.setTool(Tool.Situate), mainMenuToolSituate, Keys.F10);
            mCmdHandler.addCmd("Switch tool to inspect",
                () => mVrammed.setTool(Tool.Inspect), mainMenuToolInspect, Keys.F11);
            mCmdHandler.addCmd("Switch tool to edit",
                () => mVrammed.setTool(Tool.Edit), mainMenuToolEdit, Keys.F12);

            //Config menu.
            mCmdHandler.addCmd("Open config markers dialog",
                () => new DialogMarkers(mVrammed).ShowDialog(), mainMenuConfigMarkers, Keys.F5);
            mCmdHandler.addCmd("Open config edit values dialog",
                () => new DialogEditValues(mVrammed).ShowDialog(), mainMenuConfigEditValues, Keys.F6);
            mCmdHandler.addCmd("Open config edit slots dialog",
                () => new DialogEditSlots(mVrammed).ShowDialog(), mainMenuConfigEditSlots, Keys.F7);
            mCmdHandler.addCmd("Toggle show texture alpha",
                mVrammed.toggleTextureAlpha, mainMenuConfigShowAlphaTexture, Keys.A);
            mCmdHandler.addCmd("Toggle show compare alpha",
                mVrammed.toggleCompareAlpha, mainMenuConfigShowAlphaCompare, Keys.A | Keys.Shift);
            mCmdHandler.addCmd("Toggle show palette alpha",
                mVrammed.togglePaletteAlpha, mainMenuConfigShowAlphaPalette, Keys.A | Keys.Control);
            mCmdHandler.addCmd("Toggle update situating marker",
                mVrammed.toggleUpdateSituatingMarker, mainMenuConfigUpdateSituatingMarker);

            //Texture marker move.
            mCmdHandler.addCmd("Move texture marker up",
                () => mVrammed.TextureMarker.stepLocationUp(false), canRepeat, Keys.Up);
            mCmdHandler.addCmd("Move texture marker up alt",
                () => mVrammed.TextureMarker.stepLocationUp(true), canRepeat, Keys.Up | Keys.Alt);
            mCmdHandler.addCmd("Move texture marker down",
                () => mVrammed.TextureMarker.stepLocationDown(false), canRepeat, Keys.Down);
            mCmdHandler.addCmd("Move texture marker down alt",
                () => mVrammed.TextureMarker.stepLocationDown(true), canRepeat, Keys.Down | Keys.Alt);
            mCmdHandler.addCmd("Move texture marker left",
                () => mVrammed.TextureMarker.stepLocationLeft(false), canRepeat, Keys.Left);
            mCmdHandler.addCmd("Move texture marker left alt",
                () => mVrammed.TextureMarker.stepLocationLeft(true), canRepeat, Keys.Left | Keys.Alt);
            mCmdHandler.addCmd("Move texture marker right",
                () => mVrammed.TextureMarker.stepLocationRight(false), canRepeat, Keys.Right);
            mCmdHandler.addCmd("Move texture marker right alt",
                () => mVrammed.TextureMarker.stepLocationRight(true), canRepeat, Keys.Right | Keys.Alt);

            //Compare marker and palette editor move.
            mCmdHandler.addCmd("Move compare marker up",
                () => mVrammed.CompareMarker.stepLocationUp(false), canRepeat, Keys.Up | Keys.Shift);
            mCmdHandler.addCmd("Move compare marker up alt",
                () => mVrammed.CompareMarker.stepLocationUp(true), canRepeat, Keys.Up | Keys.Shift | Keys.Alt);
            mCmdHandler.addCmd("Move compare marker down",
                () => mVrammed.CompareMarker.stepLocationDown(false), canRepeat, Keys.Down | Keys.Shift);
            mCmdHandler.addCmd("Move compare marker down alt",
                () => mVrammed.CompareMarker.stepLocationDown(true), canRepeat, Keys.Down | Keys.Shift | Keys.Alt);
            mCmdHandler.addCmd("Move compare marker or palette editor index left",
                () => mVrammed.stepLocationLeftCompareOrPalette(false), canRepeat, Keys.Left | Keys.Shift);
            mCmdHandler.addCmd("Move compare marker or palette editor index left alt",
                () => mVrammed.stepLocationLeftCompareOrPalette(true), canRepeat, Keys.Left | Keys.Shift | Keys.Alt);
            mCmdHandler.addCmd("Move compare marker or palette editor index right",
                () => mVrammed.stepLocationRightCompareOrPalette(false), canRepeat, Keys.Right | Keys.Shift);
            mCmdHandler.addCmd("Move compare marker or palette editor index right alt",
                () => mVrammed.stepLocationRightCompareOrPalette(true), canRepeat, Keys.Right | Keys.Shift | Keys.Alt);

            //Palette marker move.
            mCmdHandler.addCmd("Move palette marker up",
                () => mVrammed.PaletteMarker.stepLocationUp(false), canRepeat, Keys.Up | Keys.Control);
            mCmdHandler.addCmd("Move palette marker up alt",
                () => mVrammed.PaletteMarker.stepLocationUp(true), canRepeat, Keys.Up | Keys.Control | Keys.Alt);
            mCmdHandler.addCmd("Move palette marker down",
                () => mVrammed.PaletteMarker.stepLocationDown(false), canRepeat, Keys.Down | Keys.Control);
            mCmdHandler.addCmd("Move palette marker down alt",
                () => mVrammed.PaletteMarker.stepLocationDown(true), canRepeat, Keys.Down | Keys.Control | Keys.Alt);
            mCmdHandler.addCmd("Move palette marker left",
                () => mVrammed.PaletteMarker.stepLocationLeft(false), canRepeat, Keys.Left | Keys.Control);
            mCmdHandler.addCmd("Move palette marker left alt",
                () => mVrammed.PaletteMarker.stepLocationLeft(true), canRepeat, Keys.Left | Keys.Control | Keys.Alt);
            mCmdHandler.addCmd("Move palette marker right",
                () => mVrammed.PaletteMarker.stepLocationRight(false), canRepeat, Keys.Right | Keys.Control);
            mCmdHandler.addCmd("Move palette marker right alt",
                () => mVrammed.PaletteMarker.stepLocationRight(true), canRepeat, Keys.Right | Keys.Control | Keys.Alt);

            //Zoom +/- texture.
            mCmdHandler.addCmd("Zoom in texture",
                () => mVrammed.zoomInView(mTextureView), canRepeat, Keys.Add, Keys.Oemplus);
            mCmdHandler.addCmd("Zoom out texture",
                () => mVrammed.zoomOutView(mTextureView), canRepeat, Keys.Subtract, Keys.OemMinus);

            //Zoom +/- compare.
            mCmdHandler.addCmd("Zoom in compare",
                () => mVrammed.zoomInView(mCompareView), canRepeat, Keys.Add | Keys.Shift, Keys.Oemplus | Keys.Shift);
            mCmdHandler.addCmd("Zoom out compare",
                () => mVrammed.zoomOutView(mCompareView), canRepeat, Keys.Subtract | Keys.Shift, Keys.OemMinus | Keys.Shift);

            //Zoom +/- palette.
            mCmdHandler.addCmd("Zoom in palette",
                () => mVrammed.zoomInView(mPaletteView), canRepeat, Keys.Add | Keys.Control, Keys.Oemplus | Keys.Control);
            mCmdHandler.addCmd("Zoom out palette",
                () => mVrammed.zoomOutView(mPaletteView), canRepeat, Keys.Subtract | Keys.Control, Keys.OemMinus | Keys.Control);

            //Texture marker snap location.
            mCmdHandler.addCmd("Texture marker snap location",
                () => mVrammed.TextureMarker.snapLocation(false), Keys.Home);
            mCmdHandler.addCmd("Texture marker snap location alt",
                () => mVrammed.TextureMarker.snapLocation(true), Keys.Home | Keys.Alt);

            //Compare marker snap location.
            mCmdHandler.addCmd("Compare marker snap location",
                () => mVrammed.CompareMarker.snapLocation(false), Keys.Home | Keys.Shift);
            mCmdHandler.addCmd("Compare marker snap location alt",
                () => mVrammed.CompareMarker.snapLocation(true), Keys.Home | Keys.Shift | Keys.Alt);

            //Palette marker snap location.
            mCmdHandler.addCmd("Palette marker snap location",
                () => mVrammed.PaletteMarker.snapLocation(false), Keys.Home | Keys.Control);
            mCmdHandler.addCmd("Palette marker snap location alt",
                () => mVrammed.PaletteMarker.snapLocation(true), Keys.Home | Keys.Control | Keys.Alt); //Doesn't do anything?

            //Texture marker step resize.
            mCmdHandler.addCmd("Texture marker step size up",
                () => mVrammed.stepSizeTextureMarker(true, true, true), canRepeat, Keys.PageUp);
            mCmdHandler.addCmd("Texture marker step size down",
                () => mVrammed.stepSizeTextureMarker(false, true, true), canRepeat, Keys.PageDown);
            mCmdHandler.addCmd("Texture marker step width up",
                () => mVrammed.stepSizeTextureMarker(true, true, false), canRepeat, Keys.PageUp | Keys.Shift);
            mCmdHandler.addCmd("Texture marker step width down",
                () => mVrammed.stepSizeTextureMarker(false, true, false), canRepeat, Keys.PageDown | Keys.Shift);
            mCmdHandler.addCmd("Texture marker step height up",
                () => mVrammed.stepSizeTextureMarker(true, false, true), canRepeat, Keys.PageUp | Keys.Control);
            mCmdHandler.addCmd("Texture marker step height down",
                () => mVrammed.stepSizeTextureMarker(false, false, true), canRepeat, Keys.PageDown | Keys.Control);
            mCmdHandler.addCmd("Texture marker reset size to default",
                mVrammed.resetSizeTextureMarker, Keys.N);

            //Compare marker flip.
            mCmdHandler.addCmd("Compare marker flip horizontally",
                mVrammed.CompareMarker.toggleFlipH, Keys.H);
            mCmdHandler.addCmd("Compare marker flip vertically",
                mVrammed.CompareMarker.toggleFlipV, Keys.V);

            ////Compare marker toggle show result.
            //mCmdHandler.addCmd("Compare marker toggle show result",
            //    mVrammed.CompareMarker.toggleResultVisible, Keys.T | Keys.Shift);

            //Compare marker step result alpha.
            mCmdHandler.addCmd("Compare marker step result alpha up",
                () => mVrammed.CompareMarker.stepResultAlpha(1), canRepeat, Keys.G | Keys.Shift);
            mCmdHandler.addCmd("Compare marker step result alpha down",
                () => mVrammed.CompareMarker.stepResultAlpha(-1), canRepeat, Keys.B | Keys.Shift);

            //Edit slots.
            mCmdHandler.addCmd("Open slot 1 edit", () => mVrammed.openEditSlot(1), Keys.D1);
            mCmdHandler.addCmd("Open slot 2 edit", () => mVrammed.openEditSlot(2), Keys.D2);
            mCmdHandler.addCmd("Open slot 3 edit", () => mVrammed.openEditSlot(3), Keys.D3);
            mCmdHandler.addCmd("Open slot 4 edit", () => mVrammed.openEditSlot(4), Keys.D4);
            mCmdHandler.addCmd("Open slot 5 edit", () => mVrammed.openEditSlot(5), Keys.D5);
            mCmdHandler.addCmd("Open slot 6 edit", () => mVrammed.openEditSlot(6), Keys.D6);
            mCmdHandler.addCmd("Open slot 7 edit", () => mVrammed.openEditSlot(7), Keys.D7);
            mCmdHandler.addCmd("Open slot 8 edit", () => mVrammed.openEditSlot(8), Keys.D8);
            mCmdHandler.addCmd("Open slot 9 edit", () => mVrammed.openEditSlot(9), Keys.D9);
            mCmdHandler.addCmd("Open slot 10 edit", () => mVrammed.openEditSlot(10), Keys.D0);

#if LOGSTUFF
            //Tests.
            mCmdHandler.addCmd("test stuff", mVrammed.testStuff, Keys.W);

            //Add empty line to log.
            mCmdHandler.addCmd("Add empty line to log", () => Log.add(""), Keys.Space);
#endif

            Log.timeEnd("initKeyHandler()");

            if (mCmdHandler.HasKeyTableBuilder)
            {
                mCmdHandler.printKeyTable();
            }
        }
    }
}
