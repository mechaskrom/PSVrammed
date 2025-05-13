using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MyCustomStuff;

namespace PSVrammed
{
    //Similar to vram marker, but always actual location in vram.
    class VramIndicator
    {
        private readonly VramMarker mMarker;
        private Rectangle mRc;

        public VramIndicator(VramMarker marker)
        {
            mMarker = marker;
            mRc = marker.Rectangle;

            mMarker.LocationChanged += Marker_LocationChanged;
            mMarker.SizeChanged += Marker_SizeChanged;
        }

        private void Marker_LocationChanged(VramMarker sender, Point oldLoc, Point newLoc, bool isPreview)
        {
            if (!isPreview)
            {
                setLocation(newLoc);
            }
        }

        private void Marker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            setSize(newSize);
        }

        public Rectangle Rectangle
        {
            get { return mRc; }
        }

        public Point Location
        {
            get { return Rectangle.Location; }
        }

        public Size Size
        {
            get { return Rectangle.Size; }
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

        public event EventChangeOldNew<VramIndicator, Point> LocationChanged;

        private void setLocation(Point newLoc)
        {
            newLoc = mMarker.checkLocation(newLoc);
            Point oldLoc = Location;
            if (oldLoc != newLoc)
            {
                mRc.Location = newLoc;
                OnLocationChanged(oldLoc, newLoc);
            }
        }

        private void setSize(Size newSize)
        {
            Size oldSize = Size;
            if (oldSize != newSize)
            {
                mRc.Size = newSize;
                //Update location so indicator is still inside bounds.
                setLocation(Location);
            }
        }

        private void OnLocationChanged(Point oldLoc, Point newLoc)
        {
            if (LocationChanged != null) LocationChanged(this, oldLoc, newLoc);
        }
    }
}
