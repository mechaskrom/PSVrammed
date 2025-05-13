using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSVrammed
{
    partial class DialogEditValues : Form
    {
        private readonly Vrammed mVrammed;

        public DialogEditValues(Vrammed vrammed)
        {
            InitializeComponent();
            mVrammed = vrammed;
            colorEditor4Bit.setVrammed(vrammed);
            colorEditor8Bit.setVrammed(vrammed);
        }

        private void DialogEditValues_Shown(object sender, EventArgs e)
        {
            colorEditor4Bit.Value = mVrammed.VramMngr.EditValue4Bit;
            colorEditor8Bit.Value = mVrammed.VramMngr.EditValue8Bit;
            colorEditor16Bit.Value = mVrammed.VramMngr.EditValue16Bit;
            colorEditor16Bit.Mask = mVrammed.VramMngr.EditMask16Bit;
            colorEditor24Bit.Value = mVrammed.VramMngr.EditValue24Bit;
            colorEditor24Bit.Mask = mVrammed.VramMngr.EditMask24Bit;
        }

        private void DialogEditValues_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                mVrammed.VramMngr.EditValue4Bit = colorEditor4Bit.Value;
                mVrammed.VramMngr.EditValue8Bit = colorEditor8Bit.Value;
                mVrammed.VramMngr.EditValue16Bit = colorEditor16Bit.Value;
                mVrammed.VramMngr.EditMask16Bit = colorEditor16Bit.Mask;
                mVrammed.VramMngr.EditValue24Bit = colorEditor24Bit.Value;
                mVrammed.VramMngr.EditMask24Bit = colorEditor24Bit.Mask;
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            colorEditor4Bit.Value = VramMngr.EditValueDefault4Bit;
            colorEditor8Bit.Value = VramMngr.EditValueDefault8Bit;
            colorEditor16Bit.Value = VramMngr.EditValueDefault16Bit;
            colorEditor16Bit.Mask = VramMngr.EditMaskDefault;
            colorEditor24Bit.Value = VramMngr.EditValueDefault24Bit;
            colorEditor24Bit.Mask = VramMngr.EditMaskDefault;
        }
    }
}
