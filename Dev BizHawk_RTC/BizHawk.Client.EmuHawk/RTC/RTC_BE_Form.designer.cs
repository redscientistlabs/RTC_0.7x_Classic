namespace RTC
{
    partial class RTC_BE_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_BE_Form));
            this.pnBottomPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lbRTC = new System.Windows.Forms.Label();
            this.lbBlastLayerStash = new System.Windows.Forms.ListBox();
            this.lbCurrentBlastLayer = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbReloadAtBlastSave = new System.Windows.Forms.CheckBox();
            this.btnClearCheats = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnBottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBottomPanel
            // 
            this.pnBottomPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.pnBottomPanel.Controls.Add(this.label3);
            this.pnBottomPanel.Controls.Add(this.lbRTC);
            this.pnBottomPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnBottomPanel.Location = new System.Drawing.Point(0, 0);
            this.pnBottomPanel.Name = "pnBottomPanel";
            this.pnBottomPanel.Size = new System.Drawing.Size(413, 32);
            this.pnBottomPanel.TabIndex = 71;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(8, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(284, 16);
            this.label3.TabIndex = 35;
            this.label3.Text = "Injection works by selecting an unit in the Blast Layer Stash";
            // 
            // lbRTC
            // 
            this.lbRTC.AutoSize = true;
            this.lbRTC.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRTC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbRTC.Location = new System.Drawing.Point(8, 0);
            this.lbRTC.Margin = new System.Windows.Forms.Padding(0);
            this.lbRTC.Name = "lbRTC";
            this.lbRTC.Size = new System.Drawing.Size(154, 16);
            this.lbRTC.TabIndex = 35;
            this.lbRTC.Text = "Tool for Blast Layer sanitizing. ";
            // 
            // lbBlastLayerStash
            // 
            this.lbBlastLayerStash.BackColor = System.Drawing.Color.Black;
            this.lbBlastLayerStash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbBlastLayerStash.FormattingEnabled = true;
            this.lbBlastLayerStash.Location = new System.Drawing.Point(11, 87);
            this.lbBlastLayerStash.Name = "lbBlastLayerStash";
            this.lbBlastLayerStash.ScrollAlwaysVisible = true;
            this.lbBlastLayerStash.Size = new System.Drawing.Size(181, 355);
            this.lbBlastLayerStash.TabIndex = 81;
            // 
            // lbCurrentBlastLayer
            // 
            this.lbCurrentBlastLayer.BackColor = System.Drawing.Color.Black;
            this.lbCurrentBlastLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbCurrentBlastLayer.FormattingEnabled = true;
            this.lbCurrentBlastLayer.Location = new System.Drawing.Point(198, 87);
            this.lbCurrentBlastLayer.Name = "lbCurrentBlastLayer";
            this.lbCurrentBlastLayer.ScrollAlwaysVisible = true;
            this.lbCurrentBlastLayer.Size = new System.Drawing.Size(204, 355);
            this.lbCurrentBlastLayer.TabIndex = 82;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 16);
            this.label2.TabIndex = 83;
            this.label2.Text = "Blast Layer Stash History:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(195, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 16);
            this.label1.TabIndex = 84;
            this.label1.Text = "Current Blast Layer:";
            // 
            // cbReloadAtBlastSave
            // 
            this.cbReloadAtBlastSave.AutoSize = true;
            this.cbReloadAtBlastSave.ForeColor = System.Drawing.Color.White;
            this.cbReloadAtBlastSave.Location = new System.Drawing.Point(11, 42);
            this.cbReloadAtBlastSave.Name = "cbReloadAtBlastSave";
            this.cbReloadAtBlastSave.Size = new System.Drawing.Size(260, 17);
            this.cbReloadAtBlastSave.TabIndex = 121;
            this.cbReloadAtBlastSave.Text = "Reload state on selected SaveState after change";
            this.cbReloadAtBlastSave.UseVisualStyleBackColor = true;
            // 
            // btnClearCheats
            // 
            this.btnClearCheats.BackColor = System.Drawing.Color.Black;
            this.btnClearCheats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearCheats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnClearCheats.Location = new System.Drawing.Point(11, 440);
            this.btnClearCheats.Name = "btnClearCheats";
            this.btnClearCheats.Size = new System.Drawing.Size(181, 23);
            this.btnClearCheats.TabIndex = 122;
            this.btnClearCheats.Text = "Clear all cheats";
            this.btnClearCheats.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Location = new System.Drawing.Point(198, 440);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(204, 23);
            this.button1.TabIndex = 123;
            this.button1.Text = "Clear all cheats";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // RTC_BE_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(413, 478);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClearCheats);
            this.Controls.Add(this.cbReloadAtBlastSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbCurrentBlastLayer);
            this.Controls.Add(this.lbBlastLayerStash);
            this.Controls.Add(this.pnBottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_BE_Form";
            this.Text = "RTC : Blast Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_BE_Form_FormClosing);
            this.pnBottomPanel.ResumeLayout(false);
            this.pnBottomPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnBottomPanel;
        private System.Windows.Forms.Label lbRTC;
        public System.Windows.Forms.ListBox lbBlastLayerStash;
        public System.Windows.Forms.ListBox lbCurrentBlastLayer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbReloadAtBlastSave;
        private System.Windows.Forms.Button btnClearCheats;
        private System.Windows.Forms.Button button1;
    }
}