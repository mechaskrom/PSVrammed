namespace PSVrammed
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.mainMenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileReopen = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuFileSaveTexture = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSaveTextureAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuFileOpenEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSaveEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuFileInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuFileSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mainMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuEditSetValue = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditClearAlpha = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditSetAlpha = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditInvertAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode4bitView = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode4bitComp = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode4bitPale = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode8bitView = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode8bitComp = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode8bitPale = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode16bitView = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuMode24bitView = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuTool = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuToolNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuToolZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuToolSituate = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuToolInspect = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuToolEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigEditValues = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigEditSlots = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuConfigShowAlphaTexture = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigShowAlphaCompare = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigShowAlphaPalette = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuConfigUpdateSituatingMarker = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarTexture = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarCompare = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarPalette = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarInspect = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuEditTextureClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditTextureAllClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainerMain = new MyCustomStuff.SmoothSplitContainer();
            this.splitContainerTexCmp = new MyCustomStuff.SmoothSplitContainer();
            this.mTextureView = new PSVrammed.VramViewTexture();
            this.mCompareView = new PSVrammed.VramViewCompare();
            this.splitContainerPalEdt = new MyCustomStuff.SmoothSplitContainer();
            this.mPaletteView = new PSVrammed.VramViewPalette();
            this.tableLayoutPanelPalette = new System.Windows.Forms.TableLayoutPanel();
            this.mainMenuEditSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuEditPaletteEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuEditPaletteEntryRepeat = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerTexCmp.Panel1.SuspendLayout();
            this.splitContainerTexCmp.Panel2.SuspendLayout();
            this.splitContainerTexCmp.SuspendLayout();
            this.splitContainerPalEdt.Panel1.SuspendLayout();
            this.splitContainerPalEdt.Panel2.SuspendLayout();
            this.splitContainerPalEdt.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuFile
            // 
            this.mainMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuFileOpen,
            this.mainMenuFileReopen,
            this.mainMenuFileSave,
            this.mainMenuFileSaveAs,
            this.mainMenuFileSep1,
            this.mainMenuFileSaveTexture,
            this.mainMenuFileSaveTextureAll,
            this.mainMenuFileSep2,
            this.mainMenuFileOpenEdit,
            this.mainMenuFileSaveEdit,
            this.mainMenuFileSep3,
            this.mainMenuFileInfo,
            this.mainMenuFileAbout,
            this.mainMenuFileSep4,
            this.mainMenuFileExit});
            this.mainMenuFile.Name = "mainMenuFile";
            this.mainMenuFile.Size = new System.Drawing.Size(37, 20);
            this.mainMenuFile.Text = "&File";
            this.mainMenuFile.DropDownOpening += new System.EventHandler(this.mainMenuFile_DropDownOpening);
            // 
            // mainMenuFileOpen
            // 
            this.mainMenuFileOpen.Image = global::PSVrammed.Properties.Resources.file_open;
            this.mainMenuFileOpen.ImageTransparentColor = System.Drawing.Color.White;
            this.mainMenuFileOpen.Name = "mainMenuFileOpen";
            this.mainMenuFileOpen.ShortcutKeyDisplayString = "";
            this.mainMenuFileOpen.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileOpen.Text = "&Open...";
            // 
            // mainMenuFileReopen
            // 
            this.mainMenuFileReopen.Name = "mainMenuFileReopen";
            this.mainMenuFileReopen.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileReopen.Text = "Reopen";
            // 
            // mainMenuFileSave
            // 
            this.mainMenuFileSave.Image = global::PSVrammed.Properties.Resources.file_save;
            this.mainMenuFileSave.ImageTransparentColor = System.Drawing.Color.White;
            this.mainMenuFileSave.Name = "mainMenuFileSave";
            this.mainMenuFileSave.ShortcutKeyDisplayString = "";
            this.mainMenuFileSave.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileSave.Text = "&Save";
            // 
            // mainMenuFileSaveAs
            // 
            this.mainMenuFileSaveAs.Name = "mainMenuFileSaveAs";
            this.mainMenuFileSaveAs.ShortcutKeyDisplayString = "";
            this.mainMenuFileSaveAs.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileSaveAs.Text = "Save &as...";
            // 
            // mainMenuFileSep1
            // 
            this.mainMenuFileSep1.Name = "mainMenuFileSep1";
            this.mainMenuFileSep1.Size = new System.Drawing.Size(185, 6);
            // 
            // mainMenuFileSaveTexture
            // 
            this.mainMenuFileSaveTexture.Image = global::PSVrammed.Properties.Resources.file_texture;
            this.mainMenuFileSaveTexture.Name = "mainMenuFileSaveTexture";
            this.mainMenuFileSaveTexture.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileSaveTexture.Text = "Save &texture...";
            // 
            // mainMenuFileSaveTextureAll
            // 
            this.mainMenuFileSaveTextureAll.ImageTransparentColor = System.Drawing.Color.White;
            this.mainMenuFileSaveTextureAll.Name = "mainMenuFileSaveTextureAll";
            this.mainMenuFileSaveTextureAll.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileSaveTextureAll.Text = "Save t&exture all...";
            // 
            // mainMenuFileSep2
            // 
            this.mainMenuFileSep2.Name = "mainMenuFileSep2";
            this.mainMenuFileSep2.Size = new System.Drawing.Size(185, 6);
            // 
            // mainMenuFileOpenEdit
            // 
            this.mainMenuFileOpenEdit.Name = "mainMenuFileOpenEdit";
            this.mainMenuFileOpenEdit.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileOpenEdit.Text = "Open edit se&quence...";
            // 
            // mainMenuFileSaveEdit
            // 
            this.mainMenuFileSaveEdit.Name = "mainMenuFileSaveEdit";
            this.mainMenuFileSaveEdit.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileSaveEdit.Text = "Save edit seq&uence...";
            // 
            // mainMenuFileSep3
            // 
            this.mainMenuFileSep3.Name = "mainMenuFileSep3";
            this.mainMenuFileSep3.Size = new System.Drawing.Size(185, 6);
            // 
            // mainMenuFileInfo
            // 
            this.mainMenuFileInfo.Image = global::PSVrammed.Properties.Resources.file_info;
            this.mainMenuFileInfo.ImageTransparentColor = System.Drawing.Color.White;
            this.mainMenuFileInfo.Name = "mainMenuFileInfo";
            this.mainMenuFileInfo.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileInfo.Text = "Show file i&nfo...";
            // 
            // mainMenuFileAbout
            // 
            this.mainMenuFileAbout.Name = "mainMenuFileAbout";
            this.mainMenuFileAbout.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileAbout.Text = "A&bout...";
            // 
            // mainMenuFileSep4
            // 
            this.mainMenuFileSep4.Name = "mainMenuFileSep4";
            this.mainMenuFileSep4.Size = new System.Drawing.Size(185, 6);
            // 
            // mainMenuFileExit
            // 
            this.mainMenuFileExit.Name = "mainMenuFileExit";
            this.mainMenuFileExit.Size = new System.Drawing.Size(188, 22);
            this.mainMenuFileExit.Text = "E&xit";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuFile,
            this.mainMenuEdit,
            this.mainMenuMode,
            this.mainMenuTool,
            this.mainMenuConfig});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(792, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "mainMenu";
            // 
            // mainMenuEdit
            // 
            this.mainMenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuEditUndo,
            this.mainMenuEditRedo,
            this.mainMenuEditSep1,
            this.mainMenuEditSetValue,
            this.mainMenuEditClearAlpha,
            this.mainMenuEditSetAlpha,
            this.mainMenuEditInvert,
            this.mainMenuEditInvertAll,
            this.mainMenuEditSep2,
            this.mainMenuEditCopy,
            this.mainMenuEditCut,
            this.mainMenuEditPaste,
            this.mainMenuEditSep3,
            this.mainMenuEditTextureClipboard,
            this.mainMenuEditTextureAllClipboard,
            this.mainMenuEditSep4,
            this.mainMenuEditPaletteEntry,
            this.mainMenuEditPaletteEntryRepeat});
            this.mainMenuEdit.Name = "mainMenuEdit";
            this.mainMenuEdit.ShortcutKeyDisplayString = "";
            this.mainMenuEdit.Size = new System.Drawing.Size(39, 20);
            this.mainMenuEdit.Text = "&Edit";
            this.mainMenuEdit.DropDownOpening += new System.EventHandler(this.mainMenuEdit_DropDownOpening);
            // 
            // mainMenuEditUndo
            // 
            this.mainMenuEditUndo.Name = "mainMenuEditUndo";
            this.mainMenuEditUndo.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditUndo.Text = "&Undo";
            // 
            // mainMenuEditRedo
            // 
            this.mainMenuEditRedo.Name = "mainMenuEditRedo";
            this.mainMenuEditRedo.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditRedo.Text = "&Redo";
            // 
            // mainMenuEditSep1
            // 
            this.mainMenuEditSep1.Name = "mainMenuEditSep1";
            this.mainMenuEditSep1.Size = new System.Drawing.Size(191, 6);
            // 
            // mainMenuEditSetValue
            // 
            this.mainMenuEditSetValue.Name = "mainMenuEditSetValue";
            this.mainMenuEditSetValue.ShortcutKeyDisplayString = "";
            this.mainMenuEditSetValue.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditSetValue.Text = "Set &value";
            // 
            // mainMenuEditClearAlpha
            // 
            this.mainMenuEditClearAlpha.Name = "mainMenuEditClearAlpha";
            this.mainMenuEditClearAlpha.ShortcutKeyDisplayString = "";
            this.mainMenuEditClearAlpha.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditClearAlpha.Text = "Clear alp&ha";
            // 
            // mainMenuEditSetAlpha
            // 
            this.mainMenuEditSetAlpha.Name = "mainMenuEditSetAlpha";
            this.mainMenuEditSetAlpha.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditSetAlpha.Text = "Set &alpha";
            // 
            // mainMenuEditInvert
            // 
            this.mainMenuEditInvert.Name = "mainMenuEditInvert";
            this.mainMenuEditInvert.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditInvert.Text = "&Invert";
            // 
            // mainMenuEditInvertAll
            // 
            this.mainMenuEditInvertAll.Name = "mainMenuEditInvertAll";
            this.mainMenuEditInvertAll.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditInvertAll.Text = "I&nvert all";
            // 
            // mainMenuEditSep2
            // 
            this.mainMenuEditSep2.Name = "mainMenuEditSep2";
            this.mainMenuEditSep2.Size = new System.Drawing.Size(191, 6);
            // 
            // mainMenuEditCopy
            // 
            this.mainMenuEditCopy.Name = "mainMenuEditCopy";
            this.mainMenuEditCopy.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditCopy.Text = "&Copy";
            // 
            // mainMenuEditCut
            // 
            this.mainMenuEditCut.Name = "mainMenuEditCut";
            this.mainMenuEditCut.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditCut.Text = "Cu&t";
            // 
            // mainMenuEditPaste
            // 
            this.mainMenuEditPaste.Name = "mainMenuEditPaste";
            this.mainMenuEditPaste.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditPaste.Text = "&Paste";
            // 
            // mainMenuMode
            // 
            this.mainMenuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuMode4bitView,
            this.mainMenuMode4bitComp,
            this.mainMenuMode4bitPale,
            this.mainMenuMode8bitView,
            this.mainMenuMode8bitComp,
            this.mainMenuMode8bitPale,
            this.mainMenuMode16bitView,
            this.mainMenuMode24bitView});
            this.mainMenuMode.Name = "mainMenuMode";
            this.mainMenuMode.Size = new System.Drawing.Size(50, 20);
            this.mainMenuMode.Text = "&Mode";
            this.mainMenuMode.DropDownOpening += new System.EventHandler(this.mainMenuMode_DropDownOpening);
            // 
            // mainMenuMode4bitView
            // 
            this.mainMenuMode4bitView.Checked = true;
            this.mainMenuMode4bitView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mainMenuMode4bitView.Name = "mainMenuMode4bitView";
            this.mainMenuMode4bitView.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode4bitView.Text = "4 bit indexed view";
            // 
            // mainMenuMode4bitComp
            // 
            this.mainMenuMode4bitComp.Name = "mainMenuMode4bitComp";
            this.mainMenuMode4bitComp.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode4bitComp.Text = "4 bit indexed compare";
            // 
            // mainMenuMode4bitPale
            // 
            this.mainMenuMode4bitPale.Name = "mainMenuMode4bitPale";
            this.mainMenuMode4bitPale.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode4bitPale.Text = "4 bit indexed palette editor";
            // 
            // mainMenuMode8bitView
            // 
            this.mainMenuMode8bitView.Name = "mainMenuMode8bitView";
            this.mainMenuMode8bitView.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode8bitView.Text = "8 bit indexed view";
            // 
            // mainMenuMode8bitComp
            // 
            this.mainMenuMode8bitComp.Name = "mainMenuMode8bitComp";
            this.mainMenuMode8bitComp.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode8bitComp.Text = "8 bit indexed compare";
            // 
            // mainMenuMode8bitPale
            // 
            this.mainMenuMode8bitPale.Name = "mainMenuMode8bitPale";
            this.mainMenuMode8bitPale.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode8bitPale.Text = "8 bit indexed palette editor";
            // 
            // mainMenuMode16bitView
            // 
            this.mainMenuMode16bitView.Name = "mainMenuMode16bitView";
            this.mainMenuMode16bitView.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode16bitView.Text = "16 bit rgb view";
            // 
            // mainMenuMode24bitView
            // 
            this.mainMenuMode24bitView.Name = "mainMenuMode24bitView";
            this.mainMenuMode24bitView.Size = new System.Drawing.Size(215, 22);
            this.mainMenuMode24bitView.Text = "24 bit rgb view";
            // 
            // mainMenuTool
            // 
            this.mainMenuTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuToolNone,
            this.mainMenuToolZoom,
            this.mainMenuToolSituate,
            this.mainMenuToolInspect,
            this.mainMenuToolEdit});
            this.mainMenuTool.Name = "mainMenuTool";
            this.mainMenuTool.Size = new System.Drawing.Size(41, 20);
            this.mainMenuTool.Text = "&Tool";
            this.mainMenuTool.DropDownOpening += new System.EventHandler(this.mainMenuTool_DropDownOpening);
            // 
            // mainMenuToolNone
            // 
            this.mainMenuToolNone.Name = "mainMenuToolNone";
            this.mainMenuToolNone.Size = new System.Drawing.Size(150, 22);
            this.mainMenuToolNone.Text = "&None";
            // 
            // mainMenuToolZoom
            // 
            this.mainMenuToolZoom.Name = "mainMenuToolZoom";
            this.mainMenuToolZoom.Size = new System.Drawing.Size(150, 22);
            this.mainMenuToolZoom.Text = "&Zoom image";
            // 
            // mainMenuToolSituate
            // 
            this.mainMenuToolSituate.Name = "mainMenuToolSituate";
            this.mainMenuToolSituate.Size = new System.Drawing.Size(150, 22);
            this.mainMenuToolSituate.Text = "&Situate marker";
            // 
            // mainMenuToolInspect
            // 
            this.mainMenuToolInspect.Name = "mainMenuToolInspect";
            this.mainMenuToolInspect.Size = new System.Drawing.Size(150, 22);
            this.mainMenuToolInspect.Text = "&Inspect pixel";
            // 
            // mainMenuToolEdit
            // 
            this.mainMenuToolEdit.Name = "mainMenuToolEdit";
            this.mainMenuToolEdit.Size = new System.Drawing.Size(150, 22);
            this.mainMenuToolEdit.Text = "&Edit";
            // 
            // mainMenuConfig
            // 
            this.mainMenuConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuConfigMarkers,
            this.mainMenuConfigEditValues,
            this.mainMenuConfigEditSlots,
            this.mainMenuConfigSep1,
            this.mainMenuConfigShowAlphaTexture,
            this.mainMenuConfigShowAlphaCompare,
            this.mainMenuConfigShowAlphaPalette,
            this.mainMenuConfigUpdateSituatingMarker});
            this.mainMenuConfig.Name = "mainMenuConfig";
            this.mainMenuConfig.Size = new System.Drawing.Size(55, 20);
            this.mainMenuConfig.Text = "&Config";
            this.mainMenuConfig.DropDownOpening += new System.EventHandler(this.mainMenuConfig_DropDownOpening);
            // 
            // mainMenuConfigMarkers
            // 
            this.mainMenuConfigMarkers.Image = global::PSVrammed.Properties.Resources.config_settings;
            this.mainMenuConfigMarkers.ImageTransparentColor = System.Drawing.Color.White;
            this.mainMenuConfigMarkers.Name = "mainMenuConfigMarkers";
            this.mainMenuConfigMarkers.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigMarkers.Text = "&Markers...";
            // 
            // mainMenuConfigEditValues
            // 
            this.mainMenuConfigEditValues.Name = "mainMenuConfigEditValues";
            this.mainMenuConfigEditValues.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigEditValues.Text = "Edit &values...";
            // 
            // mainMenuConfigEditSlots
            // 
            this.mainMenuConfigEditSlots.Name = "mainMenuConfigEditSlots";
            this.mainMenuConfigEditSlots.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigEditSlots.Text = "Edit sl&ots...";
            // 
            // mainMenuConfigSep1
            // 
            this.mainMenuConfigSep1.Name = "mainMenuConfigSep1";
            this.mainMenuConfigSep1.Size = new System.Drawing.Size(198, 6);
            // 
            // mainMenuConfigShowAlphaTexture
            // 
            this.mainMenuConfigShowAlphaTexture.Name = "mainMenuConfigShowAlphaTexture";
            this.mainMenuConfigShowAlphaTexture.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigShowAlphaTexture.Text = "Show &texture alpha";
            // 
            // mainMenuConfigShowAlphaCompare
            // 
            this.mainMenuConfigShowAlphaCompare.Name = "mainMenuConfigShowAlphaCompare";
            this.mainMenuConfigShowAlphaCompare.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigShowAlphaCompare.Text = "Show &compare alpha";
            // 
            // mainMenuConfigShowAlphaPalette
            // 
            this.mainMenuConfigShowAlphaPalette.Name = "mainMenuConfigShowAlphaPalette";
            this.mainMenuConfigShowAlphaPalette.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigShowAlphaPalette.Text = "Show &palette alpha";
            // 
            // mainMenuConfigUpdateSituatingMarker
            // 
            this.mainMenuConfigUpdateSituatingMarker.Name = "mainMenuConfigUpdateSituatingMarker";
            this.mainMenuConfigUpdateSituatingMarker.Size = new System.Drawing.Size(201, 22);
            this.mainMenuConfigUpdateSituatingMarker.Text = "&Update situating marker";
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarTexture,
            this.statusBarCompare,
            this.statusBarPalette,
            this.statusBarInspect});
            this.statusBar.Location = new System.Drawing.Point(0, 542);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(792, 24);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusBar";
            // 
            // statusBarTexture
            // 
            this.statusBarTexture.Name = "statusBarTexture";
            this.statusBarTexture.Size = new System.Drawing.Size(48, 19);
            this.statusBarTexture.Text = "Texture:";
            this.statusBarTexture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusBarCompare
            // 
            this.statusBarCompare.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusBarCompare.Name = "statusBarCompare";
            this.statusBarCompare.Size = new System.Drawing.Size(63, 19);
            this.statusBarCompare.Text = "Compare:";
            this.statusBarCompare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusBarPalette
            // 
            this.statusBarPalette.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusBarPalette.Name = "statusBarPalette";
            this.statusBarPalette.Size = new System.Drawing.Size(50, 19);
            this.statusBarPalette.Text = "Palette:";
            this.statusBarPalette.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusBarInspect
            // 
            this.statusBarInspect.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusBarInspect.Name = "statusBarInspect";
            this.statusBarInspect.Size = new System.Drawing.Size(616, 19);
            this.statusBarInspect.Spring = true;
            this.statusBarInspect.Text = "Inspect:";
            this.statusBarInspect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainMenuEditTextureClipboard
            // 
            this.mainMenuEditTextureClipboard.Name = "mainMenuEditTextureClipboard";
            this.mainMenuEditTextureClipboard.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditTextureClipboard.Text = "Texture to clip&board";
            // 
            // mainMenuEditTextureAllClipboard
            // 
            this.mainMenuEditTextureAllClipboard.Name = "mainMenuEditTextureAllClipboard";
            this.mainMenuEditTextureAllClipboard.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditTextureAllClipboard.Text = "Texture all to clipb&oard";
            // 
            // mainMenuEditSep3
            // 
            this.mainMenuEditSep3.Name = "mainMenuEditSep3";
            this.mainMenuEditSep3.Size = new System.Drawing.Size(191, 6);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerMain.CanKeepFocus = false;
            this.splitContainerMain.DistanceWhenVisible = 259;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.MinimumSize = new System.Drawing.Size(80, 80);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTexCmp);
            this.splitContainerMain.Panel1MinSize = 70;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerPalEdt);
            this.splitContainerMain.Panel2MinSize = 70;
            this.splitContainerMain.Size = new System.Drawing.Size(792, 518);
            this.splitContainerMain.SplitterDistance = 259;
            this.splitContainerMain.SplitterWidth = 6;
            this.splitContainerMain.TabIndex = 2;
            this.splitContainerMain.TabStop = false;
            this.splitContainerMain.UseSmoothSplitter = true;
            // 
            // splitContainerTexCmp
            // 
            this.splitContainerTexCmp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerTexCmp.CanKeepFocus = false;
            this.splitContainerTexCmp.DistanceWhenVisible = 135;
            this.splitContainerTexCmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTexCmp.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTexCmp.MinimumSize = new System.Drawing.Size(80, 80);
            this.splitContainerTexCmp.Name = "splitContainerTexCmp";
            this.splitContainerTexCmp.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTexCmp.Panel1
            // 
            this.splitContainerTexCmp.Panel1.Controls.Add(this.mTextureView);
            this.splitContainerTexCmp.Panel1MinSize = 70;
            // 
            // splitContainerTexCmp.Panel2
            // 
            this.splitContainerTexCmp.Panel2.Controls.Add(this.mCompareView);
            this.splitContainerTexCmp.Panel2MinSize = 70;
            this.splitContainerTexCmp.Size = new System.Drawing.Size(792, 259);
            this.splitContainerTexCmp.SplitterDistance = 135;
            this.splitContainerTexCmp.SplitterWidth = 6;
            this.splitContainerTexCmp.TabIndex = 0;
            this.splitContainerTexCmp.TabStop = false;
            this.splitContainerTexCmp.UseSmoothSplitter = true;
            // 
            // mTextureView
            // 
            this.mTextureView.AutoScroll = true;
            this.mTextureView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTextureView.Location = new System.Drawing.Point(0, 0);
            this.mTextureView.Name = "mTextureView";
            this.mTextureView.Size = new System.Drawing.Size(788, 131);
            this.mTextureView.TabIndex = 0;
            this.mTextureView.TabStop = false;
            this.mTextureView.Text = "TextureView";
            this.mTextureView.Zoom = 1F;
            this.mTextureView.ZoomMax = 8F;
            this.mTextureView.ZoomMin = 1F;
            this.mTextureView.MouseLeave += new System.EventHandler(this.VramView_MouseLeave);
            this.mTextureView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VramView_MouseMove);
            this.mTextureView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VramView_MouseDown);
            this.mTextureView.MouseEnter += new System.EventHandler(this.VramView_MouseEnter);
            // 
            // mCompareView
            // 
            this.mCompareView.AutoScroll = true;
            this.mCompareView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mCompareView.Location = new System.Drawing.Point(0, 0);
            this.mCompareView.Name = "mCompareView";
            this.mCompareView.Size = new System.Drawing.Size(788, 114);
            this.mCompareView.TabIndex = 0;
            this.mCompareView.TabStop = false;
            this.mCompareView.Text = "CompareView";
            this.mCompareView.Zoom = 1F;
            this.mCompareView.ZoomMax = 8F;
            this.mCompareView.ZoomMin = 1F;
            this.mCompareView.MouseLeave += new System.EventHandler(this.VramView_MouseLeave);
            this.mCompareView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VramView_MouseMove);
            this.mCompareView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VramView_MouseDown);
            this.mCompareView.MouseEnter += new System.EventHandler(this.VramView_MouseEnter);
            // 
            // splitContainerPalEdt
            // 
            this.splitContainerPalEdt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerPalEdt.CanKeepFocus = false;
            this.splitContainerPalEdt.DistanceWhenVisible = 114;
            this.splitContainerPalEdt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPalEdt.Location = new System.Drawing.Point(0, 0);
            this.splitContainerPalEdt.MinimumSize = new System.Drawing.Size(80, 80);
            this.splitContainerPalEdt.Name = "splitContainerPalEdt";
            this.splitContainerPalEdt.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerPalEdt.Panel1
            // 
            this.splitContainerPalEdt.Panel1.Controls.Add(this.mPaletteView);
            this.splitContainerPalEdt.Panel1MinSize = 70;
            // 
            // splitContainerPalEdt.Panel2
            // 
            this.splitContainerPalEdt.Panel2.Controls.Add(this.tableLayoutPanelPalette);
            this.splitContainerPalEdt.Panel2MinSize = 70;
            this.splitContainerPalEdt.Size = new System.Drawing.Size(792, 253);
            this.splitContainerPalEdt.SplitterDistance = 114;
            this.splitContainerPalEdt.SplitterWidth = 6;
            this.splitContainerPalEdt.TabIndex = 0;
            this.splitContainerPalEdt.TabStop = false;
            this.splitContainerPalEdt.UseSmoothSplitter = true;
            // 
            // mPaletteView
            // 
            this.mPaletteView.AutoScroll = true;
            this.mPaletteView.AutoScrollMinSize = new System.Drawing.Size(788, 110);
            this.mPaletteView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPaletteView.Location = new System.Drawing.Point(0, 0);
            this.mPaletteView.Name = "mPaletteView";
            this.mPaletteView.Size = new System.Drawing.Size(788, 110);
            this.mPaletteView.TabIndex = 0;
            this.mPaletteView.TabStop = false;
            this.mPaletteView.Text = "PaletteView";
            this.mPaletteView.Zoom = 4F;
            this.mPaletteView.ZoomMax = 8F;
            this.mPaletteView.ZoomMin = 1F;
            this.mPaletteView.MouseLeave += new System.EventHandler(this.VramView_MouseLeave);
            this.mPaletteView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VramView_MouseMove);
            this.mPaletteView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VramView_MouseDown);
            this.mPaletteView.MouseEnter += new System.EventHandler(this.VramView_MouseEnter);
            // 
            // tableLayoutPanelPalette
            // 
            this.tableLayoutPanelPalette.AutoScroll = true;
            this.tableLayoutPanelPalette.ColumnCount = 2;
            this.tableLayoutPanelPalette.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPalette.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPalette.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPalette.MinimumSize = new System.Drawing.Size(0, 70);
            this.tableLayoutPanelPalette.Name = "tableLayoutPanelPalette";
            this.tableLayoutPanelPalette.RowCount = 2;
            this.tableLayoutPanelPalette.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPalette.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPalette.Size = new System.Drawing.Size(788, 129);
            this.tableLayoutPanelPalette.TabIndex = 0;
            // 
            // mainMenuEditSep4
            // 
            this.mainMenuEditSep4.Name = "mainMenuEditSep4";
            this.mainMenuEditSep4.Size = new System.Drawing.Size(191, 6);
            // 
            // mainMenuEditPaletteEntry
            // 
            this.mainMenuEditPaletteEntry.Name = "mainMenuEditPaletteEntry";
            this.mainMenuEditPaletteEntry.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditPaletteEntry.Text = "&Palette entry...";
            // 
            // mainMenuEditPaletteEntryRepeat
            // 
            this.mainMenuEditPaletteEntryRepeat.Name = "mainMenuEditPaletteEntryRepeat";
            this.mainMenuEditPaletteEntryRepeat.Size = new System.Drawing.Size(194, 22);
            this.mainMenuEditPaletteEntryRepeat.Text = "R&epeat palette entry";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "FormMain";
            this.Text = "PSVrammed";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTexCmp.Panel1.ResumeLayout(false);
            this.splitContainerTexCmp.Panel2.ResumeLayout(false);
            this.splitContainerTexCmp.ResumeLayout(false);
            this.splitContainerPalEdt.Panel1.ResumeLayout(false);
            this.splitContainerPalEdt.Panel2.ResumeLayout(false);
            this.splitContainerPalEdt.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem mainMenuFile;
        private System.Windows.Forms.ToolStripSeparator mainMenuFileSep2;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileExit;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mainMenuTool;
        private System.Windows.Forms.ToolStripMenuItem mainMenuToolZoom;
        private System.Windows.Forms.ToolStripMenuItem mainMenuToolNone;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBarPalette;
        private System.Windows.Forms.ToolStripStatusLabel statusBarTexture;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode4bitView;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode4bitComp;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode8bitView;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode8bitComp;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode16bitView;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode24bitView;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileInfo;
        private System.Windows.Forms.ToolStripStatusLabel statusBarCompare;
        private System.Windows.Forms.ToolStripMenuItem mainMenuToolSituate;
        private System.Windows.Forms.ToolStripSeparator mainMenuFileSep4;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileAbout;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileSave;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileSaveTextureAll;
        private System.Windows.Forms.ToolStripSeparator mainMenuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileSaveTexture;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditUndo;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditRedo;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileSaveEdit;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileOpenEdit;
        private System.Windows.Forms.ToolStripSeparator mainMenuFileSep3;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditInvert;
        private System.Windows.Forms.ToolStripMenuItem mainMenuToolInspect;
        private System.Windows.Forms.ToolStripStatusLabel statusBarInspect;
        private System.Windows.Forms.ToolStripMenuItem mainMenuToolEdit;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditSetValue;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditClearAlpha;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditSetAlpha;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfig;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigEditValues;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigMarkers;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigShowAlphaTexture;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigShowAlphaCompare;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigShowAlphaPalette;
        private System.Windows.Forms.ToolStripSeparator mainMenuConfigSep1;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigUpdateSituatingMarker;
        private System.Windows.Forms.ToolStripMenuItem mainMenuConfigEditSlots;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPalette;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode4bitPale;
        private System.Windows.Forms.ToolStripMenuItem mainMenuMode8bitPale;
        private System.Windows.Forms.ToolStripMenuItem mainMenuFileReopen;
        private MyCustomStuff.SmoothSplitContainer splitContainerMain;
        private MyCustomStuff.SmoothSplitContainer splitContainerTexCmp;
        private MyCustomStuff.SmoothSplitContainer splitContainerPalEdt;
        private VramViewTexture mTextureView;
        private VramViewCompare mCompareView;
        private VramViewPalette mPaletteView;
        private System.Windows.Forms.ToolStripSeparator mainMenuEditSep1;
        private System.Windows.Forms.ToolStripSeparator mainMenuEditSep2;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditCut;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditInvertAll;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditTextureClipboard;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditTextureAllClipboard;
        private System.Windows.Forms.ToolStripSeparator mainMenuEditSep3;
        private System.Windows.Forms.ToolStripSeparator mainMenuEditSep4;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditPaletteEntry;
        private System.Windows.Forms.ToolStripMenuItem mainMenuEditPaletteEntryRepeat;
    }
}

