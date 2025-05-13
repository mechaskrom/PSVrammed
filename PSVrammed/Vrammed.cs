using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using MyCustomStuff;

namespace PSVrammed
{
    //Main class that glues everything together. Also contains helper functions for main form.
    class Vrammed
    {
        //false: Update after situate marker ends.
        //true: Update when mouse moves a situating marker. Uses more CPU-time.
        private const bool UpdateSituatingMarkerDefault = false;

        //Open files filters.
        private const string FileFilterStates = "State files|*.psv;*.00?;*.state;*.sav;*.bin|All files|*.*";
        private const string FileFilterEdits = "Edit files|*.pve";

        //Save one file filter.
        private const string FileFilterFormat = "{0} files (*.{0})|*.{0}";

        public const int NumberOfEditSlots = 10;

        public const Mode ModeDefault =
            //Mode.Ind4BitView;
            //Mode.Ind4BitComp;
            //Mode.Ind4BitPalE;
            //Mode.Ind8BitView;
            //Mode.Ind8BitComp;
            //Mode.Ind8BitPalE;
            Mode.Rgb16BitView;
        //Mode.Rgb24BitView;

        private static string mDebugBasePath = null; //Base folder for debug tests/saves.

        private readonly FormMain mFormMain;
        private bool mIsVramSaved;
        private bool mUpdateSituatingMarker; //Update a marker moved by mouse (situate tool)?
        private bool mIsInZoom;
        private Mode mMode;
        private Tool mTool;
        private StateFile mStateFile;
        private readonly VramMngr mVramMngr;
        private readonly VramMarkerTexture mTextureMarker;
        private readonly VramMarkerCompare mCompareMarker;
        private readonly VramMarkerPalette mPaletteMarker;
        private readonly PaletteEditor mPaletteEditor;
        private readonly FormPixelInspector mPixelInspector;
        private readonly StatusBarUpdater mStatusBarUpdater;
        private readonly string[] mEditSlotFiles;
        private DateTime mSoundPlayedTimestamp;

        public Vrammed(FormMain formMain)
        {
            mFormMain = formMain;
            mIsVramSaved = true;
            mUpdateSituatingMarker = UpdateSituatingMarkerDefault;
            mIsInZoom = false;
            mMode = ModeDefault;
            mTool = Tool.None;
            mStateFile = null; //Set in open state.
            mVramMngr = new VramMngr(this);
            mTextureMarker = new VramMarkerTexture(this);
            mCompareMarker = new VramMarkerCompare(this);
            mPaletteMarker = new VramMarkerPalette(this);
            mPaletteEditor = new PaletteEditor(this);
            mPixelInspector = new FormPixelInspector(this);
            mStatusBarUpdater = new StatusBarUpdater(this);
            mEditSlotFiles = new string[NumberOfEditSlots];
            mSoundPlayedTimestamp = DateTime.Now;
        }

        public void init()
        {
            //Init stuff that cannot (shouldn't) be done in constructors.
            //Like subscribing to events in objects not yet constructed (null).
            mVramMngr.init();
            mTextureMarker.init();
            mCompareMarker.init();
            mPaletteMarker.init();
            mPaletteEditor.init();
            mStatusBarUpdater.init();

            mVramMngr.VramChanged += VramMngr_VramChanged;
        }

        public void testStuff()
        {
            //mVramMngr.saveTexture(DebugOutPath + mVramMngr.BitDepthTexture + " save texture.png", false);
            //mVramMngr.saveTexture(DebugOutPath + mVramMngr.BitDepthTexture + " save texture marked.png", true);

            //mVramMngr.editPaletteAddSuikoden2Docks();

            mCompareMarker.debugSaveCompareResultBmp(DebugBasePath + "save compare.png");
        }

        //*******************************************************************************
        //*******************************************************************************

        #region Properties

        public static string DebugBasePath
        {
            get { return getDebugBasePath(); }
        }

        public FormMain FormMain
        {
            get { return mFormMain; }
        }

