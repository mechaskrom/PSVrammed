using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace PSVrammed
{
    partial class DialogEditSlots : Form
    {
        private const int nSlots = Vrammed.NumberOfEditSlots;
        private const int TextBoxWidth = 550;

        private readonly Vrammed mVrammed;
        private readonly MaskedTextBox[] mTextBoxes; //TextBox doesn't support Ctrl+A. MaskedTextBox does.
        private readonly Button[] mButtons;

        public DialogEditSlots(Vrammed vrammed)
        {
            InitializeComponent();

            mVrammed = vrammed;
            mTextBoxes = new MaskedTextBox[nSlots];
            mButtons = new Button[nSlots];

            initTable();
        }

        private string[] EditSlotFiles
        {
            get { return mVrammed.EditSlotFiles; }
        }

        private void initTable()
        {
            tableLayoutPanelMain.SuspendLayout();

            tableLayoutPanelMain.ColumnCount = 3;
            tableLayoutPanelMain.RowCount = nSlots + 1;

            //Columns.
            TableLayoutColumnStyleCollection colStyles = tableLayoutPanelMain.ColumnStyles;
            colStyles.Clear();
            for (int i = 0; i < tableLayoutPanelMain.ColumnCount; i++)
            {
                colStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }

            //Rows.
            TableLayoutRowStyleCollection rowStyles = tableLayoutPanelMain.RowStyles;
            rowStyles.Clear();
            for (int i = 0; i < nSlots; i++)
            {
                Label label = new Label();
                label.Text = "Slot " + (i + 1) + ":";
                label.Anchor = AnchorStyles.Left;
                label.AutoSize = true;

                MaskedTextBox textBox = new MaskedTextBox();
                textBox.Text = EditSlotFiles[i];
                textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                textBox.Width = TextBoxWidth;
                textBox.Multiline = false;
                textBox.ShortcutsEnabled = true;

                Button button = new Button();
                button.Text = "Browse...";
                button.Anchor = AnchorStyles.Right;
                button.Click += new EventHandler(button_Click);

                tableLayoutPanelMain.Controls.Add(label, 0, i);
                tableLayoutPanelMain.Controls.Add(textBox, 1, i);
                tableLayoutPanelMain.Controls.Add(button, 2, i);

                rowStyles.Add(new RowStyle(SizeType.AutoSize));

                mTextBoxes[i] = textBox;
                mButtons[i] = button;
            }

            tableLayoutPanelMain.SetCellPosition(flowLayoutPanelButtons,
                new TableLayoutPanelCellPosition(2, nSlots));

            tableLayoutPanelMain.ResumeLayout();
        }

        private void DialogEditSlots_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < nSlots; i++)
            {
                mTextBoxes[i].Text = EditSlotFiles[i];
            }
        }

        private void DialogEditSlots_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                for (int i = 0; i < nSlots; i++)
                {
                    EditSlotFiles[i] = mTextBoxes[i].Text;
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            for (int i = 0; i < nSlots; i++)
            {
                if (mButtons[i] == clicked)
                {
                    browseEdit(mTextBoxes[i]);
                    break;
                }
            }
        }

        private void browseEdit(MaskedTextBox textBox)
        {
            OpenFileDialog open = mVrammed.createOpenEditDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = open.FileName;
            }
        }
    }
}
