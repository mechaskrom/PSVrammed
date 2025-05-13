using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

using MyCustomStuff;

namespace PSVrammed
{
    //Visual marker used in vram views.
    abstract class VramMarker
    {
        protected readonly Vrammed mVrammed;
        protected readonly VramMarkerDefault mDefault; //Default settings.
        protected readonly VramView mView; //View associated with marker.

        protected bool mIsVisible;
        protected SolidBrush mBrush; //Draw style and color.
        protected Rectangle mRc; //"Visual" location and size of marker in view.
        protected readonly VramIndicator mIndicator; //"Actual" location in vram.
        protected Point mSituateStartLoc; //Location when situating started.
        protected bool mIsSituating;
        protected bool mUpdateSituating; //Update indicator location also when situating?

        //Marker has two rectangles. One is its visual rectangle and should be used when
        //painting it. The other is more of an actual rectangle and should be used when
        //reading data from vram.

        //Their size is always the same, but location can be different in some cases like
        //when handling the situate and edit mouse tools.

        protected VramMarker(Vrammed vrammed, VramMarkerDefault dflt, VramView view)
        {
            mVrammed = vrammed;
            mDefault = dflt;
            mView = view;

            mIsVisible = false;
            mBrush = new SolidBrush(dflt.Color);
            mRc = new Rectangle(dflt.X, dflt.Y, dflt.Width, dflt.Height);
            mSituateStartLoc = Location;
            mIsSituating = false;
            mUpdateSituating = false;

            mIndicator = new VramIndicator(this);
        }

        //*******************************************************************************
        //*******************************************************************************

        #region Properties
        protected virtual string Name
        {
            get { return "VramMarker"; }
        }

        public bool IsVisible
        {
            get { return mIsVisible; }
            set { setIsVisible(value); }
        }

        public Color Color
        {
            get { return mBrush.Color; }
            set { setColor(value); }
        }

        public bool IsSituating
        {
            get { return mIsSituating; }
        }

        public VramIndicator Indicator
        {
            get { return mIndicator; }
        }

        public Rectangle Rectangle
        {
            get { return mRc; }
        }

        public Point Location
        {
            get { return Rectangle.Location; }
            set { setLocation(value); }
        }

        public Size Size
        {
            get { return Rectangle.Size; }
            set { setSize(value); }
        }

        public int X
        {
            get { return Rectangle.X; }
        }

        public int Y
        {
            get { return Rectangle.Y; }
        }

        public int Width
        {
            get { return Rectangle.Width; }
        }

        public int Height
        {
            get { return Rectangle.Height; }
        }

        protected virtual int StepX
        {
            get { return mDefault.SnapX; }
        }

        protected virtual int StepY
        {
            get { return mDefault.SnapY; }
        }

        protected virtual int StepXAlt
        {
            get { return 64; }
        }

        protected virtual int StepYAlt
        {
            get { return 64; }
        }

        #endregion Properties

        //*******************************************************************************
        //*******************************************************************************

        #region Appearance functions

        private void setIsVisible(bool newIsVisible)
        {
            if (IsVisible != newIsVisible)
            {
                mIsVisible = newIsVisible;
                OnIsVisibleChanged(newIsVisible);
            }
        }

        private void setColor(Color newColor)
        {
            Color oldColor = Color;
            if (oldColor != newColor)
            {
                mBrush.Color = newColor;
                OnColorChanged(oldColor, newColor);
            }
        }

        #endregion Appearance functions

        //*******************************************************************************
        //*******************************************************************************

        #region Situate functions

        public bool enterSituate(Point centLoc, bool updateSituating)
        {
            if (!mIsSituating && mIsVisible)
            {
                Log.add(this.Name + " entered situate");
                mSituateStartLoc = Location;
                mIsSituating = true;
                mUpdateSituating = updateSituating;
                setLocationCenter(centLoc);
                return true;
            }
            else return false;
        }

        public void moveSituate(Point centLoc)
        {
            if (mIsSituating)
            {
                setLocationCenter(centLoc);
            }
        }

