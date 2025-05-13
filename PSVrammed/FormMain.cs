using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using MyCustomStuff;

namespace PSVrammed
{
    partial class FormMain : Form
    {
        private bool mIgnoreMouseMove; //Ignore extra mouse move event caused by menu click away.
        private readonly Vrammed mVrammed;

        public FormMain()
        {
            Log.init(this); //Only if logging stuff.
            InitializeComponent();

            mIgnoreMouseMove = false;
            mVrammed = new Vrammed(this);

            init();
        }

        private void init()
        {
            mVrammed.init();
            mVrammed.FileChanged += Vrammed_FileChanged;
            mVrammed.ModeChanged += Vrammed_ModeChanged;
            mVrammed.ToolChanged += Vrammed_ToolChanged;
            mVrammed.VramMngr.VramChanged += VramMngr_VramChanged;

            mTextureView.init(mVrammed);
            mCompareView.init(mVrammed);
            mPaletteView.init(mVrammed);

            initCmdHandler();
        }

        private void loadProgramSettings()
        {
            this.WindowState = Properties.Settings.Default.MainWindowIsMaximized ?
                FormWindowState.Maximized : FormWindowState.Normal;
            this.DesktopBounds = Properties.Settings.Default.MainWindowBounds.IsEmpty ?
                this.DesktopBounds : Properties.Settings.Default.MainWindowBounds;
            mVrammed.Mode = Properties.Settings.Default.VrammedMode.toMode(Vrammed.ModeDefault);
            this.splitContainerMain.DistanceWhenVisible = Properties.Settings.Default.MainWindowSplitterDstMain <= 0 ?
                splitContainerMain.Height / 3 : Properties.Settings.Default.MainWindowSplitterDstMain;
            this.splitContainerTexCmp.DistanceWhenVisible = Properties.Settings.Default.MainWindowSplitterDstTexCmp <= 0 ?
                splitContainerMain.Height / 4 : Properties.Settings.Default.MainWindowSplitterDstTexCmp;
            this.splitContainerPalEdt.DistanceWhenVisible = Properties.Settings.Default.MainWindowSplitterDstPalEdt <= 0 ?
                splitContainerMain.Height / 4 : Properties.Settings.Default.MainWindowSplitterDstPalEdt;
            mVrammed.TextureMarker.Color = Properties.Settings.Default.TextureMarkerColor == Color.Transparent ?
                VramMarkerTexture.Default.Color : Properties.Settings.Default.TextureMarkerColor;
            mVrammed.CompareMarker.Color = Properties.Settings.Default.CompareMarkerColor == Color.Transparent ?
                VramMarkerCompare.Default.Color : Properties.Settings.Default.CompareMarkerColor;
            mVrammed.PaletteMarker.Color = Properties.Settings.Default.PaletteMarkerColor == Color.Transparent ?
                VramMarkerPalette.Default.Color : Properties.Settings.Default.PaletteMarkerColor;
            mVrammed.CompareMarker.ResultColor = Properties.Settings.Default.CompareResultColor == Color.Transparent ?
                VramMarkerCompare.Default.ResultColor : Properties.Settings.Default.CompareResultColor;
            mVrammed.CompareMarker.ResultAlpha = Properties.Settings.Default.CompareResultAlpha <= 0 ?
                VramMarkerCompare.Default.ResultAlpha : Properties.Settings.Default.CompareResultAlpha;
            mVrammed.TextureMarker.Location = Properties.Settings.Default.TextureMarkerLocation;
            mVrammed.TextureMarker.Size = Properties.Settings.Default.TextureMarkerSize.IsEmpty ?
                VramMarkerTexture.Default.SizeDef : Properties.Settings.Default.TextureMarkerSize;
            mVrammed.CompareMarker.Location = Properties.Settings.Default.CompareMarkerLocation;
            mVrammed.PaletteMarker.Location = Properties.Settings.Default.PaletteMarkerLocation;
            mVrammed.EditSlotFiles[0] = Properties.Settings.Default.EditFileSlot1;
            mVrammed.EditSlotFiles[1] = Properties.Settings.Default.EditFileSlot2;
            mVrammed.EditSlotFiles[2] = Properties.Settings.Default.EditFileSlot3;
            mVrammed.EditSlotFiles[3] = Properties.Settings.Default.EditFileSlot4;
            mVrammed.EditSlotFiles[4] = Properties.Settings.Default.EditFileSlot5;
            mVrammed.EditSlotFiles[5] = Properties.Settings.Default.EditFileSlot6;
            mVrammed.EditSlotFiles[6] = Properties.Settings.Default.EditFileSlot7;
            mVrammed.EditSlotFiles[7] = Properties.Settings.Default.EditFileSlot8;
            mVrammed.EditSlotFiles[8] = Properties.Settings.Default.EditFileSlot9;
            mVrammed.EditSlotFiles[9] = Properties.Settings.Default.EditFileSlot10;
        }

