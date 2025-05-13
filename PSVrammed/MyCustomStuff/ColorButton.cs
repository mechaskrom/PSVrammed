using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MyCustomStuff
{
    //Control that looks like a normal button, but with a colored box on it.
    class ColorButton : Button
    {
        private const int ColorBoxWidth = 30;
        private const int ColorBoxHeight = 15;

        //The color box.
        private readonly Bitmap mBmp;
        private Color mColor;

        //For drawing the dotted frame.
        private readonly Pen mPen;
        private readonly Rectangle mRc;

        public ColorButton()
        {
            mBmp = new Bitmap(ColorBoxWidth, ColorBoxHeight);
            mColor = Color.White;

            mPen = new Pen(Color.Black);
            mPen.Alignment = PenAlignment.Center;
            mPen.DashStyle = DashStyle.Dot;
            mRc = new Rectangle(0, 0, mBmp.Width - 1, mBmp.Height - 1);
            //mRc.Height -= Misc.IsRunningOnMono() ? 0 : 2;
            //Bottom of too big image is cropped (about 2 pixels?) in CLR, but not in Mono.
            //Oh why? Tried changing clipping bounds but it didn't help.

            this.Image = mBmp;
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.MinimumSize = new Size(ColorBoxWidth + 2, ColorBoxHeight + 2);
            this.Padding = new Padding(2);
        }

        public Color ColorBox
        {
            get { return mColor; }
            set { setColorBox(value); }
        }

        private void setColorBox(Color newColor)
        {
            Color oldColor = mColor;
            if (oldColor != newColor)
            {
                mColor = newColor;
                changeColor();
            }
        }

        private void changeColor()
        {
            using (Graphics g = Graphics.FromImage(mBmp))
            {
                g.Clear(mColor);
                g.DrawRectangle(mPen, mRc); //Dotted frame.
            }
            this.Image = mBmp;
            this.Invalidate();
        }
    }
}
