using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyCustomStuff
{
    public static class MiscForms
    {
        public static void SetDoubleBuffered(this Control c, bool value)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                return;
            }

            System.Reflection.PropertyInfo pi = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            pi.SetValue(c, value, null);

            ////Same as DoubleBuffered property: https://stackoverflow.com/questions/37641217/doublebuffered-versus-setstyle/37641317#37641317
            //System.Reflection.MethodInfo mi = typeof(Control).GetMethod("SetStyle",
            //    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //object[] args = new object[] { ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true };
            //mi.Invoke(c, args);
        }

        public static bool isMouseOver(this Control c, Point mouseScreen)
        {
            //Mouse pointer is currently over this control?
            return c.Visible && c.RectangleToScreen(c.ClientRectangle).Contains(mouseScreen);
        }

        public static void InvokeMethod(this Control c, MethodInvoker action)
        {
            //Call action on same thread that created control.
            if (c.InvokeRequired)
            {
                c.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static void CenterForm(this Form child, Form parent)
        {
            //FormStartPosition.CenterParent doesn't work for modeless dialogs???
            child.StartPosition = FormStartPosition.Manual;
            child.Location = new Point(
                parent.Location.X + (parent.Width - child.Width) / 2,
                parent.Location.Y + (parent.Height - child.Height) / 2);
        }

        public static void ShowCentered(this Form child, Form parent)
        {
            child.CenterForm(parent);
            child.Show(parent);
        }

        public static Timer CreateOneShotTimer(Action action, int milliseconds)
        {
            Timer timer = new Timer();
            timer.Tick += (object sender, EventArgs e) =>
            {
                action();
                timer.Stop();
                timer.Dispose();
            };
            timer.Interval = milliseconds;
            return timer;
        }

        public static Timer StartOneShotTimer(Action action, int milliseconds)
        {
            Timer timer = CreateOneShotTimer(action, milliseconds);
            timer.Start();
            return timer;
        }

        //Really a drawing extension, but uses TextRenderer which is in windows forms so...
        public static void DrawText(this Graphics gr, String text, Point location, Font font, Color color)
        {
            //TextRenderer.DrawText often looks better than Graphics.DrawString, but is slower and
            //seems to not respect transformation matrix settings?
            //Graphics.DrawString is probably better(?) if you don't care about matching how windows
            //draw text in controls i.e. you're not creating a custom control with text.
            location.Offset((int)gr.Transform.OffsetX, (int)gr.Transform.OffsetY);
            TextRenderer.DrawText(gr, text, font, location, color);

            //TextRenderer.MeasureText vs Graphics.MeasureString is also different as they don't
            //report same sizes. Probably like DrawText vs DrawString that TextRenderer should be
            //used if you want to match windows text in custom controls?
        }
    }
}
