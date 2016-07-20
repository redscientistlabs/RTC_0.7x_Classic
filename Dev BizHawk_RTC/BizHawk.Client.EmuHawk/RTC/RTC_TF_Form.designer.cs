namespace RTC
{
    partial class RTC_TF_Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_TF_Form));
			this.pbTimeFlow = new System.Windows.Forms.PictureBox();
			this.pbFullMap = new System.Windows.Forms.PictureBox();
			this.pnTopPanel = new System.Windows.Forms.Panel();
			this.pbManualBlast = new System.Windows.Forms.PictureBox();
			this.lbIntensityErrorDelay = new RTC.LabelPassthrough();
			this.lbCorruptFactor = new RTC.LabelPassthrough();
			this.lbAvailableJumps = new RTC.LabelPassthrough();
			this.lbTimeFlow = new RTC.LabelPassthrough();
			this.pnLogo = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pbTimeFlow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbFullMap)).BeginInit();
			this.pnTopPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbManualBlast)).BeginInit();
			this.SuspendLayout();
			// 
			// pbTimeFlow
			// 
			this.pbTimeFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pbTimeFlow.BackColor = System.Drawing.Color.Black;
			this.pbTimeFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbTimeFlow.Location = new System.Drawing.Point(171, 0);
			this.pbTimeFlow.Name = "pbTimeFlow";
			this.pbTimeFlow.Size = new System.Drawing.Size(32, 32);
			this.pbTimeFlow.TabIndex = 63;
			this.pbTimeFlow.TabStop = false;
			// 
			// pbFullMap
			// 
			this.pbFullMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbFullMap.BackColor = System.Drawing.Color.Black;
			this.pbFullMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbFullMap.Location = new System.Drawing.Point(0, 31);
			this.pbFullMap.Name = "pbFullMap";
			this.pbFullMap.Size = new System.Drawing.Size(203, 140);
			this.pbFullMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbFullMap.TabIndex = 76;
			this.pbFullMap.TabStop = false;
			// 
			// pnTopPanel
			// 
			this.pnTopPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnTopPanel.BackColor = System.Drawing.Color.MidnightBlue;
			this.pnTopPanel.Controls.Add(this.pnLogo);
			this.pnTopPanel.Controls.Add(this.pbManualBlast);
			this.pnTopPanel.Controls.Add(this.lbTimeFlow);
			this.pnTopPanel.Controls.Add(this.pbTimeFlow);
			this.pnTopPanel.Location = new System.Drawing.Point(0, 0);
			this.pnTopPanel.Name = "pnTopPanel";
			this.pnTopPanel.Size = new System.Drawing.Size(204, 32);
			this.pnTopPanel.TabIndex = 77;
			this.pnTopPanel.DoubleClick += new System.EventHandler(this.pnPanel_DoubleClick);
			// 
			// pbManualBlast
			// 
			this.pbManualBlast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pbManualBlast.BackColor = System.Drawing.Color.Black;
			this.pbManualBlast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbManualBlast.Location = new System.Drawing.Point(164, 0);
			this.pbManualBlast.Name = "pbManualBlast";
			this.pbManualBlast.Size = new System.Drawing.Size(8, 32);
			this.pbManualBlast.TabIndex = 64;
			this.pbManualBlast.TabStop = false;
			// 
			// lbIntensityErrorDelay
			// 
			this.lbIntensityErrorDelay.AutoSize = true;
			this.lbIntensityErrorDelay.Enabled = false;
			this.lbIntensityErrorDelay.ForeColor = System.Drawing.Color.White;
			this.lbIntensityErrorDelay.Location = new System.Drawing.Point(6, 212);
			this.lbIntensityErrorDelay.Name = "lbIntensityErrorDelay";
			this.lbIntensityErrorDelay.Size = new System.Drawing.Size(132, 13);
			this.lbIntensityErrorDelay.TabIndex = 79;
			this.lbIntensityErrorDelay.Text = "Intensity/ErrorDelay isAuto";
			// 
			// lbCorruptFactor
			// 
			this.lbCorruptFactor.AutoSize = true;
			this.lbCorruptFactor.Enabled = false;
			this.lbCorruptFactor.ForeColor = System.Drawing.Color.White;
			this.lbCorruptFactor.Location = new System.Drawing.Point(6, 195);
			this.lbCorruptFactor.Name = "lbCorruptFactor";
			this.lbCorruptFactor.Size = new System.Drawing.Size(134, 13);
			this.lbCorruptFactor.TabIndex = 78;
			this.lbCorruptFactor.Text = "Corruption engine @ factor";
			// 
			// lbAvailableJumps
			// 
			this.lbAvailableJumps.AutoSize = true;
			this.lbAvailableJumps.Enabled = false;
			this.lbAvailableJumps.ForeColor = System.Drawing.Color.White;
			this.lbAvailableJumps.Location = new System.Drawing.Point(6, 178);
			this.lbAvailableJumps.Name = "lbAvailableJumps";
			this.lbAvailableJumps.Size = new System.Drawing.Size(148, 13);
			this.lbAvailableJumps.TabIndex = 23;
			this.lbAvailableJumps.Text = "TimeStack Jumps available: 0";
			// 
			// lbTimeFlow
			// 
			this.lbTimeFlow.AutoSize = true;
			this.lbTimeFlow.Enabled = false;
			this.lbTimeFlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbTimeFlow.ForeColor = System.Drawing.Color.White;
			this.lbTimeFlow.Location = new System.Drawing.Point(56, 11);
			this.lbTimeFlow.Margin = new System.Windows.Forms.Padding(0);
			this.lbTimeFlow.Name = "lbTimeFlow";
			this.lbTimeFlow.Size = new System.Drawing.Size(69, 16);
			this.lbTimeFlow.TabIndex = 35;
			this.lbTimeFlow.Text = "Time Map";
			// 
			// pnLogo
			// 
			this.pnLogo.BackColor = System.Drawing.Color.MidnightBlue;
			this.pnLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnLogo.BackgroundImage")));
			this.pnLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pnLogo.Cursor = System.Windows.Forms.Cursors.Default;
			this.pnLogo.Location = new System.Drawing.Point(6, 4);
			this.pnLogo.Name = "pnLogo";
			this.pnLogo.Size = new System.Drawing.Size(50, 24);
			this.pnLogo.TabIndex = 79;
			// 
			// RTC_TF_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(203, 233);
			this.Controls.Add(this.lbIntensityErrorDelay);
			this.Controls.Add(this.lbCorruptFactor);
			this.Controls.Add(this.lbAvailableJumps);
			this.Controls.Add(this.pbFullMap);
			this.Controls.Add(this.pnTopPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "RTC_TF_Form";
			this.Text = "Map";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_TF_Form_FormClosing);
			this.Load += new System.EventHandler(this.RTC_TF_Form_Load);
			this.ResizeEnd += new System.EventHandler(this.RTC_TF_Form_ResizeEnd);
			((System.ComponentModel.ISupportInitialize)(this.pbTimeFlow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbFullMap)).EndInit();
			this.pnTopPanel.ResumeLayout(false);
			this.pnTopPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbManualBlast)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private LabelPassthrough lbAvailableJumps;
        private LabelPassthrough lbTimeFlow;
        private System.Windows.Forms.Panel pnTopPanel;
        public System.Windows.Forms.PictureBox pbFullMap;
        public System.Windows.Forms.PictureBox pbTimeFlow;
        private LabelPassthrough lbCorruptFactor;
		private LabelPassthrough lbIntensityErrorDelay;
		public System.Windows.Forms.PictureBox pbManualBlast;
		private System.Windows.Forms.Panel pnLogo;
	}
}