        private void saveProgramSettings()
        {
            Properties.Settings.Default.MainWindowIsMaximized = this.WindowState == FormWindowState.Maximized;
            Properties.Settings.Default.MainWindowBounds = this.WindowState == FormWindowState.Normal ?
                this.DesktopBounds : this.RestoreBounds;
            Properties.Settings.Default.VrammedMode = (int)mVrammed.Mode;
            Properties.Settings.Default.MainWindowSplitterDstMain = this.splitContainerMain.DistanceWhenVisible;
            Properties.Settings.Default.MainWindowSplitterDstTexCmp = this.splitContainerTexCmp.DistanceWhenVisible;
            Properties.Settings.Default.MainWindowSplitterDstPalEdt = this.splitContainerPalEdt.DistanceWhenVisible;
            Properties.Settings.Default.TextureMarkerColor = mVrammed.TextureMarker.Color;
            Properties.Settings.Default.CompareMarkerColor = mVrammed.CompareMarker.Color;
            Properties.Settings.Default.PaletteMarkerColor = mVrammed.PaletteMarker.Color;
            Properties.Settings.Default.CompareResultColor = mVrammed.CompareMarker.ResultColor;
            Properties.Settings.Default.CompareResultAlpha = mVrammed.CompareMarker.ResultAlpha;
            Properties.Settings.Default.TextureMarkerLocation = mVrammed.TextureMarker.Location;
            Properties.Settings.Default.TextureMarkerSize = mVrammed.TextureMarker.Size;
            Properties.Settings.Default.CompareMarkerLocation = mVrammed.CompareMarker.Location;
            Properties.Settings.Default.PaletteMarkerLocation = mVrammed.PaletteMarker.Location;
            Properties.Settings.Default.EditFileSlot1 = mVrammed.EditSlotFiles[0];
            Properties.Settings.Default.EditFileSlot2 = mVrammed.EditSlotFiles[1];
            Properties.Settings.Default.EditFileSlot3 = mVrammed.EditSlotFiles[2];
            Properties.Settings.Default.EditFileSlot4 = mVrammed.EditSlotFiles[3];
            Properties.Settings.Default.EditFileSlot5 = mVrammed.EditSlotFiles[4];
            Properties.Settings.Default.EditFileSlot6 = mVrammed.EditSlotFiles[5];
            Properties.Settings.Default.EditFileSlot7 = mVrammed.EditSlotFiles[6];
            Properties.Settings.Default.EditFileSlot8 = mVrammed.EditSlotFiles[7];
            Properties.Settings.Default.EditFileSlot9 = mVrammed.EditSlotFiles[8];
            Properties.Settings.Default.EditFileSlot10 = mVrammed.EditSlotFiles[9];
        }

        //*******************************************************************************
        //*******************************************************************************

        #region Properties

        public VramViewTexture TextureView
        {
            get { return mTextureView; }
        }

