using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.ComponentModel;

namespace RTC
{

    public class LabelPassthrough : Label
    {

        protected override void OnPaint(PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, this.Text.ToString(), this.Font, ClientRectangle, ForeColor);
        }

    }

	public class RefreshingListBox : ListBox
	{
		public void RefreshItemsReal()
		{
			base.RefreshItems();
		}
	}

	public class MenuButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        public void SetMenu(ContextMenuStrip _menu)
        {
            Menu = _menu;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (Menu != null && mevent.Button == MouseButtons.Left)
            {
                Menu.Show(this, mevent.Location);
            }
        }

        /*
        protected override void OnPaint(PaintEventArgs pevent)
        {

            base.OnPaint(pevent);
            
            int arrowX = ClientRectangle.Width - 14;
            int arrowY = ClientRectangle.Height / 2 - 1;

            Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
            pevent.Graphics.FillPolygon(brush, arrows);
             
        }
        */
    }
    
    [Serializable()]
    public class Stockpile
    {
        public List<StashKey> stashkeys = new List<StashKey>();

        public string Filename = null;
        public string ShortFilename;
		public string RtcVersion;

        public string descrip = "";

        public string Name;
        public string CloudCorruptID = null;

        public List<string> ComputerSerials = new List<string>();
        public List<string> MakersName = new List<string>();
        public List<string> MakersID = new List<string>();


        public Stockpile(ListBox lbStockpile)
        {
            foreach (StashKey sk in lbStockpile.Items)
            {
                stashkeys.Add(sk);
            }
        }

        public override string ToString()
        {
            return (Name != null ? Name : "");
        }


        public static void Save(Stockpile sks)
        {
            Stockpile.Save(sks, false);
        }

        public static void Save(Stockpile sks, bool IsQuickSave)
        {
            if (sks.stashkeys.Count == 0)
            {
                MessageBox.Show("Can't save because the Current Stockpile is empty");
                return;
            }

            if (!IsQuickSave)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = "sks";
                saveFileDialog1.Title = "Save Stockpile File";
                saveFileDialog1.Filter = "SKS files|*.sks";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    sks.Filename = saveFileDialog1.FileName;
                    sks.ShortFilename = sks.Filename.Substring(sks.Filename.LastIndexOf("\\") + 1, sks.Filename.Length - (sks.Filename.LastIndexOf("\\") + 1));
                }
                else
                    return;
            }
            else
            {
                sks.Filename = RTC_Core.currentStockpile.Filename;
                sks.ShortFilename = RTC_Core.currentStockpile.ShortFilename;
            }

			//Watermarking RTC Version
			sks.RtcVersion = RTC_Core.RtcVersion;

            FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            List<string> AllRoms = new List<string>();

			//populating Allroms array
			foreach (StashKey key in sks.stashkeys)
				if (!AllRoms.Contains(key.RomFile))
				{
					AllRoms.Add(key.RomFile);

					if(key.RomFile.ToUpper().Contains(".CUE"))
						AllRoms.Add(key.RomFile.Substring(0, key.RomFile.Length -4) + ".bin");
				}

            //clean temp2 folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
                File.Delete(file);


            //populating temp2 folder with roms
            for (int i = 0; i < AllRoms.Count; i++)
            {
                string rom = AllRoms[i];
                string romTempfilename = RTC_Core.rtcDir + "\\TEMP2\\" + (rom.Substring(rom.LastIndexOf("\\") + 1, rom.Length - (rom.LastIndexOf("\\") + 1)));


                if (File.Exists(romTempfilename))
                {
                    File.Delete(romTempfilename);
                    File.Copy(rom, romTempfilename);
                }
                else
                    File.Copy(rom, romTempfilename);


            }

            //clean temp folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP"))
                File.Delete(file);

            //sending back filtered files from temp2 folder to temp
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
                File.Move(file, RTC_Core.rtcDir + "\\TEMP\\" + (file.Substring(file.LastIndexOf("\\") + 1, file.Length - (file.LastIndexOf("\\") + 1))));

            //clean temp2 folder again
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
                File.Delete(file);

            foreach (StashKey key in sks.stashkeys)
            {
                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + statefilename))
                    File.Copy(RTC_Core.bizhawkDir + "\\" + key.GameSystem + "\\State\\" + statefilename, RTC_Core.rtcDir + "\\TEMP\\" + statefilename); // copy savestates to temp folder

            }


            for (int i = 0; i < sks.stashkeys.Count; i++) //changes RomFile to short filename
                sks.stashkeys[i].RomFile = sks.stashkeys[i].RomFile.Substring(sks.stashkeys[i].RomFile.LastIndexOf("\\") + 1, sks.stashkeys[i].RomFile.Length - (sks.stashkeys[i].RomFile.LastIndexOf("\\") + 1));


            //creater master.sk to temp folder from stockpile object
            FS = File.Open(RTC_Core.rtcDir + "\\TEMP\\master.sk", FileMode.OpenOrCreate);
            bformatter.Serialize(FS, sks);
            FS.Close();


            //7z the temp folder to destination filename
            string[] stringargs = { "-c", sks.Filename, RTC_Core.rtcDir + "\\TEMP\\" };
            FastZipProgram.Exec(stringargs);


			//Imagine how long would it be with isos in that shit
            //Load(sks.Filename, true); //Reload file after for test and clean

        }

        public static void Load()
        {
            Load(null, false);
        }

		public static void Load(string Filename, bool CorruptCloud)
		{

			if (Filename == null)
			{
				OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
				OpenFileDialog1.DefaultExt = "sks";
				OpenFileDialog1.Title = "Open Stockpile File";
				OpenFileDialog1.Filter = "SKS files|*.sks";
				OpenFileDialog1.RestoreDirectory = true;
				if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
				{
					Filename = OpenFileDialog1.FileName.ToString();
				}
				else
					return;
			}

			if (!File.Exists(Filename))
			{
				MessageBox.Show("The Stockpile file wasn't found");
				return;
			}


			//clean temp folder
			foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP"))
				File.Delete(file);


			//7z extract part

			string[] stringargs = { "-x", Filename, RTC_Core.rtcDir + "\\TEMP\\" };

			FastZipProgram.Exec(stringargs);

			if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\master.sk"))
			{
				MessageBox.Show("The file could not be read properly");
				return;
			}



			//stockpile part
			FileStream FS;
			BinaryFormatter bformatter = new BinaryFormatter();

			Stockpile sks;
			bformatter = new BinaryFormatter();

			try
			{
				FS = File.Open(RTC_Core.rtcDir + "\\TEMP\\master.sk", FileMode.OpenOrCreate);
				sks = (Stockpile)bformatter.Deserialize(FS);
				FS.Close();
			}
			catch
			{
				MessageBox.Show("The Stockpile file could not be loaded");
				return;
			}

			RTC_Core.currentStockpile = sks;

			// repopulating savestates out of temp folder
			foreach (StashKey key in sks.stashkeys)
			{

				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

				if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.GameSystem + "\\State\\" + statefilename))
					File.Copy(RTC_Core.rtcDir + "\\TEMP\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.GameSystem + "\\State\\" + statefilename); // copy savestates to temp folder
			}


			for (int i = 0; i < sks.stashkeys.Count; i++)
			{
				sks.stashkeys[i].RomFile = RTC_Core.rtcDir + "\\TEMP\\" + sks.stashkeys[i].RomFile;
			}


			//fill list controls
			RTC_Core.ghForm.lbStockpile.Items.Clear();
			RTC_Core.spForm.lbStockpile.Items.Clear();

			foreach (StashKey key in sks.stashkeys)
			{
				RTC_Core.ghForm.lbStockpile.Items.Add(key);
				RTC_Core.spForm.lbStockpile.Items.Add(key);
			}


			RTC_Core.ghForm.btnSaveStockpile.Enabled = true;
			RTC_Core.ghForm.btnSaveStockpile.BackColor = Color.Tomato;
			sks.Filename = Filename;

			if (sks.RtcVersion != RTC_Core.RtcVersion)
			{
				if (sks.RtcVersion == null)
					MessageBox.Show("WARNING: You have loaded a pre-0.77 stockpile using RTC " + RTC_Core.RtcVersion + "\n Items might not appear identical to how they when they were created.");
				else
					MessageBox.Show("WARNING: You have loaded a stockpile created with RTC " + sks.RtcVersion + " using RTC " + RTC_Core.RtcVersion + "\n Items might not appear identical to how they when they were created.");
			}

		}

        public static void Import()
        {
            Import(null, false);
        }

        public static void Import(string Filename, bool CorruptCloud)
        {

            //clean temp folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3"))
                File.Delete(file);

            if (Filename == null)
            {
                OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.DefaultExt = "sks";
                OpenFileDialog1.Title = "Open Stockpile File";
                OpenFileDialog1.Filter = "SKS files|*.sks";
                OpenFileDialog1.RestoreDirectory = true;
                if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Filename = OpenFileDialog1.FileName.ToString();
                }
                else
                    return;
            }

            if (!File.Exists(Filename))
            {
                MessageBox.Show("The Stockpile file wasn't found");
                return;
            }

            //7z extract part

            string[] stringargs = { "-x", Filename, RTC_Core.rtcDir + "\\TEMP3\\" };

            FastZipProgram.Exec(stringargs);

            if (!File.Exists(RTC_Core.rtcDir + "\\TEMP3\\master.sk"))
            {
                MessageBox.Show("The file could not be read properly");
                return;
            }



            //stockpile part
            FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            Stockpile sks;
            bformatter = new BinaryFormatter();

            try
            {
                FS = File.Open(RTC_Core.rtcDir + "\\TEMP3\\master.sk", FileMode.OpenOrCreate);
                sks = (Stockpile)bformatter.Deserialize(FS);
                FS.Close();
            }
            catch
            {
                MessageBox.Show("The Stockpile file could not be loaded");
                return;
            }


            // repopulating savestates out of temp folder
            foreach (StashKey key in sks.stashkeys)
            {

                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.GameSystem + "\\State\\" + statefilename))
                    File.Copy(RTC_Core.rtcDir + "\\TEMP3\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.GameSystem + "\\State\\" + statefilename); // copy savestates to temp folder
            }

            for (int i = 0; i < sks.stashkeys.Count; i++)
            {
                sks.stashkeys[i].RomFile = RTC_Core.rtcDir + "\\TEMP\\" + sks.stashkeys[i].RomFile;
            }

            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3\\"))
                if(!file.Contains(".sk") && !file.Contains(".timejump.State"))
                    try
                    {
                        File.Copy(file, RTC_Core.rtcDir + "\\TEMP\\" + file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'))); // copy roms to temp
                    }
                    catch{}

            //fill list controls

            foreach (StashKey key in sks.stashkeys)
            {
                RTC_Core.ghForm.lbStockpile.Items.Add(key);
            }

        }

    }
    

    
    [Serializable()]
    public class StashKey
    {

        public String RomFile;
        public String GameSystem;
        public List<string> MemoryZones = new List<string>();
        public String GameName;

        public String Key;
        public String ParentKey = null;
        public BlastLayer blastlayer = null;

        public String Alias
        {
            get
            {
                if (_Alias != null)
                    return _Alias;
                else
                    return Key;
            }
            set
            {
                _Alias = value;
            }
        }

        private String _Alias;

        public StashKey(String _key, String _parentkey, BlastLayer _blastlayer)
        {
            PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
            Global.Config.PathEntries[Global.Game.System, "Base"];

            Key = _key;
            ParentKey = _parentkey;
            blastlayer = _blastlayer;
            RomFile = GlobalWin.MainForm.CurrentlyOpenRom;
            GameSystem = RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName);
            GameName = PathManager.FilesystemSafeName(Global.Game);

            MemoryZones.AddRange(RTC_MemoryZones.SelectedDomains);

        }

        public override string ToString()
        {
            return Alias;
        }

        public bool Run()
        {
            if (RTC_Core.currentGameSystem != RTC_Core.currentStashkey.GameSystem || RTC_Core.currentGameName != RTC_Core.currentStashkey.GameName)
                RTC_Core.LoadRom(RTC_Core.currentStashkey.RomFile);

            RTC_Core.ghForm.LoadState(ParentKey, RTC_Core.currentStashkey.GameSystem, RTC_Core.currentStashkey.GameName);
            RTC_Core.Blast(blastlayer);

            return (blastlayer.Layer.Count > 0);
        }

        public void RunOriginal()
        {
            if (RTC_Core.currentGameSystem != RTC_Core.currentStashkey.GameSystem || RTC_Core.currentGameName != RTC_Core.currentStashkey.GameName)
                RTC_Core.LoadRom(RTC_Core.currentStashkey.RomFile);

            RTC_Core.ghForm.LoadState(ParentKey, RTC_Core.currentStashkey.GameSystem, RTC_Core.currentStashkey.GameName);
        }

        public bool Inject()
        {
            RTC_Core.Blast(blastlayer);

            return (blastlayer.Layer.Count > 0);
        }

    }
    

    [Serializable()]
    public class BlastLayer
    {
        public List<BlastUnit> Layer;
        public CorruptCloudGameData CCGD = null;

        public BlastLayer()
        {
            Layer = new List<BlastUnit>();
        }

        public BlastLayer(List<BlastUnit> _layer)
        {
            Layer = _layer;
        }

        public void Apply()
        {
            if(RTC_Core.SelectedEngine != CorruptionEngine.HELLGENIE && RTC_Core.SelectedEngine != CorruptionEngine.FREEZE)
                RTC_Core.lastBlastLayerBackup = GetBackup();

            foreach (BlastUnit bb in Layer)
                bb.Apply();
        }

        public BlastLayer GetBackup()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>(); ;
            
            foreach (BlastUnit bb in Layer)
                BackupLayer.Add(bb.GetBackup());

            BlastLayer Recovery = new BlastLayer(BackupLayer);

            return Recovery;
        }

    }

    [Serializable()]
    public abstract class BlastUnit
    {
        public abstract void Apply();
        public abstract BlastUnit GetBackup();
    }

    [Serializable()]
    public class BlastByte : BlastUnit
    {
        public string Domain;
        public long Address;
        public BlastByteType Type;
        public int Value;
        public bool IsEnabled;

        public BlastByte(string _domain, long _address, BlastByteType _type, int _value, bool _isEnabled)
        {
            Domain = _domain;
            Address = _address;
            Type = _type;
            Value = _value;
            IsEnabled = _isEnabled;
        }

        public override void Apply()
        {
            if (!IsEnabled)
                return;

            try
            {
                MemoryDomain md = RTC_MemoryZones.getDomain(Domain);

                if (md == null || md.PokeByte == null)
                    return;

                switch (Type)
                {
                    case BlastByteType.SET:
                        md.PokeByte(Address, (byte)Value);
                        break;

                    case BlastByteType.ADD:
                        md.PokeByte(Address, (byte)(md.PeekByte(Address) + Value));
                        break;

                    case BlastByteType.SUBSTRACT:
                        md.PokeByte(Address, (byte)(md.PeekByte(Address) - Value));
                        break;

                    case BlastByteType.NONE:
                        return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("The BlastByte apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                ex.ToString());
                return;
            }

        }

        public override BlastUnit GetBackup()
        {
            if (!IsEnabled)
                return null;

            try
            {
                MemoryDomain md = RTC_MemoryZones.getDomain(Domain);

                if (md == null || Type == BlastByteType.NONE)
                    return null;

                return new BlastByte(Domain, Address, BlastByteType.SET, md.PeekByte(Address), true);

            }
            catch (Exception ex)
            {
                MessageBox.Show("The BlastByte apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                ex.ToString());
                return null;
            }

        }

        public override string ToString()
        {
            string EnabledString = "[ ] ";
            if (IsEnabled)
                EnabledString = "[x] ";

            string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
            return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + Type.ToString() + "(" + Value.ToString() + ")");
        }
    }

    [Serializable()]
    public class BlastCheat : BlastUnit
    {
        public string Domain;
        public long address;
        public BizHawk.Client.Common.DisplayType displayType;
        public bool bigEndian;
        public int value;
        public bool IsEnabled;
        public bool IsFreeze;
        WatchSize size;

        public BlastCheat(string _domain, long _address, BizHawk.Client.Common.DisplayType _displayType, bool _bigEndian, int _value, bool _isEnabled, bool _isFreeze)
        {

            var settings = new RamSearchEngine.Settings(RTC_Core.hexeditor.MemoryDomains);


            Domain = _domain;
            address = _address;

            size = settings.Size;
            displayType = settings.Type;
            bigEndian = settings.BigEndian;

            //DisplayType = _displayType;
            //BigEndian = _bigEndian;

            value = _value;
            IsEnabled = _isEnabled;
            IsFreeze = _isFreeze;
            
        }

        public override void Apply()
        {
            try
            {
                if (!IsEnabled)
                    return;

                MemoryDomain md = RTC_MemoryZones.getDomain(Domain);

                if (md == null)
                    return;

                string cheatName = "RTC Cheat|" + Domain + "|" + address.ToString() + "|" + displayType.ToString() + "|" + bigEndian.ToString() + "|" + value.ToString() + "|" + IsEnabled.ToString() + "|" + IsFreeze.ToString();

                if (!IsFreeze)
                {
                    Watch somewatch = Watch.GenerateWatch(md, address, size, displayType, bigEndian, cheatName, value, 0,0);
                    Cheat ch = new Cheat(somewatch, value, null, true);
                    Global.CheatList.Add(ch);

                    RTC_HellgenieEngine.RemoveExcessCheats();
                }
                else
                {
                    RTC_Core.hexeditor.FreezeAddress(address, cheatName);

                    RTC_FreezeEngine.RemoveExcessCheats();
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("The BlastCheat apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                ex.ToString());
                return;
            }
        }

        public override BlastUnit GetBackup()
        {
            return null;
        }

        public override string ToString()
        {
            string EnabledString = "[ ] ";
            if (IsEnabled)
                EnabledString = "[x] ";

            string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"

            //RTC_TODO: Rewrite the toString method for this
            return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(address).ToString() + ")." + displayType.ToString() + "(" + value.ToString() + ")");
        }
    }

    [Serializable()]
    public class CorruptCloudGameData
    {
        public string fileHash = null;
        public string originalFileName = null;
        public string originalGameName = null;
        public string originalGameSystem = null;
        public string originalKey = null;
        public byte[] originalSavestate = null;

        public CorruptCloudGameData(StashKey key)
        {
            PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
            Global.Config.PathEntries[Global.Game.System, "Base"];

            fileHash = Global.Game.Hash;

            originalFileName = GlobalWin.MainForm.CurrentlyOpenRom;

            if(originalFileName.IndexOf("\\") != -1)
                originalFileName = originalFileName.Substring(originalFileName.LastIndexOf("\\")+1);

            originalGameName = PathManager.FilesystemSafeName(Global.Game);
            originalGameSystem = RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName);

            if (key.ParentKey == null)
                originalKey = key.Key;
            else
                originalKey = key.ParentKey;


            string originalSavestateFilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
            originalSavestate = File.ReadAllBytes(RTC_Core.bizhawkDir + "\\" + key.GameSystem + "\\State\\" + originalSavestateFilename);

            originalKey = RTC_Core.GetRandomKey();
        }

        public void PutBackSavestate()
        {
            string filename = RTC_Core.bizhawkDir + "\\" + originalGameSystem + "\\State\\" + originalGameName + "." + originalKey + ".timejump.State";

            if(File.Exists(filename))
                File.Delete(filename);

            File.WriteAllBytes(filename, originalSavestate);
        }

        public bool CheckCompatibility()
        {
            if (Global.Emulator is NullEmulator)
                return false;


            if (fileHash != Global.Game.Hash)
            {
                PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
                Global.Config.PathEntries[Global.Game.System, "Base"];

                string currentFilename = GlobalWin.MainForm.CurrentlyOpenRom;

                if (currentFilename.IndexOf("\\") != -1)
                    currentFilename = currentFilename.Substring(currentFilename.LastIndexOf("\\") + 1);

                string cctext =
                    "RTC has detected that loaded game doesn't match the game from the savestate: \n" +
                    "\n" +
                    "Original hash: " + fileHash + "\n" +
                    "Current hash: " + Global.Game.Hash + "\n" +
                    "\n" +
                    "Original name: " + originalGameName + "\n" +
                    "Current name: " + PathManager.FilesystemSafeName(Global.Game) + "\n" +
                    "\n" +
                    "Original system: " + originalGameSystem + "\n" +
                    "Current system: " + RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName) + "\n" +
                    "\n" +
                    "Original filename: " + originalFileName + "\n" +
                    "Current filename: " + currentFilename + "\n" +
                    "\n" +
                    "If you press OK, RTC will try to load it anyway.";


                if (MessageBox.Show(cctext, "CorruptCloud Compatibility Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return false;
            }

            return true;
        }

    }

    [Serializable()]
    public class ActiveTableObject
    {
        public long[] data;

        public ActiveTableObject(long[] _data)
        {
            data = _data;
        }
    }
}
