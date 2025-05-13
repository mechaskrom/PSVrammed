using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    class VramMarkerPalette : VramMarker
    {
        public static readonly VramMarkerDefaultPalette Default = new VramMarkerDefaultPalette();

        public VramMarkerPalette(Vrammed vrammed)
            : base(vrammed, Default, vrammed.PaletteView)
        {
        }

        public void init()
        {
            mVrammed.VramMngr.BitDepthTextureChanged += VramMngr_BitDepthTextureChanged;
            mVrammed.PaletteView.Paint += PaletteView_Paint;
        }

        private void VramMngr_BitDepthTextureChanged(VramMngr sender, VramMngr.BitDepth oldBitDepth, VramMngr.BitDepth newBitDepth)
        {
            //Palette width could have changed.
            Size size = Size;
            size.Width = VramMngr.toPaletteWidth(newBitDepth);
            setSize(size);
        }

        private void PaletteView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        protected override string Name
        {
            get { return "PaletteMarker"; }
        }

        public override Point checkLocation(Point loc)
        {
            Rectangle bounds = mView.BmpBounds;
            //Allow 8 bit palette to move outside bmp's right side by pretending it's always 4 bit wide.
            bounds.Width -= VramMngr.PaletteWidth4Bit;
            bounds.Height -= Height;
            return checkLocation(loc, bounds);
        }

        protected override int StepYAlt
        {
            get { return 16; }
        }

        private void paint(Graphics gr)
        {
            if (mIsVisible)
            {
                drawPaletteMarker(gr, Rectangle);

                if (Location != Indicator.Location) //Paint extra marker at vram indicator?
                {
                    drawPaletteMarker(gr, Indicator.Rectangle);
                }
            }
        }

        private void drawPaletteMarker(Graphics gr, Rectangle rc)
        {
            //drawSquareMarker(gr, rc);
            //drawRoundedMarker(gr, rc);
            //drawEgyptianMarker(gr, rc);
            //drawCornerMarker(gr, Rectangle);
            drawDashedMarker(gr, rc);

            //If the palette editor is open/visible then draw two small notches at pixel
            //corresponding to currently selected column (palette index).
            if (mVrammed.PaletteEditor.Visible)
            {
                int x = rc.X + mVrammed.PaletteEditor.SelectedColumn;
                gr.FillRectangle(mBrush, x, rc.Top - 3, 1, 2); //Notch above column.
                gr.FillRectangle(mBrush, x, rc.Bottom + 1, 1, 2); //Notch below.
            }
        }
    }

    //***********************************************************************************
    //***********************************************************************************

    class VramMarkerDefaultPalette : VramMarkerDefault
    {
        public VramMarkerDefaultPalette()
            : base(
                0, //X.
                480, //Y.
                VramMngr.PaletteWidth8Bit, //Width.
                VramMngr.PaletteHeight, //Height.
                Color.FromArgb(64, 224, 255), //Color.
                16, //Snap x.
                1, //Snap y.
                new List<int> { VramMngr.PaletteWidth4Bit, VramMngr.PaletteWidth8Bit }, //Widths.
                new List<int> { VramMngr.PaletteHeight } //Heights.
            )
        {
        }
    }
}
