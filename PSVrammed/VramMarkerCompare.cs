using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using MyCustomStuff;

namespace PSVrammed
{
    class VramMarkerCompare : VramMarker
    {
        //Minimum pixels to keep visible when moving compare marker outside bmp.
        private const int MinVisible = 16;

        //32bit PARGB bitmap with result of comparing content of texture and compare indicators.
        private readonly BitmapPinned32bppPArgb mCmpResultBmp;
        private Color mCmpResultColor; //Color and transparency of compare result.
        private float mCmpResultAlpha;
        private bool mUpdateCmpResultBmp; //Flag to check if we need to update compare result bmp.
        private bool mFlipH; //Flags for horizontal and vertical texture flip.
        private bool mFlipV;

        public static readonly VramMarkerDefaultCompare Default = new VramMarkerDefaultCompare();

        public VramMarkerCompare(Vrammed vrammed)
            : base(vrammed, Default, vrammed.CompareView)
        {
            //Set compare result bmp to max size so it can hold all sizes.
            //Otherwise we would need to resize it when marker's size changes.
            mCmpResultBmp = new BitmapPinned32bppPArgb(Default.SizeMax.Width, Default.SizeMax.Height);

            mCmpResultColor = Default.ResultColor;
            mCmpResultAlpha = Default.ResultAlpha;
            mUpdateCmpResultBmp = true;
            mFlipH = mFlipV = false;
        }

        public void init()
        {
            mVrammed.VramMngr.VramChanged += VramMngr_VramChanged;
            mVrammed.CompareView.Paint += CompareView_Paint;
            mVrammed.TextureIndicator.LocationChanged += TextureIndicator_LocationChanged;
            mVrammed.CompareIndicator.LocationChanged += CompareIndicator_LocationChanged;
            mVrammed.PaletteIndicator.LocationChanged += PaletteIndicator_LocationChanged;
            mVrammed.TextureMarker.SizeChanged += TextureMarker_SizeChanged;
            mVrammed.CompareMarker.SizeChanged += CompareMarker_SizeChanged;
            mVrammed.PaletteMarker.SizeChanged += PaletteMarker_SizeChanged;
        }

        private void VramMngr_VramChanged(VramMngr sender, bool isPaletteAffected)
        {
            compareResultChanged();
        }

        private void CompareView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        private void TextureIndicator_LocationChanged(VramIndicator sender, Point oldLoc, Point newLoc)
        {
            compareResultChanged();
        }

        private void CompareIndicator_LocationChanged(VramIndicator sender, Point oldLoc, Point newLoc)
        {
            compareResultChanged();
        }

        private void PaletteIndicator_LocationChanged(VramIndicator sender, Point oldLoc, Point newLoc)
        {
            compareResultChanged();
        }

        private void TextureMarker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            //Set compare marker's size same as texture marker.
            setSize(newSize);
            compareResultChanged();
        }

