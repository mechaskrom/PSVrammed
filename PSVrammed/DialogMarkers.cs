using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    partial class DialogMarkers : Form
    {
        private readonly Vrammed mVrammed;

        public DialogMarkers(Vrammed vrammed)
        {
            InitializeComponent();
            mVrammed = vrammed;
        }

        private void DialogMarkers_Shown(object sender, EventArgs e)
        {
            //Texture marker.
            colorButtonTexture.ColorBox = mVrammed.TextureMarker.Color;
            addItems(comboBoxTextureWidth, VramMarkerTexture.Default.Widths);
            comboBoxTextureWidth.SelectedItem = mVrammed.TextureMarker.Width;
            addItems(comboBoxTextureHeight, VramMarkerTexture.Default.Heights);
            comboBoxTextureHeight.SelectedItem = mVrammed.TextureMarker.Height;

            //Compare marker.
            colorButtonCompare.ColorBox = mVrammed.CompareMarker.Color;
            addItems(comboBoxCompareResultAlpha, VramMarkerCompare.Default.ResultAlphas);
            comboBoxCompareResultAlpha.SelectedItem = mVrammed.CompareMarker.ResultAlpha;
            colorButtonCompareResult.ColorBox = mVrammed.CompareMarker.ResultColor;

            //Palette marker.
            colorButtonPalette.ColorBox = mVrammed.PaletteMarker.Color;
        }

        private void DialogMarkers_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                //Texture marker.
                mVrammed.TextureMarker.Color = colorButtonTexture.ColorBox;
                mVrammed.TextureMarker.setSize(new Size(
                    (int)comboBoxTextureWidth.SelectedItem,
                    (int)comboBoxTextureHeight.SelectedItem));

                //Compare marker.
                mVrammed.CompareMarker.Color = colorButtonCompare.ColorBox;
                mVrammed.CompareMarker.ResultAlpha = (float)comboBoxCompareResultAlpha.SelectedItem;
                mVrammed.CompareMarker.ResultColor = colorButtonCompareResult.ColorBox;

                //Palette marker.
                mVrammed.PaletteMarker.Color = colorButtonPalette.ColorBox;
            }
        }

        private void addItems<T>(ComboBox comboBox, List<T> items)
        {
            //comboBox.MaxDropDownItems = items.Length;
            comboBox.Items.AddRange(items.Cast<object>().ToArray());
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            //Texture marker.
            colorButtonTexture.ColorBox = VramMarkerTexture.Default.Color;
            comboBoxTextureWidth.SelectedItem = VramMarkerTexture.Default.Width;
            comboBoxTextureHeight.SelectedItem = VramMarkerTexture.Default.Height;

            //Compare marker.
            colorButtonCompare.ColorBox = VramMarkerCompare.Default.Color;
            comboBoxCompareResultAlpha.SelectedItem = VramMarkerCompare.Default.ResultAlpha;
            colorButtonCompareResult.ColorBox = VramMarkerCompare.Default.ResultColor;

            //Palette marker.
            colorButtonPalette.ColorBox = VramMarkerPalette.Default.Color;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.setCustomColors(new Color[]{
                colorButtonTexture.ColorBox,
                colorButtonCompare.ColorBox,
                colorButtonCompareResult.ColorBox,
                colorButtonPalette.ColorBox });
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ((ColorButton)sender).ColorBox = colorDialog.Color;
            }
        }
    }

    static class ColorDialogExt
    {
        public static void setCustomColors(this ColorDialog cd, IEnumerable<Color> colors)
        {
            Color[] cs = colors.ToArray();
            int[] vs = new int[cs.Length];
            for (int i = 0; i < cs.Length; i++)
            {
                //ColorDialog's custom color array uses BGR format and not RGB.
                Color c = cs[i];
                vs[i] = (c.B << 16) | (c.G << 8) | (c.R << 0);
            }
            cd.CustomColors = vs;
        }

        public static Color[] getCustomColors(this ColorDialog cd)
        {
            int[] vs = cd.CustomColors;
            Color[] cs = new Color[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                //ColorDialog's custom color array uses BGR format and not RGB.
                int v = vs[i];
                cs[i] = Color.FromArgb((v >> 0) & 0xFF, (v >> 8) & 0xFF, (v >> 16) & 0xFF);
            }
            return cs;
        }
    }
}
