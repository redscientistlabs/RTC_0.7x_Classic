using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using System.Windows.Forms;
using System.Reflection;

namespace RTC
{
    public static class RTC_Hooks
    {
        //Instead of writing code inside bizhawk, hooks are placed inside of it so will be easier
        //to upgrade BizHawk when they'll release a new version.

        // Here are the keywords for searching hooks and fixes: //RTC_HIJACK

        static int CPU_STEP_Count = 0;
        public static void CPU_STEP(bool _isRewinding, bool _isFastForwarding, bool _isPaused)
        {
            if (Global.Emulator is NullEmulator)
                return;

            // Unique step hooks
            if (_isPaused)
                STEP_PAUSED();
            else if (!_isRewinding && !_isFastForwarding)
                STEP_FORWARD();
            else if (_isRewinding)
                STEP_REWIND();
            else if (_isFastForwarding)
                STEP_FASTFORWARD();

            //Any step hook for corruption
            STEP_CORRUPT(_isRewinding, _isFastForwarding, _isPaused);
        }

        static void STEP_PAUSED()
        {
            RTC_TimeFlow.PausedStep();
        }

        static void STEP_FORWARD()
        {
            RTC_TimeFlow.ForwardStep();
        }

        static void STEP_REWIND()
        {

            if (RTC_Core.ClearCheatsOnRewind)
                RTC_HellgenieEngine.ClearCheats();

            RTC_TimeFlow.RewindStep();
        }

        static void STEP_FASTFORWARD()
        {
            RTC_TimeFlow.FastForwardStep();
        }

        static void STEP_CORRUPT(bool _isRewinding, bool _isFastForwarding, bool _isPaused)
        {
            if (_isRewinding || _isFastForwarding || _isPaused)
                return;

            CPU_STEP_Count++;

            if (RTC_Core.AutoCorrupt && CPU_STEP_Count >= RTC_Core.IteratorSteps)
            {
                CPU_STEP_Count = 0;
                RTC_Core.Blast();
            }
        }

        public static void MAINFORM_CREATE(string[] args)
        {
      
                RTC_Core.args = args;

        }

        public static void MAINFORM_FORM_LOAD_END()
        {
            
            RTC_Core.HexEditorTimer = new Timer();
            RTC_Core.HexEditorTimer.Interval = 200;
            RTC_Core.HexEditorTimer.Tick += new EventHandler(RTC_Core.HexEditorTimer_Tick);
            RTC_Core.HexEditorTimer.Start();
            

            RTC_Core.Start();

            GlobalWin.MainForm.Focus();
        }

        public static void MAINFORM_RESIZEEND()
        {
            RTC_Restore.SaveRestore();
        }

        static Timer ForceCloseTimer;
        public static void MAINFORM_CLOSING()
        {
            RTC_Core.lastOpenRom = null;

            RTC_Core.AutoCorrupt = false;
            RTC_Core.coreForm.cbUseTimeStack.Checked = false;
            RTC_TimeFlow.Stop();

            RTC_Restore.SaveRestore();
            RTC_RPC.SendToKillSwitch("CLOSE");

            ForceCloseTimer = new Timer();
            ForceCloseTimer.Interval = 5000;
            ForceCloseTimer.Tick += new EventHandler(ForceCloseTimer_Tick);
            ForceCloseTimer.Start();
        }

        public static void ForceCloseTimer_Tick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static void LOAD_GAME_BEGIN()
        {
            RTC_RPC.SendToKillSwitch("FREEZE");
        }

        static string lastGameName = "";
        public static void LOAD_GAME_DONE()
        {
            if (!RTC_Core.isLoaded)
                return;

            if (RTC_Core.currentGameName == lastGameName)
                RTC_MemoryZones.RefreshAndKeepDomains();

            RTC_HellgenieEngine.ClearCheats();


            //Load Game vars into RTC_Core
            PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
            Global.Config.PathEntries[Global.Game.System, "Base"];

            RTC_Core.currentGameSystem = RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName);
            RTC_Core.currentGameName = PathManager.FilesystemSafeName(Global.Game);
            RTC_Core.lastOpenRom = GlobalWin.MainForm.CurrentlyOpenRom;
            RTC_RPC.RefreshPlugin();

            if (RTC_Core.currentGameName != lastGameName)
            {
                RTC_TimeStack.Reset();
                RTC_MemoryZones.AutoSelectDomains();
            }