        public VramViewCompare CompareView
        {
            get { return mCompareView; }
        }

        public VramViewPalette PaletteView
        {
            get { return mPaletteView; }
        }

        public TableLayoutPanel PaletteTablePanel
        {
            get { return tableLayoutPanelPalette; }
        }

        public ToolStripStatusLabel StatusBarTexture
        {
            get { return statusBarTexture; }
        }

        public ToolStripStatusLabel StatusBarCompare
        {
            get { return statusBarCompare; }
        }

        public ToolStripStatusLabel StatusBarPalette
        {
            get { return statusBarPalette; }
        }

        public ToolStripStatusLabel StatusBarInspect
        {
            get { return statusBarInspect; }
        }

        #endregion Properties

        //*******************************************************************************
        //*******************************************************************************

        #region Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            //This must be done before opening any file to properly init the main form.
            loadProgramSettings();
            updateViews();

#if TESTOPENFILE
            string path = Vrammed.DebugBasePath + "test states\\";
            string file;
            //file = "invalid.pve"; //Invalid file.
            //file = "suikoden1 epsxe SLES_005.27.000"; //ePSXe (gzip compressed).
            file = "suikoden2 pSX.psv"; //pSX.
            //file = "suikoden1 bizhawk zst.State"; //BizHawk (zstandard compressed).
            //file = "bof3 zst.sav"; //DuckStation (zstandard compressed).
            //file = "suikoden2 vram 24bit.bin"; //1MB VRAM dump. Also 24bit mode test.

            mVrammed.openState(path + file);