        public bool saveSituate(Point centLoc)
        {
            if (mIsSituating)
            {
                Log.add(this.Name + " saved situate");
                mIsSituating = false;
                mUpdateSituating = false;
                mRc.Location = mSituateStartLoc;
                //Change bools and reset marker back to start location before setting
                //the new location so it will look like an "actual" move.
                setLocationCenter(centLoc);
                return true;
            }
            else return false;
        }

        public bool leaveSituate()
        {
            if (mIsSituating)
            {
                Log.add(this.Name + " left situate");
                //Reset marker back to start location. Do it before changing
                //bools so it will not look like an "actual" move.
                setLocation(mSituateStartLoc);
                mIsSituating = false;
                mUpdateSituating = false;
                return true;
            }
            else return false;
        }

        #endregion Situate functions

        //*******************************************************************************
        //*******************************************************************************

        #region Location functions

        public void snapLocation(bool altSnap)
        {
            if (IsVisible && !IsSituating)
            {
                Point loc = Location;
                loc.X = loc.X.Snap(altSnap ? Width : 8);
                loc.Y = loc.Y.Snap(altSnap ? Height : 8);
                setLocation(loc);
            }
        }

        public void stepLocationUp(bool altStep)
        {
            stepLocation(0, -(altStep ? StepYAlt : StepY));
        }

        public void stepLocationDown(bool altStep)
        {
            stepLocation(0, altStep ? StepYAlt : StepY);
        }

        public void stepLocationLeft(bool altStep)
        {
            stepLocation(-(altStep ? StepXAlt : StepX), 0);
        }

        public void stepLocationRight(bool altStep)
        {
            stepLocation(altStep ? StepXAlt : StepX, 0);
        }

        protected void stepLocation(int dX, int dY)
        {
            if (IsVisible && !IsSituating) setLocation(Location.OffsetRet(dX, dY));
        }

        public void setLocation(Point newLoc)
        {
            newLoc = checkLocation(newLoc);
            Point oldLoc = Location;
            if (oldLoc != newLoc)
            {
                mRc.Location = newLoc;
                //Location change happened in preview mode?
                bool isPreview = mIsSituating && !mUpdateSituating;
                OnLocationChanged(oldLoc, newLoc, isPreview);
            }
        }

        private void setLocationCenter(Point centLoc)
        {
            //Offset centLoc so if set as marker's location it will be equal to its center.
            centLoc.Offset(-Width / 2, -Height / 2);
            setLocation(centLoc);
        }

        public abstract Point checkLocation(Point loc);

        protected Point checkLocation(Point loc, Rectangle bounds)
        {
            //Snap location to nearest multiple.
            loc.X = loc.X.Snap(mDefault.SnapX);
            loc.Y = loc.Y.Snap(mDefault.SnapY);

            //Adjust location so it's inside bounds.
            loc.X = loc.X.Clamp(bounds.X, bounds.Right);
            loc.Y = loc.Y.Clamp(bounds.Y, bounds.Bottom);

            return loc;
        }

        #endregion Location functions

        //*******************************************************************************
        //*******************************************************************************

        #region Size functions

        public void stepSize(bool stepUp, bool stepWidth, bool stepHeight)
        {
            int dir = stepUp ? 1 : -1;
            stepSize(stepWidth ? dir : 0, stepHeight ? dir : 0);
        }

        private void stepSize(int dirWidth, int dirHeight)
        {
            //Use closest size after/before current in selectable.
            Size size = Size;
            if (dirWidth != 0)
            {
                List<int> widths = mDefault.Widths;
                int indWidth = widths.FindNearestIndex(size.Width);
                indWidth = (indWidth + dirWidth).Clamp(0, widths.Count - 1);
                size.Width = widths[indWidth];
            }
            if (dirHeight != 0)
            {
                List<int> heights = mDefault.Heights;
                int indHeight = heights.FindNearestIndex(size.Height);
                indHeight = (indHeight + dirHeight).Clamp(0, heights.Count - 1);
                size.Height = heights[indHeight];
            }
            setSize(size);
        }

        public void resetSize()
        {
            setSize(mDefault.SizeDef);
        }