        public VramViewTexture TextureView
        {
            get { return mFormMain.TextureView; }
        }

        public VramViewCompare CompareView
        {
            get { return mFormMain.CompareView; }
        }

        public VramViewPalette PaletteView
        {
            get { return mFormMain.PaletteView; }
        }

        public bool IsVramSaved
        {
            get { return mIsVramSaved; }
        }

        public bool UpdateSituatingMarker
        {
            get { return mUpdateSituatingMarker; }
        }

        public Mode Mode
        {
            get { return mMode; }
            set { setMode(value); }
        }

        public Tool Tool
        {
            get { return mTool; }
            set { setTool(value); }
        }

        public StateFile StateFile
        {
            get { return mStateFile; }
        }

        public VramMngr VramMngr
        {
            get { return mVramMngr; }
        }

        public VramMarkerTexture TextureMarker
        {
            get { return mTextureMarker; }
        }

        public VramMarkerCompare CompareMarker
        {
            get { return mCompareMarker; }
        }

        public VramMarkerPalette PaletteMarker
        {
            get { return mPaletteMarker; }
        }

        public VramIndicator TextureIndicator
        {
            get { return mTextureMarker.Indicator; }
        }

        public VramIndicator CompareIndicator
        {
            get { return mCompareMarker.Indicator; }
        }

        public VramIndicator PaletteIndicator
        {
            get { return mPaletteMarker.Indicator; }
        }

        public PaletteEditor PaletteEditor
        {
            get { return mPaletteEditor; }
        }

        public FormPixelInspector PixelInspector
        {
            get { return mPixelInspector; }
        }

        public StatusBarUpdater StatusBarUpdater
        {
            get { return mStatusBarUpdater; }
        }

        public string[] EditSlotFiles
        {
            get { return mEditSlotFiles; }
        }

        public bool IsFileOpen
        {
            get { return mStateFile != null; }
        }

        public bool CanReopen
        {
            get { return IsFileOpen; }
        }

        public bool CanSave
        {
            get { return IsFileOpen; }
        }

        public bool CanSaveTexture
        {
            get { return CanSaveTextureAll && TextureMarker.IsVisible; }
        }

        public bool CanSaveTextureAll
        {
            get { return IsFileOpen; }
        }

        public bool CanOpenEdit
        {
            get { return IsFileOpen; }
        }

        public bool CanSaveEdit
        {
            get { return VramMngr.CanEditUndo; }
        }

        public bool CanShowFileInfo
        {
            get { return IsFileOpen; }
        }

        #endregion Properties

        //*******************************************************************************
        //*******************************************************************************

        #region File operations (paths, dialogs, open, save, etc)

        public static string getAssemblyVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return version != null ? version.ToString() : "?.?.?.?";
        }

        private static string getDebugBasePath()
        {
            if (mDebugBasePath == null)
            {
                //Return base path used when doing debug tests and saves i.e. running program within visual studio.

                string exePath = AppDomain.CurrentDomain.BaseDirectory.GetFullPathWithEndingSlashes();
                DirectoryInfo di = Directory.GetParent(exePath); //Get exe folder.
                while (di != null && !di.GetFiles("*.sln").Any()) //Search up for the solution.
                {
                    di = di.Parent;
                }
                mDebugBasePath = di.FullName.GetFullPathWithEndingSlashes(); //Get solution folder.
            }
            return mDebugBasePath;
        }

