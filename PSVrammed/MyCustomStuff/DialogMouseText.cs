using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCustomStuff
{
    //Displays a tool tip-like textbox at mouse position.
    //Uses a borderless form with a text label.
    //ToolTip could also work, but it uses more cpu-time and doesn't look good when form doesn't has focus.
    public partial class DialogMouseText : Form
    {
        public DialogMouseText()
        {
            Application.AddMessageFilter(new GlobalMouseHandler());
            InitializeComponent();
            this.Hide();
        }

        public string MouseText
        {
            get { return labelText.Text; }
            set { labelText.Text = value; }
        }

        public void Show(string text)
        {
            if (MouseText != text)
            {
                MouseText = text;
            }
            if (!this.Visible)
            {
                this.Show();
            }
        }

        private void setLocation(Point location)
        {
            Rectangle mouseRc = new Rectangle(location, Cursor.Size);
            //Rectangle boundsRc = Screen.GetBounds(cursorRc.Location); //Better???
            Rectangle boundsRc = Screen.PrimaryScreen.Bounds; //Better???
            Rectangle textRc = new Rectangle(
                mouseRc.X, mouseRc.Y + mouseRc.Height,
                this.Width, this.Height);
            if (textRc.Bottom > boundsRc.Bottom)
            {
                textRc.Y = mouseRc.Y - textRc.Height - 5;
            }
            if (textRc.Right > boundsRc.Right)
            {
                textRc.X -= textRc.Right - boundsRc.Right;
            }
            this.Location = textRc.Location;
        }

        private void updateLocation()
        {
            setLocation(Cursor.Position);
        }

        private void handleGlobalMouseMoved(object sender, MouseEventArgs e)
        {
            setLocation(e.Location);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Visible)
            {
                updateLocation(); //Make sure text is still inside screen bounds.
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                updateLocation();
                GlobalMouseHandler.GlobalMouseMoved += handleGlobalMouseMoved;
            }
            else
            {
                GlobalMouseHandler.GlobalMouseMoved -= handleGlobalMouseMoved;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; } //Don't activate form when shown.
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOPMOST = 0x00000008;

                CreateParams param = base.CreateParams;
                param.ExStyle |= WS_EX_TOPMOST; //Make form topmost.
                param.ExStyle |= WS_EX_NOACTIVATE; //Prevent form activated.
                return param;
            }
        }
    }

    //https://stackoverflow.com/questions/2063974/how-do-i-capture-the-mouse-move-event/29770130#29770130
    public class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;
        private Point mMousePos = new Point();
        public static event EventHandler<MouseEventArgs> GlobalMouseMoved;

        #region IMessageFilter Members
        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)
            {
                Point newMousePos = Control.MousePosition;
                if (mMousePos != newMousePos)
                {
                    mMousePos = newMousePos;
                    OnGlobalMouseMoved(newMousePos);
                }
            }
            //Always allow message to continue to the next filter control.
            return false;
        }
        #endregion

        private void OnGlobalMouseMoved(Point mousePos)
        {
            if (GlobalMouseMoved != null)
            {
                GlobalMouseMoved(this, new MouseEventArgs(
                    MouseButtons.None, 0, mousePos.X, mousePos.Y, 0));
            }
        }
    }
}
