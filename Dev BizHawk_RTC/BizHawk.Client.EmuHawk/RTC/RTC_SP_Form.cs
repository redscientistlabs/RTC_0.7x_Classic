using BizHawk.Client.EmuHawk;
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
    public partial class RTC_SP_Form : Form
    {
        public RTC_SP_Form()
        {
            InitializeComponent();
        }

        private void RTC_BE_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

		private void btnLoadStockpile_Click(object sender, EventArgs e)
		{
			try
			{
				GlobalWin.Sound.StopSound();
				RTC_RPC.SendToKillSwitch("FREEZE");
				Stockpile.Load();
			}
			finally
			{
				RTC_RPC.SendToKillSwitch("UNFREEZE");
				GlobalWin.Sound.StartSound();
			}

			RTC_Restore.SaveRestore();
		}

		private void btnPreviousItem_Click(object sender, EventArgs e)
		{
			if (lbStockpile.SelectedIndex == -1)
				return;

			if (lbStockpile.SelectedIndex == 0)
				lbStockpile.SelectedIndex = lbStockpile.Items.Count - 1;
			else
				lbStockpile.SelectedIndex--;

			RTC_Restore.SaveRestore();
		}

		private void btnNextItem_Click(object sender, EventArgs e)
		{
			if (lbStockpile.SelectedIndex == -1)
				return;

			if (lbStockpile.SelectedIndex == lbStockpile.Items.Count - 1)
				lbStockpile.SelectedIndex = 0;
			else
				lbStockpile.SelectedIndex++;

			RTC_Restore.SaveRestore();
		}

		private void lbStockpile_SelectedIndexChanged(object sender, EventArgs e)
		{
			RTC_Core.ghForm.rbCorrupt.Checked = true;
			RTC_Core.currentStashkey = (lbStockpile.SelectedItem as StashKey);
			RTC_Core.ghForm.ApplyCurrentStashkey();
		}

		private void btnReloadItem_Click(object sender, EventArgs e)
		{
			lbStockpile_SelectedIndexChanged(null, null);
		}

		private void btnBlastToggle_Click(object sender, EventArgs e)
		{
			RTC_Core.ghForm.btnBlastToggle_Click(null, null);
		}
	}
}