        private void CompareMarker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            compareResultChanged();
        }

        private void PaletteMarker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            compareResultChanged();
        }

        protected override string Name
        {
            get { return "CompareMarker"; }
        }

        public override Point checkLocation(Point loc)
        {
            Rectangle bounds = mView.BmpBounds;
            //Inflate bounds so compare marker can move outside bmp.
            bounds.Inflate(Math.Max(0, Width - MinVisible), Math.Max(0, Height - MinVisible));
            bounds.Width -= Width;
            bounds.Height -= Height;
            return checkLocation(loc, bounds);
        }

        public Color ResultColor
        {
            get { return mCmpResultColor; }
            set { setResultColor(value); }
        }

        public float ResultAlpha
        {
            get { return mCmpResultAlpha; }
            set { setResultAlpha(value); }
        }

        public bool FlipH
        {
            get { return mFlipH; }
            set { setFlip(value, FlipV); }
        }

        public bool FlipV
        {
            get { return mFlipV; }
            set { setFlip(FlipH, value); }
        }

        public void stepResultAlpha(int dirAlpha)
        {
            if (mIsVisible)
            {
                //Use closest result alpha after/before current in selectable.
                List<float> alphas = Default.ResultAlphas;
                int indAlphas = alphas.FindNearestIndex(mCmpResultAlpha);
                indAlphas = (indAlphas + dirAlpha).Clamp(0, alphas.Count - 1);
                ResultAlpha = alphas[indAlphas];
            }
        }

        public void toggleFlipH()
        {
            if (mIsVisible) FlipH = !FlipH;
        }

        public void toggleFlipV()
        {
            if (mIsVisible) FlipV = !FlipV;
        }

        private void setResultColor(Color newColor)
        {
            Color oldColor = ResultColor;
            if (oldColor != newColor)
            {
                mCmpResultColor = newColor;
                OnResultColorChanged(oldColor, newColor);
            }
        }

        private void setResultAlpha(float newAlpha)
        {
            newAlpha = newAlpha.Clamp(0.00f, 1.00f);
            float oldAlpha = ResultAlpha;
            if (oldAlpha != newAlpha)
            {
                mCmpResultAlpha = newAlpha;
                OnResultAlphaChanged(oldAlpha, newAlpha);
            }
        }

        private void setFlip(bool newFlipH, bool newFlipV)
        {
            bool oldFlipH = FlipH;
            bool oldFlipV = FlipV;
            if (oldFlipH != newFlipH || oldFlipV != newFlipV)
            {
                mFlipH = newFlipH;
                mFlipV = newFlipV;
                OnFlipChanged(newFlipH, newFlipV);
            }
        }

        public event EventChangeOldNew<VramMarkerCompare, Color> ResultColorChanged;
        public event EventChangeOldNew<VramMarkerCompare, float> ResultAlphaChanged;
        public event EventChangeNew<VramMarkerCompare, bool, bool> FlipChanged;
        public event EventChange<VramMarkerCompare> ResultChanged;

        private void OnResultColorChanged(Color oldColor, Color newColor)
        {
            compareResultChanged();
            if (ResultColorChanged != null) ResultColorChanged(this, oldColor, newColor);
        }

        private void OnResultAlphaChanged(float oldAlpha, float newAlpha)
        {
            compareResultChanged();
            if (ResultAlphaChanged != null) ResultAlphaChanged(this, oldAlpha, newAlpha);
        }

        private void OnFlipChanged(bool newFlipH, bool newFlipV)
        {
            compareResultChanged();
            if (FlipChanged != null) FlipChanged(this, newFlipH, newFlipV);
        }

        private void OnResultChanged()
        {
            if (ResultChanged != null) ResultChanged(this);
        }

        private void compareResultChanged()
        {
            mUpdateCmpResultBmp = true;
            OnResultChanged();
        }

        private void updateCompareBmp()
        {
            if (mUpdateCmpResultBmp)
            {
                Log.timeBegin();

                mVrammed.VramMngr.updateCompareBmp(mCmpResultBmp,
                    mFlipH, mFlipV, mCmpResultAlpha, mCmpResultColor);

                mUpdateCmpResultBmp = false;

                Log.timeEnd("updateCompareBmp()");
            }
        }

        public void debugSaveCompareResultBmp(string path) //TODO: Remove?! Testing!
        {
            updateCompareBmp();
            mCmpResultBmp.Bitmap.Save(path);
        }

        private void paint(Graphics gr)
        {
            if (mIsVisible)
            {
                updateCompareBmp();
                if (mUpdateSituating)
                {
                    drawResultBmp(gr, Location);
                }
                else
                {
                    drawResultBmp(gr, Indicator.Location);
                }

                //drawSquareMarker(gr, Rectangle);
                //drawRoundedMarker(gr, Rectangle);
                drawDashedMarker(gr, Rectangle);

                if (Location != Indicator.Location) //Paint extra marker at vram indicator?
                {
                    //drawSquareMarker(gr, Indicator.Rectangle);
                    //drawRoundedMarker(gr, Indicator.Rectangle);
                    drawDashedMarker(gr, Rectangle);
                }
            }
        }

        private void drawResultBmp(Graphics gr, Point location)
        {
            gr.DrawImage(mCmpResultBmp.Bitmap, location.X, location.Y,
                new Rectangle(0, 0, Width, Height), GraphicsUnit.Pixel);
        }
    }

    //***********************************************************************************
    //***********************************************************************************

    class VramMarkerDefaultCompare : VramMarkerDefault
    {
        private readonly Color mResultColor;
        private readonly float mResultAlpha;

        //Available values in settings dialog.
        private readonly List<float> mResultAlphas;

        private VramMarkerDefaultCompare(int x, int y, int width, int height, Color color,
            int snapX, int snapY, List<int> widths, List<int> heights,
            Color resultColor, float resultAlpha, List<float> resultAlphas)
            : base(x, y, width, height, color, snapX, snapY, widths, heights)
        {
            mResultColor = resultColor;
            mResultAlpha = resultAlpha;

            //Make sure default values are included.
            resultAlphas.Add(resultAlpha);

            //Remove duplicate values.
            mResultAlphas = resultAlphas.Distinct().ToList();

            //Make sure values are sorted ascending.
            mResultAlphas.Sort();
        }

        public Color ResultColor { get { return mResultColor; } }
        public float ResultAlpha { get { return mResultAlpha; } }

        public List<float> ResultAlphas { get { return mResultAlphas.ToList(); } }

        public VramMarkerDefaultCompare()
            : this(
                0, //X.
                0, //Y.
                VramMarkerTexture.Default.Width, //Width.
                VramMarkerTexture.Default.Height, //Height.
                Color.FromArgb(255, 255, 0), //Color.
                1, //Snap x.
                1, //Snap y.
                VramMarkerTexture.Default.Widths, //Widths.
                VramMarkerTexture.Default.Heights, //Heights.
                Color.FromArgb(255, 0, 255), //Result color.
                0.40f, //Result alpha, 0==transparent, 1==opaque.
                new List<float> { 0.30f, 0.40f, 0.50f, 0.60f, 0.70f } //Result alphas.
            )
        {
        }
    }
}
