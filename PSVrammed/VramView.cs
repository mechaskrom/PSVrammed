using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    abstract class VramView : AutoScroller
    {
        protected Vrammed mVrammed;

        public virtual void init(Vrammed vrammed)
        {
            mVrammed = vrammed;

            mVrammed.ModeChanged += Vrammed_ModeChanged;
            mVrammed.VramMngr.VramChanged += VramMngr_VramChanged;

            Marker.IsVisibleChanged += Marker_IsVisibleChanged;
            Marker.ColorChanged += Marker_ColorChanged;
            Marker.LocationChanged += Marker_LocationChanged;
            Marker.SizeChanged += Marker_SizeChanged;

            setObjectSize(BmpBounds.Size);
        }

        public abstract VramMarker Marker { get; } //Get marker associated with view.
        public abstract Rectangle BmpBounds { get; }

        private void Vrammed_ModeChanged(Vrammed sender, Mode oldValue, Mode newValue)
        {
            scrollByMarkerCenter();
        }

        private void VramMngr_VramChanged(VramMngr sender, bool isPaletteAffected)
        {
            Invalidate();
        }

        private void Marker_IsVisibleChanged(VramMarker sender, bool newIsVisible)
        {
            Invalidate();
        }

        private void Marker_ColorChanged(VramMarker sender, Color oldColor, Color newColor)
        {
            Invalidate();
        }

        private void Marker_LocationChanged(VramMarker sender, Point oldLoc, Point newLoc, bool isPreview)
        {
            if (!sender.IsSituating)
            {
                scrollByMarkerCenter();
            }
            Invalidate();
        }

        private void Marker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            Invalidate();
        }

        public void scrollByMarkerCenter()
        {
            scrollByCenter(Marker.Rectangle.GetCenterPoint());
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //Log.add("mouse wheel");
            if (ModifierKeys == Keys.Control) //Zoom.
            {
                Point centerOn = getObjectPointFromClient(e.Location);
                Zoom += e.Delta > 0 ? 1 : -1;
                scrollByCenter(centerOn);
            }
            else if (ModifierKeys == Keys.Shift) //Scroll horizontally.
            {
                scrollByOffset((HorizontalScroll.LargeChange * (e.Delta > 0 ? 1 : -1)) / 3, 0);
            }
            else if (ModifierKeys == Keys.None) //Scroll vertically.
            {
                base.OnMouseWheel(e);
            }
        }

        protected override void paintObject(Graphics gr)
        {
            if (mVrammed == null) return; //Just to make the designer happy.

            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
            gr.PixelOffsetMode = PixelOffsetMode.Half;
            paintView(gr);
        }

        protected abstract void paintView(Graphics gr);
    }
}
