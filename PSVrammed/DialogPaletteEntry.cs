using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    partial class DialogPaletteEntry : Form
    {
        private class ColorMask
        {
            public bool Alpha = false;
            public bool Red = false;
            public bool Green = false;
            public bool Blue = false;

            public ColorMask(bool alpha, bool red, bool green, bool blue)
            {
                Alpha = alpha;
                Red = red;
                Green = green;
                Blue = blue;
            }

            public ColorMask(ColorEditor16Bit ce)
                : this(ce.MaskAlpha, ce.MaskRed, ce.MaskGreen, ce.MaskBlue)
            {
            }

            public string getItemText()
            {
                //return string.Format("{0},{1},{2},{3}",
                //    Alpha ? 'A' : '_',
                //    Red ? 'R' : '_',
                //    Green ? 'G' : '_',
                //    Blue ? 'B' : '_');
                return string.Format("{0},{1},{2},{3}",
                    Alpha ? '☑' : '☐',
                    Red ? '☑' : '☐',
                    Green ? '☑' : '☐',
                    Blue ? '☑' : '☐');
            }

            public bool isEqual(ColorMask cmp)
            {
                return Alpha == cmp.Alpha && Red == cmp.Red && Green == cmp.Green && Blue == cmp.Blue;
            }
        }

        private const int RecentItemsMax = 15;

        private bool mSetAll = false;

        public DialogPaletteEntry()
        {
            InitializeComponent();

            listViewRecentColors.Items.Clear();
            listViewRecentMasks.Items.Clear();

            //Add some common values to recently used lists.
            addRecentColorItem(PsColor16Bit.fromColor(Color.Magenta));
            addRecentColorItem(PsColor16Bit.fromColor(Color.Black));
            addRecentColorItem(PsColor16Bit.fromColor(Color.White));
            addRecentMaskItem(new ColorMask(false, false, false, false)); //No mask.
            addRecentMaskItem(new ColorMask(false, true, true, true)); //Alpha mask.
        }

        public ColorEditor16Bit ColorEditor
        {
            get { return colorEditor16Bit; }
        }

        public bool SetAll //Was the set all entries button clicked?
        {
            get { return mSetAll; }
        }

        private void listViewRecentColors_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ColorEditor.Value = (PsColor16Bit)e.Item.Tag;
            }
        }

        private void listViewRecentMasks_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ColorMask mask = (ColorMask)e.Item.Tag;
                ColorEditor.MaskAlpha = mask.Alpha;
                ColorEditor.MaskRed = mask.Red;
                ColorEditor.MaskGreen = mask.Green;
                ColorEditor.MaskBlue = mask.Blue;
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            colorEditor16Bit.Value = VramMngr.EditValueDefault16Bit;
            colorEditor16Bit.Mask = VramMngr.EditMaskDefault;
        }

        private void buttonSetAll_Click(object sender, EventArgs e)
        {
            mSetAll = true;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DialogPaletteEntry_Shown(object sender, EventArgs e)
        {
            mSetAll = false;
            ColorEditor.CustomColors = getRecentColors();
        }

        private void DialogPaletteEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                addRecentColorItem(ColorEditor.getPsColor16Bit());
                addRecentMaskItem(new ColorMask(ColorEditor));
            }

            //Clear selected recent items or else the ItemSelectionChanged event
            //will fire the next time this dialog opens for some stupid reason.
            listViewRecentColors.SelectedIndices.Clear();
            listViewRecentMasks.SelectedIndices.Clear();
        }

        private void addRecentColorItem(PsColor16Bit color)
        {
            if (isRecentColorUnique(color))
            {
                ListViewItem lvi = new ListViewItem(color.toStringShort());
                lvi.Tag = color;
                lvi.BackColor = color.Color;
                lvi.ForeColor = lvi.BackColor.GetBrightness() < 0.4 ? Color.White : Color.Black;
                if (listViewRecentColors.Items.Count >= RecentItemsMax)
                {
                    removeLastItem(listViewRecentColors);
                }
                listViewRecentColors.Items.Insert(0, lvi);
            }
        }

        private bool isRecentColorUnique(PsColor16Bit color)
        {
            foreach (ListViewItem item in listViewRecentColors.Items)
            {
                if (color == ((PsColor16Bit)item.Tag)) //Color is already in recent list?
                {
                    return false;
                }
            }
            return true;
        }

        private void addRecentMaskItem(ColorMask mask)
        {
            if (isRecentMaskUnique(mask))
            {
                ListViewItem lvi = new ListViewItem(mask.getItemText());
                lvi.Tag = mask;
                if (listViewRecentMasks.Items.Count >= RecentItemsMax)
                {
                    removeLastItem(listViewRecentMasks);
                }
                listViewRecentMasks.Items.Insert(0, lvi);
            }
        }

        private bool isRecentMaskUnique(ColorMask mask)
        {
            foreach (ListViewItem item in listViewRecentMasks.Items)
            {
                if (mask.isEqual((ColorMask)item.Tag)) //Mask is already in recent list?
                {
                    return false;
                }
            }
            return true;
        }

        private static void removeLastItem(ListView listView)
        {
            int count = listView.Items.Count;
            if (count > 0)
            {
                listView.Items.RemoveAt(count - 1);
            }
        }

        private List<Color> getRecentColors()
        {
            List<Color> colors = new List<Color>();
            foreach (ListViewItem item in listViewRecentColors.Items)
            {
                colors.Add(((PsColor16Bit)item.Tag).Color);
            }
            return colors;
        }
    }
}
