using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using MyCustomStuff;

namespace PSVrammed
{
    //Manages info in status bar.
    class StatusBarUpdater
    {
        private readonly Vrammed mVrammed;

        //Text output formatting.
        private const string FormatTexture = "Texture{0} (x{1}): XY={2:D4},{3:D3}  WH={4:D3},{5:D3}";
        private const string FormatCompare = "Compare{0} (x{1}): XY={2:D4},{3:D3}  F={4}{5}  A={6:F2}";
        private const string FormatPalette = "Palette{0} (x{1}): XY={2:D4},{3:D3}";
        private const string FormatInspect = "Inspect: {0}";

        //TODO: Remove inspect from statusbar. Not that useful info? Maybe better to copy pixel info
        //to clipboard when inspect ends? So user can paste info to text-file if interested?

        private static readonly System.Globalization.CultureInfo culture =
            System.Globalization.CultureInfo.InvariantCulture;

        public StatusBarUpdater(Vrammed vrammed)
        {
            mVrammed = vrammed;
        }

        public void init()
        {
            mVrammed.TextureView.ZoomChanged += TextureView_ZoomChanged;
            mVrammed.CompareView.ZoomChanged += CompareView_ZoomChanged;
            mVrammed.PaletteView.ZoomChanged += PaletteView_ZoomChanged;

            mVrammed.VramMngr.ShowAlphaTextureChanged += VramMngr_ShowAlphaTextureChanged;
            mVrammed.VramMngr.ShowAlphaCompareChanged += VramMngr_ShowAlphaCompareChanged;
            mVrammed.VramMngr.ShowAlphaPaletteChanged += VramMngr_ShowAlphaPaletteChanged;

            mVrammed.TextureMarker.LocationChanged += TextureMarker_LocationChanged;
            mVrammed.CompareMarker.LocationChanged += CompareMarker_LocationChanged;
            mVrammed.PaletteMarker.LocationChanged += PaletteMarker_LocationChanged;

            mVrammed.TextureMarker.SizeChanged += TextureMarker_SizeChanged;

            mVrammed.CompareMarker.ResultAlphaChanged += CompareMarker_ResultAlphaChanged;
            mVrammed.CompareMarker.FlipChanged += CompareMarker_FlipChanged;

            mVrammed.PixelInspector.PixelInfoChanged += PixelInspector_PixelInfoChanged;

            updateTexture();
            updateCompare();
            updatePalette();
            updateInspect();
        }

        private void TextureView_ZoomChanged(AutoScroller sender, float oldZoom, float newZoom)
        {
            updateTexture();
        }

        private void CompareView_ZoomChanged(AutoScroller sender, float oldZoom, float newZoom)
        {
            updateCompare();
        }

        private void PaletteView_ZoomChanged(AutoScroller sender, float oldZoom, float newZoom)
        {
            updatePalette();
        }

        private void VramMngr_ShowAlphaTextureChanged(VramMngr sender, bool newShowAlpha)
        {
            updateTexture();
        }

        private void VramMngr_ShowAlphaCompareChanged(VramMngr sender, bool newShowAlpha)
        {
            updateCompare();
        }

        private void VramMngr_ShowAlphaPaletteChanged(VramMngr sender, bool newShowAlpha)
        {
            updatePalette();
        }

        private void TextureMarker_LocationChanged(VramMarker sender, Point oldLoc, Point newLoc, bool isPreview)
        {
            updateTexture();
        }

        private void CompareMarker_LocationChanged(VramMarker sender, Point oldLoc, Point newLoc, bool isPreview)
        {
            updateCompare();
        }

        private void PaletteMarker_LocationChanged(VramMarker sender, Point oldLoc, Point newLoc, bool isPreview)
        {
            updatePalette();
        }

        private void TextureMarker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            updateTexture();
        }

        private void CompareMarker_ResultAlphaChanged(VramMarkerCompare sender, float oldAlpha, float newAlpha)
        {
            updateCompare();
        }

        private void CompareMarker_FlipChanged(VramMarkerCompare sender, bool newFlipH, bool newFlipV)
        {
            updateCompare();
        }

        private void PixelInspector_PixelInfoChanged(FormPixelInspector sender, string oldInfo, string newInfo)
        {
            updateInspect();
        }

        private void updateTexture()
        {
            setTextTexture(
                mVrammed.VramMngr.ShowAlphaTexture,
                mVrammed.TextureView.Zoom,
                mVrammed.TextureMarker.Rectangle);
        }

        private void updateCompare()
        {
            setTextCompare(
                mVrammed.VramMngr.ShowAlphaCompare,
                mVrammed.CompareView.Zoom,
                mVrammed.CompareMarker.Location,
                mVrammed.CompareMarker.FlipH,
                mVrammed.CompareMarker.FlipV,
                mVrammed.CompareMarker.ResultAlpha);
        }

        private void updatePalette()
        {
            setTextPalette(
                mVrammed.VramMngr.ShowAlphaPalette,
                mVrammed.PaletteView.Zoom,
                mVrammed.PaletteMarker.Location);
        }

        private void updateInspect()
        {
            setTextInspect(mVrammed.PixelInspector.PixelInfo);
        }

        private void setTextTexture(bool showAlpha, float zoom, Rectangle rc)
        {
            mVrammed.FormMain.StatusBarTexture.Text = String.Format(culture, FormatTexture,
                showAlpha ? "@" : string.Empty,
                zoom,
                rc.X,
                rc.Y,
                rc.Width,
                rc.Height);
        }

        private void setTextCompare(bool showAlpha, float zoom, Point loc, bool flipH, bool flipV, float resultAlpha)
        {
            mVrammed.FormMain.StatusBarCompare.Text = String.Format(culture, FormatCompare,
                showAlpha ? "@" : string.Empty,
                zoom,
                loc.X,
                loc.Y,
                flipH ? "h" : "-",
                flipV ? "v" : "-",
                resultAlpha);
        }

        private void setTextPalette(bool showAlpha, float zoom, Point loc)
        {
            mVrammed.FormMain.StatusBarPalette.Text = String.Format(culture, FormatPalette,
                showAlpha ? "@" : string.Empty,
                zoom,
                loc.X,
                loc.Y);
        }

        private void setTextInspect(string pixelInfo)
        {
            mVrammed.FormMain.StatusBarInspect.Text = String.Format(culture, FormatInspect,
                pixelInfo);
        }
    }
}
