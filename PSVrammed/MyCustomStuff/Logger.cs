using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyCustomStuff
{
    //For logging stuff.
    public class LoggerBase
    {
        protected const int LinesMax = 50;

        protected readonly Queue<string> mStrings = new Queue<string>();
        protected readonly Stack<Stopwatch> mStopWatches = new Stack<Stopwatch>();

        protected bool mIsFreezed = false;

        public LoggerBase()
        {
        }

        protected virtual void textChanged()
        {
        }

        public string[] Lines
        {
            get { return mStrings.ToArray(); }
        }

        public bool IsFreezed
        {
            get { return mIsFreezed; }
            set { mIsFreezed = value; }
        }

        [Conditional("LOGSTUFF")]
        public void clear()
        {
            mStrings.Clear();
            textChanged();
        }

        [Conditional("LOGSTUFF")]
        public void add(string str)
        {
            if (!mIsFreezed)
            {
                if (mStrings.Count > LinesMax) mStrings.Dequeue();
                mStrings.Enqueue(str);
                textChanged();
            }
        }

        [Conditional("LOGSTUFF")]
        public void addGcAllocMem(string str)
        {
            add("GC allocMem " + str + " = " + GC.GetTotalMemory(false));
        }

        [Conditional("LOGSTUFF")]
        public void timeBegin()
        {
            Stopwatch sw = new Stopwatch();
            mStopWatches.Push(sw);
            sw.Start();
        }

        [Conditional("LOGSTUFF")]
        public void timeBegin(string str)
        {
            add(str + "time start");
            timeBegin();
        }

        [Conditional("LOGSTUFF")]
        public void timeEnd(string str)
        {
            Stopwatch sw = mStopWatches.Pop();
            sw.Stop();
            add(str + " time = " + sw.Elapsed);
        }
    }

    //***********************************************************************************
    //***********************************************************************************

    //Output log to a RichTextBox.
    public class RichTextBoxLogger : LoggerBase
    {
        protected readonly RichTextBox mRichTextBox;

        public RichTextBoxLogger(RichTextBox richTextBox)
        {
            mRichTextBox = richTextBox;
        }

        protected override void textChanged()
        {
            mRichTextBox.InvokeMethod(() => mRichTextBox.Lines = mStrings.ToArray());
        }
    }
}
