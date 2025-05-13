using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    class FormPixelInspector : DialogMouseText
    {
        private const string NoInfo = "???";

        //Pixel info output formatting.
        private const string FormatInfo4Bit = "XY={0:D4},{1:D3}  ARGB={2:D1},{3:D2},{4:D2},{5:D2}  Index={6:D2}";
        private const string FormatInfo8Bit = "XY={0:D4},{1:D3}  ARGB={2:D1},{3:D2},{4:D2},{5:D2}  Index={6:D3}";
        private const string FormatInfo16Bit = "XY={0:D4},{1:D3}  ARGB={2:D1},{3:D2},{4:D2},{5:D2}";
        private const string FormatInfo24Bit = "XY={0:D4},{1:D3}  RGB={2:D3},{3:D3},{4:D3}";

        private readonly Vrammed mVrammed;
        private string mPixelInfo;
        private bool mIsInspecting;

        public FormPixelInspector(Vrammed vrammed)
        {
            mVrammed = vrammed;
            mPixelInfo = NoInfo;
            mIsInspecting = false;
        }

        private VramMngr VramMngr
        {
            get { return mVrammed.VramMngr; }
        }

        public string PixelInfo
        {
            get { return mPixelInfo; }
        }

        public event EventChangeOldNew<FormPixelInspector, string> PixelInfoChanged;

        private void OnPixelInfoChanged(string oldInfo, string newInfo)
        {
            if (PixelInfoChanged != null) PixelInfoChanged(this, oldInfo, newInfo);
        }

        private void setPixelInfo(string pixelInfo)
        {
            if (!String.IsNullOrEmpty(pixelInfo) && mPixelInfo != pixelInfo)
            {
                string oldInfo = mPixelInfo;
                mPixelInfo = pixelInfo;
                OnPixelInfoChanged(oldInfo, pixelInfo);
            }
        }

        public void enter(VramView vv, Point mouseScreen)
        {
            if (!mIsInspecting)
            {
                mIsInspecting = true;
                move(vv, vv.PointToClient(mouseScreen));
                vv.Cursor = Cursors.Help;
                this.Show();
                Log.add(vv.Name + " entered inspect");
            }
        }

        public void move(VramView vv, Point mouseClient)
        {
            if (mIsInspecting)
            {
                Point pixelLoc = vv.getObjectPointFromClientInside(mouseClient);
                string pixelInfo = getPixelInfo(vv, pixelLoc);
                this.MouseText = pixelInfo;
            }
        }

        public void leave(VramView vv)
        {
            if (mIsInspecting)
            {
                vv.Cursor = Cursors.Default;
                this.Hide();
                mIsInspecting = false;
                Log.add(vv.Name + " left inspect");
            }
        }

        public void save(VramView vv, Point imageLoc)
        {
            if (mIsInspecting)
            {
                quit(vv);
                string pixelInfo = getPixelInfo(vv, imageLoc);
                setPixelInfo(pixelInfo);
            }
        }

        public void quit(VramView vv)
        {
            if (mIsInspecting)
            {
                leave(vv);
                mVrammed.setTool(Tool.None);
            }
        }

        private string getPixelInfo(VramView vv, Point pixelLoc)
        {
            if (vv == mVrammed.TextureView) return getPixelInfo(pixelLoc, VramMngr.BitDepthTexture);
            if (vv == mVrammed.CompareView) return getPixelInfo(pixelLoc, VramMngr.BitDepthCompare);
            if (vv == mVrammed.PaletteView) return getPixelInfo(pixelLoc, VramMngr.BitDepthPalette);
            return NoInfo;
        }

        private string getPixelInfo(Point pixelLoc, VramMngr.BitDepth bitDepth)
        {
            pixelLoc.Y = pixelLoc.Y.Clamp(0, VramMngr.VramHeight);
            if (bitDepth == VramMngr.BitDepth.Ind4Bit)
            {
                pixelLoc.X = pixelLoc.X.Clamp(0, VramMngr.VramWidth4Bit);
                int index;
                PsColor16Bit color = mVrammed.VramMngr.getPixel4Bit(pixelLoc, out index);
                return String.Format(FormatInfo4Bit, pixelLoc.X, pixelLoc.Y,
                    color.A, color.R, color.G, color.B, index);
            }
            else if (bitDepth == VramMngr.BitDepth.Ind8Bit)
            {
                pixelLoc.X = pixelLoc.X.Clamp(0, VramMngr.VramWidth8Bit);
                int index;
                PsColor16Bit color = mVrammed.VramMngr.getPixel8Bit(pixelLoc, out index);
                return String.Format(FormatInfo8Bit, pixelLoc.X, pixelLoc.Y,
                    color.A, color.R, color.G, color.B, index);
            }
            else if (bitDepth == VramMngr.BitDepth.Rgb16Bit)
            {
                pixelLoc.X = pixelLoc.X.Clamp(0, VramMngr.VramWidth16Bit);
                PsColor16Bit color = mVrammed.VramMngr.getPixel16Bit(pixelLoc);
                return String.Format(FormatInfo16Bit, pixelLoc.X, pixelLoc.Y,
                    color.A, color.R, color.G, color.B);
            }
            else if (bitDepth == VramMngr.BitDepth.Rgb24Bit)
            {
                pixelLoc.X = pixelLoc.X.Clamp(0, VramMngr.VramWidth24Bit);
                PsColor24Bit color = mVrammed.VramMngr.getPixel24Bit(pixelLoc);
                return String.Format(FormatInfo24Bit, pixelLoc.X, pixelLoc.Y,
                    color.R, color.G, color.B);
            }
            else return NoInfo;
        }
    }
}
