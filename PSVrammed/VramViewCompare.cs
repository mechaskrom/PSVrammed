using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSVrammed
{
    class VramViewCompare : VramView
    {
        public override void init(Vrammed vrammed)
        {
            base.init(vrammed);

            mVrammed.VramMngr.ShowAlphaCompareChanged += VramMngr_ShowAlphaCompareChanged;

            mVrammed.CompareMarker.ResultChanged += CompareMarker_ResultChanged;
        }

        private void VramMngr_ShowAlphaCompareChanged(VramMngr sender, bool newShowAlpha)
        {
            Invalidate();
        }

        private void CompareMarker_ResultChanged(VramMarkerCompare sender)
        {
            Invalidate();
        }

        public override VramMarker Marker
        {
            get { return mVrammed.CompareMarker; }
        }

        public override Rectangle BmpBounds
        {
            get { return mVrammed.VramMngr.BmpCompareBounds; }
        }

        protected override void paintView(Graphics gr)
        {
            //MyCustomStuff.Log.timeBegin();
            mVrammed.VramMngr.drawBmpCompare(gr);
            //MyCustomStuff.Log.timeEnd("new drawBmpCompare()");
        }
    }
}
