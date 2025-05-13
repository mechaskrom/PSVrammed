namespace PSVrammed
{
    partial class DialogPaletteEntry
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPaletteEntry));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelRecentColors = new System.Windows.Forms.Label();
            this.labelRecentMasks = new System.Windows.Forms.Label();
            this.listViewRecentColors = new System.Windows.Forms.ListView();
            this.listViewRecentMasks = new System.Windows.Forms.ListView();
            this.colorEditor16Bit = new PSVrammed.ColorEditor16Bit();
            this.flowLayoutButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSetAll = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.flowLayoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelRecentColors, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelRecentMasks, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.listViewRecentColors, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.listViewRecentMasks, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.colorEditor16Bit, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutButtons, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(494, 271);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelRecentColors
            // 
            this.labelRecentColors.AutoSize = true;
            this.labelRecentColors.Location = new System.Drawing.Point(3, 0);
            this.labelRecentColors.Name = "labelRecentColors";
            this.labelRecentColors.Size = new System.Drawing.Size(115, 13);
            this.labelRecentColors.TabIndex = 0;
            this.labelRecentColors.Text = "Recent colors (ARGB):";
            // 
            // labelRecentMasks
            // 
            this.labelRecentMasks.AutoSize = true;
            this.labelRecentMasks.Location = new System.Drawing.Point(250, 0);
            this.labelRecentMasks.Name = "labelRecentMasks";
            this.labelRecentMasks.Size = new System.Drawing.Size(117, 13);
            this.labelRecentMasks.TabIndex = 2;
            this.labelRecentMasks.Text = "Recent masks (ARGB):";
            // 
            // listViewRecentColors
            // 
            this.listViewRecentColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewRecentColors.LabelWrap = false;
            this.listViewRecentColors.Location = new System.Drawing.Point(3, 16);
            this.listViewRecentColors.MinimumSize = new System.Drawing.Size(100, 100);
            this.listViewRecentColors.MultiSelect = false;
            this.listViewRecentColors.Name = "listViewRecentColors";
            this.listViewRecentColors.Size = new System.Drawing.Size(241, 100);
            this.listViewRecentColors.TabIndex = 1;
            this.listViewRecentColors.TileSize = new System.Drawing.Size(168, 30);
            this.listViewRecentColors.UseCompatibleStateImageBehavior = false;
            this.listViewRecentColors.View = System.Windows.Forms.View.List;
            this.listViewRecentColors.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewRecentColors_ItemSelectionChanged);
            // 
            // listViewRecentMasks
            // 
            this.listViewRecentMasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewRecentMasks.LabelWrap = false;
            this.listViewRecentMasks.Location = new System.Drawing.Point(250, 16);
            this.listViewRecentMasks.MinimumSize = new System.Drawing.Size(100, 100);
            this.listViewRecentMasks.MultiSelect = false;
            this.listViewRecentMasks.Name = "listViewRecentMasks";
            this.listViewRecentMasks.Size = new System.Drawing.Size(241, 100);
            this.listViewRecentMasks.TabIndex = 3;
            this.listViewRecentMasks.UseCompatibleStateImageBehavior = false;
            this.listViewRecentMasks.View = System.Windows.Forms.View.List;
            this.listViewRecentMasks.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewRecentMasks_ItemSelectionChanged);
            // 
            // colorEditorPalette
            // 
            this.colorEditor16Bit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.colorEditor16Bit.AutoSize = true;
            this.colorEditor16Bit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.SetColumnSpan(this.colorEditor16Bit, 2);
            this.colorEditor16Bit.CustomColors = ((System.Collections.Generic.List<System.Drawing.Color>)(resources.GetObject("colorEditorPalette.CustomColors")));
            this.colorEditor16Bit.HexValueVisible = false;
            this.colorEditor16Bit.Location = new System.Drawing.Point(27, 122);
            this.colorEditor16Bit.Mask = 0;
            this.colorEditor16Bit.MaskAlpha = false;
            this.colorEditor16Bit.MaskBlue = false;
            this.colorEditor16Bit.MaskGreen = false;
            this.colorEditor16Bit.MaskRed = false;
            this.colorEditor16Bit.Name = "colorEditorPalette";
            this.colorEditor16Bit.Size = new System.Drawing.Size(439, 68);
            this.colorEditor16Bit.TabIndex = 4;
            this.colorEditor16Bit.Value = 0;
            // 
            // flowLayoutButtons
            // 
            this.flowLayoutButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutButtons.AutoSize = true;
            this.flowLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.SetColumnSpan(this.flowLayoutButtons, 2);
            this.flowLayoutButtons.Controls.Add(this.buttonReset);
            this.flowLayoutButtons.Controls.Add(this.buttonSetAll);
            this.flowLayoutButtons.Controls.Add(this.buttonCancel);
            this.flowLayoutButtons.Controls.Add(this.buttonOk);
            this.flowLayoutButtons.Location = new System.Drawing.Point(71, 239);
            this.flowLayoutButtons.Name = "flowLayoutButtons";
            this.flowLayoutButtons.Size = new System.Drawing.Size(420, 29);
            this.flowLayoutButtons.TabIndex = 5;
            // 
            // buttonReset
            // 
            this.buttonReset.AutoSize = true;
            this.buttonReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonReset.Location = new System.Drawing.Point(3, 3);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(92, 23);
            this.buttonReset.TabIndex = 0;
            this.buttonReset.Text = "Reset to default";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSetAll
            // 
            this.buttonSetAll.AutoSize = true;
            this.buttonSetAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSetAll.Location = new System.Drawing.Point(138, 3);
            this.buttonSetAll.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
            this.buttonSetAll.Name = "buttonSetAll";
            this.buttonSetAll.Size = new System.Drawing.Size(80, 23);
            this.buttonSetAll.TabIndex = 1;
            this.buttonSetAll.Text = "Set all entries";
            this.buttonSetAll.UseVisualStyleBackColor = true;
            this.buttonSetAll.Click += new System.EventHandler(this.buttonSetAll_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(261, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(342, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // DialogPaletteEntry
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(494, 271);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPaletteEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Palette entry";
            this.Shown += new System.EventHandler(this.DialogPaletteEntry_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogPaletteEntry_FormClosed);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.flowLayoutButtons.ResumeLayout(false);
            this.flowLayoutButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonSetAll;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutButtons;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelRecentColors;
        private System.Windows.Forms.Label labelRecentMasks;
        private System.Windows.Forms.ListView listViewRecentColors;
        private System.Windows.Forms.ListView listViewRecentMasks;
        private ColorEditor16Bit colorEditor16Bit;
    }
}