            string edit;
            edit = "suikoden2 edits.pve";
            mVrammed.EditSlotFiles[0] = path + edit;
#else
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                mVrammed.openState(args[1]);
            }
#endif
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = mVrammed.checkChangesSaved();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveProgramSettings();
            Properties.Settings.Default.Save();
        }

        private void Vrammed_FileChanged(Vrammed sender, StateFile oldFile, StateFile newFile)
        {
            updateTitlebar();
            if (oldFile == null) //First file opened?
            {
                //Special update for the first file opened.
                //Files opened subsequently are handled by the vram changed update.
                updateViews();
                TextureView.scrollByMarkerCenter();
                CompareView.scrollByMarkerCenter();
                PaletteView.scrollByMarkerCenter();
            }
        }

        private void Vrammed_ModeChanged(Vrammed sender, Mode oldMode, Mode newMode)
        {
            updateViews();
            updateTitlebar();
            updateMouse();
        }

        private void Vrammed_ToolChanged(Vrammed sender, Tool oldTool, Tool newTool)
        {
            //If using shortcut keys mouse can be over a view.
            VramView vv = getViewUnderMouse();
            if (vv != null)
            {
                //Perform mouse leave and mouse enter to update tool.
                leaveMouse(oldTool, vv);
                enterMouse(newTool, vv);
            }

            //If no tool then set the default mouse cursor for all views.
            //Other tools set the mouse cursor in their enter/leave function.
            if (newTool == Tool.None)
            {
                mTextureView.Cursor = Cursors.Default;
                mCompareView.Cursor = Cursors.Default;
                mPaletteView.Cursor = Cursors.Default;
            }
        }

        private void VramMngr_VramChanged(VramMngr sender, bool isPaletteAffected)
        {
            //Mostly (only?) needed for updating the inspect pixel info at mouse cursor.
            if (mVrammed.Tool == Tool.Inspect)
            {
                updateMouse();
            }
        }

        #endregion Events

        //*******************************************************************************
        //*******************************************************************************

        public void updateTitlebar()
        {
            string fileTitle = mVrammed.getFileTitle();
            this.Text = fileTitle + " - " + Strings.ProductName + " [" + mVrammed.Mode.toDescr() + "]";
        }

        private void updateViews()
        {
            SuspendLayout(); //Try to reduce flicker when changing modes. Has no effect?

            bool showTexture = mVrammed.IsFileOpen;
            bool showCompare = mVrammed.IsFileOpen && mVrammed.Mode.isCompare();
            bool showPalette = mVrammed.IsFileOpen && mVrammed.Mode.usesPalette();
            bool showPalEdit = mVrammed.IsFileOpen && mVrammed.Mode.isPaletteEditor();

            splitContainerTexCmp.Panel1Collapsed = !showTexture;
            splitContainerTexCmp.Panel2Collapsed = !showCompare;
            splitContainerPalEdt.Panel1Collapsed = !showPalette;
            splitContainerPalEdt.Panel2Collapsed = !showPalEdit;
            splitContainerMain.Panel1Collapsed = !(showTexture || showCompare);
            splitContainerMain.Panel2Collapsed = !(showPalette || showPalEdit);

            mTextureView.Visible = showTexture;
            mCompareView.Visible = showCompare;
            mPaletteView.Visible = showPalette;

            mVrammed.PaletteEditor.Visible = showPalEdit;

            mVrammed.TextureMarker.IsVisible = showTexture;
            mVrammed.CompareMarker.IsVisible = showCompare;
            mVrammed.PaletteMarker.IsVisible = showPalette;

            updateSplitters();

            ResumeLayout();
        }

        private void updateSplitters()
        {
            splitContainerTexCmp.Panel1MinSize = splitContainerTexCmp.Panel1Collapsed ? 0 : 70;
            splitContainerTexCmp.Panel2MinSize = splitContainerTexCmp.Panel2Collapsed ? 0 : 70;
            splitContainerPalEdt.Panel1MinSize = splitContainerPalEdt.Panel1Collapsed ? 0 : 70;
            splitContainerPalEdt.Panel2MinSize = splitContainerPalEdt.Panel2Collapsed ? 0 : 70;
            splitContainerMain.Panel1MinSize = splitContainerTexCmp.SplitterWidth +
                splitContainerTexCmp.Panel1MinSize + splitContainerTexCmp.Panel2MinSize;
            splitContainerMain.Panel2MinSize = splitContainerPalEdt.SplitterWidth +
                splitContainerPalEdt.Panel1MinSize + splitContainerPalEdt.Panel2MinSize;

            //Splitter distances don't update properly when panels change. Force an update.
            splitContainerMain.updateDistance();
            splitContainerTexCmp.updateDistance();
            splitContainerPalEdt.updateDistance();
        }

        //*******************************************************************************
        //*******************************************************************************

        #region Menu functions

        private void mainMenuFile_DropDownOpening(object sender, EventArgs e)
        {
            mainMenuFileReopen.Enabled = mVrammed.CanReopen;
            mainMenuFileSave.Enabled = mVrammed.CanSave;
            mainMenuFileSaveAs.Enabled = mVrammed.CanSave;
            mainMenuFileSaveTexture.Enabled = mVrammed.CanSaveTexture;
            mainMenuFileSaveTextureAll.Enabled = mVrammed.CanSaveTextureAll;
            mainMenuFileOpenEdit.Enabled = mVrammed.CanOpenEdit;
            mainMenuFileSaveEdit.Enabled = mVrammed.CanSaveEdit;
            mainMenuFileInfo.Enabled = mVrammed.CanShowFileInfo;
        }

        private void mainMenuEdit_DropDownOpening(object sender, EventArgs e)
        {
            mainMenuEditUndo.Enabled = mVrammed.VramMngr.CanEditUndo;
            mainMenuEditRedo.Enabled = mVrammed.VramMngr.CanEditRedo;
            mainMenuEditSetValue.Enabled = mVrammed.VramMngr.CanEditValue;
            mainMenuEditSetAlpha.Enabled = mVrammed.VramMngr.CanEditAlpha;
            mainMenuEditClearAlpha.Enabled = mVrammed.VramMngr.CanEditAlpha;
            mainMenuEditInvert.Enabled = mVrammed.VramMngr.CanEditInvert;
            mainMenuEditInvertAll.Enabled = mVrammed.VramMngr.CanEditInvertAll;
            mainMenuEditCopy.Enabled = mVrammed.VramMngr.CanEditCopy;
            mainMenuEditCut.Enabled = mVrammed.VramMngr.CanEditCut;
            mainMenuEditPaste.Enabled = mVrammed.VramMngr.CanEditPaste;
            mainMenuEditTextureClipboard.Enabled = mVrammed.CanSaveTexture;
            mainMenuEditTextureAllClipboard.Enabled = mVrammed.CanSaveTextureAll;
            mainMenuEditPaletteEntry.Enabled = mVrammed.PaletteEditor.CanEditEntry;
            mainMenuEditPaletteEntryRepeat.Enabled = mVrammed.PaletteEditor.CanEditEntry;
        }

        private void mainMenuMode_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in mainMenuMode.DropDownItems)
            {
                item.Enabled = mVrammed.IsFileOpen;
            }
            Mode mode = mVrammed.Mode;
            mainMenuMode4bitView.Checked = mode == Mode.Ind4BitView;
            mainMenuMode4bitComp.Checked = mode == Mode.Ind4BitComp;
            mainMenuMode4bitPale.Checked = mode == Mode.Ind4BitPalE;
            mainMenuMode8bitView.Checked = mode == Mode.Ind8BitView;
            mainMenuMode8bitComp.Checked = mode == Mode.Ind8BitComp;
            mainMenuMode8bitPale.Checked = mode == Mode.Ind8BitPalE;
            mainMenuMode16bitView.Checked = mode == Mode.Rgb16BitView;
            mainMenuMode24bitView.Checked = mode == Mode.Rgb24BitView;
        }

        private void mainMenuTool_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in mainMenuTool.DropDownItems)
            {
                item.Enabled = mVrammed.IsFileOpen;
            }
            Tool tool = mVrammed.Tool;
            mainMenuToolNone.Checked = tool == Tool.None;
            mainMenuToolZoom.Checked = tool == Tool.Zoom;
            mainMenuToolSituate.Checked = tool == Tool.Situate;
            mainMenuToolInspect.Checked = tool == Tool.Inspect;
            mainMenuToolEdit.Checked = tool == Tool.Edit;
        }

        private void mainMenuConfig_DropDownOpening(object sender, EventArgs e)
        {
            mainMenuConfigShowAlphaTexture.Checked = mVrammed.VramMngr.ShowAlphaTexture;
            mainMenuConfigShowAlphaCompare.Checked = mVrammed.VramMngr.ShowAlphaCompare;
            mainMenuConfigShowAlphaPalette.Checked = mVrammed.VramMngr.ShowAlphaPalette;

            mainMenuConfigUpdateSituatingMarker.Checked = mVrammed.UpdateSituatingMarker;
        }

        #endregion Menu functions

        //*******************************************************************************
        //*******************************************************************************

        #region Keyboard presses

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (!mainMenu.Focused) //Not needed? Menu always steals input anyway?
            {
                if (mCmdHandler.call(keyData, msg)) return true; //Key handled.
            }
            return base.ProcessCmdKey(ref msg, keyData); //Key not handled.
        }

        #endregion Keyboard presses

        //*******************************************************************************
        //*******************************************************************************

        #region Mouse functions

        private VramView getViewUnderMouse()
        {
            VramView vv = null;
            if (mTextureView.isMouseOver(MousePosition)) vv = TextureView;
            else if (mCompareView.isMouseOver(MousePosition)) vv = CompareView;
            else if (mPaletteView.isMouseOver(MousePosition)) vv = PaletteView;

            //Log.add("Mouse is over " + (vv == null ? "no view" : vv.Name));
            return vv;
        }

        private void VramView_MouseDown(object sender, MouseEventArgs e)
        {
            VramView vv = (VramView)sender;

            //Get point in image from mouse's client location.
            Point point = vv.getObjectPointFromClientInside(e.Location);

            //Check which tool is used and which mouse button was pressed.
            Tool tool = mVrammed.Tool;
            if (tool == Tool.Zoom)
            {
                if (e.Button == MouseButtons.Left) mVrammed.toolPerformZoomUp(vv, point);
                else if (e.Button == MouseButtons.Right) mVrammed.toolPerformZoomDown(vv, point);
            }
            else if (tool == Tool.Situate)
            {
                if (e.Button == MouseButtons.Left) mVrammed.toolSaveSituate(vv, point);
                else if (e.Button == MouseButtons.Right) mVrammed.toolQuitSituate(vv);
            }
            else if (tool == Tool.Inspect)
            {
                if (e.Button == MouseButtons.Left) mVrammed.PixelInspector.save(vv, point);
                else if (e.Button == MouseButtons.Right) mVrammed.PixelInspector.quit(vv);
            }
            else if (tool == Tool.Edit && vv == mTextureView)
            {
                //Ignore mouse move after click so edit is not performed after menu click away.
                mIgnoreMouseMove = true;
                if (e.Button == MouseButtons.Left) mVrammed.toolPerformEdit(vv, ModifierKeys);
                else if (e.Button == MouseButtons.Right) mVrammed.toolQuitEdit(vv);
            }
        }

        private void VramView_MouseEnter(object sender, EventArgs e)
        {
            enterMouse(mVrammed.Tool, (VramView)sender);
        }

        private void VramView_MouseLeave(object sender, EventArgs e)
        {
            leaveMouse(mVrammed.Tool, (VramView)sender);
        }

        private void VramView_MouseMove(object sender, MouseEventArgs e)
        {
            if (mIgnoreMouseMove)
            {
                mIgnoreMouseMove = false;
            }
            else
            {
                moveMouse((VramView)sender, e.Location);
            }
        }

        private void enterMouse(Tool tool, VramView vv)
        {
            if (tool == Tool.Zoom)
            {
                mVrammed.toolEnterZoom(vv);
            }
            else if (tool == Tool.Situate)
            {
                mVrammed.toolEnterSituate(vv, MousePosition);
            }
            else if (tool == Tool.Inspect)
            {
                mVrammed.PixelInspector.enter(vv, MousePosition);
            }
            else if (tool == Tool.Edit && vv == mTextureView)
            {
                mVrammed.toolEnterEdit(vv, MousePosition);
            }
        }

        private void leaveMouse(Tool tool, VramView vv)
        {
            if (tool == Tool.Zoom)
            {
                mVrammed.toolLeaveZoom(vv);
            }
            else if (tool == Tool.Situate)
            {
                mVrammed.toolLeaveSituate(vv);
            }
            else if (tool == Tool.Inspect)
            {
                mVrammed.PixelInspector.leave(vv);
            }
            else if (tool == Tool.Edit) // && vv == mTextureView) //???
            {
                mVrammed.toolLeaveEdit(vv);
            }
        }

        private void moveMouse(VramView vv, Point mouseClient)
        {
            Tool tool = mVrammed.Tool;
            if (tool == Tool.Situate)
            {
                mVrammed.toolMoveSituate(vv, mouseClient);
            }
            else if (tool == Tool.Inspect)
            {
                mVrammed.PixelInspector.move(vv, mouseClient);
            }
            else if (tool == Tool.Edit && vv == mTextureView)
            {
                mVrammed.toolMoveEdit(vv, mouseClient);
                if (MouseButtons == MouseButtons.Left)
                {
                    mVrammed.toolPerformEdit(vv, ModifierKeys);
                }
            }
        }

        private void updateMouse()
        {
            if (mVrammed.Tool != Tool.None)
            {
                VramView vv = getViewUnderMouse();
                if (vv != null)
                {
                    moveMouse(vv, vv.PointToClient(MousePosition));
                }
            }
        }

        #endregion Mouse functions
    }
}
