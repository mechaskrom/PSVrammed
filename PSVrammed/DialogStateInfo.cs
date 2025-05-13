using System;
using System.Windows.Forms;

namespace PSVrammed
{
    partial class DialogStateInfo : Form
    {
        private readonly Vrammed mVrammed;

        public DialogStateInfo(Vrammed vrammed)
        {
            InitializeComponent();
            mVrammed = vrammed;
            richTextBox.Text = vrammed.StateFile.Info;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = mVrammed.createSaveInfoDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                mVrammed.saveStateInfo(save.FileName);
            }
        }
    }
}
