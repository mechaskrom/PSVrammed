using System;
using System.Text;
using System.Windows.Forms;

namespace PSVrammed
{
    partial class DialogAbout : Form
    {
        public DialogAbout()
        {
            InitializeComponent();
            setText();
        }

        private void setText()
        {
            labelProduct.Text = Strings.ProductName;
            labelVersion.Text = Vrammed.getAssemblyVersion();

            StringBuilder str = new StringBuilder();
            str.AppendLine(Strings.AboutDescription);
            str.AppendLine();
            str.AppendLine(Strings.AboutCreator);
            str.AppendLine(Strings.AboutContact + Strings.AboutEMail);
            str.AppendLine();
            str.AppendLine(Strings.AboutDonation);
            richTextBox.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox.Text = str.ToString();
        }
    }
}