            if (RTC_MemoryZones.pendingSelectedDomains != null)
            {
                RTC_MemoryZones.setSelectedDomains(RTC_MemoryZones.pendingSelectedDomains);
                RTC_MemoryZones.pendingSelectedDomains = null;
            }
            lastGameName = RTC_Core.currentGameName;

            //RTC_Restore.SaveRestore();

            RTC_RPC.SendToKillSwitch("UNFREEZE");

        }
        public static void LOAD_GAME_FAILED()
        {
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        static bool CLOSE_GAME_loop_flag = false;
        public static void CLOSE_GAME(bool loadDefault = false)
        {
            if (CLOSE_GAME_loop_flag == true)
                return;

            CLOSE_GAME_loop_flag = true;

            //RTC_Core.AutoCorrupt = false;
            RTC_MemoryZones.Clear();

            RTC_Core.lastOpenRom = null;

			if(loadDefault)
				RTC_Core.LoadDefaultRom();

            //RTC_RPC.SendToKillSwitch("UNFREEZE");

            CLOSE_GAME_loop_flag = false;
        }


        public static void RESET()
        {
        }
        public static void LOAD_SAVESTATE_BEGIN()
        {
            RTC_RPC.SendToKillSwitch("FREEZE");
        }

        public static void LOAD_SAVESTATE_END()
        {
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        public static void EMU_CRASH(string msg)
        {
            RTC_RPC.Stop();
            MessageBox.Show("SORRY EMULATOR CRASHED\n\n" + msg);
        }

        public static bool HOTKEY_CHECK(string trigger)
        {// You can go to the injected Hotkey Hijack by searching #HotkeyHijack
            switch (trigger)
            {
            default:
                return false;

            case "Manual Blast":
                RTC_Core.coreForm.btnManualBlast_Click(null, null);
                break;

            case "Start/Stop AutoCorrupt":
                RTC_Core.coreForm.btnAutoCorrupt_Click(null, null);
                break;

            case "Error Delay--":
                if (RTC_Core.coreForm.track_ErrorDelay.Value > 1)
                {
                    RTC_Core.coreForm.track_ErrorDelay.Value--;
                    RTC_Core.coreForm.track_ErrorDelay_Scroll(null, null);
                }
                break;

            case "Error Delay++":
                if (RTC_Core.coreForm.track_ErrorDelay.Value < RTC_Core.coreForm.track_ErrorDelay.Maximum)
                {
                    RTC_Core.coreForm.track_ErrorDelay.Value++;
                    RTC_Core.coreForm.track_ErrorDelay_Scroll(null, null);
                }
                break;

            case "Intensity--":
                if (RTC_Core.coreForm.track_Intensity.Value > 1)
                {
                    RTC_Core.coreForm.track_Intensity.Value--;
                    RTC_Core.coreForm.track_Intensity_Scroll(null, null);
                }
                break;

            case "Intensity++":
                if (RTC_Core.coreForm.track_Intensity.Value < RTC_Core.coreForm.track_Intensity.Maximum)
                {
                    RTC_Core.coreForm.track_Intensity.Value++;
                    RTC_Core.coreForm.track_Intensity_Scroll(null, null);
                }
                break;

            case "GH Load and Corrupt":
                RTC_Core.ghForm.cbAutoLoadState.Checked = true;
                RTC_Core.ghForm.btnCorrupt_Click(null,null);
                break;

            case "GH Corrupt w/o Load":
                bool isload = RTC_Core.ghForm.cbAutoLoadState.Checked;
                RTC_Core.ghForm.cbAutoLoadState.Checked = false;
                RTC_Core.ghForm.btnCorrupt_Click(null, null);
                RTC_Core.ghForm.cbAutoLoadState.Checked = isload;
                break;

            case "GH Load":
                RTC_Core.ghForm.btnSaveLoad.Text = "LOAD";
                RTC_Core.ghForm.btnSaveLoad_Click(null, null);
                break;

            case "GH Save":
                RTC_Core.ghForm.btnSaveLoad.Text = "SAVE";
                RTC_Core.ghForm.btnSaveLoad_Click(null, null);
                break;

            case "Reset TimeStack":
                RTC_TimeStack.Reset();
                break;

            case "TimeStack Jump":
                RTC_TimeStack.Jump();
                break;

            case "Induce KS Crash":
                RTC_RPC.Stop();
                break;

            case "Send Raw to Stash":
                RTC_Core.ghForm.btnSendRaw_Click(null, null);
                break;

            case "Blast Toggle":
                RTC_Core.ghForm.btnBlastToggle_Click(null, null);
                break;
            }
            return true;
        }
    }
}