        public OpenFileDialog createOpenStateDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = FileFilterStates;
            return open;
        }

        public OpenFileDialog createOpenEditDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = FileFilterEdits;
            return open;
        }

        private SaveFileDialog createSaveDialog(string extension)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = String.Format(FileFilterFormat, extension);
            save.AddExtension = true;
            save.DefaultExt = extension;
            save.FileName = mStateFile.NameWithExtension;
            return save;
        }

        public SaveFileDialog createSaveStateDialog()
        {
            return createSaveDialog(mStateFile.Extension);
        }

        public SaveFileDialog createSaveImageDialog()
        {
            return createSaveDialog("png");
        }

        public SaveFileDialog createSaveEditDialog()
        {
            return createSaveDialog("pve");
        }

        public SaveFileDialog createSaveInfoDialog()
        {
            return createSaveDialog("txt");
        }

        public bool checkChangesSaved()
        {
            //Return true if asked save was cancelled.
            bool cancel = false;
            if (!mIsVramSaved)
            {
                DialogResult dr;
                dr = MessageBox.Show(Strings.SaveChangesWarning,
                    Strings.SaveChangesWarningCaption,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    SaveFileDialog save = createSaveStateDialog();
                    dr = save.ShowDialog();
                    cancel = !(dr == DialogResult.OK && saveState(save.FileName));
                }
                else if (dr == DialogResult.Cancel) cancel = true;
            }
            return cancel;
        }

        private void setIsVramSaved(bool isSaved)
        {
            if (mIsVramSaved != isSaved)
            {
                mIsVramSaved = isSaved;
                mFormMain.updateTitlebar();
            }
        }

        public void openStateDialog()
        {
            bool cancel = checkChangesSaved();
            if (!cancel)
            {
                OpenFileDialog open = createOpenStateDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    openState(open.FileName);
                }
            }
        }

        public void openState(string filePath)
        {
            try
            {
                Log.timeBegin();

                Log.addGcAllocMem("openState start");

                StateFile state;
                try
                {
                    state = StateFile.open(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Strings.OpenStateException + ex.Message, Strings.OpenStateErrorCaption,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; //Open state failed!
                }

                Log.addGcAllocMem("openState state opened");

                StateFile oldStateFile = mStateFile;
                mStateFile = state;
                OnFileChanged(oldStateFile, mStateFile);

                //Fix titlebar after vram was changed.
                setIsVramSaved(true);

                Log.addGcAllocMem("openState end");
            }
            finally
            {
                Log.timeEnd("openState()");
            }
        }

        public void reopenState()
        {
            if (CanReopen)
            {
                bool cancel = checkChangesSaved();
                if (!cancel)
                {
                    openState(StateFile.FullName);
                }
            }
        }

        public void saveStateDialog()
        {
            if (CanSave)
            {
                SaveFileDialog save = createSaveStateDialog();
                if (save.ShowDialog() == DialogResult.OK)
                {
                    saveState(save.FileName);
                }
            }
        }

        public bool saveState(string filePath)
        {
            if (!CanSave) return false;

            try
            {
                mStateFile.save(filePath);
                setIsVramSaved(true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.SaveStateException + ex.Message, Strings.SaveStateErrorCaption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void saveState()
        {
            if (CanSave)
            {
                saveState(mStateFile.FullName);
            }
        }

        public void saveTextureDialog()
        {
            saveTextureDialog(CanSaveTexture, true);
        }

        public void saveTextureAllDialog()
        {
            saveTextureDialog(CanSaveTextureAll, false);
        }

        private void saveTextureDialog(bool canSave, bool onlyMarked)
        {
            if (!canSave) return;

            SaveFileDialog save = createSaveImageDialog();
            if (save.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (BitmapPinned bmp = mVramMngr.createSaveTextureBmp(onlyMarked))
                {
                    bmp.Bitmap.Save(save.FileName, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.SaveTextureException + ex.Message, Strings.SaveTextureErrorCaption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void copyTextureClipboard()
        {
            if (CanSaveTexture)
            {
                Clipboard.SetImage(mVramMngr.createSaveTextureBmp(true));
            }
        }

        public void copyTextureAllClipboard()
        {
            if (CanSaveTextureAll)
            {
                Clipboard.SetImage(mVramMngr.createSaveTextureBmp(false));
            }
        }

        public void openEditDialog()
        {
            if (CanOpenEdit)
            {
                OpenFileDialog open = createOpenEditDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    openEdit(open.FileName);
                }
            }
        }

        public void openEdit(string filePath)
        {
            if (CanOpenEdit)
            {
                try
                {
                    mVramMngr.editOpenSequence(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Strings.OpenEditException + ex.Message, Strings.OpenEditErrorCaption,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void saveEditDialog()
        {
            if (CanSaveEdit)
            {
                SaveFileDialog save = createSaveEditDialog();
                if (save.ShowDialog() == DialogResult.OK)
                {
                    saveEdit(save.FileName);
                }
            }
        }

        public void saveEdit(string filePath)
        {
            if (CanSaveEdit)
            {
                try
                {
                    mVramMngr.editSaveSequence(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Strings.SaveEditException + ex.Message, Strings.SaveEditErrorCaption,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void showFileInfoDialog()
        {
            if (CanShowFileInfo)
            {
                new DialogStateInfo(this).ShowDialog();
            }
        }

        public void saveStateInfo(string filePath)
        {
            try
            {
                mStateFile.saveInfo(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.SaveInfoException + ex.Message, Strings.SaveInfoErrorCaption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void openEditSlot(int slot)
        {
            if (IsFileOpen)
            {
                slot.Clamp(1, NumberOfEditSlots);
                string filePath = mEditSlotFiles[slot - 1];
                if (!String.IsNullOrEmpty(filePath))
                {
                    openEdit(filePath);
                }
            }
        }

        #endregion File operations (paths, dialogs, open, save, etc)

        //*******************************************************************************
        //*******************************************************************************

        public event EventChangeOldNew<Vrammed, StateFile> FileChanged;
        public event EventChangeOldNew<Vrammed, Mode> ModeChanged;
        public event EventChangeOldNew<Vrammed, Tool> ToolChanged;

        private void OnFileChanged(StateFile oldFile, StateFile newFile)
        {
            if (FileChanged != null) FileChanged(this, oldFile, newFile);
        }

        private void OnModeChanged(Mode oldMode, Mode newMode)
        {
            if (ModeChanged != null) ModeChanged(this, oldMode, newMode);
        }

        private void OnToolChanged(Tool oldTool, Tool newTool)
        {
            if (ToolChanged != null) ToolChanged(this, oldTool, newTool);
        }

        private void VramMngr_VramChanged(VramMngr sender, bool isPaletteAffected)
        {
            setIsVramSaved(false);
        }

        //*******************************************************************************
        //*******************************************************************************

        public void setMode(Mode mode)
        {
            if (mode != mMode)
            {
                Mode oldMode = mMode;
                mMode = mode;
                OnModeChanged(oldMode, mode);
            }
        }

        public void setTool(Tool tool)
        {
            if (IsFileOpen && tool != mTool)
            {
                Tool oldTool = mTool;
                mTool = tool;
                OnToolChanged(oldTool, tool);
            }
        }

        public string getFileTitle() //File in main form's title bar.
        {
            string fileTitle;
            if (IsFileOpen)
            {
                fileTitle = StateFile.NameWithExtension;
                if (!IsVramSaved)
                {
                    fileTitle += "*";
                }
            }
            else
            {
                fileTitle = Strings.FormTitleNoFile;
            }
            return fileTitle;
        }

        public void stepLocationLeftCompareOrPalette(bool isAltPressed)
        {
            mPaletteEditor.stepIndexLeft(isAltPressed);
            mCompareMarker.stepLocationLeft(isAltPressed);
        }

        public void stepLocationRightCompareOrPalette(bool isAltPressed)
        {
            mPaletteEditor.stepIndexRight(isAltPressed);
            mCompareMarker.stepLocationRight(isAltPressed);
        }

        public void stepSizeTextureMarker(bool stepUp, bool stepWidth, bool stepHeight)
        {
            if (mTextureMarker.IsVisible)
            {
                mTextureMarker.stepSize(stepUp, stepWidth, stepHeight);
            }
        }

        public void resetSizeTextureMarker()
        {
            if (mTextureMarker.IsVisible)
            {
                mTextureMarker.resetSize();
            }
        }

        public void toggleTextureAlpha()
        {
            mVramMngr.ShowAlphaTexture = !mVramMngr.ShowAlphaTexture;
        }

        public void toggleCompareAlpha()
        {
            mVramMngr.ShowAlphaCompare = !mVramMngr.ShowAlphaCompare;
        }

        public void togglePaletteAlpha()
        {
            mVramMngr.ShowAlphaPalette = !mVramMngr.ShowAlphaPalette;
        }

        public void toggleUpdateSituatingMarker()
        {
            mUpdateSituatingMarker = !mUpdateSituatingMarker;
        }

        public void zoomInView(VramView vv)
        {
            if (vv.Visible)
            {
                vv.Zoom++;
                vv.scrollByMarkerCenter();
            }
        }

        public void zoomOutView(VramView vv)
        {
            if (vv.Visible)
            {
                vv.Zoom--;
                vv.scrollByMarkerCenter();
            }
        }

        public void toolEnterZoom(VramView vv)
        {
            vv.Cursor = Cursors.SizeAll;
            mIsInZoom = true;
            Log.add(vv.Name + " entered zoom");
        }

        public void toolLeaveZoom(VramView vv)
        {
            vv.Cursor = Cursors.Default;
            mIsInZoom = false;
            Log.add(vv.Name + " left zoom");
        }

        public void toolPerformZoomUp(VramView vv, Point imageLocation)
        {
            if (mIsInZoom)
            {
                vv.Zoom++;
                vv.scrollByCenter(imageLocation);
            }
        }

        public void toolPerformZoomDown(VramView vv, Point imageLocation)
        {
            if (mIsInZoom)
            {
                vv.Zoom--;
                vv.scrollByCenter(imageLocation);
            }
        }

        public void toolEnterSituate(VramView vv, Point mouseScreen)
        {
            if (vv.Marker.enterSituate(vv.getObjectPointFromScreenInside(mouseScreen), UpdateSituatingMarker))
            {
                vv.Cursor = Cursors.Help;
            }
        }

        public void toolMoveSituate(VramView vv, Point mouseClient)
        {
            Point point = vv.getObjectPointFromClientInside(mouseClient);
            vv.Marker.moveSituate(point);
        }

        public void toolLeaveSituate(VramView vv)
        {
            if (vv.Marker.leaveSituate())
            {
                vv.Cursor = Cursors.Default;
            }
        }

        public void toolSaveSituate(VramView vv, Point imageLocation)
        {
            if (vv.Marker.saveSituate(imageLocation))
            {
                setTool(Tool.None);
            }
        }

        public void toolQuitSituate(VramView vv)
        {
            if (vv.Marker.leaveSituate())
            {
                setTool(Tool.None);
            }
        }

        public void toolEnterEdit(VramView vv, Point mouseScreen)
        {
            if (vv.Marker.enterSituate(vv.getObjectPointFromScreenInside(mouseScreen), false))
            {
                vv.Cursor = Cursors.Default;
            }
        }

        public void toolMoveEdit(VramView vv, Point mouseClient)
        {
            toolMoveSituate(vv, mouseClient);
        }

        public void toolLeaveEdit(VramView vv)
        {
            toolLeaveSituate(vv);
        }

        public void toolPerformEdit(VramView vv, Keys modifierKeys)
        {
            if (vv.Marker.IsSituating)
            {
                if (modifierKeys == Keys.Shift) mVramMngr.editAlphaSet();
                else if (modifierKeys == Keys.Control) mVramMngr.editAlphaClear();
                else mVramMngr.editValue();
            }
        }

        public void toolQuitEdit(VramView vv)
        {
            toolQuitSituate(vv);
        }

        public void playSound(System.Media.SystemSound sound)
        {
            //If sound is rapidly (repeat) played it'll not be heard so use a timestamp
            //to skip sounds played too closely together.
            if ((DateTime.Now - mSoundPlayedTimestamp).TotalSeconds > 0.5)
            {
                sound.Play();
                mSoundPlayedTimestamp = DateTime.Now;
            }
        }
    }
}