        public void setSize(Size newSize)
        {
            //Make sure that new size is within range of default values and inside view.
            newSize = newSize.Clamp(mDefault.SizeMin, mDefault.SizeMax);
            newSize = newSize.Clamp(new Size(0, 0), mView.BmpBounds.Size);
            Size oldSize = Size;
            if (oldSize != newSize)
            {
                mRc.Size = newSize;
                //Update location so marker is still inside bounds.
                setLocation(Location);
                OnSizeChanged(oldSize, newSize);
            }
        }

        #endregion Size functions

        //*******************************************************************************
        //*******************************************************************************

        #region Events

        public event EventChangeNew<VramMarker, bool> IsVisibleChanged;
        public event EventChangeOldNew<VramMarker, Color> ColorChanged;
        public event EventChangeOldNewArg<VramMarker, Point, bool> LocationChanged;
        public event EventChangeOldNew<VramMarker, Size> SizeChanged;

        private void OnIsVisibleChanged(bool newIsVisible)
        {
            if (IsVisibleChanged != null) IsVisibleChanged(this, newIsVisible);
        }

        private void OnColorChanged(Color oldColor, Color newColor)
        {
            if (ColorChanged != null) ColorChanged(this, oldColor, newColor);
        }

        private void OnLocationChanged(Point oldLoc, Point newLoc, bool isPreview)
        {
            if (LocationChanged != null) LocationChanged(this, oldLoc, newLoc, isPreview);
        }

        private void OnSizeChanged(Size oldSize, Size newSize)
        {
            if (SizeChanged != null) SizeChanged(this, oldSize, newSize);
        }

        #endregion Events

        //*******************************************************************************
        //*******************************************************************************

        #region Paint markers

        protected void drawSquareMarker(Graphics gr, Rectangle rc)
        {
            drawSquareMarker(gr, rc, mBrush);
        }

        protected static void drawSquareMarker(Graphics gr, Rectangle rc, Brush brush)
        {
            Matrix m = gr.Transform;
            //Vram view's coordinates already scaled. Just need to translate them.
            gr.TranslateTransform(rc.X, rc.Y);

            gr.DrawLineHor(brush, -2, -1, rc.Width + 4, 1);
            gr.DrawLineHor(brush, -2, rc.Height, rc.Width + 4, 1);

            gr.DrawLineVer(brush, -1, -2, rc.Height + 4, 1);
            gr.DrawLineVer(brush, rc.Width, -2, rc.Height + 4, 1);

            gr.Transform = m;
        }

        protected void drawRoundedMarker(Graphics gr, Rectangle rc)
        {
            drawRoundedMarker(gr, rc, mBrush);
        }

        protected static void drawRoundedMarker(Graphics gr, Rectangle rc, Brush brush)
        {
            Matrix m = gr.Transform;
            //Vram view's coordinates already scaled. Just need to translate them.
            gr.TranslateTransform(rc.X, rc.Y);

            gr.DrawLineHor(brush, 0, -1, rc.Width, 1);
            gr.DrawLineHor(brush, 0, rc.Height, rc.Width, 1);

            gr.DrawLineVer(brush, -1, 0, rc.Height, 1);
            gr.DrawLineVer(brush, rc.Width, 0, rc.Height, 1);

            gr.Transform = m;
        }

        protected void drawEgyptianMarker(Graphics gr, Rectangle rc)
        {
            drawEgyptianMarker(gr, rc, mBrush);
        }

        protected static void drawEgyptianMarker(Graphics gr, Rectangle rc, Brush brush)
        {
            // .--
            // |
            //      |
            //    --'
            Matrix m = gr.Transform;
            //Vram view's coordinates already scaled. Just need to translate them.
            gr.TranslateTransform(rc.X, rc.Y);

            int halfWidth = Math.Max(rc.Width / 2, 2);
            int halfHeight = Math.Max(rc.Height / 2, 2);

            gr.DrawLineHor(brush, -1, -1, halfWidth, 1);
            gr.DrawLineHor(brush, rc.Width - halfWidth, rc.Height, halfWidth + 1, 1);

            gr.DrawLineVer(brush, -1, 0, halfHeight, 1);
            gr.DrawLineVer(brush, rc.Width, rc.Height - halfHeight, halfHeight, 1);

            gr.Transform = m;
        }

        protected void drawCornerMarker(Graphics gr, Rectangle rc)
        {
            drawCornerMarker(gr, rc, mBrush);
        }

