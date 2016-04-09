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
            this.lbAvailableJumps = new RTC.LabelPassthrough();
            this.lbNbJumps = new RTC.LabelPassthrough();
            this.lbTimeFlow = new RTC.LabelPassthrough();
            this.pbTimeFlow = new System.Windows.Forms.PictureBox();
            this.pbFullMap = new System.Windows.Forms.PictureBox();
            this.pnTopPanel = new System.Windows.Forms.Panel();
            this.lbCorruptFactor = new RTC.LabelPassthrough();
            ((System.ComponentModel.ISupportInitialize)(this.pbTimeFlow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFullMap)).BeginInit();
            this.pnTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAvailableJumps
            // 
            this.lbAvailableJumps.AutoSize = true;
            this.lbAvailableJumps.Enabled = false;
            this.lbAvailableJumps.ForeColor = System.Drawing.Color.White;
            this.lbAvailableJumps.Location = new System.Drawing.Point(4, 177);
            this.lbAvailableJumps.Name = "lbAvailableJumps";
            this.lbAvailableJumps.Size = new System.Drawing.Size(85, 13);
            this.lbAvailableJumps.TabIndex = 23;
            this.lbAvailableJumps.Text = "Jumps available:";
            // 
            // lbNbJumps
            // 
            this.lbNbJumps.AutoSize = true;
            this.lbNbJumps.Enabled = false;
            this.lbNbJumps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNbJumps.ForeColor = System.Drawing.Color.White;
            this.lbNbJumps.Location = new System.Drawing.Point(88, 175);
            this.lbNbJumps.Name = "lbNbJumps";
            this.lbNbJumps.Size = new System.Drawing.Size(14, 15);
            this.lbNbJumps.TabIndex = 23;
            this.lbNbJumps.Text = "0";
            // 
            // lbTimeFlow
            // 
            this.lbTimeFlow.AutoSize = true;
            this.lbTimeFlow.Enabled = false;
            this.lbTimeFlow.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimeFlow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbTimeFlow.Location = new System.Drawing.Point(4, 7);
            this.lbTimeFlow.Margin = new System.Windows.Forms.Padding(0);
            this.lbTimeFlow.Name = "lbTimeFlow";
            this.lbTimeFlow.Size = new System.Drawing.Size(79, 18);
            this.lbTimeFlow.TabIndex = 35;
            this.lbTimeFlow.Text = "Time Flow";
            // 
            // pbTimeFlow
            // 
            this.pbTimeFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTimeFlow.BackColor = System.Drawing.Color.Black;
            this.pbTimeFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTimeFlow.Location = new System.Drawing.Point(108, 0);
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
            this.pbFullMap.Size = new System.Drawing.Size(140, 140);
            this.pbFullMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFullMap.TabIndex = 76;
            this.pbFullMap.TabStop = false;
            // 
            // pnTopPanel
            // 
            this.pnTopPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnTopPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.pnTopPanel.Controls.Add(this.lbTimeFlow);
            this.pnTopPanel.Controls.Add(this.pbTimeFlow);
            this.pnTopPanel.Location = new System.Drawing.Point(0, 0);
            this.pnTopPanel.Name = "pnTopPanel";
            this.pnTopPanel.Size = new System.Drawing.Size(141, 32);
            this.pnTopPanel.TabIndex = 77;
            this.pnTopPanel.DoubleClick += new System.EventHandler(this.pnPanel_DoubleClick);
            // 
            // lbCorruptFactor
            // 
            this.lbCorruptFactor.AutoSize = true;
            this.lbCorruptFactor.Enabled = false;
            this.lbCorruptFactor.ForeColor = System.Drawing.Color.White;
            this.lbCorruptFactor.Location = new System.Drawing.Point(4, 194);
            this.lbCorruptFactor.Name = "lbCorruptFactor";
            this.lbCorruptFactor.Size = new System.Drawing.Size(88, 13);
            this.lbCorruptFactor.TabIndex = 78;
            this.lbCorruptFactor.Text = "Corrupt factor: 1x";
            // 
            // RTC_TF_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(140, 215);
            this.Controls.Add(this.lbCorruptFactor);
            this.Controls.Add(this.lbAvailableJumps);
            this.Controls.Add(this.lbNbJumps);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LabelPassthrough lbAvailableJumps;
        private LabelPassthrough lbNbJumps;
        private LabelPassthrough lbTimeFlow;
        private System.Windows.Forms.Panel pnTopPanel;
        public System.Windows.Forms.PictureBox pbFullMap;
        public System.Windows.Forms.PictureBox pbTimeFlow;
        private LabelPassthrough lbCorruptFactor;



    }
}