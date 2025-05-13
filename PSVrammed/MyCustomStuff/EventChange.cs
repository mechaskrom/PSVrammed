using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCustomStuff
{
    //TODO: Better to use "standard" EventArgs and EventHandler than custom delegates?
    //EventArgs seems unnecessary complex with no obvious benefit though?

    //Custom event delegates.
    public delegate void EventChange<TSend>(TSend sender);
    public delegate void EventChange<TSend, TArg1>(TSend sender, TArg1 arg1);
    public delegate void EventChangeNew<TSend, TVal>(TSend sender, TVal newValue);
    public delegate void EventChangeNew<TSend, TVal1, TVal2>(TSend sender, TVal1 newValue1, TVal2 newValue2);
    public delegate void EventChangeOldNew<TSend, TVal>(TSend sender, TVal oldValue, TVal newValue);
    public delegate void EventChangeOldNewArg<TSend, TVal, TArg1>(TSend sender, TVal oldValue, TVal newValue, TArg1 arg1);
    public delegate void EventChangeOldNew<TSend, TVal1, TVal2>(TSend sender, TVal1 oldValue1, TVal2 oldValue2, TVal1 newValue1, TVal2 newValue2);

    //Standard EventArgs.
    public delegate void EventChangeArgs<TSend, T>(TSend sender, T e) where T : EventArgs;
    public delegate void EventChangeArgsOldNew<TSend, T>(TSend sender, EventArgsChangeOldNew<T> e);

    public class EventArgsChangeOldNew<T> : EventArgs
    {
        protected T mOldValue;
        protected T mNewValue;

        public EventArgsChangeOldNew(T oldValue, T newValue)
        {
            mOldValue = oldValue;
            mNewValue = newValue;
        }

        public T OldValue
        {
            get { return mOldValue; }
        }

        public T NewValue
        {
            get { return mNewValue; }
        }
    }
}
