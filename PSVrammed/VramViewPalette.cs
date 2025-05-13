using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    class VramViewPalette : VramView
    {
        public override void init(Vrammed vrammed)
        {
            base.init(vrammed);

            mVrammed.VramMngr.ShowAlphaPaletteChanged += VramMngr_ShowAlphaPaletteChanged;
            mVrammed.PaletteEditor.SelectedColumnChanged += PaletteEditor_SelectedColumnChanged;
        }

        private void VramMngr_ShowAlphaPaletteChanged(VramMngr sender, bool newShowAlpha)
        {
            Invalidate();
        }

        private void PaletteEditor_SelectedColumnChanged(PaletteEditor sender, int oldColumn, int newColumn)
        {
            if (!mVrammed.PaletteMarker.IsSituating)
            {
                Point centerOn = mVrammed.PaletteMarker.Location;
                centerOn.X += newColumn;
                scrollByCenter(centerOn);
            }
        }

        public override VramMarker Marker
        {
            get { return mVrammed.PaletteMarker; }
        }

        public override Rectangle BmpBounds
        {
            get { return mVrammed.VramMngr.BmpPaletteBounds; }
        }

        protected override void paintView(Graphics gr)
        {
            //MyCustomStuff.Log.timeBegin();
            mVrammed.VramMngr.drawBmpPalette(gr);
            //MyCustomStuff.Log.timeEnd("drawBmpPalette()");
        }
    }
}
