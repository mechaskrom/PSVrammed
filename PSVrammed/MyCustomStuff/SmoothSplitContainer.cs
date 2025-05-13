using System;
using System.Reflection;
using System.Windows.Forms;

namespace MyCustomStuff
{
    public class SmoothSplitContainer : SplitContainer
    {
        protected bool mUseSmoothSplitter = false;
        protected bool mCanKeepFocus = false;
        protected Control mFocusedCtrl = null;
        protected int mDistanceWhenVisible = 0;

        public SmoothSplitContainer()
        {
            //This has little to no effect on flickering!?
            this.DoubleBuffered = true;
            this.Panel1.SetDoubleBuffered(true);
            this.Panel2.SetDoubleBuffered(true);

            //Splitter distance changes when panels are collapsed/uncollapsed.
            //Try and keep track of the correct value by listening to a few events.
            this.SplitterMoved += SmoothSplitContainer_SplitterMoved;
            ((Panel)this.Panel1).VisibleChanged += SmoothSplitContainer_PanelVisibleChanged;
            ((Panel)this.Panel2).VisibleChanged += SmoothSplitContainer_PanelVisibleChanged;
        }

        public bool UseSmoothSplitter //Constantly update panels when moving splitter.
        {
            get { return mUseSmoothSplitter; }
            set { mUseSmoothSplitter = value; }
        }

        public bool CanKeepFocus //Keep focus or return it to previously focused control?
        {
            get { return mCanKeepFocus; }
            set { mCanKeepFocus = value; }
        }

        public bool IsSplitterBarVisible //The draggable bar between split container panels.
        {
            get { return !(Panel1Collapsed || Panel2Collapsed); }
        }

        public int DistanceWhenVisible //Distance when bar was visible.
        {
            get { return mDistanceWhenVisible; }
            set
            {
                mDistanceWhenVisible = value;
                if (IsSplitterBarVisible) //Splitter bar visible?
                {
                    SplitterDistance = value;
                }
            }
        }

        public bool canHandleKey(Keys keyData)
        {
            //bool isInputKey = this.IsInputKey(keyData); //Always false. :(
            //keyData &= ~Keys.Modifiers;
            return this.Focused &&
                (keyData == Keys.Up || keyData == Keys.Down ||
                keyData == Keys.Left || keyData == Keys.Right);
        }

        public void updateDistance()
        {
            //Splitter distance doesn't update properly when panels collapse/open.
            //Force an update by changing the value.
            int current = SplitterDistance;
            SplitterDistance = current != 0 ? 0 : 1; //Change to zero or one if already zero.
            SplitterDistance = current;
        }

        private void SmoothSplitContainer_PanelVisibleChanged(object sender, System.EventArgs e)
        {
            if (IsSplitterBarVisible) //Splitter bar became visible?
            {
                SplitterDistance = mDistanceWhenVisible;
            }
        }

        private void SmoothSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (IsSplitterBarVisible)
            {
                mDistanceWhenVisible = SplitterDistance;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //This disables the normal move behavior.
            IsSplitterFixed = mUseSmoothSplitter;

            if (!CanKeepFocus) //Don't allow splitter to keep focus?
            {
                //Get the focused control before the splitter is focused.
                mFocusedCtrl = getFocused();
            }

            //Call base last. I get some glitchy drawing otherwise.
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //Check to make sure the splitter won't be updated by the normal move behavior also.
            if (IsSplitterFixed)
            {
                //Make sure that the button used to move the splitter is the left mouse button.
                if (e.Button.Equals(MouseButtons.Left))
                {
                    //Check splitters alignment.
                    if (Orientation.Equals(Orientation.Vertical))
                    {
                        //Only move the splitter if the mouse is within the appropriate bounds.
                        if (e.X > 0 && e.X < Width)
                        {
                            SplitterDistance = e.X;
                            Refresh();
                        }
                    }
                    else
                    {
                        //Only move the splitter if the mouse is within the appropriate bounds.
                        if (e.Y > 0 && e.Y < Height)
                        {
                            SplitterDistance = e.Y;
                            Refresh();
                        }
                    }
                }
                //If a button other than left is pressed or no button at all.
                else
                {
                    //This allows the splitter to be moved normally again.
                    IsSplitterFixed = false;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            //If a previous control had focus.
            if (mFocusedCtrl != null)
            {
                //Return focus and clear the temp variable for garbage collection.
                mFocusedCtrl.Focus();
                mFocusedCtrl = null;
            }
            //This allows the splitter to be moved normally again.
            IsSplitterFixed = false;
        }

        protected Control getFocused()
        {
            return getFocused(this.ParentForm.Controls);
        }

        protected static Control getFocused(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.Focused)
                {
                    return c;
                }
                else if (c.ContainsFocus) //One of control's children has focus?
                {
                    return getFocused(c.Controls);
                }
            }
            return null; //No control on the form has focus.
        }
    }
}
