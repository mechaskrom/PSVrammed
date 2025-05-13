using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyCustomStuff
{
    //Control that handles scrolling and zooming of an object.
    public abstract class AutoScroller : ScrollableControl
    {
        protected const float ZoomMinDef = 0.10f;
        public const float ZoomDef = 1.00f;
        protected const float ZoomMaxDef = 10.00f;
        protected const int ScrollMouseAmount = 10;
        protected const int ScrollMouseMargin = 20;

        protected float mZoom = ZoomDef;
        protected float mZoomMin = ZoomMinDef;
        protected float mZoomMax = ZoomMaxDef;
        protected Size mObjectSize = Size.Empty;
        protected Rectangle mObjectDirtyRc = Rectangle.Empty;
        protected Rectangle mClipRc = Rectangle.Empty;

        public AutoScroller()
        {
            AutoScroll = true;
            DoubleBuffered = true;
        }

        protected abstract void paintObject(Graphics gr);

        public float Zoom
        {
            get { return mZoom; }
            set { setZoom(value); }
        }

        public float ZoomMin
        {
            get { return mZoomMin; }
            set
            {
                mZoomMin = value;
                setZoom(Zoom); //Update zoom.
            }
        }

        public float ZoomMax
        {
            get { return mZoomMax; }
            set
            {
                mZoomMax = value;
                setZoom(Zoom); //Update zoom.
            }
        }

        public int HorizontalScrollMaxUser //Max value that can be reached through user interaction.
        {
            get { return 1 + HorizontalScroll.Maximum - HorizontalScroll.LargeChange; }
        }

        public int VerticalScrollMaxUser //Max value that can be reached through user interaction.
        {
            get { return 1 + VerticalScroll.Maximum - VerticalScroll.LargeChange; }
        }

        public event EventChangeOldNew<AutoScroller, float> ZoomChanged;

        public void setZoom(float zoom)
        {
            zoom = zoom.Clamp(ZoomMin, ZoomMax);
            if (mZoom != zoom)
            {
                float oldZoom = mZoom;
                mZoom = zoom;
                updateObjectSize();
                Invalidate();
                OnZoomChanged(oldZoom, mZoom);
            }
        }

        protected virtual void OnZoomChanged(float oldZoom, float newZoom)
        {
            if (ZoomChanged != null) ZoomChanged(this, oldZoom, newZoom);
        }

        protected void setObjectSize(Size objectSize)
        {
            if (mObjectSize != objectSize)
            {
                mObjectSize = objectSize;
                updateObjectSize();
            }
        }

        private void updateObjectSize()
        {
            if (mObjectSize.IsEmpty)
            {
                AutoScrollMinSize = Size;
            }
            else
            {
                AutoScrollMinSize = new Size(
                    (int)(mObjectSize.Width * mZoom + 0.5f), (int)(mObjectSize.Height * mZoom + 0.5f));
                mClipRc = new Rectangle(0, 0, mObjectSize.Width, mObjectSize.Height);
            }
        }

        protected Point getClientCenter()
        {
            return new Point(ClientSize.Width / 2, ClientSize.Height / 2);
        }

        public void scrollByOffset(int dx, int dy)
        {
            Point scroll = AutoScrollPosition;
            scroll.X = dx - scroll.X;
            scroll.Y = dy - scroll.Y;
            AutoScrollPosition = scroll;
        }

        public void scrollByCenter()
        {
            scrollByCenter(getObjectPointFromClientCenter());
        }

        public void scrollByCenter(Point centerOn) //Scroll centers on position in object.
        {
            //Adjust for zoom.
            centerOn.X = (int)(centerOn.X * mZoom);
            centerOn.Y = (int)(centerOn.Y * mZoom);

            //Scrollposition is set to upper left corner so adjust it to center of client area.
            centerOn.X -= ClientSize.Width / 2;
            centerOn.Y -= ClientSize.Height / 2;

            Invalidate(); //Scrolling when following an object looks awful without this.
            AutoScrollPosition = centerOn;
        }

        protected void scrollMouseEdge(Point mouseClient)
        {
            //Scroll if mouse is near an edge.
            int dx = 0, dy = 0;
            Rectangle clientRc = ClientRectangle;
            if ((clientRc.Right - mouseClient.X) < ScrollMouseMargin)
            {
                dx = ScrollMouseAmount; //Scroll right.
            }
            else if ((mouseClient.X - clientRc.Left) < ScrollMouseMargin)
            {
                dx = -ScrollMouseAmount; //Scroll left.
            }
            if ((clientRc.Bottom - mouseClient.Y) < ScrollMouseMargin)
            {
                dy = ScrollMouseAmount; //Scroll down.
            }
            else if ((mouseClient.Y - clientRc.Top) < ScrollMouseMargin)
            {
                dy = -ScrollMouseAmount; //Scroll up.
            }

            if (dx != 0 || dy != 0)
            {
                scrollByOffset(dx, dy);
                //Update(); //Sometimes scroll isn't updated otherwise???
            }
        }

        public Point getObjectPointFromClientCenter()
        {
            return getObjectPointFromClient(getClientCenter());
        }

        public Point getObjectPointFromScreen(Point screenPoint)
        {
            //Calculate point in object from point in screen.
            return getObjectPointFromClient(PointToClient(screenPoint));
        }

        public Point getObjectPointFromClient(Point clientPoint)
        {
            //Calculate point in object from point in client area (visible area).
            float zoom = mZoom; //Seems a bit faster to use local copy.
            return new Point(
                ((clientPoint.X + HorizontalScroll.Value) / zoom).Floor(),
                ((clientPoint.Y + VerticalScroll.Value) / zoom).Floor());
        }

        public Point getObjectPointFromScreenInside(Point screenPoint) //Adjust result to be inside object.
        {
            return adjustPointInsideObject(getObjectPointFromScreen(screenPoint));
        }

        public Point getObjectPointFromClientInside(Point clientPoint) //Adjust result to be inside object.
        {
            return adjustPointInsideObject(getObjectPointFromClient(clientPoint));
        }

        public Point getClientPointFromObject(Point objectPoint)
        {
            //Calculate point in client area (visible area) from point in object.
            float zoom = mZoom; //Seems a bit faster to use local copy.
            return new Point(
                ((objectPoint.X * zoom) - HorizontalScroll.Value).Floor(),
                ((objectPoint.Y * zoom) - VerticalScroll.Value).Floor());
        }

        public Rectangle getObjectRectangleFromClient(Rectangle clientRc)
        {
            //Seems a bit faster to use local copies.
            int scrollX = HorizontalScroll.Value;
            int scrollY = VerticalScroll.Value;
            float zoom = mZoom;
            return Rectangle.FromLTRB(
                ((clientRc.X + scrollX) / zoom).Floor(),
                ((clientRc.Y + scrollY) / zoom).Floor(),
                ((clientRc.Right + scrollX) / zoom).Ceiling(),
                ((clientRc.Bottom + scrollY) / zoom).Ceiling());
        }

        public Rectangle getClientRectangleFromObject(Rectangle objectRc)
        {
            //Seems a bit faster to use local copies.
            int scrollX = HorizontalScroll.Value;
            int scrollY = VerticalScroll.Value;
            float zoom = mZoom;
            return Rectangle.FromLTRB(
                ((objectRc.X * zoom) - scrollX).Floor(),
                ((objectRc.Y * zoom) - scrollY).Floor(),
                ((objectRc.Right * zoom) - scrollX).Ceiling(),
                ((objectRc.Bottom * zoom) - scrollY).Ceiling());
        }

        public Point adjustPointInsideObject(Point objectPoint)
        {
            if (mObjectSize.IsEmpty)
            {
                return Point.Empty;
            }
            return objectPoint.Clamp(0, 0, mObjectSize.Width - 1, mObjectSize.Height - 1);
        }

        public void invalidateObject(Rectangle objectRc)
        {
            Invalidate(getClientRectangleFromObject(objectRc));
        }

        public void invalidateObjectDeferred(Rectangle objectRc)
        {
            //Call invalidate after current operation is finished.
            //Similar(?) to what multiple calls to Control.Invalidate does, but faster.

            //Log.timeBegin();
            if (!objectRc.IsEmpty)
            {
                if (mObjectDirtyRc.IsEmpty)
                {
                    mObjectDirtyRc = objectRc;
                    //Log.add("Invoke invalidate object");
                    BeginInvoke((MethodInvoker)invalidateObjectInvoker);
                }
                else
                {
                    mObjectDirtyRc = Rectangle.Union(mObjectDirtyRc, objectRc);
                }
            }
            //Log.timeEnd("invalidateObjectDeferred()");
        }

        protected void invalidateObjectInvoker()
        {
            //Log.add("Deferred invalidate object called.");
            invalidateObject(mObjectDirtyRc);
            mObjectDirtyRc = new Rectangle();
        }

        public new void Update()
        {
            if (!mObjectDirtyRc.IsEmpty)
            {
                invalidateObjectInvoker(); //Add dirty rectangle to paint.
            }
            base.Update();
        }

        public new void Refresh()
        {
            mObjectDirtyRc = new Rectangle();
            base.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Log.add("AutoScroller.OnPaint() called");

            //Log.timeBegin();

            Graphics gr = e.Graphics;

            //Transform.
            gr.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
            gr.ScaleTransform(mZoom, mZoom);

            //Clip. Automatically transformed.
            //Log.add("gr clip before = " + gr.ClipBounds);
            gr.SetClip(mClipRc, CombineMode.Intersect);
            //Log.add("gr clip after = " + gr.ClipBounds);

            //Paint object.
            paintObject(gr);

            base.OnPaint(e); //Call base last so paint event handlers are done after object.

            //Log.timeEnd("OnPaint()");
        }
    }
}
