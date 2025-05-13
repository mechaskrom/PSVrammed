namespace PSVrammed
{
    partial class DialogMarkers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogMarkers));
            this.groupBoxCompare = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelCompare = new System.Windows.Forms.TableLayoutPanel();
            this.labelCompareColor = new System.Windows.Forms.Label();
            this.colorButtonCompare = new MyCustomStuff.ColorButton();
            this.labelCompareResult = new System.Windows.Forms.Label();
            this.labelCompareResultAlpha = new System.Windows.Forms.Label();
            this.comboBoxCompareResultAlpha = new System.Windows.Forms.ComboBox();
            this.labelCompareResultColor = new System.Windows.Forms.Label();
            this.colorButtonCompareResult = new MyCustomStuff.ColorButton();
            this.groupBoxPalette = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelPalette = new System.Windows.Forms.TableLayoutPanel();
            this.labelPaletteColor = new System.Windows.Forms.Label();
            this.colorButtonPalette = new MyCustomStuff.ColorButton();
            this.groupBoxTexture = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelTexture = new System.Windows.Forms.TableLayoutPanel();
            this.labelTextureWidth = new System.Windows.Forms.Label();
            this.labelTextureColor = new System.Windows.Forms.Label();
            this.labelTextureHeight = new System.Windows.Forms.Label();
            this.colorButtonTexture = new MyCustomStuff.ColorButton();
            this.comboBoxTextureHeight = new System.Windows.Forms.ComboBox();
            this.comboBoxTextureWidth = new System.Windows.Forms.ComboBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxCompare.SuspendLayout();
            this.tableLayoutPanelCompare.SuspendLayout();
            this.groupBoxPalette.SuspendLayout();
            this.tableLayoutPanelPalette.SuspendLayout();
            this.groupBoxTexture.SuspendLayout();
            this.tableLayoutPanelTexture.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCompare
            // 
            this.groupBoxCompare.AutoSize = true;
            this.groupBoxCompare.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxCompare.Controls.Add(this.tableLayoutPanelCompare);
            this.groupBoxCompare.Location = new System.Drawing.Point(158, 3);
            this.groupBoxCompare.Name = "groupBoxCompare";
            this.groupBoxCompare.Size = new System.Drawing.Size(247, 104);
            this.groupBoxCompare.TabIndex = 1;
            this.groupBoxCompare.TabStop = false;
            this.groupBoxCompare.Text = "Compare marker";
            // 
            // tableLayoutPanelCompare
            // 
            this.tableLayoutPanelCompare.AutoSize = true;
            this.tableLayoutPanelCompare.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelCompare.ColumnCount = 3;
            this.tableLayoutPanelCompare.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCompare.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCompare.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCompare.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCompare.Controls.Add(this.labelCompareColor, 0, 0);
            this.tableLayoutPanelCompare.Controls.Add(this.colorButtonCompare, 1, 0);
            this.tableLayoutPanelCompare.Controls.Add(this.labelCompareResult, 0, 1);
            this.tableLayoutPanelCompare.Controls.Add(this.labelCompareResultAlpha, 1, 1);
            this.tableLayoutPanelCompare.Controls.Add(this.comboBoxCompareResultAlpha, 2, 1);
            this.tableLayoutPanelCompare.Controls.Add(this.labelCompareResultColor, 1, 2);
            this.tableLayoutPanelCompare.Controls.Add(this.colorButtonCompareResult, 2, 2);
            this.tableLayoutPanelCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCompare.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelCompare.Name = "tableLayoutPanelCompare";
            this.tableLayoutPanelCompare.RowCount = 3;
            this.tableLayoutPanelCompare.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCompare.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCompare.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCompare.Size = new System.Drawing.Size(241, 85);
            this.tableLayoutPanelCompare.TabIndex = 0;
            // 
            // labelCompareColor
            // 
            this.labelCompareColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCompareColor.AutoSize = true;
            this.labelCompareColor.Location = new System.Drawing.Point(3, 8);
            this.labelCompareColor.Name = "labelCompareColor";
            this.labelCompareColor.Size = new System.Drawing.Size(31, 13);
            this.labelCompareColor.TabIndex = 0;
            this.labelCompareColor.Text = "Color";
            // 
            // colorButtonCompare
            // 
            this.colorButtonCompare.AutoSize = true;
            this.colorButtonCompare.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.colorButtonCompare.ColorBox = System.Drawing.Color.White;
            this.colorButtonCompare.Image = ((System.Drawing.Image)(resources.GetObject("colorButtonCompare.Image")));
            this.colorButtonCompare.Location = new System.Drawing.Point(46, 3);
            this.colorButtonCompare.Name = "colorButtonCompare";
            this.colorButtonCompare.Size = new System.Drawing.Size(93, 23);
            this.colorButtonCompare.TabIndex = 1;
            this.colorButtonCompare.Text = "Change...";
            this.colorButtonCompare.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.colorButtonCompare.UseVisualStyleBackColor = true;
            this.colorButtonCompare.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // labelCompareResult
            // 
            this.labelCompareResult.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCompareResult.AutoSize = true;
            this.labelCompareResult.Location = new System.Drawing.Point(3, 36);
            this.labelCompareResult.Name = "labelCompareResult";
            this.labelCompareResult.Size = new System.Drawing.Size(37, 13);
            this.labelCompareResult.TabIndex = 2;
            this.labelCompareResult.Text = "Result";
            // 
            // labelCompareResultAlpha
            // 
            this.labelCompareResultAlpha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCompareResultAlpha.AutoSize = true;
            this.labelCompareResultAlpha.Location = new System.Drawing.Point(52, 36);
            this.labelCompareResultAlpha.Margin = new System.Windows.Forms.Padding(9, 0, 3, 0);
            this.labelCompareResultAlpha.Name = "labelCompareResultAlpha";
            this.labelCompareResultAlpha.Size = new System.Drawing.Size(72, 13);
            this.labelCompareResultAlpha.TabIndex = 3;
            this.labelCompareResultAlpha.Text = "Transparency";
            // 
            // comboBoxCompareResultAlpha
            // 
            this.comboBoxCompareResultAlpha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxCompareResultAlpha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCompareResultAlpha.FormattingEnabled = true;
            this.comboBoxCompareResultAlpha.Location = new System.Drawing.Point(145, 32);
            this.comboBoxCompareResultAlpha.Name = "comboBoxCompareResultAlpha";
            this.comboBoxCompareResultAlpha.Size = new System.Drawing.Size(93, 21);
            this.comboBoxCompareResultAlpha.TabIndex = 4;
            // 
            // labelCompareResultColor
            // 
            this.labelCompareResultColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCompareResultColor.AutoSize = true;
            this.labelCompareResultColor.Location = new System.Drawing.Point(52, 64);
            this.labelCompareResultColor.Margin = new System.Windows.Forms.Padding(9, 0, 3, 0);
            this.labelCompareResultColor.Name = "labelCompareResultColor";
            this.labelCompareResultColor.Size = new System.Drawing.Size(31, 13);
            this.labelCompareResultColor.TabIndex = 5;
            this.labelCompareResultColor.Text = "Color";
            // 
            // colorButtonCompareResult
            // 
            this.colorButtonCompareResult.AutoSize = true;
            this.colorButtonCompareResult.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.colorButtonCompareResult.ColorBox = System.Drawing.Color.White;
            this.colorButtonCompareResult.Image = ((System.Drawing.Image)(resources.GetObject("colorButtonCompareResult.Image")));
            this.colorButtonCompareResult.Location = new System.Drawing.Point(145, 59);
            this.colorButtonCompareResult.Name = "colorButtonCompareResult";
            this.colorButtonCompareResult.Size = new System.Drawing.Size(93, 23);
            this.colorButtonCompareResult.TabIndex = 6;
            this.colorButtonCompareResult.Text = "Change...";
            this.colorButtonCompareResult.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.colorButtonCompareResult.UseVisualStyleBackColor = true;
            this.colorButtonCompareResult.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // groupBoxPalette
            // 
            this.groupBoxPalette.AutoSize = true;
            this.groupBoxPalette.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxPalette.Controls.Add(this.tableLayoutPanelPalette);
            this.groupBoxPalette.Location = new System.Drawing.Point(411, 3);
            this.groupBoxPalette.Name = "groupBoxPalette";
            this.groupBoxPalette.Size = new System.Drawing.Size(142, 48);
            this.groupBoxPalette.TabIndex = 2;
            this.groupBoxPalette.TabStop = false;
            this.groupBoxPalette.Text = "Palette marker";
            // 
            // tableLayoutPanelPalette
            // 
            this.tableLayoutPanelPalette.AutoSize = true;
            this.tableLayoutPanelPalette.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelPalette.ColumnCount = 2;
            this.tableLayoutPanelPalette.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPalette.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPalette.Controls.Add(this.labelPaletteColor, 0, 0);
            this.tableLayoutPanelPalette.Controls.Add(this.colorButtonPalette, 1, 0);
            this.tableLayoutPanelPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPalette.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelPalette.Name = "tableLayoutPanelPalette";
            this.tableLayoutPanelPalette.RowCount = 1;
            this.tableLayoutPanelPalette.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPalette.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelPalette.Size = new System.Drawing.Size(136, 29);
            this.tableLayoutPanelPalette.TabIndex = 0;
            // 
            // labelPaletteColor
            // 
            this.labelPaletteColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelPaletteColor.AutoSize = true;
            this.labelPaletteColor.Location = new System.Drawing.Point(3, 8);
            this.labelPaletteColor.Name = "labelPaletteColor";
            this.labelPaletteColor.Size = new System.Drawing.Size(31, 13);
            this.labelPaletteColor.TabIndex = 0;
            this.labelPaletteColor.Text = "Color";
            // 
            // colorButtonPalette
            // 
            this.colorButtonPalette.AutoSize = true;
            this.colorButtonPalette.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.colorButtonPalette.ColorBox = System.Drawing.Color.White;
            this.colorButtonPalette.Image = ((System.Drawing.Image)(resources.GetObject("colorButtonPalette.Image")));
            this.colorButtonPalette.Location = new System.Drawing.Point(40, 3);
            this.colorButtonPalette.Name = "colorButtonPalette";
            this.colorButtonPalette.Size = new System.Drawing.Size(93, 23);
            this.colorButtonPalette.TabIndex = 1;
            this.colorButtonPalette.Text = "Change...";
            this.colorButtonPalette.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.colorButtonPalette.UseVisualStyleBackColor = true;
            this.colorButtonPalette.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // groupBoxTexture
            // 
            this.groupBoxTexture.AutoSize = true;
            this.groupBoxTexture.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxTexture.Controls.Add(this.tableLayoutPanelTexture);
            this.groupBoxTexture.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTexture.Name = "groupBoxTexture";
            this.groupBoxTexture.Size = new System.Drawing.Size(149, 102);
            this.groupBoxTexture.TabIndex = 0;
            this.groupBoxTexture.TabStop = false;
            this.groupBoxTexture.Text = "Texture marker";
            // 
            // tableLayoutPanelTexture
            // 
            this.tableLayoutPanelTexture.AutoSize = true;
            this.tableLayoutPanelTexture.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTexture.ColumnCount = 2;
            this.tableLayoutPanelTexture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTexture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTexture.Controls.Add(this.labelTextureWidth, 0, 1);
            this.tableLayoutPanelTexture.Controls.Add(this.labelTextureColor, 0, 0);
            this.tableLayoutPanelTexture.Controls.Add(this.labelTextureHeight, 0, 2);
            this.tableLayoutPanelTexture.Controls.Add(this.colorButtonTexture, 1, 0);
            this.tableLayoutPanelTexture.Controls.Add(this.comboBoxTextureHeight, 1, 2);
            this.tableLayoutPanelTexture.Controls.Add(this.comboBoxTextureWidth, 1, 1);
            this.tableLayoutPanelTexture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTexture.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelTexture.Name = "tableLayoutPanelTexture";
            this.tableLayoutPanelTexture.RowCount = 3;
            this.tableLayoutPanelTexture.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTexture.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTexture.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTexture.Size = new System.Drawing.Size(143, 83);
            this.tableLayoutPanelTexture.TabIndex = 0;
            // 
            // labelTextureWidth
            // 
            this.labelTextureWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTextureWidth.AutoSize = true;
            this.labelTextureWidth.Location = new System.Drawing.Point(3, 36);
            this.labelTextureWidth.Name = "labelTextureWidth";
            this.labelTextureWidth.Size = new System.Drawing.Size(35, 13);
            this.labelTextureWidth.TabIndex = 2;
            this.labelTextureWidth.Text = "Width";
            // 
            // labelTextureColor
            // 
            this.labelTextureColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTextureColor.AutoSize = true;
            this.labelTextureColor.Location = new System.Drawing.Point(3, 8);
            this.labelTextureColor.Name = "labelTextureColor";
            this.labelTextureColor.Size = new System.Drawing.Size(31, 13);
            this.labelTextureColor.TabIndex = 0;
            this.labelTextureColor.Text = "Color";
            // 
            // labelTextureHeight
            // 
            this.labelTextureHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTextureHeight.AutoSize = true;
            this.labelTextureHeight.Location = new System.Drawing.Point(3, 63);
            this.labelTextureHeight.Name = "labelTextureHeight";
            this.labelTextureHeight.Size = new System.Drawing.Size(38, 13);
            this.labelTextureHeight.TabIndex = 4;
            this.labelTextureHeight.Text = "Height";
            // 
            // colorButtonTexture
            // 
            this.colorButtonTexture.AutoSize = true;
            this.colorButtonTexture.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.colorButtonTexture.ColorBox = System.Drawing.Color.White;
            this.colorButtonTexture.Image = ((System.Drawing.Image)(resources.GetObject("colorButtonTexture.Image")));
            this.colorButtonTexture.Location = new System.Drawing.Point(47, 3);
            this.colorButtonTexture.Name = "colorButtonTexture";
            this.colorButtonTexture.Size = new System.Drawing.Size(93, 23);
            this.colorButtonTexture.TabIndex = 1;
            this.colorButtonTexture.Text = "Change...";
            this.colorButtonTexture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.colorButtonTexture.UseVisualStyleBackColor = true;
            this.colorButtonTexture.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // comboBoxTextureHeight
            // 
            this.comboBoxTextureHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxTextureHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextureHeight.FormattingEnabled = true;
            this.comboBoxTextureHeight.Location = new System.Drawing.Point(47, 59);
            this.comboBoxTextureHeight.Name = "comboBoxTextureHeight";
            this.comboBoxTextureHeight.Size = new System.Drawing.Size(93, 21);
            this.comboBoxTextureHeight.TabIndex = 5;
            // 
            // comboBoxTextureWidth
            // 
            this.comboBoxTextureWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxTextureWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextureWidth.FormattingEnabled = true;
            this.comboBoxTextureWidth.Location = new System.Drawing.Point(47, 32);
            this.comboBoxTextureWidth.Name = "comboBoxTextureWidth";
            this.comboBoxTextureWidth.Size = new System.Drawing.Size(93, 21);
            this.comboBoxTextureWidth.TabIndex = 3;
            // 
            // buttonDefault
            // 
            this.buttonDefault.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonDefault.AutoSize = true;
            this.buttonDefault.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonDefault.Location = new System.Drawing.Point(3, 3);
            this.buttonDefault.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(105, 23);
            this.buttonDefault.TabIndex = 0;
            this.buttonDefault.Text = "Reset all to default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(141, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(222, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTop.SetColumnSpan(this.flowLayoutPanelButtons, 3);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonDefault);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonCancel);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonOk);
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(281, 140);
            this.flowLayoutPanelButtons.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(300, 29);
            this.flowLayoutPanelButtons.TabIndex = 3;
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTop.ColumnCount = 3;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.Controls.Add(this.groupBoxTexture, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.groupBoxCompare, 1, 0);
            this.tableLayoutPanelTop.Controls.Add(this.groupBoxPalette, 2, 0);
            this.tableLayoutPanelTop.Controls.Add(this.flowLayoutPanelButtons, 0, 1);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 2;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(584, 168);
            this.tableLayoutPanelTop.TabIndex = 0;
            // 
            // DialogMarkers
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(584, 168);
            this.Controls.Add(this.tableLayoutPanelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogMarkers";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Markers";
            this.Shown += new System.EventHandler(this.DialogMarkers_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogMarkers_FormClosed);
            this.groupBoxCompare.ResumeLayout(false);
            this.groupBoxCompare.PerformLayout();
            this.tableLayoutPanelCompare.ResumeLayout(false);
            this.tableLayoutPanelCompare.PerformLayout();
            this.groupBoxPalette.ResumeLayout(false);
            this.groupBoxPalette.PerformLayout();
            this.tableLayoutPanelPalette.ResumeLayout(false);
            this.tableLayoutPanelPalette.PerformLayout();
            this.groupBoxTexture.ResumeLayout(false);
            this.groupBoxTexture.PerformLayout();
            this.tableLayoutPanelTexture.ResumeLayout(false);
            this.tableLayoutPanelTexture.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            this.flowLayoutPanelButtons.PerformLayout();
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPalette;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPalette;
        private System.Windows.Forms.Label labelPaletteColor;
        private System.Windows.Forms.GroupBox groupBoxTexture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTexture;
        private System.Windows.Forms.Label labelTextureWidth;
        private System.Windows.Forms.Label labelTextureHeight;
        private System.Windows.Forms.ComboBox comboBoxTextureHeight;
        private System.Windows.Forms.ComboBox comboBoxTextureWidth;
        private System.Windows.Forms.Label labelTextureColor;
        private System.Windows.Forms.GroupBox groupBoxCompare;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCompare;
        private System.Windows.Forms.Label labelCompareColor;
        private System.Windows.Forms.Label labelCompareResult;
        private System.Windows.Forms.ComboBox comboBoxCompareResultAlpha;
        private System.Windows.Forms.Label labelCompareResultAlpha;
        private System.Windows.Forms.Label labelCompareResultColor;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
        private MyCustomStuff.ColorButton colorButtonTexture;
        private MyCustomStuff.ColorButton colorButtonCompare;
        private MyCustomStuff.ColorButton colorButtonCompareResult;
        private MyCustomStuff.ColorButton colorButtonPalette;
    }
}