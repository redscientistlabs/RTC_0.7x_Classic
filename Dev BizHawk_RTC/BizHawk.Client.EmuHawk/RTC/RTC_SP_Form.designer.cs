namespace RTC
{
    partial class RTC_SP_Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SP_Form));
			this.pnBottomPanel = new System.Windows.Forms.Panel();
			this.lbStockpile = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnLoadStockpile = new System.Windows.Forms.Button();
			this.btnPreviousItem = new System.Windows.Forms.Button();
			this.btnReloadItem = new System.Windows.Forms.Button();
			this.btnNextItem = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnBlastToggle = new System.Windows.Forms.Button();
			this.pnBottomPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnBottomPanel
			// 
			this.pnBottomPanel.BackColor = System.Drawing.Color.MidnightBlue;
			this.pnBottomPanel.Controls.Add(this.btnNextItem);
			this.pnBottomPanel.Controls.Add(this.btnReloadItem);
			this.pnBottomPanel.Controls.Add(this.btnPreviousItem);
			this.pnBottomPanel.Controls.Add(this.btnLoadStockpile);
			this.pnBottomPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnBottomPanel.Location = new System.Drawing.Point(0, 0);
			this.pnBottomPanel.Name = "pnBottomPanel";
			this.pnBottomPanel.Size = new System.Drawing.Size(302, 39);
			this.pnBottomPanel.TabIndex = 71;
			// 
			// lbStockpile
			// 
			this.lbStockpile.BackColor = System.Drawing.Color.Black;
			this.lbStockpile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.lbStockpile.FormattingEnabled = true;
			this.lbStockpile.Location = new System.Drawing.Point(11, 66);
			this.lbStockpile.Name = "lbStockpile";
			this.lbStockpile.ScrollAlwaysVisible = true;
			this.lbStockpile.Size = new System.Drawing.Size(281, 303);
			this.lbStockpile.TabIndex = 81;
			this.lbStockpile.SelectedIndexChanged += new System.EventHandler(this.lbStockpile_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(12, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 16);
			this.label2.TabIndex = 83;
			this.label2.Text = "Current Stockpile:";
			// 
			// btnLoadStockpile
			// 
			this.btnLoadStockpile.BackColor = System.Drawing.Color.Black;
			this.btnLoadStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLoadStockpile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.btnLoadStockpile.Location = new System.Drawing.Point(7, 7);
			this.btnLoadStockpile.Name = "btnLoadStockpile";
			this.btnLoadStockpile.Size = new System.Drawing.Size(94, 23);
			this.btnLoadStockpile.TabIndex = 123;
			this.btnLoadStockpile.Text = "Load Stockpile";
			this.btnLoadStockpile.UseVisualStyleBackColor = false;
			this.btnLoadStockpile.Click += new System.EventHandler(this.btnLoadStockpile_Click);
			// 
			// btnPreviousItem
			// 
			this.btnPreviousItem.BackColor = System.Drawing.Color.Black;
			this.btnPreviousItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPreviousItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.btnPreviousItem.Location = new System.Drawing.Point(104, 7);
			this.btnPreviousItem.Name = "btnPreviousItem";
			this.btnPreviousItem.Size = new System.Drawing.Size(74, 23);
			this.btnPreviousItem.TabIndex = 124;
			this.btnPreviousItem.Text = "<< Previous";
			this.btnPreviousItem.UseVisualStyleBackColor = false;
			this.btnPreviousItem.Click += new System.EventHandler(this.btnPreviousItem_Click);
			// 
			// btnReloadItem
			// 
			this.btnReloadItem.BackColor = System.Drawing.Color.Black;
			this.btnReloadItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReloadItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.btnReloadItem.Location = new System.Drawing.Point(181, 7);
			this.btnReloadItem.Name = "btnReloadItem";
			this.btnReloadItem.Size = new System.Drawing.Size(51, 23);
			this.btnReloadItem.TabIndex = 126;
			this.btnReloadItem.Text = "Reload";
			this.btnReloadItem.UseVisualStyleBackColor = false;
			this.btnReloadItem.Click += new System.EventHandler(this.btnReloadItem_Click);
			// 
			// btnNextItem
			// 
			this.btnNextItem.BackColor = System.Drawing.Color.Black;
			this.btnNextItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNextItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.btnNextItem.Location = new System.Drawing.Point(235, 7);
			this.btnNextItem.Name = "btnNextItem";
			this.btnNextItem.Size = new System.Drawing.Size(57, 23);
			this.btnNextItem.TabIndex = 127;
			this.btnNextItem.Text = "Next >>";
			this.btnNextItem.UseVisualStyleBackColor = false;
			this.btnNextItem.Click += new System.EventHandler(this.btnNextItem_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnBlastToggle);
			this.groupBox1.ForeColor = System.Drawing.Color.White;
			this.groupBox1.Location = new System.Drawing.Point(11, 375);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(281, 54);
			this.groupBox1.TabIndex = 120;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Real-Time Uncorruption Module";
			// 
			// btnBlastToggle
			// 
			this.btnBlastToggle.BackColor = System.Drawing.Color.Black;
			this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBlastToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBlastToggle.ForeColor = System.Drawing.Color.Silver;
			this.btnBlastToggle.Location = new System.Drawing.Point(15, 20);
			this.btnBlastToggle.Name = "btnBlastToggle";
			this.btnBlastToggle.Size = new System.Drawing.Size(203, 23);
			this.btnBlastToggle.TabIndex = 131;
			this.btnBlastToggle.Text = "BlastLayer Corruption Toggle OFF";
			this.btnBlastToggle.UseVisualStyleBackColor = false;
			this.btnBlastToggle.Click += new System.EventHandler(this.btnBlastToggle_Click);
			// 
			// RTC_SP_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.SteelBlue;
			this.ClientSize = new System.Drawing.Size(302, 438);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbStockpile);
			this.Controls.Add(this.pnBottomPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RTC_SP_Form";
			this.Text = "RTC : Stockpile Player";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_BE_Form_FormClosing);
			this.pnBottomPanel.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnBottomPanel;
        public System.Windows.Forms.ListBox lbStockpile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoadStockpile;
		private System.Windows.Forms.Button btnNextItem;
		private System.Windows.Forms.Button btnReloadItem;
		private System.Windows.Forms.Button btnPreviousItem;
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Button btnBlastToggle;
	}
}