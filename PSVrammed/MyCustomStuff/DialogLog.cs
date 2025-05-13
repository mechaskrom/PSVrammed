using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MyCustomStuff
{
    public partial class Log : Form
    {
        protected static readonly Log mDialogLog = new Log();
        protected static readonly RichTextBoxLogger mLogger = new RichTextBoxLogger(RichTextBox);
        protected static Form mForm;

        public Log()
        {
            InitializeComponent();
        }

        public static RichTextBox RichTextBox
        {
            get { return mDialogLog.richTextBox; }
        }

        public static RichTextBoxLogger Logger
        {
            get { return mLogger; }
        }

        public static Form FormParent
        {
            get { return mForm; }
        }

        protected static void mForm_Shown(object sender, EventArgs e)
        {
            moveByForm();
            mDialogLog.Show();
            mForm.Focus();
        }

        protected static void Form_Move(object sender, EventArgs e)
        {
            moveByForm();
        }

        protected static void mForm_Resize(object sender, EventArgs e)
        {
            moveByForm();
        }

        protected static void moveByForm()
        {
            //Move log dialog.
            Point location = mForm.Location;
            location.X += mForm.Width;
            mDialogLog.Location = location;
        }

        [Conditional("LOGSTUFF")]
        public static void init(Form form)
        {
            mForm = form;
            mForm.Shown += new EventHandler(mForm_Shown);
            mForm.Move += new EventHandler(Form_Move);
            mForm.Resize += new EventHandler(mForm_Resize);
            mDialogLog.StartPosition = FormStartPosition.Manual;
        }

        [Conditional("LOGSTUFF")]
        public static void freeze()
        {
            mLogger.IsFreezed = true;
        }

        [Conditional("LOGSTUFF")]
        public static void unfreeze()
        {
            mLogger.IsFreezed = false;
        }

        [Conditional("LOGSTUFF")]
        public static void clear()
        {
            mLogger.clear();
        }

        [Conditional("LOGSTUFF")]
        public static void add(string str)
        {
            mLogger.add(str);
        }

        [Conditional("LOGSTUFF")]
        public static void addGcAllocMem(string str)
        {
            mLogger.addGcAllocMem(str);
        }

        [Conditional("LOGSTUFF")]
        public static void timeBegin()
        {
            mLogger.timeBegin();
        }

        [Conditional("LOGSTUFF")]
        public static void timeBegin(string str)
        {
            mLogger.timeBegin(str);
        }

        [Conditional("LOGSTUFF")]
        public static void timeEnd(string str)
        {
            mLogger.timeEnd(str);
        }
    }
}
