using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    class VramViewTexture : VramView
    {
        public override void init(Vrammed vrammed)
        {
            base.init(vrammed);

            mVrammed.VramMngr.BitDepthTextureChanged += VramMngr_BitDepthTextureChanged;
            mVrammed.VramMngr.ShowAlphaTextureChanged += VramMngr_ShowAlphaTextureChanged;
            mVrammed.PaletteIndicator.LocationChanged += PaletteIndicator_LocationChanged;
            mVrammed.PaletteMarker.SizeChanged += PaletteMarker_SizeChanged;
        }

        private void VramMngr_BitDepthTextureChanged(VramMngr sender, VramMngr.BitDepth oldBitDepth, VramMngr.BitDepth newBitDepth)
        {
            setObjectSize(BmpBounds.Size);
            Invalidate();
        }

        private void VramMngr_ShowAlphaTextureChanged(VramMngr sender, bool newShowAlpha)
        {
            Invalidate();
        }

        private void PaletteIndicator_LocationChanged(VramIndicator sender, Point oldLoc, Point newLoc)
        {
            Invalidate();
        }

        private void PaletteMarker_SizeChanged(VramMarker sender, Size oldSize, Size newSize)
        {
            Invalidate();
        }

        public override VramMarker Marker
        {
            get { return mVrammed.TextureMarker; }
        }

        public override Rectangle BmpBounds
        {
            get { return mVrammed.VramMngr.BmpTextureBounds; }
        }

        protected override void paintView(Graphics gr)
        {
            MyCustomStuff.Log.timeBegin();
            mVrammed.VramMngr.drawBmpTexture(gr);
            MyCustomStuff.Log.timeEnd(mVrammed.VramMngr.BitDepthTexture + " drawBmpTexture()");
        }
    }
}
