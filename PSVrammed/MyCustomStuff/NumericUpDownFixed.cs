using System;

using System.Windows.Forms;

namespace MyCustomStuff
{
    //NumericUpDown with settable minimum digits to display and guard against some events.
    class NumericUpDownFixed : NumericUpDown
    {
        protected int mDigits = 1; //Minimum digits to display.
        protected bool mPreventValueChangedEvent = false;

        public int Digits
        {
            get { return mDigits; }
            set { mDigits = Math.Max(value, 1); }
        }

        public bool PreventValueChangedEvent
        {
            get { return mPreventValueChangedEvent; }
            set { mPreventValueChangedEvent = value; }
        }

        public decimal ValueNoEvent
        {
            get { return base.Value; }
            set
            {
                mPreventValueChangedEvent = true;
                base.Value = value;
                mPreventValueChangedEvent = false;
            }
        }

        public decimal MinimumNoEvent
        {
            get { return base.Minimum; }
            set
            {
                mPreventValueChangedEvent = true;
                base.Minimum = value;
                mPreventValueChangedEvent = false;
            }
        }

        public decimal MaximumNoEvent
        {
            get { return base.Maximum; }
            set
            {
                mPreventValueChangedEvent = true;
                base.Maximum = value;
                mPreventValueChangedEvent = false;
            }
        }

        public decimal clampValue(decimal value)
        {
            return Math.Max(this.Minimum, Math.Min(value, this.Maximum));
        }

        protected override void UpdateEditText()
        {
            string format = (this.Hexadecimal ? "X" : "D") + mDigits;
            this.Text = ((long)this.Value).ToString(format);
        }

        protected override void OnValueChanged(EventArgs e)
        {
            if (!mPreventValueChangedEvent) base.OnValueChanged(e);
        }
    }
}
