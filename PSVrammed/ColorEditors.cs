using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    abstract partial class ColorEditor : UserControl
    {
        protected const int MinNumericWidth = 80;
        protected const int MinHexLabelWidth = 140;

        protected TableLayoutPanel mTableLayout;
        protected bool mIsHexValueVisible;
        protected Label mLabelHex;
        protected NumericUpDownFixed mNumericHex;
        protected Label mLabelArrow;

        public ColorEditor()
        {
            InitializeComponent();
            mTableLayout = tableLayoutPanel;
            //mTableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; //Debug.
            mIsHexValueVisible = true;

            this.SuspendLayout();
            mTableLayout.SuspendLayout();
            initControls();
            initTableLayout();
            mTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        protected T initControl<T>(T c) where T : Control
        {
            c.Anchor = AnchorStyles.Left;
            //c.AutoSize = true; //Sloooooooooow!
            if (c is NumericUpDownFixed) c.Size = new Size(MinNumericWidth, 20);
            else if (c is CheckBox) c.Size = new Size(MinNumericWidth, 17);
            else c.AutoSize = true;
            return c;
        }

        protected virtual void initControls()
        {
            //TableLayout.
            //Let visual studio designer handle this. I cannot get it to show with
            //dynamic code only for some really annoying reason. :(

            //LabelHex.
            mLabelHex = initControl(new Label());
            mLabelHex.MinimumSize = new Size(MinHexLabelWidth, 0);
            mLabelHex.Text = HexText;

            //NumericHex.
            mNumericHex = initControl(new NumericUpDownFixed());
            mNumericHex.MinimumNoEvent = 0;
            mNumericHex.MaximumNoEvent = HexMax;
            mNumericHex.Hexadecimal = true;
            mNumericHex.Digits = HexDigits;

            //LabelArrow.
            mLabelArrow = initControl(new Label());
            mLabelArrow.Text = "<=>";
            //mLabelArrow.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            mLabelArrow.Font = new Font(Font.FontFamily, 12f, FontStyle.Bold);
        }

        protected virtual void initTableLayout()
        {
            mTableLayout.ColumnCount = 2;
            mTableLayout.ColumnStyles.Clear();
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            mTableLayout.RowCount = 2;
            mTableLayout.RowStyles.Clear();
            mTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            mTableLayout.Controls.Add(mLabelHex, 0, 0);
            mTableLayout.Controls.Add(mNumericHex, 0, 1);
            mTableLayout.Controls.Add(mLabelArrow, 1, 1);
        }

        protected string HexText
        {
            get
            {
                string hexFormat = String.Format("Hexadecimal ({0}1:X{1}{2}):", "{0}-{", HexDigits, "}");
                return String.Format(hexFormat, 0, HexMax);
            }
        }

        public bool HexValueVisible
        {
            get { return mIsHexValueVisible; }
            set
            {
                mIsHexValueVisible = value;
                mLabelHex.Visible = mIsHexValueVisible;
                mNumericHex.Visible = mIsHexValueVisible;
                mLabelArrow.Visible = mIsHexValueVisible;
            }
        }

        public int Value
        {
            get { return (int)mNumericHex.Value; }
            set { mNumericHex.Value = mNumericHex.clampValue(value); }
        }

        protected abstract int HexMax { get; }
        protected abstract int HexDigits { get; }
    }

    //*************************************************************************
    //*************************************************************************

    abstract class ColorEditorIndex : ColorEditor
    {
        protected Vrammed mVrammed; //Needed to read palette color.
        protected Label mLabelIndex;
        protected Label mLabelColor;
        protected Label mLabelColorArgb;
        protected Label mLabelColorBox;

        protected NumericUpDownFixed mNumericIndex;

        protected abstract int IndexMax { get; }

        public void setVrammed(Vrammed vrammed)
        {
            mVrammed = vrammed;
        }

        protected override void initControls()
        {
            base.initControls();

            //LabelIndex.
            mLabelIndex = initControl(new Label());
            mLabelIndex.Text = String.Format("Palette index ({0}-{1}):", 0, IndexMax);
            mLabelIndex.MinimumSize = new Size(MinHexLabelWidth, 0);

            //LabelColor.
            mLabelColor = initControl(new Label());
            mLabelColor.Text = "Indexed color:";

            //LabelColorArgb.
            mLabelColorArgb = initControl(new Label());
            mLabelColorArgb.Text = "ARGB=0,00,00,00";

            //LabelColorBox.
            mLabelColorBox = initControl(new Label());
            mLabelColorBox.AutoSize = false;
            mLabelColorBox.Size = new Size(25, 20);
            mLabelColorBox.Text = "";
            mLabelColorBox.BorderStyle = BorderStyle.FixedSingle;

            //NumericIndex.
            mNumericIndex = initControl(new NumericUpDownFixed());
            mNumericIndex.MinimumNoEvent = 0;
            mNumericIndex.MaximumNoEvent = IndexMax;

            mNumericHex.ValueChanged += new EventHandler(mNumericHex_ValueChanged);
            mNumericIndex.ValueChanged += new EventHandler(mNumericIndex_ValueChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            updateIndexedColor();
        }

        private void mNumericHex_ValueChanged(object sender, EventArgs e)
        {
            updateIndexedColor();
        }

        private void mNumericIndex_ValueChanged(object sender, EventArgs e)
        {
            mNumericHex.Value = mNumericIndex.Value;
        }

        protected void updateIndexedColor()
        {
            mNumericIndex.ValueNoEvent = mNumericHex.Value;
            if (mVrammed != null && mVrammed.IsFileOpen)
            {
                PsColor16Bit color = getPsColor16Bit();
                mLabelColorArgb.Text = color.toString();
                mLabelColorBox.BackColor = color.Color;
            }
            else
            {
                mLabelColorArgb.Text = Strings.ModeNoFile;
                mLabelColorBox.BackColor = Color.Transparent;
            }
        }

        protected override void initTableLayout()
        {
            base.initTableLayout();

            mTableLayout.ColumnCount += 3;
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            mTableLayout.Controls.Add(mLabelIndex, 2, 0);
            mTableLayout.Controls.Add(mNumericIndex, 2, 1);
            mTableLayout.Controls.Add(mLabelColor, 3, 0);
            mTableLayout.Controls.Add(mLabelColorArgb, 3, 1);
            mTableLayout.Controls.Add(mLabelColorBox, 4, 1);
        }

        public PsColor16Bit getPsColor16Bit()
        {
            return mVrammed.VramMngr.getPixelPalette((int)mNumericHex.Value);
        }
    }

    //*************************************************************************
    //*************************************************************************

    abstract class ColorEditorRgb : ColorEditor
    {
        protected Label mLabelAlpha;
        protected Label mLabelRed;
        protected Label mLabelGreen;
        protected Label mLabelBlue;
        protected Label mLabelColor;

        protected NumericUpDownFixed mNumericAlpha;
        protected NumericUpDownFixed mNumericRed;
        protected NumericUpDownFixed mNumericGreen;
        protected NumericUpDownFixed mNumericBlue;

        protected CheckBox mCheckBoxAlpha;
        protected CheckBox mCheckBoxRed;
        protected CheckBox mCheckBoxGreen;
        protected CheckBox mCheckBoxBlue;

        protected ColorButton mColorButton;

        protected List<Color> mCustomColors = new List<Color>();

        protected abstract int AlphaMax { get; }
        protected abstract int RedMax { get; }
        protected abstract int GreenMax { get; }
        protected abstract int BlueMax { get; }

        public bool MaskAlpha
        {
            get { return mCheckBoxAlpha.Checked; }
            set { mCheckBoxAlpha.Checked = value; }
        }

        public bool MaskRed
        {
            get { return mCheckBoxRed.Checked; }
            set { mCheckBoxRed.Checked = value; }
        }

        public bool MaskGreen
        {
            get { return mCheckBoxGreen.Checked; }
            set { mCheckBoxGreen.Checked = value; }
        }

        public bool MaskBlue
        {
            get { return mCheckBoxBlue.Checked; }
            set { mCheckBoxBlue.Checked = value; }
        }

        public List<Color> CustomColors //Custom colors displayed in standard color dialog.
        {
            get { return mCustomColors; }
            set { mCustomColors = value; }
        }

        public abstract int Mask { get; set; }

        protected abstract int colorToHex(Color color);

        protected override void initControls()
        {
            base.initControls();

            //LabelAlpha.
            mLabelAlpha = initControl(new Label());
            mLabelAlpha.Text = String.Format("Alpha ({0}-{1}):", 0, AlphaMax);

            //LabelRed.
            mLabelRed = initControl(new Label());
            mLabelRed.Text = String.Format("Red ({0}-{1}):", 0, RedMax);

            //LabelGreen.
            mLabelGreen = initControl(new Label());
            mLabelGreen.Text = String.Format("Green ({0}-{1}):", 0, GreenMax);

            //LabelBlue.
            mLabelBlue = initControl(new Label());
            mLabelBlue.Text = String.Format("Blue ({0}-{1}):", 0, BlueMax);

            //LabelColor.
            mLabelColor = initControl(new Label());
            mLabelColor.Text = "Color:";

            //NumericAlpha.
            mNumericAlpha = initControl(new NumericUpDownFixed());
            mNumericAlpha.MinimumNoEvent = 0;
            mNumericAlpha.MaximumNoEvent = AlphaMax;

            //NumericRed.
            mNumericRed = initControl(new NumericUpDownFixed());
            mNumericRed.MinimumNoEvent = 0;
            mNumericRed.MaximumNoEvent = RedMax;

            //NumericGreen.
            mNumericGreen = initControl(new NumericUpDownFixed());
            mNumericGreen.MinimumNoEvent = 0;
            mNumericGreen.MaximumNoEvent = GreenMax;

            //NumericBlue.
            mNumericBlue = initControl(new NumericUpDownFixed());
            mNumericBlue.MinimumNoEvent = 0;
            mNumericBlue.MaximumNoEvent = BlueMax;

            //CheckBoxAlpha.
            mCheckBoxAlpha = initControl(new CheckBox());
            mCheckBoxAlpha.Text = "Mask";

            //CheckBoxRed.
            mCheckBoxRed = initControl(new CheckBox());
            mCheckBoxRed.Text = "Mask";

            //CheckBoxGreen.
            mCheckBoxGreen = initControl(new CheckBox());
            mCheckBoxGreen.Text = "Mask";

            //CheckBoxBlue.
            mCheckBoxBlue = initControl(new CheckBox());
            mCheckBoxBlue.Text = "Mask";

            //ColorButton.
            mColorButton = initControl(new ColorButton());
            mColorButton.Text = "Select...";
            mColorButton.Click += new EventHandler(mColorButton_Click);
        }

        private void mColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.setCustomColors(mCustomColors);
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                mNumericHex.Value = colorToHex(colorDialog.Color);
            }
        }

        protected override void initTableLayout()
        {
            base.initTableLayout();

            mTableLayout.ColumnCount += 5;
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            mTableLayout.RowCount += 1;
            mTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            mTableLayout.Controls.Add(mLabelAlpha, 2, 0);
            mTableLayout.Controls.Add(mNumericAlpha, 2, 1);
            mTableLayout.Controls.Add(mCheckBoxAlpha, 2, 2);
            mTableLayout.Controls.Add(mLabelRed, 3, 0);
            mTableLayout.Controls.Add(mNumericRed, 3, 1);
            mTableLayout.Controls.Add(mCheckBoxRed, 3, 2);
            mTableLayout.Controls.Add(mLabelGreen, 4, 0);
            mTableLayout.Controls.Add(mNumericGreen, 4, 1);
            mTableLayout.Controls.Add(mCheckBoxGreen, 4, 2);
            mTableLayout.Controls.Add(mLabelBlue, 5, 0);
            mTableLayout.Controls.Add(mNumericBlue, 5, 1);
            mTableLayout.Controls.Add(mCheckBoxBlue, 5, 2);
            mTableLayout.Controls.Add(mLabelColor, 6, 0);
            mTableLayout.Controls.Add(mColorButton, 6, 1);
        }
    }

    //*************************************************************************
    //*************************************************************************

    class ColorEditor4Bit : ColorEditorIndex
    {
        protected override int HexMax
        {
            get { return 0x0F; }
        }

        protected override int HexDigits
        {
            get { return 1; }
        }

        protected override int IndexMax
        {
            get { return 0x0F; }
        }
    }

    //*************************************************************************
    //*************************************************************************

    class ColorEditor8Bit : ColorEditorIndex
    {
        protected override int HexMax
        {
            get { return 0xFF; }
        }

        protected override int HexDigits
        {
            get { return 2; }
        }

        protected override int IndexMax
        {
            get { return 0xFF; }
        }
    }

    //*************************************************************************
    //*************************************************************************

    class ColorEditor16Bit : ColorEditorRgb
    {
        protected override int HexMax
        {
            get { return 0xFFFF; }
        }

        protected override int HexDigits
        {
            get { return 4; }
        }

        protected override int AlphaMax
        {
            get { return 1; }
        }

        protected override int RedMax
        {
            get { return 0x1F; }
        }

        protected override int GreenMax
        {
            get { return 0x1F; }
        }

        protected override int BlueMax
        {
            get { return 0x1F; }
        }

        public override int Mask
        {
            get
            {
                return VramMngr.editMask16BitBoolToInt(MaskAlpha, MaskRed, MaskGreen, MaskBlue);
            }
            set
            {
                bool maskAlpha, maskRed, maskGreen, maskBlue;
                VramMngr.editMask16BitIntToBool(value, out maskAlpha, out maskRed, out maskGreen, out maskBlue);
                MaskAlpha = maskAlpha;
                MaskRed = maskRed;
                MaskGreen = maskGreen;
                MaskBlue = maskBlue;
            }
        }

        protected override int colorToHex(Color color)
        {
            return PsColor16Bit.fromColor(color) | (((int)mNumericAlpha.Value) << 15);
        }

        protected override void initControls()
        {
            base.initControls();

            mNumericHex.ValueChanged += new EventHandler(mNumericHex_ValueChanged);
            mNumericAlpha.ValueChanged += new EventHandler(mNumericArgb_ValueChanged);
            mNumericRed.ValueChanged += new EventHandler(mNumericArgb_ValueChanged);
            mNumericGreen.ValueChanged += new EventHandler(mNumericArgb_ValueChanged);
            mNumericBlue.ValueChanged += new EventHandler(mNumericArgb_ValueChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            updateRgbColor();
        }

        protected void mNumericHex_ValueChanged(object sender, EventArgs e)
        {
            updateRgbColor();
        }

        protected void mNumericArgb_ValueChanged(object sender, EventArgs e)
        {
            mNumericHex.Value = PsColor16Bit.fromArgb1555(
                (byte)mNumericAlpha.Value,
                (byte)mNumericRed.Value,
                (byte)mNumericGreen.Value,
                (byte)mNumericBlue.Value);
        }

        protected void updateRgbColor()
        {
            PsColor16Bit color = getPsColor16Bit();
            mNumericAlpha.ValueNoEvent = color.A;
            mNumericRed.ValueNoEvent = color.R;
            mNumericGreen.ValueNoEvent = color.G;
            mNumericBlue.ValueNoEvent = color.B;

            mColorButton.ColorBox = color.Color;
        }

        public PsColor16Bit getPsColor16Bit()
        {
            return new PsColor16Bit((ushort)mNumericHex.Value);
        }
    }

    //*************************************************************************
    //*************************************************************************

    class ColorEditor24Bit : ColorEditorRgb
    {
        protected override int HexMax
        {
            get { return 0xFFFFFF; }
        }

        protected override int HexDigits
        {
            get { return 6; }
        }

        protected override int AlphaMax
        {
            get { return 0xFF; }
        }

        protected override int RedMax
        {
            get { return 0xFF; }
        }

        protected override int GreenMax
        {
            get { return 0xFF; }
        }

        protected override int BlueMax
        {
            get { return 0xFF; }
        }

        public override int Mask
        {
            get
            {
                return VramMngr.editMask24BitBoolToInt(MaskRed, MaskGreen, MaskBlue);
            }
            set
            {
                bool maskRed, maskGreen, maskBlue;
                VramMngr.editMask24BitIntToBool(value, out maskRed, out maskGreen, out maskBlue);
                MaskAlpha = false;
                MaskRed = maskRed;
                MaskGreen = maskGreen;
                MaskBlue = maskBlue;
            }
        }

        protected override int colorToHex(Color color)
        {
            return PsColor24Bit.fromColor(color);
        }

        protected override void initControls()
        {
            base.initControls();

            //Hide alpha controls.
            mLabelAlpha.Text = "";
            mLabelAlpha.MinimumSize = new Size(MinNumericWidth, 0);
            mNumericAlpha.Visible = false;
            mCheckBoxAlpha.Visible = false;

            mNumericHex.ValueChanged += new EventHandler(mNumericHex_ValueChanged);
            mNumericRed.ValueChanged += new EventHandler(mNumericRgb_ValueChanged);
            mNumericGreen.ValueChanged += new EventHandler(mNumericRgb_ValueChanged);
            mNumericBlue.ValueChanged += new EventHandler(mNumericRgb_ValueChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            updateRgbColor();
        }

        protected void mNumericHex_ValueChanged(object sender, EventArgs e)
        {
            updateRgbColor();
        }

        protected void mNumericRgb_ValueChanged(object sender, EventArgs e)
        {
            mNumericHex.Value = PsColor24Bit.fromRgb888(
                (byte)mNumericRed.Value,
                (byte)mNumericGreen.Value,
                (byte)mNumericBlue.Value);
        }

        protected void updateRgbColor()
        {
            PsColor24Bit color = getPsColor24Bit();
            mNumericRed.ValueNoEvent = color.R;
            mNumericGreen.ValueNoEvent = color.G;
            mNumericBlue.ValueNoEvent = color.B;

            mColorButton.ColorBox = color.Color;
        }

        public PsColor24Bit getPsColor24Bit()
        {
            return new PsColor24Bit((int)mNumericHex.Value);
        }
    }
}