        protected static void drawCornerMarker(Graphics gr, Rectangle rc, Brush brush)
        {
            // .--  --.
            // |      |
            //
            // |      |
            // '--  --'
            Matrix m = gr.Transform;
            //Vram view's coordinates already scaled. Just need to translate them.
            gr.TranslateTransform(rc.X, rc.Y);

            int thirdWidth = Math.Max(rc.Width / 3, 2);
            int thirdHeight = Math.Max(rc.Height / 3, 2);

            gr.DrawLineHor(brush, -1, -1, thirdWidth, 1);
            gr.DrawLineHor(brush, -1, rc.Height, thirdWidth, 1);
            gr.DrawLineHor(brush, rc.Width - thirdWidth, -1, thirdWidth + 1, 1);
            gr.DrawLineHor(brush, rc.Width - thirdWidth, rc.Height, thirdWidth + 1, 1);

            gr.DrawLineVer(brush, -1, 0, thirdHeight, 1);
            gr.DrawLineVer(brush, rc.Width, 0, thirdHeight, 1);
            gr.DrawLineVer(brush, -1, rc.Height - thirdHeight, thirdHeight, 1);
            gr.DrawLineVer(brush, rc.Width, rc.Height - thirdHeight, thirdHeight, 1);

            gr.Transform = m;
        }

        protected void drawDashedMarker(Graphics gr, Rectangle rc)
        {
            rc.Inflate(1, 1);
            gr.DrawRectangleDashed(mBrush, rc, 1, 6);
        }

        #endregion Paint markers
    }

    //***********************************************************************************
    //***********************************************************************************

    //There are many settings for markers. Gather all default values for them in here.
    class VramMarkerDefault
    {
        private readonly int mX;
        private readonly int mY;
        private readonly int mWidth;
        private readonly int mHeight;
        private readonly Color mColor;
        private readonly int mSnapX;
        private readonly int mSnapY;

        //Valid sizes.
        private readonly List<int> mWidths;
        private readonly List<int> mHeights;
        private readonly Size mSizeMin;
        private readonly Size mSizeMax;

        public VramMarkerDefault(int x, int y, int width, int height, Color color,
            int snapX, int snapY, List<int> widths, List<int> heights)
        {
            mX = x;
            mY = y;
            mWidth = width;
            mHeight = height;
            mColor = color;
            mSnapX = snapX;
            mSnapY = snapY;

            //Make sure default values are included.
            widths.Add(width);
            heights.Add(height);

            //Remove duplicate values.
            mWidths = widths.Distinct().ToList();
            mHeights = heights.Distinct().ToList();

            if (mWidths.Count == 0 || mHeights.Count == 0)
            {
                throw new ArgumentException("Widths or heights list cannot be empty!");
            }

            //Make sure values are sorted ascending.
            mWidths.Sort();
            mHeights.Sort();

            mSizeMin = new Size(mWidths[0], mHeights[0]);
            mSizeMax = new Size(mWidths[mWidths.Count - 1], mHeights[mHeights.Count - 1]);

            if (mSizeMin.Width < 0 || mSizeMin.Height < 0)
            {
                throw new ArgumentException("Min width or height cannot be negative!");
            }
            if (mSizeMax.Width > VramMngr.VramWidth16Bit || mSizeMax.Height > VramMngr.VramHeight)
            {
                throw new ArgumentException("Max width or height cannot be larger than VRAM dimensions!");
            }
        }

        public int X { get { return mX; } }
        public int Y { get { return mY; } }
        public int Width { get { return mWidth; } }
        public int Height { get { return mHeight; } }
        public Rectangle Rect { get { return new Rectangle(X, Y, Width, Height); } }
        public Color Color { get { return mColor; } }
        public int SnapX { get { return mSnapX; } }
        public int SnapY { get { return mSnapY; } }
        public List<int> Widths { get { return mWidths.ToList(); } }
        public List<int> Heights { get { return mHeights.ToList(); } }
        public Size SizeMin { get { return mSizeMin; } }
        public Size SizeDef { get { return new Size(Width, Height); } }
        public Size SizeMax { get { return mSizeMax; } }
    }
}
