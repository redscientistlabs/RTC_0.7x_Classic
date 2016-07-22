using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using BizHawk.Client.EmuHawk;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Media;

namespace RTC
{
    public partial class RTC_Form : Form // replace by : UserControl for panel
    {
        SoundPlayer simpleSound = new SoundPlayer(RTC_Core.rtcDir + "\\ASSETS\\quack.wav"); //QUACK

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


        public RTC_Form()
        {
            InitializeComponent();
        }

        private void btnRefreshZones_Click(object sender, EventArgs e)
        {
            RTC_MemoryZones.RefreshDomains();

            //RTC_Restore.SaveRestore();
        }

        public void btnManualBlast_Click(object sender, EventArgs e)
        {
            RTC_Core.Blast();

            RTC_Restore.SaveRestore();
        }

        private void btnClearCheats_Click(object sender, EventArgs e)
        {
            RTC_HellgenieEngine.ClearCheats();

            //RTC_Restore.SaveRestore();
        }

        public void btnAutoCorrupt_Click(object sender, EventArgs e)
        {
            if (btnAutoCorrupt.ForeColor == Color.Silver)
                return;

            if (!RTC_Core.AutoCorrupt)
                RTC_Core.AutoCorrupt = true;
            else
                RTC_Core.AutoCorrupt = false;

			if(RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void RTC_Form_Load(object sender, EventArgs e)
        {

            //As of 0.71+ HexEditor must be loaded to hook memory domains
            //thx bizhawk devs for increasing complexity
            GlobalWin.Tools.Load<HexEditor>();


            cbBlastRadius.SelectedIndex = 0;
            cbBlastType.SelectedIndex = 0;
            cbExternalSelectedPlugin.SelectedIndex = 0;

            gbNightmareEngine.Location = new Point(gbSelectedEngine.Location.X + gbGeneralSettings.Location.X,gbSelectedEngine.Location.Y + gbGeneralSettings.Location.Y);
            gbHellgenieEngine.Location = new Point(gbSelectedEngine.Location.X + gbGeneralSettings.Location.X, gbSelectedEngine.Location.Y + gbGeneralSettings.Location.Y);
            gbDistortionEngine.Location = new Point(gbSelectedEngine.Location.X + gbGeneralSettings.Location.X, gbSelectedEngine.Location.Y + gbGeneralSettings.Location.Y);
            gbFreezeEngine.Location = new Point(gbSelectedEngine.Location.X + gbGeneralSettings.Location.X, gbSelectedEngine.Location.Y + gbGeneralSettings.Location.Y);
            gbExternalRomPlugin.Location = new Point(gbSelectedEngine.Location.X + gbGeneralSettings.Location.X, gbSelectedEngine.Location.Y + gbGeneralSettings.Location.Y);
            gbFreezeEngineActive.Location = gbMemoryZonesManagement.Location;

            cbSelectedEngine.SelectedIndex = 0;

            foreach (string item in Directory.GetDirectories(RTC_Core.rtcDir + "\\PLUGINS"))
                cbExternalSelectedPlugin.Items.Add(item.Substring(item.LastIndexOf("\\") + 1));

            //if (RTC_Core.args.Length != 0 && RTC_Core.args[0].ToUpper().Contains("RESTORE"))
            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\Restore.dat"))
                RTC_Restore.LoadRestore();
            else
                RTC_Core.LoadDefaultRom();

            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\WindowRestore.dat"))
                RTC_Restore.LoadWindowRestore();

                RTC_Restore.IsEnabled = true;

			this.BringToFront();
			this.Focus();
            GlobalWin.MainForm.Focus();

        }

        public void track_ErrorDelay_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_ErrorDelay.Value) == Convert.ToInt32(nmIteratorSteps.Value))
                return;

            nmIteratorSteps.Value = Convert.ToDecimal(track_ErrorDelay.Value);
            RTC_Core.IteratorSteps = Convert.ToInt32(nmIteratorSteps.Value);

