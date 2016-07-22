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

			RecalculateCorruptFactor();
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
            lbAvailableJumps.Text = "TimeStack Jumps available: " + RTC_TimeStack.ts.Count().ToString();
        }

        public void RecalculateCorruptFactor()
        {
            double factor = Convert.ToDouble(RTC_Core.Intensity) / Convert.ToDouble(RTC_Core.IteratorSteps);
            lbCorruptFactor.Text = RTC_Core.SelectedEngine.ToString() + " @ " + String.Format("{0:0.####}",factor) + "x " + RTC_Core.Radius.ToString();

			string EngineParams = "";

			switch(RTC_Core.SelectedEngine)
			{
				case CorruptionEngine.NIGHTMARE:
					EngineParams = $"[{RTC.RTC_NightmareEngine.Algo.ToString()}]";
					break;
				case CorruptionEngine.HELLGENIE:
					EngineParams = $"[Cheats:{RTC.RTC_HellgenieEngine.MaxCheats.ToString()}]";
					break;
				case CorruptionEngine.DISTORTION:
					EngineParams = $"[Delay:{RTC.RTC_DistortionEngine.MaxAge.ToString()}]";
					break;
				case CorruptionEngine.FREEZE:
					EngineParams = $"[Freezes:{RTC.RTC_DistortionEngine.MaxAge.ToString()}]";
					break;
				case CorruptionEngine.EXTERNALROM:
					EngineParams = $"[Plugin:{RTC.RTC_ExternalRomPlugin.SelectedPlugin.ToString()}]";
					break;
			}

			lbIntensityErrorDelay.Text = $"{RTC_Core.Intensity.ToString()}/{RTC_Core.IteratorSteps.ToString()} " + EngineParams + (RTC_Core.AutoCorrupt ? " (AUTO)" : "");
		}

        private void pnPanel_DoubleClick(object sender, EventArgs e)
        {
            if (pnTopPanel.BackColor == Color.MidnightBlue)
            {
                pnTopPanel.BackColor = Color.Black;
                this.BackColor = Color.Black;
				pnLogo.BackColor = Color.Black;
			}
            else
            {
                pnTopPanel.BackColor = Color.MidnightBlue;
                this.BackColor = Color.MidnightBlue;
				pnLogo.BackColor = Color.MidnightBlue;
			}

            RTC_Restore.SaveRestore();

        }

        private void RTC_TF_Form_ResizeEnd(object sender, EventArgs e)
        {
            RTC_Restore.SaveRestore();
        }

	}
}
