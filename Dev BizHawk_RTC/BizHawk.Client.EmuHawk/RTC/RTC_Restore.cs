using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;

namespace RTC
{
    public static class RTC_Restore
    {
        public static bool IsEnabled = false;
        public static RestoreFile RTC_Status = new RestoreFile();
        public static WindowRestoreFile RTC_WindowStatus = new WindowRestoreFile();

        public static void LoadRestore()
        {
            try
            {

                #region Loading the Restore.dat File

                FileStream FS;
                BinaryFormatter bformatter = new BinaryFormatter();
                FS = File.Open(RTC_Core.rtcDir + "\\SESSION\\Restore.dat", FileMode.OpenOrCreate);
                RTC_Status = (RestoreFile)bformatter.Deserialize(FS);
                FS.Close();

                #endregion

                #region Replacing Form Values

                switch (RTC_Status.SelectedEngine)
                {
                    case CorruptionEngine.NIGHTMARE:
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        break;
                    case CorruptionEngine.HELLGENIE:
                        RTC_Core.SetEngineByName("Hellgenie Engine");
                        break;
                    case CorruptionEngine.DISTORTION:
                        RTC_Core.SetEngineByName("Distortion Engine");
                        break;
                    case CorruptionEngine.NONE:
                        break;
                }

                RTC_Core.coreForm.nmIteratorSteps.Value = Convert.ToDecimal(RTC_Status.IteratorSteps);
                RTC_Core.coreForm.nmIntensity.Value = Convert.ToDecimal(RTC_Status.Intensity);

                RTC_Core.coreForm.cbClearCheatsOnRewind.Checked = RTC_Status.ClearCheatsOnRewind;

                string cbBlastRadius = RTC_Status.Radius.ToString();
                for (int i = 0; i < RTC_Core.coreForm.cbBlastRadius.Items.Count; i++)
                    if (RTC_Core.coreForm.cbBlastRadius.Items[i].ToString() == cbBlastRadius)
                        RTC_Core.coreForm.cbBlastRadius.SelectedIndex = i;

                string cbBlastType = RTC_Status.Algo.ToString();
                for (int i = 0; i < RTC_Core.coreForm.cbBlastRadius.Items.Count; i++)
                    if (RTC_Core.coreForm.cbBlastType.Items[i].ToString() == cbBlastType)
                        RTC_Core.coreForm.cbBlastType.SelectedIndex = i;




                RTC_Core.coreForm.nmMaxCheats.Value = Convert.ToDecimal(RTC_Status.MaxCheats);
                RTC_Core.coreForm.nmMaxFreezes.Value = Convert.ToDecimal(RTC_Status.MaxFreezes);
                RTC_Core.coreForm.nmDistortionDelay.Value = Convert.ToDecimal(RTC_Status.MaxAge);

                if (RTC_Status.AutoCorrupt)
                    RTC_Core.coreForm.btnAutoCorrupt.Text = "Stop Auto-Corrupt";
                else
                    RTC_Core.coreForm.btnAutoCorrupt.Text = "Start Auto-Corrupt";

                //Set Up the Memory Domains Here

                #endregion

                #region RTC_NightmareEngine

                RTC_NightmareEngine.Algo = RTC_Status.Algo;

                #endregion

                #region RTC_HellgenieEngine

                RTC_HellgenieEngine.MaxCheats = RTC_Status.MaxCheats;

                #endregion

                #region RTC_HellgenieEngine

                RTC_FreezeEngine.MaxFreezes = RTC_Status.MaxFreezes;

                #endregion

                #region RTC_DistortionEngine

                RTC_DistortionEngine.MaxAge = RTC_Status.MaxAge;
                RTC_DistortionEngine.CurrentAge = RTC_Status.CurrentAge;
                RTC_DistortionEngine.AllDistortionBytes = RTC_Status.AllDistortionBytes;

                #endregion

                #region RTC_ExternalRomPlugin
                RTC_ExternalRomPlugin.SelectedPlugin = RTC_Status.SelectedPlugin;

                for (int i = 0;i<RTC_Core.coreForm.cbExternalSelectedPlugin.Items.Count;i++)
                    if (RTC_ExternalRomPlugin.SelectedPlugin == RTC_Core.coreForm.cbExternalSelectedPlugin.Items[i].ToString())
                    {
                        RTC_Core.coreForm.cbExternalSelectedPlugin.SelectedIndex = i;
                        break;

                    }
                        

                #endregion

                #region RTC_coreForm

                switch (RTC_Status.SelectedEngine)
                {
                    case CorruptionEngine.NIGHTMARE:
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        break;
                    case CorruptionEngine.HELLGENIE:
                        RTC_Core.SetEngineByName("Hellgenie Engine");
                        break;
                    case CorruptionEngine.DISTORTION:
                        RTC_Core.SetEngineByName("Distortion Engine");
                        break;
                    case CorruptionEngine.FREEZE:
                        RTC_Core.SetEngineByName("Freeze Engine");
                        break;
                    case CorruptionEngine.EXTERNALROM:
                        RTC_Core.SetEngineByName("External ROM Plugin");
                        break;
                }

                #endregion

                #region RTC_GH_Form

                RTC_Core.ghForm.currentSelectedState = RTC_Status.currentSelectedState;

                switch (RTC_Core.ghForm.currentSelectedState)
                {
                    case "01":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate01, new EventArgs());
                        break;
                    case "02":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate02, new EventArgs());
                        break;
                    case "03":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate03, new EventArgs());
                        break;
                    case "04":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate04, new EventArgs());
                        break;
                    case "05":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate05, new EventArgs());
                        break;
                    case "06":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate06, new EventArgs());
                        break;
                    case "07":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate07, new EventArgs());
                        break;
                    case "08":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate08, new EventArgs());
                        break;
                    case "09":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate09, new EventArgs());
                        break;
                    case "10":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate10, new EventArgs());
                        break;
                    case "11":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate11, new EventArgs());
                        break;
                    case "12":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate12, new EventArgs());
                        break;
                    case "13":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate13, new EventArgs());
                        break;
                    case "14":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate14, new EventArgs());
                        break;
                    case "15":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate15, new EventArgs());
                        break;
                    case "16":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate16, new EventArgs());
                        break;
                    case "17":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate17, new EventArgs());
                        break;
                    case "18":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate18, new EventArgs());
                        break;
                    case "19":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate19, new EventArgs());
                        break;
                    case "20":
                        RTC_Core.ghForm.btnSavestate_Click(RTC_Core.ghForm.btnSavestate20, new EventArgs());
                        break;
                }


                RTC_Core.ghForm.btnParentKeys = RTC_Status.btnParentKeys;
                RTC_Core.ghForm.btnAttachedRom = RTC_Status.btnAttachedRom;

                RTC_Core.ghForm.DontLoadSelectedStash = RTC_Status.DontLoadSelectedStash;
                RTC_Core.ghForm.DontLoadSelectedStockpile = RTC_Status.DontLoadSelectedStockpile;


                if (RTC_Status.ghMode == "CORRUPT")
                    RTC_Core.ghForm.rbCorrupt.Checked = true;
                else if (RTC_Status.ghMode == "INJECT")
                    RTC_Core.ghForm.rbInject.Checked = true;
                else
                    RTC_Core.ghForm.rbOriginal.Checked = true;


                RTC_Core.ghForm.cbAutoLoadState.Checked = RTC_Status.AutoLoadState;
                RTC_Core.ghForm.cbLoadOnSelect.Checked = RTC_Status.LoadOnSelect;
                RTC_Core.ghForm.cbStashCorrupted.Checked = RTC_Status.StashCorrupted;
                RTC_Core.ghForm.cbStashInjected.Checked = RTC_Status.StashInjected;
                RTC_Core.ghForm.cbRenderAtLoad.Checked = RTC_Status.RenderAtLoad;
                RTC_Core.ghForm.cbRenderAtCorrupt.Checked = RTC_Status.RenderAtCorrupt;
                RTC_Core.ghForm.cbSavestateLoadOnClick.Checked = RTC_Status.SavestateLoadOnClick;

                if (RTC_Status.ExportFormat == "NONE")
                    RTC_Core.ghForm.rbRenderNone.Checked = true;
                else if (RTC_Status.ExportFormat == "MPEG")
                    RTC_Core.ghForm.rbRenderMPEG.Checked = true;
                else if (RTC_Status.ExportFormat == "WAV")
                    RTC_Core.ghForm.rbRenderWAV.Checked = true;
                else if (RTC_Status.ExportFormat == "AVI")
                    RTC_Core.ghForm.rbRenderAVI.Checked = true;

                RTC_Core.ghForm.lbStockpile.Items.AddRange(RTC_Status.StockpileListboxItems.ToArray());

				RTC_Core.ghForm.cbBackupHistory.Checked = RTC_Status.BackupHistory;

                if (RTC_Status.BackupHistory)
                    RTC_Core.ghForm.lbStashHistory.Items.AddRange(RTC_Status.StashHistoryListboxItems.ToArray());


                #endregion

                #region RTC_Core

                //Default Values
                RTC_Core.SelectedEngine = RTC_Status.SelectedEngine;
                RTC_Core.IteratorSteps = RTC_Status.IteratorSteps;
                RTC_Core.Radius = RTC_Status.Radius;
                RTC_Core.Intensity = RTC_Status.Intensity;
                RTC_Core.ClearCheatsOnRewind = RTC_Status.ClearCheatsOnRewind;

                //Flags
                RTC_Core.AutoCorrupt = RTC_Status.AutoCorrupt;
                RTC_Core.ExtractBlastLayer = RTC_Status.ExtractBlastLayer;

                RTC_Core.lastOpenRom = RTC_Status.lastOpenRom;

                if (RTC_Core.lastOpenRom != null)
                {
                    RTC_Core.LoadRom(RTC_Core.lastOpenRom);
                    RTC_MemoryZones.pendingSelectedDomains = RTC_Status.SelectedDomains;
                }
                else
                {
                    RTC_Core.AutoCorrupt = false;
                    RTC_Core.LoadDefaultRom();
                }

                //General Values

                //Memory object references
                RTC_Core.currentStockpile = RTC_Status.currentStockpile;

				if (RTC_Core.currentStockpile != null && RTC_Core.currentStockpile.Filename != null)
				{
					RTC_Core.ghForm.btnSaveStockpile.Enabled = true;
					RTC_Core.ghForm.btnSaveStockpile.BackColor = Color.Tomato;
				}

				#endregion

				#region RTC_TimeStack & RTC_TimeFlow

				RTC_Core.coreForm.cbUseTimeStack.Checked = RTC_Status.TimeStack;
                RTC_Core.coreForm.nmTimeStackDelay.Value = Convert.ToDecimal(RTC_Status.TimeStackDelay);

                if (RTC_Status.TimeStack)
                    RTC_TimeStack.LoadTimeStack();

                if(RTC_Status.TimeMap)
                    RTC_TimeFlow.Start();

                #endregion

                GlobalWin.MainForm.Activate();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happenned in RTC_Restore -> LoadRestore() \n\n" + ex.ToString());
            }

        }

        public static void LoadWindowRestore()
        {
            try
            {

                #region Loading the WindowRestore.dat File

                FileStream FS;
                BinaryFormatter bformatter = new BinaryFormatter();
                FS = File.Open(RTC_Core.rtcDir + "\\SESSION\\WindowRestore.dat", FileMode.OpenOrCreate);
                RTC_WindowStatus = (WindowRestoreFile)bformatter.Deserialize(FS);
                FS.Close();

                #endregion

                #region RTC_coreForm

                if (RTC_WindowStatus.RTC_coreFormLocation.X > -32000 && RTC_WindowStatus.RTC_coreFormLocation.Y > -32000)
                    RTC_Core.coreForm.Location = RTC_WindowStatus.RTC_coreFormLocation;

                RTC_Core.coreForm.WindowState = RTC_WindowStatus.RTC_coreFormWindowState;

                #endregion

                #region RTC_GH_Form

                RTC_Core.ghForm.Visible = RTC_WindowStatus.IsRTC_ghFormOpen;

                if(RTC_WindowStatus.RTC_ghFormLocation.X > -32000 && RTC_WindowStatus.RTC_ghFormLocation.Y > -32000)
                    RTC_Core.ghForm.Location = RTC_WindowStatus.RTC_ghFormLocation;

                if(RTC_WindowStatus.IsRTC_ghFormOpen)
                    RTC_Core.ghForm.WindowState = RTC_WindowStatus.RTC_ghFormWindowState;

                #endregion

                #region RTC_TF_Form

                RTC_Core.tfForm.Visible = RTC_WindowStatus.IsRTC_tfFormOpen;

                if (RTC_WindowStatus.RTC_tfFormLocation.X > -32000 && RTC_WindowStatus.RTC_tfFormLocation.Y > -32000)
                    RTC_Core.tfForm.Location = RTC_WindowStatus.RTC_tfFormLocation;

                if (RTC_WindowStatus.IsRTC_tfFormOpen)
                {
                    RTC_Core.tfForm.Show();
                    RTC_Core.tfForm.WindowState = RTC_WindowStatus.RTC_tfFormWindowState;
                }

                #endregion

                #region Bizhawk

                if (RTC_WindowStatus.MainFormLocation.X > -32000 && RTC_WindowStatus.MainFormLocation.Y > -32000)
                    GlobalWin.MainForm.Location = RTC_WindowStatus.MainFormLocation;

                if (RTC_WindowStatus.MainFormSize.Width > 320 && RTC_WindowStatus.MainFormSize.Height > 240)
                    GlobalWin.MainForm.Size = RTC_WindowStatus.MainFormSize;

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happenned in RTC_Restore -> LoadWindowRestore() \n\n" + ex.ToString());
            }

        }

        public static void SaveRestore()
        {
            if (!RTC_Restore.IsEnabled)
                return;


            try
            {
                #region RTC_Core

                //Default Values
                RTC_Status.SelectedEngine = RTC_Core.SelectedEngine;
                RTC_Status.IteratorSteps = RTC_Core.IteratorSteps;
                RTC_Status.Radius = RTC_Core.Radius;
                RTC_Status.Intensity = RTC_Core.Intensity;
                RTC_Status.ClearCheatsOnRewind = RTC_Core.ClearCheatsOnRewind;

                //Flags
                RTC_Status.AutoCorrupt = RTC_Core.AutoCorrupt;
                RTC_Status.ExtractBlastLayer = RTC_Core.ExtractBlastLayer;

                RTC_Status.lastOpenRom = RTC_Core.lastOpenRom;

                //General Values

                //Memory object references
                RTC_Status.currentStockpile = RTC_Core.currentStockpile;

                #endregion

                #region RTC_MemoryZones

                RTC_Status.SelectedDomains = RTC_MemoryZones.getSelectedDomains();

                #endregion

                #region RTC_NightmareEngine

                RTC_Status.Algo = RTC_NightmareEngine.Algo;

                #endregion

                #region RTC_HellgenieEngine

                RTC_Status.MaxCheats = RTC_HellgenieEngine.MaxCheats;

                #endregion

                #region RTC_FreezeEngine

                RTC_Status.MaxFreezes = RTC_FreezeEngine.MaxFreezes;

                #endregion

                #region RTC_DistortionEngine

                RTC_Status.MaxAge = RTC_DistortionEngine.MaxAge;
                RTC_Status.CurrentAge = RTC_DistortionEngine.CurrentAge;
                RTC_Status.AllDistortionBytes = RTC_DistortionEngine.AllDistortionBytes;

                #endregion

                #region RTC_ExternalRomPlugin

                RTC_Status.SelectedPlugin = RTC_ExternalRomPlugin.SelectedPlugin;

                #endregion

                #region RTC_coreForm

                RTC_WindowStatus.RTC_coreFormLocation = RTC_Core.coreForm.Location;
                RTC_WindowStatus.RTC_coreFormWindowState = RTC_Core.coreForm.WindowState;

                #endregion

                #region RTC_GH_Form

                RTC_Status.currentSelectedState = RTC_Core.ghForm.currentSelectedState;
                RTC_Status.btnParentKeys = RTC_Core.ghForm.btnParentKeys;
                RTC_Status.btnAttachedRom = RTC_Core.ghForm.btnAttachedRom;

                RTC_Status.DontLoadSelectedStash = RTC_Core.ghForm.DontLoadSelectedStash;
                RTC_Status.DontLoadSelectedStockpile = RTC_Core.ghForm.DontLoadSelectedStockpile;

                if(RTC_Core.ghForm.rbCorrupt.Checked)
                    RTC_Status.ghMode = "CORRUPT";
                else if(RTC_Core.ghForm.rbInject.Checked)
                    RTC_Status.ghMode = "INJECT";
                else
                    RTC_Status.ghMode = "ORIGINAL";

                RTC_Status.AutoLoadState = RTC_Core.ghForm.cbAutoLoadState.Checked;
                RTC_Status.LoadOnSelect = RTC_Core.ghForm.cbLoadOnSelect.Checked;
                RTC_Status.StashCorrupted = RTC_Core.ghForm.cbStashCorrupted.Checked;
                RTC_Status.StashInjected = RTC_Core.ghForm.cbStashInjected.Checked;
                RTC_Status.RenderAtLoad = RTC_Core.ghForm.cbRenderAtLoad.Checked;
                RTC_Status.RenderAtCorrupt = RTC_Core.ghForm.cbRenderAtCorrupt.Checked;
                RTC_Status.SavestateLoadOnClick = RTC_Core.ghForm.cbSavestateLoadOnClick.Checked;

                if (RTC_Core.ghForm.rbRenderNone.Checked)
                    RTC_Status.ExportFormat = "NONE";
                else if (RTC_Core.ghForm.rbRenderMPEG.Checked)
                    RTC_Status.ExportFormat = "MPEG";
                else if (RTC_Core.ghForm.rbRenderWAV.Checked)
                    RTC_Status.ExportFormat = "WAV";
                else if (RTC_Core.ghForm.rbRenderAVI.Checked)
                    RTC_Status.ExportFormat = "AVI";

                RTC_Status.StockpileListboxItems = new List<object>();

                foreach (object item in RTC_Core.ghForm.lbStockpile.Items)
                    RTC_Status.StockpileListboxItems.Add(item);

                RTC_Status.StashHistoryListboxItems = new List<object>();

                RTC_Status.BackupHistory = RTC_Core.ghForm.cbBackupHistory.Checked;

                if(RTC_Status.BackupHistory)
                    foreach (object item in RTC_Core.ghForm.lbStashHistory.Items)
                        RTC_Status.StashHistoryListboxItems.Add(item);



                RTC_WindowStatus.RTC_ghFormLocation = RTC_Core.ghForm.Location;
                RTC_WindowStatus.IsRTC_ghFormOpen = RTC_Core.ghForm.Visible;
                RTC_WindowStatus.RTC_tfFormWindowState = RTC_Core.ghForm.WindowState;

                #endregion

                #region RTC_TimeStack & RTC_TimeFlow

                RTC_Status.TimeStack = RTC_Core.coreForm.cbUseTimeStack.Checked;
                RTC_Status.TimeStackDelay = RTC_TimeStack.TimeStackDelay;
                RTC_Status.TimeMap = RTC_TimeFlow.Running;

                RTC_WindowStatus.RTC_tfFormLocation = RTC_Core.tfForm.Location;
                RTC_WindowStatus.IsRTC_tfFormOpen = RTC_Core.tfForm.Visible;
                RTC_WindowStatus.RTC_tfFormWindowState = RTC_Core.tfForm.WindowState;

                #endregion

                #region Bizhawk

                RTC_WindowStatus.MainFormLocation = GlobalWin.MainForm.Location;
                RTC_WindowStatus.MainFormSize = GlobalWin.MainForm.Size;

                #endregion

                #region Saving the Restore Files

                FileStream FS;
                BinaryFormatter bformatter = new BinaryFormatter();
                FS = File.Open(RTC_Core.rtcDir + "\\SESSION\\Restore.dat", FileMode.OpenOrCreate);
                bformatter.Serialize(FS, RTC_Status);
                FS.Close();

                bformatter = new BinaryFormatter();
                FS = File.Open(RTC_Core.rtcDir + "\\SESSION\\WindowRestore.dat", FileMode.OpenOrCreate);
                bformatter.Serialize(FS, RTC_WindowStatus);
                FS.Close();

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happenned in RTC_Restore -> SaveRestore() \n\n" + ex.ToString());
            }
        }
    }

    [Serializable()]
    public class RestoreFile
    {

        #region RTC_Core

        //Default Values
        public CorruptionEngine SelectedEngine;
        public int IteratorSteps;
        public BlastRadius Radius;
        public int Intensity;
        public bool ClearCheatsOnRewind;

        //Flags
        public bool AutoCorrupt;
        public bool ExtractBlastLayer;

        //Memory object references
        public Stockpile currentStockpile;
        public Stockpile lastStockpile;
        public string lastOpenRom;

        #endregion

        #region RTC_TimeStack

        public bool TimeStack;
        public int TimeStackDelay;
        public bool TimeMap;

        #endregion

        #region RTC_MemoryZones

        public List<string> SelectedDomains;

        #endregion

        #region RTC_NightmareEngine

        public BlastByteAlgo Algo;

        #endregion

        #region RTC_HellgenieEngine

        public int MaxCheats;

        #endregion

        #region RTC_MaxFreezes

        public int MaxFreezes;

        #endregion

        #region RTC_DistortionEngine

        public int MaxAge;
        public int CurrentAge;
        public BlastByteAlgo DistortionAlgo;
        public Queue<BlastUnit> AllDistortionBytes;

        #endregion

        #region RTC_ExternalRomPlugin

        public string SelectedPlugin;

        #endregion

        #region RTC_GH_Form

        public string currentSelectedState;
        public string[] btnParentKeys;
        public string[] btnAttachedRom;

        public bool DontLoadSelectedStash;
        public bool DontLoadSelectedStockpile;

        public List<object> StockpileListboxItems = new List<object>();

        public bool BackupHistory;
        public List<object> StashHistoryListboxItems = new List<object>();

        public string ghMode;
        public bool AutoLoadState;
        public bool LoadOnSelect;
        public bool StashCorrupted;
        public bool StashInjected;
        public bool RenderAtLoad;
        public bool RenderAtCorrupt;
        public bool SavestateLoadOnClick;
        public string ExportFormat;

        #endregion

    }

    [Serializable()]
    public class WindowRestoreFile
    {
        public Point RTC_coreFormLocation;
        public FormWindowState RTC_coreFormWindowState;

        public Point RTC_ghFormLocation;
        public bool IsRTC_ghFormOpen;
        public FormWindowState RTC_ghFormWindowState;

        public Point RTC_tfFormLocation;
        public bool IsRTC_tfFormOpen;
        public FormWindowState RTC_tfFormWindowState;

        public Point MainFormLocation;
        public Size MainFormSize;
    }
}