            RTC_Core.tfForm.RecalculateCorruptFactor();
            //RTC_Restore.SaveRestore();
        }

        public void nmIteratorSteps_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_ErrorDelay.Value) == Convert.ToInt32(nmIteratorSteps.Value))
                return;

            track_ErrorDelay.Value = Convert.ToInt32(nmIteratorSteps.Value);
            RTC_Core.IteratorSteps = Convert.ToInt32(nmIteratorSteps.Value);

            RTC_Core.tfForm.RecalculateCorruptFactor();
            //RTC_Restore.SaveRestore();
        }

        public void track_Intensity_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_Intensity.Value) == Convert.ToInt32(nmIntensity.Value))
                return;

            nmIntensity.Value = Convert.ToDecimal(track_Intensity.Value);
            RTC_Core.Intensity = Convert.ToInt32(nmIntensity.Value);

            RTC_Core.ghForm.nmIntensity.Value = track_Intensity.Value;

            RTC_Core.tfForm.RecalculateCorruptFactor();
            //RTC_Restore.SaveRestore();
        }

        public void nmIntensity_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_Intensity.Value) == Convert.ToInt32(nmIntensity.Value))
                return;

            track_Intensity.Value = Convert.ToInt32(nmIntensity.Value);
            RTC_Core.Intensity = Convert.ToInt32(nmIntensity.Value);

            RTC_Core.ghForm.nmIntensity.Value = nmIntensity.Value;

            RTC_Core.tfForm.RecalculateCorruptFactor();
            //RTC_Restore.SaveRestore();
        }

        private void cbBlastRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastRadius.SelectedItem.ToString())
            {
                case "SPREAD":
                    RTC_Core.Radius = BlastRadius.SPREAD;
                    break;

                case "CHUNK":
                    RTC_Core.Radius = BlastRadius.CHUNK;
                    break;

                case "BURST":
                    RTC_Core.Radius = BlastRadius.BURST;
                    break;
            }

			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void cbBlastType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastType.SelectedItem.ToString())
            {
                case "RANDOM":
                    RTC_NightmareEngine.Algo = BlastByteAlgo.RANDOM;
                    break;

                case "RANDOMTILT":
                    RTC_NightmareEngine.Algo = BlastByteAlgo.RANDOMTILT;
                    break;

                case "TILT":
                    RTC_NightmareEngine.Algo = BlastByteAlgo.TILT;
                    break;
            }

			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void nmMaxCheats_ValueChanged(object sender, EventArgs e)
        {
            RTC_HellgenieEngine.MaxCheats = Convert.ToInt32(nmMaxCheats.Value);

			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void lbMemoryZones_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTC_MemoryZones.SelectDomains();

            //RTC_Restore.SaveRestore();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            RTC_MemoryZones.RefreshDomains();

            for (int i = 0; i < lbMemoryZones.Items.Count; i++)
            {
                lbMemoryZones.SetSelected(i, true);
            }

            RTC_Restore.SaveRestore();
        }

        private void btnAutoSelectZones_Click(object sender, EventArgs e)
        {
            RTC_MemoryZones.AutoSelectDomains();

            RTC_Restore.SaveRestore();
        }

        private void btnGlitchHarvester_Click(object sender, EventArgs e)
        {
            RTC_Core.ghForm.Show();

            RTC_Restore.SaveRestore();
        }

        private void cbClearCheatsOnRewind_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
                cbClearFreezesOnRewind.Checked = cbClearCheatsOnRewind.Checked;

            if (cbClearCheatsOnRewind.Checked)
                RTC_Core.ClearCheatsOnRewind = true;
            else
                RTC_Core.ClearCheatsOnRewind = false;

            RTC_Restore.SaveRestore();
        }

        private void RTC_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }

        }

        private void nmDistortionDelay_ValueChanged(object sender, EventArgs e)
        {
            RTC_DistortionEngine.MaxAge = Convert.ToInt32(nmDistortionDelay.Value);
            btnResyncDistortionEngine_Click(sender, e);

			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void btnResyncDistortionEngine_Click(object sender, EventArgs e)
        {
            RTC_DistortionEngine.CurrentAge = 0;
            RTC_DistortionEngine.AllDistortionBytes.Clear();

            RTC_Restore.SaveRestore();
        }

        public void btnReboot_Click(object sender, EventArgs e)
        {
            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\Restore.dat"))
                File.Delete(RTC_Core.rtcDir + "\\SESSION\\Restore.dat");

            Process.Start("KillSwitchRestart.bat");

        }

        public void btnRebootWindow_Click(object sender, EventArgs e)
        {
            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\Restore.dat"))
                File.Delete(RTC_Core.rtcDir + "\\SESSION\\Restore.dat");

            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\WindowRestore.dat"))
                File.Delete(RTC_Core.rtcDir + "\\SESSION\\WindowRestore.dat");

            Process.Start("KillSwitchRestart.bat");

        }

        public void btnFactoryClean_Click(object sender, EventArgs e)
        {
            Process.Start("FactoryClean.bat");
        }

        public void btnAutoKillSwitch_Click(object sender, EventArgs e)
        {
            Process.Start("AutoKillSwitch.exe");
        }

        private void RTC_Form_ResizeEnd(object sender, EventArgs e)
        {
            RTC_Restore.SaveRestore();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnExternalOpenWindow_Click(object sender, EventArgs e)
        {
            RTC_ExternalRomPlugin.OpenWindow();
        }

        private void nmIntensity_ValueChanged(object sender, KeyPressEventArgs e)
        {

        }

        private void nmIntensity_ValueChanged(object sender, KeyEventArgs e)
        {

        }

        public void btnResetTimeStack_Click(object sender, EventArgs e)
        {
            RTC_TimeStack.Reset();
        }

        public void btnTimeStackJump_Click(object sender, EventArgs e)
        {
            RTC_TimeStack.Jump();
        }

        public void btnShowTimeMap_Click(object sender, EventArgs e)
        {
            RTC_Core.tfForm.Show();
        }

        public void btnEasyModeCurrent_Click(object sender, EventArgs e)
        {
            StartEasyMode(false);
        }

        public void btnEasyModeTemplate_Click(object sender, EventArgs e)
        {
            StartEasyMode(true);
        }

        public void StartEasyMode(bool useTemplate)
        {
            if (Global.Emulator is NullEmulator)
            {
                MessageBox.Show("Please load a game in BizHawk before using the RTC in Easy Mode");
                return;
            }

            if (useTemplate)
            {


                //Put Console templates HERE
                string thisSystem = Global.Game.System.ToString().ToUpper();

                switch (thisSystem)
                {

                    case "NES":     //Nintendo Entertainment system
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.Intensity = 2;
                        RTC_Core.IteratorSteps = 1;
                        break;


                    case "GB":      //Gameboy
                    case "GBC":     //Gameboy Color
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.Intensity = 1;
                        RTC_Core.IteratorSteps = 4;
                        break;

                    case "SNES":    //Super Nintendo
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.Intensity = 4;
                        RTC_Core.IteratorSteps = 8;
                        break;


                    case "GBA":     //Gameboy Advance
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.Intensity = 1;
                        RTC_Core.IteratorSteps = 1;
                        break;

                    case "N64":     //Nintendo 64
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.Intensity = 70;
                        RTC_Core.IteratorSteps = 5;
                        break;

                    case "SG":      //Sega SG-1000
                    case "GG":      //Sega GameGear
                    case "SMS":     //Sega Master System
                    case "GEN":     //Sega Genesis and CD
                    case "PCE":     //PC-Engine / Turbo Grafx
                    case "PSX":     //Sony Playstation 1
                    case "A26":     //Atari 2600
                    case "A78":     //Atari 7800
                    case "LYNX":    //Atari Lynx
                    case "INTV":    //Intellivision
                    case "PCECD":   //related to PC-Engine / Turbo Grafx
                    case "SGX":     //related to PC-Engine / Turbo Grafx
                    case "TI83":    //Ti-83 Calculator
                    case "WSWAN":   //Wonderswan
                    case "C64":     //Commodore 64
                    case "Coleco":  //Colecovision
                    case "SGB":     //Super Gameboy
                    case "SAT":     //Sega Saturn
                    case "DGB": 
                        MessageBox.Show("WARNING: No Easy-Mode template was made for this system. Please configure it manually and use the current settings.");
                        break;

                    //TODO: Add more zones like gamegear, atari, turbo graphx
                }




            }

            RTC_Core.AutoCorrupt = true;

            RTC_TimeStack.Reset();
            cbUseTimeStack.Checked = true;
            RTC_TimeFlow.Start();
            GlobalWin.MainForm.Focus();
        }

        private void cbExternalSelectedPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTC_ExternalRomPlugin.SelectedPlugin = (sender as ComboBox).SelectedItem.ToString();

			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void cbUseTimeStack_CheckedChanged(object sender, EventArgs e)
        {

            if (cbUseTimeStack.Checked)
            {
                RTC_TimeStack.Start();
            }
            else
            {
                RTC_TimeStack.Stop();
            }

            RTC_Restore.SaveRestore();

        }

        private void nmTimeStackDelay_ValueChanged(object sender, EventArgs e)
        {
			UpdateTimeStackDelay();

		}

		public void UpdateTimeStackDelay()
		{
			RTC_TimeStack.TimeStackDelay = Convert.ToInt32(nmTimeStackDelay.Value);
			if (cbUseTimeStack.Checked)
			{
				RTC_TimeStack.Stop();
				RTC_TimeStack.Start();
			}

			RTC_Restore.SaveRestore();

		}

		private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbNightmareEngine.Visible = false;
            gbHellgenieEngine.Visible = false;
            gbDistortionEngine.Visible = false;
            gbFreezeEngine.Visible = false;
            gbExternalRomPlugin.Visible = false;
            gbMemoryZonesManagement.Visible = true;
            gbFreezeEngineActive.Visible = false;
            RTC_Core.ghForm.pnIntensity.Visible = true;

            btnAutoCorrupt.ForeColor = Color.OrangeRed;

            switch (cbSelectedEngine.SelectedItem.ToString())
            {
                case "Nightmare Engine":
                    RTC_Core.SelectedEngine = CorruptionEngine.NIGHTMARE;
                    gbNightmareEngine.Visible = true;
                    break;

                case "Hellgenie Engine":
                    RTC_Core.SelectedEngine = CorruptionEngine.HELLGENIE;
                    gbHellgenieEngine.Visible = true;
                    break;

                case "Distortion Engine":
                    RTC_Core.SelectedEngine = CorruptionEngine.DISTORTION;
                    gbDistortionEngine.Visible = true;
                    break;

                case "Freeze Engine":
                    RTC_Core.SelectedEngine = CorruptionEngine.FREEZE;
                    gbFreezeEngine.Visible = true;
                    break;

                case "External ROM Plugin":
                    RTC_Core.SelectedEngine = CorruptionEngine.EXTERNALROM;
                    gbExternalRomPlugin.Visible = true;

                    RTC_Core.AutoCorrupt = false;
                    btnAutoCorrupt.ForeColor = Color.Silver;

                    RTC_Core.ghForm.pnIntensity.Visible = false;
                    break;

                default:
                    break;
            }


            if (cbSelectedEngine.SelectedItem.ToString() == "External ROM Plugin")
            {
                labelBlastRadius.Visible = false;
                labelIntensity.Visible = false;
                labelIntensityTimes.Visible = false;
                labelErrorDelay.Visible = false;
                labelErrorDelaySteps.Visible = false;
                nmIteratorSteps.Visible = false;
                nmIntensity.Visible = false;
                track_ErrorDelay.Visible = false;
                track_Intensity.Visible = false;
                cbBlastRadius.Visible = false;
                gbMemoryZonesManagement.Visible = false;
            }
            else if (cbSelectedEngine.SelectedItem.ToString() == "Freeze Engine")
            {
                labelBlastRadius.Visible = true;
                labelIntensity.Visible = true;
                labelIntensityTimes.Visible = true;
                labelErrorDelay.Visible = true;
                labelErrorDelaySteps.Visible = true;
                nmIteratorSteps.Visible = true;
                nmIntensity.Visible = true;
                track_ErrorDelay.Visible = true;
                track_Intensity.Visible = true;
                cbBlastRadius.Visible = true;
                gbMemoryZonesManagement.Visible = false;
                gbFreezeEngineActive.Visible = true;
            }
            else
            {
                labelBlastRadius.Visible = true;
                labelIntensity.Visible = true;
                labelIntensityTimes.Visible = true;
                labelErrorDelay.Visible = true;
                labelErrorDelaySteps.Visible = true;
                nmIteratorSteps.Visible = true;
                nmIntensity.Visible = true;
                track_ErrorDelay.Visible = true;
                track_Intensity.Visible = true;
                cbBlastRadius.Visible = true;
            }

            RTC_HellgenieEngine.ClearCheats();
			if(RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void nmIteratorSteps_ValueChanged(object sender, KeyPressEventArgs e)
        {

        }

        private void nmIteratorSteps_ValueChanged(object sender, KeyEventArgs e)
        {

        }

        private void nmMaxCheats_ValueChanged(object sender, KeyPressEventArgs e)
        {

        }

        private void nmMaxCheats_ValueChanged(object sender, KeyEventArgs e)
        {

        }

        private void nmTimeStackDelay_ValueChanged(object sender, KeyPressEventArgs e)
        {
			UpdateTimeStackDelay();

		}

        private void nmTimeStackDelay_ValueChanged(object sender, KeyEventArgs e)
        {
			UpdateTimeStackDelay();
		}

        private void cbClearFreezesOnRewind_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
                cbClearCheatsOnRewind.Checked = cbClearFreezesOnRewind.Checked;

            if (cbClearFreezesOnRewind.Checked)
                RTC_Core.ClearCheatsOnRewind = true;
            else
                RTC_Core.ClearCheatsOnRewind = false;

            RTC_Restore.SaveRestore();
        }

        private void nmMaxFreezes_ValueChanged(object sender, EventArgs e)
        {
            RTC_FreezeEngine.MaxFreezes = Convert.ToInt32(nmMaxFreezes.Value);

			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.RecalculateCorruptFactor();

			RTC_Restore.SaveRestore();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (!RTC_TimeFlow.Running)
            {
                RTC_TimeFlow.Start();
                RTC_Restore.SaveRestore();
            }
            else
                RTC_Core.tfForm.Focus();
        }

        private void pnLogo_MouseClick(object sender, MouseEventArgs e)
        {
            simpleSound.Play();
        }

        private void nmMaxFreezes_ValueChanged(object sender, KeyPressEventArgs e)
        {

        }

        private void nmMaxFreezes_ValueChanged(object sender, KeyEventArgs e)
        {

        }

        private void btnActiveTableDumpsReset_Click(object sender, EventArgs e)
        {
            RTC_RPC.SendToKillSwitch("FREEZE");
            RTC_FreezeEngine.ResetActiveTable();
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void btnActiveTableAddDump_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.FirstInit)
                return;

            RTC_RPC.SendToKillSwitch("FREEZE");
            RTC_FreezeEngine.AddDump();
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void btnActiveTableGenerate_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.FirstInit)
                return;

            RTC_RPC.SendToKillSwitch("FREEZE");
            RTC_FreezeEngine.GenerateActiveTable();
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void cbAutoAddDump_CheckedChanged(object sender, EventArgs e)
        {
            if (RTC_FreezeEngine.ActiveTableAutodump != null)
            {
                RTC_FreezeEngine.ActiveTableAutodump.Stop();
                RTC_FreezeEngine.ActiveTableAutodump = null;
            }

            if (cbAutoAddDump.Checked)
            {
                RTC_FreezeEngine.ActiveTableAutodump = new Timer();
                RTC_FreezeEngine.ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
                RTC_FreezeEngine.ActiveTableAutodump.Tick += new EventHandler(btnActiveTableAddDump_Click);
                RTC_FreezeEngine.ActiveTableAutodump.Start();
            }

        }

        private void nmAutoAddSec_ValueChanged(object sender, EventArgs e)
        {
            if(RTC_FreezeEngine.ActiveTableAutodump != null)
                RTC_FreezeEngine.ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
        }

        private void track_ActiveTableActivityThreshold_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_ActiveTableActivityThreshold.Value) == Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100))
                return;

            nmActiveTableActivityThreshold.Value = Convert.ToDecimal((double)track_ActiveTableActivityThreshold.Value / 100);
            RTC_FreezeEngine.ActivityThreshold = Convert.ToDouble(nmActiveTableActivityThreshold.Value);
        }

        private void nmActiveTableActivityThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_ActiveTableActivityThreshold.Value) == Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100))
                return;

            track_ActiveTableActivityThreshold.Value = Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100);
            RTC_FreezeEngine.ActivityThreshold = Convert.ToDouble(nmActiveTableActivityThreshold.Value);
        }

        private void btnActiveTableSubstractFile_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.FirstInit)
                return;

            RTC_RPC.SendToKillSwitch("FREEZE");
            RTC_FreezeEngine.SubstractActiveTable();
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void btnActiveTableSaveAs_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.ActiveTableReady)
                return;

            RTC_RPC.SendToKillSwitch("FREEZE");
            RTC_FreezeEngine.SaveActiveTable(false);
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void btnActiveTableLoad_Click(object sender, EventArgs e)
        {
            RTC_RPC.SendToKillSwitch("FREEZE");
            RTC_FreezeEngine.LoadActiveTable();
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void btnActiveTableQuickSave_Click(object sender, EventArgs e)
        {
            if (btnActiveTableQuickSave.ForeColor != Color.Silver)
            {
                RTC_RPC.SendToKillSwitch("FREEZE");
                RTC_FreezeEngine.SaveActiveTable(true);
                RTC_RPC.SendToKillSwitch("UNFREEZE");
            }
        }

        private void btnReboot_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point((sender as Button).Location.X + e.Location.X, (sender as Button).Location.Y + e.Location.Y);

            RTC_Core.ParamsButtonMenu.Show(this, locate);
        }

        private void btnEasyMode_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point((sender as Button).Location.X + e.Location.X, (sender as Button).Location.Y + e.Location.Y);

            RTC_Core.EasyButtonMenu.Show(this, locate);
        }

		private void btnManualBlast_MouseDown(object sender, MouseEventArgs e)
		{
			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.pbManualBlast.BackColor = Color.DarkRed;
		}

		private void btnManualBlast_MouseUp(object sender, MouseEventArgs e)
		{
			if (RTC_Core.tfForm != null && RTC_Core.tfForm.Visible)
				RTC_Core.tfForm.pbManualBlast.BackColor = Color.Black;
		}

		private void btnManualBlast_MouseLeave(object sender, EventArgs e)
		{
			btnManualBlast_MouseUp(null, null);
		}

		private void btnBlastEditor_Click(object sender, EventArgs e)
		{
			RTC_Core.spForm.Show();

			RTC_Restore.SaveRestore();
		}
	}

}
