using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
    public partial class RTC_TF_Form : Form
    {
        Timer t = new Timer();
        public static Image imgLoadingTimeMap = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\loadtimemap.png");

        public RTC_TF_Form()
        {
            InitializeComponent();
        }

        private void RTC_TF_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                RTC_TimeFlow.Stop();
            }
        }

        private void RTC_TF_Form_Load(object sender, EventArgs e)
        {
            pbFullMap.Image = imgLoadingTimeMap;

            t.Interval = 40;
            t.Tick += new EventHandler(RefreshMap);
            t.Start();
            RTC_TimeFlow.Start();
        }

        public void RefreshMap(object sender, EventArgs e)
        {
            if (!RTC_TimeFlow.Running)
                return;

            int Xcamera = 0;

            if (RTC_TimeFlow.timeGame > (RTC_Core.tfForm.pbFullMap.Width / 2))
            {
                Xcamera = RTC_TimeFlow.timeGame - (RTC_Core.tfForm.pbFullMap.Width / 2);
            }

            pbFullMap.Image = RTC_TimeFlow.CropBitmap(RTC_TimeFlow.FullMap, Xcamera, 0, RTC_Core.tfForm.pbFullMap.Width, RTC_Core.tfForm.pbFullMap.Height);

            //base.OnPaint(null);
        }

        public void RefreshJumpLabel()
        {
            lbAvailableJumps.Text = "Jumps Available: " + RTC_TimeStack.ts.Count().ToString();
        }

        public void RecalculateCorruptFactor()
        {
            double factor = Convert.ToDouble(RTC_Core.Intensity) / Convert.ToDouble(RTC_Core.IteratorSteps);
            lbCorruptFactor.Text = "Corrupt factor: " + String.Format("{0:0.####}",factor) + "x";
        }

        private void pnPanel_DoubleClick(object sender, EventArgs e)
        {
            if (pnTopPanel.BackColor == Color.MidnightBlue)
            {
                pnTopPanel.BackColor = Color.Black;
                this.BackColor = Color.Black;
            }
            else
            {
                pnTopPanel.BackColor = Color.MidnightBlue;
                this.BackColor = Color.MidnightBlue;
            }

            RTC_Restore.SaveRestore();

        }

        private void RTC_TF_Form_ResizeEnd(object sender, EventArgs e)
        {
            RTC_Restore.SaveRestore();
        }

    }
}
