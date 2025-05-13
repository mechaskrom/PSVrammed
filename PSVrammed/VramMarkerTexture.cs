using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    class VramMarkerTexture : VramMarker
    {
        public static readonly VramMarkerDefaultTexture Default = new VramMarkerDefaultTexture();

        public VramMarkerTexture(Vrammed vrammed)
            : base(vrammed, Default, vrammed.TextureView)
        {
        }

        public void init()
        {
            mVrammed.VramMngr.BitDepthTextureChanged += VramMngr_BitDepthTextureChanged;
            mVrammed.TextureView.Paint += TextureView_Paint;
        }

        private void VramMngr_BitDepthTextureChanged(VramMngr sender, VramMngr.BitDepth oldBitDepth, VramMngr.BitDepth newBitDepth)
        {
            //Convert marker to the new bit depth. Setting location and size also makes sure
            //marker is still inside bitmap's bounds. Set size first in case marker is near
            //the right edge of the bounds so the specified location can be set afterwards.
            Rectangle rc = VramMngr.rcToBitDepth(Rectangle, oldBitDepth, newBitDepth);
            setSize(rc.Size); //Set size before location.
            setLocation(rc.Location);
        }

        private void TextureView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        protected override string Name
        {
            get { return "TextureMarker"; }
        }

        protected override int StepX
        {
            get { return Width; }
        }

        protected override int StepY
        {
            get { return Height; }
        }

        protected override int StepXAlt
        {
            get { return 1; }
        }

        protected override int StepYAlt
        {
            get { return 1; }
        }

        public override Point checkLocation(Point loc)
        {
            Rectangle bounds = mVrammed.VramMngr.BmpTextureBounds;
            bounds.Width -= Width;
            bounds.Height -= Height;
            return checkLocation(loc, bounds);
        }

        private void paint(Graphics gr)
        {
            if (mIsVisible)
            {
                //MyCustomStuff.Log.timeBegin();
                //drawSquareMarker(gr, Rectangle);
                //drawRoundedMarker(gr, Rectangle);
                //drawEgyptianMarker(gr, Rectangle);
                //drawCornerMarker(gr, Rectangle);
                drawDashedMarker(gr, Rectangle);
                //MyCustomStuff.Log.timeEnd("Draw Texture Marker()");

                if (Location != Indicator.Location) //Paint extra marker at vram indicator?
                {
                    //drawSquareMarker(gr, Indicator.Rectangle);
                    //drawRoundedMarker(gr, Indicator.Rectangle);
                    //drawEgyptianMarker(gr, Rectangle);
                    drawDashedMarker(gr, Rectangle);
                }
            }
        }
    }

    //***********************************************************************************
    //***********************************************************************************

    class VramMarkerDefaultTexture : VramMarkerDefault
    {
        public VramMarkerDefaultTexture()
            : base(
                0, //X.
                0, //Y.
                128, //Width.
                128, //Height.
                Color.FromArgb(0, 255, 0), //Color.
                1, //Snap x.
                1, //Snap y.
                new List<int> { 1, 4, 8, 16, 32, 64, 128, 240, 256, 320, 384, 512, 640, 1024 }, //Widths.
                new List<int> { 1, 4, 8, 16, 32, 64, 128, 240, 256, 320, 384, 512 } //Heights.
            )
        {
        }
    }
}
