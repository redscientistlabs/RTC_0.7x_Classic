using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace RTC
{

    public static class RTC_Core
    {
		public static string RtcVersion = "0.77";

        public static HexEditor hexeditor = null;

        public static Random RND = new Random();
        public static string[] args;

        public static Timer ResizeTimer;
        public static Size ResizeSize;

        public static Timer HexEditorTimer;

        public static ContextMenuStrip ParamsButtonMenu = new ContextMenuStrip();
        public static ContextMenuStrip EasyButtonMenu = new ContextMenuStrip();

        //Values
        public static bool isLoaded = false;

        public static CorruptionEngine SelectedEngine = CorruptionEngine.NIGHTMARE;
        public static int IteratorSteps = 1;
        public static BlastRadius Radius = BlastRadius.SPREAD;

        public static int Intensity = 1;
        public static bool ClearCheatsOnRewind = false;
        public static bool ExtractBlastLayer = false;
        public static string lastOpenRom = null;
        public static int lastLoaderRom = 0;

        private static bool _AutoCorrupt = false;
        public static bool AutoCorrupt
        {
            get
            {
                return _AutoCorrupt;
            }
            set
            {
                if (value)
                    coreForm.btnAutoCorrupt.Text = "Stop Auto-Corrupt";
                else
                    coreForm.btnAutoCorrupt.Text = "Start Auto-Corrupt";

                _AutoCorrupt = value;
            }
        }

        //General Values
        public static string bizhawkDir = Directory.GetCurrentDirectory();
        public static string rtcDir = bizhawkDir + "\\RTC";
        public static string currentGameSystem = "";
        public static string currentGameName = "";

        //Forms
        public static RTC_Form coreForm = new RTC_Form();
        public static RTC_GH_Form ghForm = new RTC_GH_Form();
        public static RTC_BE_Form beForm = new RTC_BE_Form();
        public static RTC_TF_Form tfForm = new RTC_TF_Form();

        //Object references
        public static Stockpile currentStockpile = null;
        public static StashKey currentStashkey = null;
        public static BlastLayer lastBlastLayerBackup = null;

        //Bizhawk Overrides
        public static bool Bizhawk_OSD_Enabled = false;




        public static void Start()
        {

            bool Expires = false;
            DateTime ExpiringDate = DateTime.Parse("2015-01-02");

            if (Expires && DateTime.Now > ExpiringDate)
            {
                RTC_RPC.SendToKillSwitch("CLOSE");
                MessageBox.Show("This version has expired");
                GlobalWin.MainForm.Close();
                RTC_Core.coreForm.Close();
                RTC_Core.ghForm.Close();
                RTC_Core.tfForm.Close();
                Application.Exit();
                return;
            }


            //Populating the Params Menu
            ParamsButtonMenu.Items.Add("Reset RTC Parameters", null, new EventHandler(coreForm.btnReboot_Click));
            ParamsButtonMenu.Items.Add("Reset RTC Parameters + Window Parameters", null, new EventHandler(coreForm.btnRebootWindow_Click));
            ParamsButtonMenu.Items.Add("Start AutoKillSwitch", null, new EventHandler(coreForm.btnAutoKillSwitch_Click));
            ParamsButtonMenu.Items.Add("RTC Factory Clean", null, new EventHandler(coreForm.btnFactoryClean_Click));
            
            //Populating the Easy Mode Menu
            EasyButtonMenu.Items.Add("Start with current settings", null, new EventHandler(coreForm.btnEasyModeCurrent_Click));
            EasyButtonMenu.Items.Add("Start with system template", null, new EventHandler(coreForm.btnEasyModeTemplate_Click));


            /*
            // transforming buttons into MenuButtons
            System.Windows.Forms.Button btnReboot = new RTC.MenuButton();
            btnReboot.BackColor = coreForm.btnReboot.BackColor;
            btnReboot.FlatStyle = coreForm.btnReboot.FlatStyle;
            btnReboot.ForeColor = coreForm.btnReboot.ForeColor;
            btnReboot.Image = coreForm.btnReboot.Image;
            btnReboot.Location = coreForm.btnReboot.Location;
            btnReboot.Name = coreForm.btnReboot.Name;
            btnReboot.Size = coreForm.btnReboot.Size;
            btnReboot.TabIndex = coreForm.btnReboot.TabIndex;
            btnReboot.UseVisualStyleBackColor = coreForm.btnReboot.UseVisualStyleBackColor;
            (btnReboot as RTC.MenuButton).SetMenu(ParamsButtonMenu);
            coreForm.btnReboot = btnReboot;

            System.Windows.Forms.Button btnEasyMode = new RTC.MenuButton();
            btnEasyMode.BackColor = coreForm.btnEasyMode.BackColor;
            btnEasyMode.FlatStyle = coreForm.btnEasyMode.FlatStyle;
            btnEasyMode.ForeColor = coreForm.btnEasyMode.ForeColor;
            btnEasyMode.Image = coreForm.btnEasyMode.Image;
            btnEasyMode.Location = coreForm.btnEasyMode.Location;
            btnEasyMode.Name = coreForm.btnEasyMode.Name;
            btnEasyMode.Size = coreForm.btnEasyMode.Size;
            btnEasyMode.TabIndex = coreForm.btnEasyMode.TabIndex;
            btnEasyMode.UseVisualStyleBackColor = coreForm.btnEasyMode.UseVisualStyleBackColor;
            (btnEasyMode as RTC.MenuButton).SetMenu(EasyButtonMenu);
            coreForm.btnEasyMode = btnEasyMode;

            //--------------------------------------------------------
            */

            coreForm.Show();
            RTC_RPC.Start();
            
        }

        public static long LongRandom(long max)
        {
            return LongRandom(0, max);
        }

        public static long LongRandom(long min, long max)
        {
            if(max > 2147483647)
            {
                /*
            byte[] buf = new byte[8];
            RND.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
                 * */
                return (long)RND.Next((int)min, 2147483647);

            }
            else
            return (long)RND.Next((int)min, (int)max);
        }

        public static string EmuFolderCheck(string SystemDisplayName)
        {
            string final = SystemDisplayName;

            switch (SystemDisplayName)
            {
                case "Playstation":
                    final = "PSX";
                    break;
                case "GG":
                    final = "Game Gear";
                    break;
            }

            return final;
        }

        public static void HexEditorTimer_Tick(object sender, EventArgs e)
        {
            GlobalWin.Tools.Load<HexEditor>();

            if (hexeditor != null)
            {
                hexeditor.Hide();
                HexEditorTimer.Stop();
                HexEditorTimer = null;
                isLoaded = true;
                RTC_Hooks.LOAD_GAME_DONE();
                //Start();
            }
        }

        public static BlastUnit getBlastUnit(string _domain, long _address)
        {

            BlastUnit bu = null;

            switch(SelectedEngine)
            {
                case CorruptionEngine.NIGHTMARE:
                    bu = RTC_NightmareEngine.GenerateUnit(_domain, _address);
                    break;
                case CorruptionEngine.HELLGENIE:
                    bu = RTC_HellgenieEngine.GenerateUnit(_domain, _address);
                    break;
                case CorruptionEngine.DISTORTION:
                    RTC_DistortionEngine.AddUnit(RTC_DistortionEngine.GenerateUnit(_domain, _address));
                    bu = RTC_DistortionEngine.GetUnit();
                    break;
                case CorruptionEngine.FREEZE:
                    bu = RTC_FreezeEngine.GenerateUnit(_domain, _address);
                    break;
                case CorruptionEngine.NONE:
                    return null;
            }

            return bu;
        }

        //Generates or queries a blast layer then applies it.
        public static BlastLayer Blast(BlastLayer _layer)
        {
            try
            {
                if (_layer != null)
                {
                    _layer.Apply(); //If the BlastLayer was provided, there's no need to generate a new one.

                    return _layer;
                }
                else if (RTC_Core.SelectedEngine == CorruptionEngine.EXTERNALROM)
                {
                    BlastLayer romLayer = RTC_ExternalRomPlugin.GetLayer();
                    if (romLayer == null)
                    {
                        return null;
                    }
                    else
                    {
                        romLayer.Apply();
                        return romLayer;
                    }
                }
                else
                {
                    BlastLayer bl = new BlastLayer();

                    if (RTC_MemoryZones.SelectedDomains.Count == 0)
                        return null;

                    string Domain;
                    long MaxAdress;
                    long RandomAdress = 0;
                    BlastUnit bu;

                    if (RTC_Core.SelectedEngine == CorruptionEngine.DISTORTION && RTC_DistortionEngine.CurrentAge < RTC_DistortionEngine.MaxAge)
                        RTC_DistortionEngine.CurrentAge++;

                    switch (Radius)
                    {
                        case BlastRadius.SPREAD:

                            for (int i = 0; i < Intensity; i++) //Randomly spreads all corruption bytes to all selected zones
                            {
                                Domain = RTC_MemoryZones.SelectedDomains[RND.Next(RTC_MemoryZones.SelectedDomains.Count)];
                                MaxAdress = RTC_MemoryZones.getDomain(Domain).Size;
                                RandomAdress = LongRandom(MaxAdress);

                                bu = getBlastUnit(Domain, RandomAdress);
                                if (bu != null)
                                    bl.Layer.Add(bu);
                            }

                            break;

                        case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected zone

                            Domain = RTC_MemoryZones.SelectedDomains[RND.Next(RTC_MemoryZones.SelectedDomains.Count)];
                            MaxAdress = RTC_MemoryZones.getDomain(Domain).Size;

                            for (int i = 0; i < Intensity; i++)
                            {
                                RandomAdress = LongRandom(MaxAdress);

                                bu = getBlastUnit(Domain, RandomAdress);
                                if(bu != null)
                                    bl.Layer.Add(bu);
                            }

                            break;

                        case BlastRadius.BURST:

                            for (int j = 0; j < 10; j++) // 10 shots of 10% chunk
                            {
                                Domain = RTC_MemoryZones.SelectedDomains[RND.Next(RTC_MemoryZones.SelectedDomains.Count)];
                                MaxAdress = RTC_MemoryZones.getDomain(Domain).Size;

                                for (int i = 0; i < (int)((double)Intensity / 10); i++)
                                {
                                    RandomAdress = LongRandom(MaxAdress);

                                    bu = getBlastUnit(Domain, RandomAdress);
                                    if (bu != null)
                                        bl.Layer.Add(bu);
                                }

                            }

                            break;

                        case BlastRadius.NONE:
                            return null;
                    }

                    bl.Apply();

                    RTC_HellgenieEngine.RemoveExcessCheats();

                    if (bl.Layer.Count == 0)
                        return null;
                    else
                        return bl;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Core. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                ex.ToString());
                return null;
            }
        }

        public static BlastLayer Blast()
        {
            return Blast(null);
        }

        public static string GetRandomKey()
        {
            string Key = RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString();
            return Key;
        }

        public static void SetResizeTimer()
        {
            ResizeTimer = new Timer();
            ResizeTimer.Interval = 500;
            ResizeTimer.Tick +=new EventHandler(ResizeTimer_Tick);
        }

        public static void ResizeTimer_Tick(object sender, EventArgs e)
        {
            ResizeTimer.Stop();
            GlobalWin.MainForm.Size = ResizeSize;
        }

        public static void LoadDefaultRom()
        {
            int newNumber = lastLoaderRom;

            while (newNumber == lastLoaderRom)
            {
                newNumber = RTC_Core.RND.Next(1, 7);

                if (newNumber != lastLoaderRom)
                {
                    if(File.Exists(RTC_Core.rtcDir + "\\ASSETS\\" + "overridedefault.nes"))
                        RTC_Core.LoadRom(RTC_Core.rtcDir + "\\ASSETS\\" + "overridedefault.nes");
                    else
                        RTC_Core.LoadRom(RTC_Core.rtcDir + "\\ASSETS\\" + newNumber.ToString() + "default.nes");
                    
                    lastLoaderRom = newNumber;
                    break;
                }
            }
        }

        public static void LoadRom(string RomFile)
        {
            List<string> MemoryBanks = null;

            if (isLoaded)
            {
                MemoryBanks = new List<string>(); //Saves current targetted zones
                foreach (object oneItem in RTC_MemoryZones.SelectedDomains)
                    MemoryBanks.Add(oneItem.ToString());
            }

            GlobalWin.Sound.StopSound();
            GlobalWin.DisplayManager.NeedsToPaint = false;

            var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();


            if (RomFile != null)
            {
                args.OpenAdvanced = new OpenAdvanced_OpenRom { Path = RomFile };
                GlobalWin.MainForm.LoadRom(RomFile, args); //reload rom (evade rom corruption)
            }
            else
            {
                args.OpenAdvanced = new OpenAdvanced_OpenRom { Path = GlobalWin.MainForm.CurrentlyOpenRom };
                GlobalWin.MainForm.LoadRom(GlobalWin.MainForm.CurrentlyOpenRom, args); //reload rom (evade rom corruption)
            }

            GlobalWin.DisplayManager.NeedsToPaint = true;
            GlobalWin.Sound.StartSound();

            if (isLoaded)
            {
                RTC_MemoryZones.RefreshDomains(); //refresh and reload zones
                int nbZones = coreForm.lbMemoryZones.Items.Count;

                for (int i = 0; i < nbZones; i++)
                    foreach (string SelectedItem in MemoryBanks)
                        if (coreForm.lbMemoryZones.Items[i].ToString() == SelectedItem)
                        {
                            coreForm.lbMemoryZones.SetSelected(i, true);
                            break;
                        }

            }
        }

        public static void LoadStateCorruptorSafe(string Key, string RomFile)
        {
            List<string> MemoryBanks = null;

            if (isLoaded)
            {
                MemoryBanks = new List<string>(); //Saves current targetted zones
                foreach (object oneItem in RTC_MemoryZones.SelectedDomains)
                    MemoryBanks.Add(oneItem.ToString());
            }

            GlobalWin.Sound.StopSound();
            GlobalWin.DisplayManager.NeedsToPaint = false;

            var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();

            if (RomFile != null)
            {
                args.OpenAdvanced = new OpenAdvanced_OpenRom { Path = RomFile };
                GlobalWin.MainForm.LoadRom(RomFile, args); //reload rom (evade rom corruption)
            }
            else
            {
                args.OpenAdvanced = new OpenAdvanced_OpenRom { Path = GlobalWin.MainForm.CurrentlyOpenRom };
                GlobalWin.MainForm.LoadRom(GlobalWin.MainForm.CurrentlyOpenRom, args); //reload rom (evade rom corruption)
            }

            LoadSave(Key, false); //Load state

            GlobalWin.DisplayManager.NeedsToPaint = true;
            GlobalWin.Sound.StartSound();

            if (isLoaded)
            {
                RTC_MemoryZones.RefreshDomains(); //refresh and reload zones
                int nbZones = coreForm.lbMemoryZones.Items.Count;

                for (int i = 0; i < nbZones; i++)
                    foreach (string SelectedItem in MemoryBanks)
                        if (coreForm.lbMemoryZones.Items[i].ToString() == SelectedItem)
                        {
                            coreForm.lbMemoryZones.SetSelected(i, true);
                            break;
                        }
            }

        }

        public static void SaveSave(string quickSlotName)
        {
            if (Global.Emulator is NullEmulator)
                return;

            var path = PathManager.SaveStatePrefix(Global.Game) + "." + quickSlotName + ".State";

            var file = new FileInfo(path);
            if (file.Directory != null && file.Directory.Exists == false)
            {
                file.Directory.Create();
            }

            //Filtering out parts 
            path = path.Replace(".Performance.", ".");
            path = path.Replace(".Compatibility.", ".");
            path = path.Replace(".QuickNes.", ".");
            path = path.Replace(".NesHawk.", ".");
            path = path.Replace(".VBA-Next.", ".");
			path = path.Replace(".mGBA.", ".");

			GlobalWin.MainForm.SaveState(path, quickSlotName, false);

        }

        public static void LoadSave(string quickSlotName, bool fromLua = false)
        {
            if (Global.Emulator is NullEmulator)
                return;

            var path = PathManager.SaveStatePrefix(Global.Game) + "." + quickSlotName + ".State";

            //Filtering out parts 
            path = path.Replace(".Performance.", ".");
            path = path.Replace(".Compatibility.", ".");
            path = path.Replace(".QuickNes.", ".");
            path = path.Replace(".NesHawk.", ".");
            path = path.Replace(".VBA-Next.", ".");
			path = path.Replace(".mGBA.", ".");

            if (File.Exists(path) == false)
            {
                GlobalWin.OSD.AddMessage("Unable to load " + quickSlotName + ".State");
                return;
            }

            GlobalWin.MainForm.LoadState(path, quickSlotName, fromLua);
        }

        public static void SetEngineByName(string name)
        {
            for (int i = 0; i < coreForm.cbSelectedEngine.Items.Count; i++)
            {
                if (coreForm.cbSelectedEngine.Items[i].ToString() == name)
                {
                    coreForm.cbSelectedEngine.SelectedIndex = i;
                    break;
                }
            }

        }

    }
}
