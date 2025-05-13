using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    //View and edit currently selected palette.
    class PaletteEditor : ScrollableControl
    {
        private const int ColumnCountMin = VramMngr.PaletteWidth4Bit;
        private const int ColumnCountMax = VramMngr.PaletteWidth8Bit;
        private const int RowCount = 5;

        private static readonly int EditValueDefault16Bit = PsColor16Bit.fromColor(Color.Black);
        private const int EditMaskDefault = 0;

        private static readonly StringFormat LabelStringFormat = getLabelStringFormat();
        private static readonly StringFormat ColumnStringFormat = getColumnStringFormat();

        private static StringFormat getLabelStringFormat()
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            return sf;
        }

        private static StringFormat getColumnStringFormat()
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            return sf;
        }

        private static readonly string[] RowFormatStrings = new string[RowCount]
        {
            "{0:X2}",
            "{0:D1}",
            "{0:D2}",
            "{0:D2}",
            "{0:D2}"
        };

        private static readonly string[] RowLabelStrings = new string[RowCount]
        {
            "Index:",
            "Alpha:",
            "Red:",
            "Green:",
            "Blue:"
        };

        private readonly Vrammed mVrammed;
        private int mColumnCount;
        private int mColumnWidth;
        private int mRowHeight;
        private Size mTableSize; //Table size. Row labels and any scrollbars are not included.
        private int mSelectedColumn;
        private int mEditValue16Bit;
        private int mEditMask16Bit;
        private bool mEditSetAll; //Set all entries or just one in a palette?
        private string[][] mCellsCached;
        private bool[] mDirtyColumns;
        private bool mUpdateAllColumns;
        private Control mControlRowLabels;
        private DialogPaletteEntry mDialogPaletteEntry;

        public PaletteEditor(Vrammed vrammed)
        {
            mVrammed = vrammed;
            mColumnCount = ColumnCountMax;
            mColumnWidth = 0;
            mRowHeight = 0;
            mTableSize = Size.Empty;
            mSelectedColumn = 0;
            mEditValue16Bit = EditValueDefault16Bit;
            mEditMask16Bit = EditMaskDefault;
            mEditSetAll = false;
            mCellsCached = new string[ColumnCountMax][];
            for (int i = 0; i < ColumnCountMax; i++)
            {
                mCellsCached[i] = new string[RowCount];
            }
            mDirtyColumns = new bool[ColumnCountMax];
            mUpdateAllColumns = true;
            mControlRowLabels = new Control();
            mDialogPaletteEntry = new DialogPaletteEntry();
        }

        public void init()
        {
            this.DoubleBuffered = true;
            //this.Font = new Font("Courier New", 8);
            //this.Font = new Font(FontFamily.GenericMonospace.Name, 8);
            this.Dock = DockStyle.Top;
            this.Margin = new Padding(0);
            this.MouseDown += PaletteTable_MouseDown;
            this.MouseDoubleClick += PaletteTable_MouseDoubleClick;

            //Palette table.
            TableLayoutPanel tableLayout = TablePanel;
            tableLayout.SuspendLayout();
            tableLayout.ColumnCount = 2;
            tableLayout.ColumnStyles.Clear();
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tableLayout.RowCount = 2;
            tableLayout.RowStyles.Clear();
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, mTableSize.Height));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, SystemInformation.HorizontalScrollBarHeight));
            tableLayout.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth + 0, 0); //Prevent horizontal auto scroll.
            //tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; //Debug.
            tableLayout.Controls.Add(this, 1, 0);
            tableLayout.SetRowSpan(this, 2);
            tableLayout.Controls.Add(mControlRowLabels, 0, 0);
            tableLayout.ResumeLayout(false);

            //Row labels.
            mControlRowLabels.Dock = DockStyle.Fill;
            mControlRowLabels.Margin = new Padding(2, 0, 2, 0);
            mControlRowLabels.Paint += RowLabels_Paint;

            updateTextSize();

            mVrammed.VramMngr.VramChanged += VramMngr_VramChanged;
            mVrammed.PaletteIndicator.LocationChanged += PaletteIndicator_LocationChanged;
            mVrammed.PaletteMarker.SizeChanged += PaletteMarker_SizeChanged;
        }

        public Vrammed Vrammed
        {
            get { return mVrammed; }
        }

        public bool CanEditEntry
        {
            get { return mVrammed.IsFileOpen && Visible; }
        }

        public bool CanMoveIndex
        {
            get { return mVrammed.IsFileOpen && Visible; }
        }

        private TableLayoutPanel TablePanel
        {
            get { return mVrammed.FormMain.PaletteTablePanel; }
        }

        private int ColumnCount
        {
            get { return mColumnCount; }
            set { setColumnCount(value); }
        }

        public int SelectedColumn
        {
            get { return mSelectedColumn; }
            set { setSelectedColumn(value); }
        }

        private void PaletteTable_MouseDown(object sender, MouseEventArgs e)
        {
            int colClicked = getColumnClicked(e);
            if (colClicked >= 0) setSelectedColumn(colClicked);
        }

        private void PaletteTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int colClicked = getColumnClicked(e);
            if (colClicked >= 0) editEntryOpen();
        }

        private void VramMngr_VramChanged(VramMngr sender, bool isPaletteAffected)
        {
            if (isPaletteAffected) updateAllColumns();
        }

        private void PaletteIndicator_LocationChanged(VramIndicator sender, Point oldLoc, Point newLoc)
        {
            updateAllColumns();
        }

        private void PaletteMarker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            setColumnCount(newSize.Width);
        }

        public event EventChangeOldNew<PaletteEditor, int> SelectedColumnChanged;

        private void OnSelectedColumnChanged(int oldColumn, int newColumn)
        {
            if (SelectedColumnChanged != null) SelectedColumnChanged(this, oldColumn, newColumn);
        }

        private void setColumnCount(int count)
        {
            count = count.Clamp(ColumnCountMin, ColumnCountMax);
            if (mColumnCount != count)
            {
                mColumnCount = count;

                updateTableSize();
                setSelectedColumn(mSelectedColumn); //Make sure it is still inside table.
                setAllColumnsDirty();
                this.Invalidate();
            }
        }

        private void setSelectedColumn(int column)
        {
            column = Misc.Clamp(column, 0, mColumnCount - 1);
            if (mSelectedColumn != column)
            {
                int oldColumn = mSelectedColumn;
                mSelectedColumn = column;

                Point scroll = this.AutoScrollPosition;
                scroll.X = getCellLocX(column) - (this.ClientSize.Width / 2);
                scroll.Y = Math.Abs(scroll.Y);
                this.AutoScrollPosition = scroll;
                this.Invalidate();

                OnSelectedColumnChanged(oldColumn, column);
            }
        }

        public void stepIndexLeft(bool altStep)
        {
            if (CanMoveIndex)
            {
                SelectedColumn += altStep ? -8 : -1;
            }
        }

        public void stepIndexRight(bool altStep)
        {
            if (CanMoveIndex)
            {
                SelectedColumn += altStep ? 8 : 1;
            }
        }

        private void updateTextSize()
        {
            //Measure table dimensions needed to fit cell strings and row labels.
            Size textCellSize;
            int textLabelWidth;
            measureTextSize(out textCellSize, out textLabelWidth);

            mColumnWidth = textCellSize.Width + 2; //22;
            mRowHeight = textCellSize.Height + 2; //16;

            mControlRowLabels.MinimumSize = new Size(textLabelWidth, mControlRowLabels.MinimumSize.Height);

            updateTableSize();
        }

        private void measureTextSize(out Size cellSize, out int labelWidth)
        {
            cellSize = new Size(0, 0); //20x14 usually.
            labelWidth = 0; //Measure max label width also.
            using (Graphics gr = Graphics.FromHwnd(this.Handle))
            {
                //Get estimated table cell size by measuring all hexadecimal chars (doubled).
                for (int i = 0x0; i <= 0xF; i++)
                {
                    string cellString = i.ToString("X");
                    cellString += cellString; //Double hex char e.g. A -> AA.
                    SizeF cs = gr.MeasureString(cellString, Font);
                    cellSize.Width = Math.Max(cellSize.Width, (int)(cs.Width + 0.5f));
                    cellSize.Height = Math.Max(cellSize.Height, (int)(cs.Height + 0.5f));
                }

                //Get max width of row labels.
                foreach (string rowLabel in RowLabelStrings)
                {
                    float lw = gr.MeasureString(rowLabel, Font).Width;
                    labelWidth = Math.Max(labelWidth, (int)(lw + 0.5f));
                }
            }
        }

        private void updateTableSize()
        {
            int tableWidth = getCellLocX(mColumnCount);
            int tableHeight = (RowCount * (mRowHeight + 1)) + 1;
            mTableSize = new Size(tableWidth, tableHeight);

            this.Size = new Size(tableWidth, tableHeight + SystemInformation.HorizontalScrollBarHeight);
            this.AutoScrollMinSize = mTableSize;

            //First row in table layout panel should contain the palette table.
            if (TablePanel.RowStyles.Count > 0)
            {
                TablePanel.RowStyles[0].Height = tableHeight;
            }
            mControlRowLabels.MinimumSize = new Size(mControlRowLabels.MinimumSize.Width, tableHeight);
        }

        private void updateAllColumns()
        {
            mUpdateAllColumns = true;
            this.Invalidate();
        }

        public void editEntryOpen()
        {
            //Open the palette entry dialog and write the value entered if OK was clicked.
            if (CanEditEntry)
            {
                //Init dialog with selected palette color and last mask before showing it.
                mDialogPaletteEntry.ColorEditor.Value = mVrammed.VramMngr.getPixelPalette(mSelectedColumn).Abgr;
                mDialogPaletteEntry.ColorEditor.Mask = mEditMask16Bit;

                if (mDialogPaletteEntry.ShowDialog() == DialogResult.OK)
                {
                    //Store edit value and perform it.
                    mEditValue16Bit = mDialogPaletteEntry.ColorEditor.Value;
                    mEditMask16Bit = mDialogPaletteEntry.ColorEditor.Mask;
                    mEditSetAll = mDialogPaletteEntry.SetAll;
                    editEntryPerform();
                }
            }
        }

        public void editEntryRepeat()
        {
            //Write currently stored edit value to palette.
            if (CanEditEntry)
            {
                editEntryPerform();
            }
        }

        private void editEntryPerform()
        {
            if (mEditSetAll)
            {
                mVrammed.VramMngr.editPaletteAllEntries(mEditValue16Bit, mEditMask16Bit);
            }
            else
            {
                mVrammed.VramMngr.editPaletteEntry(mSelectedColumn, mEditValue16Bit, mEditMask16Bit);
            }
        }

        private int getCellLocX(int column)
        {
            return (column * (mColumnWidth + 1)) + 1;
        }

        private int getCellLocY(int row)
        {
            return (row * (mRowHeight + 1) + 1);
        }

        private int getCellCenterX(int column)
        {
            return getCellLocX(column) + (mColumnWidth / 2);
        }

        private int getCellCenterY(int row)
        {
            return getCellLocY(row) + (mRowHeight / 2);
        }

        private int getColumnClicked(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = Math.Abs(AutoScrollPosition.X) + e.X;
                int y = Math.Abs(AutoScrollPosition.Y) + e.Y;
                if (x < mTableSize.Width && y < mTableSize.Height)
                {
                    return x / (mColumnWidth + 1);
                }
            }
            return -1; //No column clicked left on.
        }

        private void setAllColumnsDirty()
        {
            for (int i = 0; i < mDirtyColumns.Length; i++)
            {
                mDirtyColumns[i] = true;
            }
            mUpdateAllColumns = false;
        }

        private void drawStringsColumn(Graphics gr, int column)
        {
            if (mDirtyColumns[column])
            {
                PsColor16Bit color = mVrammed.VramMngr.getPixelPalette(column);
                mCellsCached[column][0] = String.Format(RowFormatStrings[0], column);
                mCellsCached[column][1] = String.Format(RowFormatStrings[1], color.A);
                mCellsCached[column][2] = String.Format(RowFormatStrings[2], color.R);
                mCellsCached[column][3] = String.Format(RowFormatStrings[3], color.G);
                mCellsCached[column][4] = String.Format(RowFormatStrings[4], color.B);
                mDirtyColumns[column] = false;
            }

            int xs = getCellCenterX(column);
            int ys = getCellCenterY(0);
            drawStrings(gr, mCellsCached[column], xs, ys, ColumnStringFormat);
        }

        private void RowLabels_Paint(object sender, PaintEventArgs e)
        {
            int ys = getCellCenterY(0);
            drawStrings(e.Graphics, RowLabelStrings, 0, ys, LabelStringFormat);
        }

        private void drawStrings(Graphics gr, string[] strings, int xs, int ys, StringFormat sf)
        {
            for (int r = 0; r < strings.Length; r++, ys += mRowHeight + 1)
            {
                gr.DrawString(strings[r], Font, Brushes.Black, xs, ys, sf);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Log.timeBegin();

            if (mUpdateAllColumns) setAllColumnsDirty();

            Graphics gr = e.Graphics;
            gr.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            //Get visible rectangle and inflate it for bigger margin.
            //Clipping draw strings is a lot faster. Does not help with other drawings.
            Rectangle rcVis = Rectangle.Round(gr.VisibleClipBounds);
            rcVis.Inflate(mColumnWidth + 1, mRowHeight + 1);

            //Painting assumes that column/row lines are 1 pixel wide.

            int tableWidth = mTableSize.Width - 2;
            int tableHeight = mTableSize.Height - 2;

            //Background.

            gr.FillRectangle(Brushes.LightGray, 1, 1, tableWidth, mRowHeight);
            gr.FillRectangle(Brushes.White, 1, mRowHeight + 1, tableWidth, tableHeight - mRowHeight);

            //Selected column.
            int xsc = getCellLocX(mSelectedColumn);
            gr.FillRectangle(Brushes.DeepSkyBlue, xsc, 1, mColumnWidth, tableHeight);

            //Column lines.
            Rectangle rcColLine = new Rectangle(0, 1, 1, tableHeight);
            for (int c = 0; c <= mColumnCount; c++, rcColLine.X += mColumnWidth + 1)
            {
                gr.FillRectangle(Brushes.Gray, rcColLine);
            }

            //Row lines.
            Rectangle rcRowLine = new Rectangle(1, 0, tableWidth, 1);
            for (int r = 0; r <= RowCount; r++, rcRowLine.Y += mRowHeight + 1)
            {
                gr.FillRectangle(Brushes.Gray, rcRowLine);
            }

            //Cells. Clipping really helps with performance here for some reason.
            //DrawString does not use clipping or String.Format slow?
            for (int c = 0, x = 1; c < mColumnCount; c++, x += mColumnWidth + 1)
            {
                if (x >= rcVis.X && x <= rcVis.Right) drawStringsColumn(gr, c);
            }

            //Log.timeEnd("palette editor paint()");
            //Log.add(rcVis.ToString());

            base.OnPaint(e);
        }
    }
}
