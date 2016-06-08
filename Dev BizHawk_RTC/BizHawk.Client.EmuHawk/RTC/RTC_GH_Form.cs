using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;
using BizHawk.Client.Common;
using System.IO;
using BizHawk.Emulation.Common;

namespace RTC
{
    public partial class RTC_GH_Form : Form
    {
        public string currentSelectedState;
        public string[] btnParentKeys = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        public string[] btnAttachedRom = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

        public bool DontLoadSelectedStash = false;
        public bool DontLoadSelectedStockpile = false;

        public bool _isStockpileSelectMultiple = false;
        public bool isStockpileSelectMultiple
        {
            get
            {
                return _isStockpileSelectMultiple;
            }
            set
            {
                if (value)
                {
                    btnSelectMultiple.Text = "Select Multiple [x]";
                    btnSelectMultiple.ForeColor = Color.White;
                    lbStockpile.SelectionMode = SelectionMode.MultiSimple;
                    btnCorrupt.Text = "Merge";

                    if (rbCorrupt.Checked)
                        rbCorrupt.Checked = false;
                    else if (rbInject.Checked)
                        rbInject.Checked = false;
                    else if (rbOriginal.Checked)
                        rbOriginal.Checked = false;

                }
                else
                {
                    btnSelectMultiple.Text = "Select Multiple [ ]";
                    btnSelectMultiple.ForeColor = Color.Black;
                    lbStockpile.SelectionMode = SelectionMode.One;

                    if (!rbCorrupt.Checked && !rbInject.Checked && !rbOriginal.Checked)
                        rbCorrupt.Checked = true;
                }

                _isStockpileSelectMultiple = value;
            }
        }

        private bool _IsCorruptionApplied = false;
        public bool IsCorruptionApplied
        {
            get
            {
                return _IsCorruptionApplied;
            }
            set
            {
                if (value)
                {
                    btnBlastToggle.BackColor = Color.Red;
                    btnBlastToggle.ForeColor = Color.Black;
                    btnBlastToggle.Text = "ON";
                }
                else
                {
                    btnBlastToggle.BackColor = Color.Black;
                    btnBlastToggle.ForeColor = Color.Silver;
                    btnBlastToggle.Text = "OFF";
                }

                _IsCorruptionApplied = value;
            }
        }
        

        public RTC_GH_Form()
        {
            InitializeComponent();
            btnSavestate_Click(btnSavestate01, new EventArgs()); //Selects first button as default
        }

        private void RTC_GH_Form_Load(object sender, EventArgs e)
        {
            GlobalWin.MainForm.Focus();
        }

        public void btnSavestate_Click(object sender, EventArgs e)
        {
            btnSavestate01.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate02.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate03.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate04.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate05.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate06.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate07.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate08.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate09.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate10.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate11.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate12.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate13.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate14.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate15.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate16.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate17.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate18.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate19.ForeColor = Color.FromArgb(192, 255, 192);
            btnSavestate20.ForeColor = Color.FromArgb(192, 255, 192);

            Button clickedButton = (sender as Button);
            clickedButton.ForeColor = Color.OrangeRed;
            clickedButton.BringToFront();

            currentSelectedState = clickedButton.Text;

            if(cbSavestateLoadOnClick.Checked)
                LoadState();

            RTC_Restore.SaveRestore();
        }

        private void btnToggleSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                btnSaveLoad.Text = "SAVE";
                btnSaveLoad.ForeColor = Color.OrangeRed;
            }
            else
            {
                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
            }

            RTC_Restore.SaveRestore();
        }

        public void btnSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                LoadState();
            }
            else
            {
                SaveState();

                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
            }

            RTC_Restore.SaveRestore();
        }

        public void LoadState()
        {


            LoadState("btn", RTC_Core.currentGameSystem, RTC_Core.currentGameName);
            IsCorruptionApplied = false;

            RTC_Restore.SaveRestore();
        }

        public void LoadState(string _key, string GameSystem, string GameName)
        {

            string Key;
            bool GameHasChanged = false;
            string TheoricalSaveStateFilename;

            if (_key == "btn")
            {
                Key = btnParentKeys[Convert.ToInt32(currentSelectedState)];

                GlobalWin.Sound.StopSound();

                if (ChangeGameWarning(btnAttachedRom[Convert.ToInt32(currentSelectedState)]))
                {
                    var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();
                    args.OpenAdvanced = new OpenAdvanced_OpenRom { Path = btnAttachedRom[Convert.ToInt32(currentSelectedState)] };
                    GlobalWin.MainForm.LoadRom(btnAttachedRom[Convert.ToInt32(currentSelectedState)], args);
                    GameHasChanged = true;
                }
                else
                {
                    GlobalWin.Sound.StartSound();
                    return;
                }

                GlobalWin.Sound.StartSound();
            }
            else if (_key == null)
            {
                RTC_Restore.SaveRestore();
                return;
            }
            else
                Key = _key;


            PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
            Global.Config.PathEntries[Global.Game.System, "Base"];

            if(!GameHasChanged)
                TheoricalSaveStateFilename = RTC_Core.bizhawkDir + "\\" + GameSystem + "\\State\\" + GameName + "." + Key + ".timejump.State";
            else
                TheoricalSaveStateFilename = RTC_Core.bizhawkDir + "\\" + RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName) + "\\State\\" + PathManager.FilesystemSafeName(Global.Game) + "." + Key + ".timejump.State";


            if (File.Exists(TheoricalSaveStateFilename))
            {
                RTC_Core.LoadStateCorruptorSafe(Key + ".timejump", null);
            }
            else
            {
                GlobalWin.Sound.StopSound();
                MessageBox.Show("Error loading savestate (File not found)");
                GlobalWin.Sound.StartSound();
                return;
            }

            RTC_Restore.SaveRestore();
        }

        public void SaveState()
        {
            string Key = RTC_Core.GetRandomKey();
            RTC_Core.SaveSave(Key + ".timejump");

            btnParentKeys[Convert.ToInt32(currentSelectedState)] = Key;
            btnAttachedRom[Convert.ToInt32(currentSelectedState)] = GlobalWin.MainForm.CurrentlyOpenRom;

            btnSaveLoad.Text = "LOAD";
            btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);

            RTC_Restore.SaveRestore();
        }


        public void btnCorrupt_Click(object sender, EventArgs e)
        {

            if (RTC_Core.coreForm.cbClearCheatsOnRewind.Checked == true)
                RTC_HellgenieEngine.ClearCheats();

            if (cbAutoLoadState.Checked && btnCorrupt.Text.ToUpper() != "MERGE")
                if (btnParentKeys[Convert.ToInt32(currentSelectedState)] != null)
                {
                    LoadState();
                }
                else
                {
                    GlobalWin.Sound.StopSound();
                    MessageBox.Show("There is no SaveState in the selected box,\nPress 'Switch: Save/Load State' then Press 'SAVE'");
                    GlobalWin.Sound.StartSound();
                    return;
                }


            if (rbCorrupt.Checked)
            {
                BlastLayer bl = RTC_Core.Blast();

                if(bl != null)
                    IsCorruptionApplied = true;

                if (cbStashCorrupted.Checked)
                {
                    if (bl == null)
                        return;

                    RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), btnParentKeys[Convert.ToInt32(currentSelectedState)], bl);

                    DontLoadSelectedStash = true;
                    lbStashHistory.Items.Add(RTC_Core.currentStashkey);
                    lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
                    lbStockpile.ClearSelected();
                    
                }

                if(cbRenderAtCorrupt.Checked)
                    StartRender();

            }
            else if (rbInject.Checked)
            {
                if (lbStashHistory.SelectedIndex == -1 && lbStockpile.SelectedIndex == -1)
                    return;

                if (cbAutoLoadState.Checked)
                    if (btnParentKeys[Convert.ToInt32(currentSelectedState)] != null)
                        LoadState();
                    else
                    {
                        GlobalWin.Sound.StopSound();
                        MessageBox.Show("There is no SaveState in the selected box,\nPress 'Switch: Save/Load State' then Press 'SAVE'");
                        GlobalWin.Sound.StartSound();
                        return;
                    }

                if (lbStashHistory.SelectedIndex != -1)
                {
                    RTC_Core.currentStashkey = (lbStashHistory.SelectedItem as StashKey);
                    RTC_Core.currentStashkey.Inject();
                    IsCorruptionApplied = true;
                }

                if (lbStockpile.SelectedIndex != -1)
                {
                    RTC_Core.currentStashkey = (lbStockpile.SelectedItem as StashKey);
                    RTC_Core.currentStashkey.Inject();
                    IsCorruptionApplied = true;
                }

                if (cbStashInjected.Checked)
                {
                    if(lbStashHistory.SelectedIndex != -1)
                        RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), btnParentKeys[Convert.ToInt32(currentSelectedState)], (lbStashHistory.SelectedItem as StashKey).blastlayer);

                    if(lbStockpile.SelectedIndex != -1)
                        RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), btnParentKeys[Convert.ToInt32(currentSelectedState)], (lbStockpile.SelectedItem as StashKey).blastlayer);

                    DontLoadSelectedStash = true;
                    lbStashHistory.Items.Add(RTC_Core.currentStashkey);
                    lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
                    lbStockpile.ClearSelected();

                }

                if (cbRenderAtCorrupt.Checked)
                    StartRender();

            }
            else if (rbOriginal.Checked)
            {
                if (lbStashHistory.SelectedIndex == -1 && lbStockpile.SelectedIndex == -1)
                    return;

                if (lbStashHistory.SelectedIndex != -1)
                {
                    RTC_Core.currentStashkey = (lbStashHistory.SelectedItem as StashKey);
                    RTC_Core.currentStashkey.RunOriginal();
                    IsCorruptionApplied = false;
                }

                if (lbStockpile.SelectedIndex != -1)
                {
                    RTC_Core.currentStashkey = (lbStockpile.SelectedItem as StashKey);
                    IsCorruptionApplied = false;
                }

            }
            else
            {
                if (lbStockpile.SelectedItems.Count > 1)
                {
                    BlastLayer bl = new BlastLayer();

                    foreach (StashKey item in lbStockpile.SelectedItems)
                        bl.Layer.AddRange(item.blastlayer.Layer);

                    //bl.Apply();
                    isStockpileSelectMultiple = false;

                    if (cbStashCorrupted.Checked)
                    {
                        if (bl == null)
                            return;

                        RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), (lbStockpile.SelectedItem as StashKey).ParentKey, bl);

                        ApplyCurrentStashkey();

                        DontLoadSelectedStash = true;
                        lbStashHistory.Items.Add(RTC_Core.currentStashkey);
                        lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
                        lbStockpile.ClearSelected();
                        IsCorruptionApplied = true;

                    }

                    if (cbRenderAtCorrupt.Checked)
                        StartRender();
                }
                else
                    MessageBox.Show("Select 2 or more items from the Current Stockpile to merge.");
            }

            RTC_Restore.SaveRestore();
        }

        private void RTC_GH_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
                RTC_Restore.SaveRestore();
            }

        }

        private void lbStashHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DontLoadSelectedStash || lbStashHistory.SelectedIndex == -1)
            {
                DontLoadSelectedStash = false;
                return;
            }

            lbStockpile.ClearSelected();

            if (!cbLoadOnSelect.Checked)
                return;

            if (!rbCorrupt.Checked && !rbInject.Checked && !rbOriginal.Checked)
            {
                rbCorrupt.Checked = true;
                isStockpileSelectMultiple = false;
            }


            if (cbAutoLoadState.Checked && rbInject.Checked)
                if (btnParentKeys[Convert.ToInt32(currentSelectedState)] != null)
                    LoadState();
                else
                {
                    GlobalWin.Sound.StopSound();
                    MessageBox.Show("There is no SaveState in the selected box,\nPress 'Switch: Save/Load State' then Press 'SAVE'");
                    GlobalWin.Sound.StartSound();
                    return;
                }

            RTC_Core.currentStashkey = (lbStashHistory.SelectedItem as StashKey);

            ApplyCurrentStashkey();

            if (cbRenderAtLoad.Checked)
                StartRender();

            RTC_Restore.SaveRestore();
        }

        private void ApplyCurrentStashkey()
        {
            if (rbCorrupt.Checked)
            {

                IsCorruptionApplied = RTC_Core.currentStashkey.Run();
            }
            else if (rbInject.Checked)
            {


                IsCorruptionApplied = RTC_Core.currentStashkey.Inject();
            }
            else if (rbOriginal.Checked)
            {
                RTC_Core.currentStashkey.RunOriginal();
                IsCorruptionApplied = false;
            }
            else
            {
                RTC_Core.currentStashkey.Run();
                IsCorruptionApplied = true;
            }

            if (cbRenderAtLoad.Checked)
                StartRender();

            RTC_Restore.SaveRestore();
        }

        private void rbInject_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInject.Checked)
            {
                isStockpileSelectMultiple = false;
                btnCorrupt.Text = "Inject";

                RTC_Restore.SaveRestore();
            }
        }

        private void rbCorrupt_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCorrupt.Checked)
            {
                isStockpileSelectMultiple = false;
                btnCorrupt.Text = "Blast/Send";

                RTC_Restore.SaveRestore();
            }
        }

        private void rbOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOriginal.Checked)
            {
                isStockpileSelectMultiple = false;
                btnCorrupt.Text = "Original";

                RTC_Restore.SaveRestore();
            }
        }

        private void btnStartRender_Click(object sender, EventArgs e)
        {
            StartRender();
        }

        
        public void StartRender()
        {
            if (!rbRenderNone.Checked) // Render Output
            {
                string Key = "RENDER_";

                if (cbRenderAtLoad.Checked && RTC_Core.currentStashkey != null)
                    Key += RTC_Core.currentStashkey.Alias;
                else
                    Key += RTC_Core.GetRandomKey();


                if (rbRenderWAV.Checked)
                    GlobalWin.MainForm._RecordAv("wave", RTC_Core.rtcDir + "\\RENDEROUTPUT\\" + Key + ".wav", true);

                if (rbRenderAVI.Checked)
                    GlobalWin.MainForm._RecordAv("vfwavi", RTC_Core.rtcDir + "\\RENDEROUTPUT\\" + Key + ".avi", true);

                if (rbRenderMPEG.Checked)
                    GlobalWin.MainForm._RecordAv("ffmpeg", RTC_Core.rtcDir + "\\RENDEROUTPUT/" + Key + ".mpg", true);

                btnStopRender.Visible = true;
                btnStartRender.Visible = false;
            }
        }

        public void StopRender()
        {
            GlobalWin.MainForm.StopAv();
            btnStopRender.Visible = false;
            btnStartRender.Visible = true;
        }

        private void btnAddStashToStockpile_Click(object sender, EventArgs e)
        {
            AddStashToStockpile();

            RTC_Restore.SaveRestore();
        }

        public void AddStashToStockpile()
        {
            if (lbStashHistory.Items.Count == 0 || lbStashHistory.SelectedIndex == -1)
            {
                MessageBox.Show("Can't add the Stash to the Stockpile because none is selected in the Stash History");
                return;
            }


            string Name = "";
            string value = "";

            GlobalWin.Sound.StopSound();
            if (this.InputBox("Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK)
            {
                Name = value.Trim();
                GlobalWin.Sound.StartSound();
            }
            else
            {
                GlobalWin.Sound.StartSound();
                return;
            }


            if (Name != "")
                RTC_Core.currentStashkey.Alias = Name;
            else
                RTC_Core.currentStashkey.Alias = RTC_Core.currentStashkey.Key;

            lbStockpile.Items.Add(RTC_Core.currentStashkey);
            lbStashHistory.Items.RemoveAt(lbStashHistory.SelectedIndex);

            DontLoadSelectedStockpile = true;
            lbStockpile.SelectedIndex = lbStockpile.Items.Count - 1;



            RTC_Restore.SaveRestore();
        }

        public DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void lbStockpile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DontLoadSelectedStockpile || lbStockpile.SelectedIndex == -1)
            {
                DontLoadSelectedStockpile = false;
                return;
            }

            lbStashHistory.ClearSelected();

            if (!cbLoadOnSelect.Checked)
                return;

            if (cbAutoLoadState.Checked && rbInject.Checked)
                if (btnParentKeys[Convert.ToInt32(currentSelectedState)] != null)
                {
                    LoadState();
                }
                else
                {
                    GlobalWin.Sound.StopSound();
                    MessageBox.Show("There is no SaveState in the selected box,\nPress 'Switch: Save/Load State' then Press 'SAVE'");
                    GlobalWin.Sound.StartSound();
                    return;
                }

            RTC_Core.currentStashkey = (lbStockpile.SelectedItem as StashKey);

            ApplyCurrentStashkey();

            if (cbRenderAtLoad.Checked)
                StartRender();

            RTC_Restore.SaveRestore();
        }

        private void btnClearStashHistory_Click(object sender, EventArgs e)
        {
            lbStashHistory.Items.Clear();

            RTC_Restore.SaveRestore();
        }

        private void btnRemoveSelectedStockpile_Click(object sender, EventArgs e)
        {
            if (lbStockpile.SelectedIndex != -1)
                lbStockpile.Items.RemoveAt(lbStockpile.SelectedIndex);

            RTC_Restore.SaveRestore();
        }

        private void btnClearStockpile_Click(object sender, EventArgs e)
        {
            lbStockpile.Items.Clear();

            RTC_Restore.SaveRestore();
        }

        private void btnLoadStockpile_Click(object sender, EventArgs e)
        {
            GlobalWin.Sound.StopSound();
			RTC_RPC.SendToKillSwitch("FREEZE");
			Stockpile.Load();
			RTC_RPC.SendToKillSwitch("UNFREEZE");
			GlobalWin.Sound.StartSound();

            RTC_Restore.SaveRestore();
        }

        private void btnSaveStockpileAs_Click(object sender, EventArgs e)
        {
            if (lbStockpile.Items.Count == 0)
            {
                MessageBox.Show("You cannot save the Stockpile because it is empty");
                return;
            }

            GlobalWin.Sound.StopSound();
			RTC_RPC.SendToKillSwitch("FREEZE");
            Stockpile sks = new Stockpile(lbStockpile);
            Stockpile.Save(sks);
			RTC_RPC.SendToKillSwitch("UNFREEZE");
			GlobalWin.Sound.StartSound();

            RTC_Restore.SaveRestore();
        }

        private void btnSaveStockpile_Click(object sender, EventArgs e)
        {
			GlobalWin.Sound.StopSound();
			RTC_RPC.SendToKillSwitch("FREEZE");
			Stockpile sks = new Stockpile(lbStockpile);
            Stockpile.Save(sks, true);
			RTC_RPC.SendToKillSwitch("UNFREEZE");
			GlobalWin.Sound.StartSound();
		}

        public void btnBlastToggle_Click(object sender, EventArgs e)
        {
            if (Global.Emulator is NullEmulator)
                return;

            if (RTC_Core.currentStashkey == null || RTC_Core.currentStashkey.blastlayer.Layer.Count == 0)
            {
                IsCorruptionApplied = false;
                return;
            }

            if (!IsCorruptionApplied)
            {
                IsCorruptionApplied = true;
                RTC_Core.currentStashkey.blastlayer.Apply();

            }
            else
            {
                IsCorruptionApplied = false;

                RTC_HellgenieEngine.ClearCheats();

                if(RTC_Core.lastBlastLayerBackup != null)
                    RTC_Core.lastBlastLayerBackup.Apply();
            }


            RTC_Restore.SaveRestore();
        }

        private void btnImportStockpile_Click(object sender, EventArgs e)
        {
            GlobalWin.Sound.StopSound();
            Stockpile.Import();
            GlobalWin.Sound.StartSound();

            RTC_Restore.SaveRestore();
        }

        private void btnStashUP_Click(object sender, EventArgs e)
        {
            if(lbStashHistory.SelectedIndex == -1)
                return;

            if (lbStashHistory.SelectedIndex == 0)
                lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
            else
                lbStashHistory.SelectedIndex--;

            RTC_Restore.SaveRestore();
        }

        private void btnStashDOWN_Click(object sender, EventArgs e)
        {
                if (lbStashHistory.SelectedIndex == -1)
                    return;

                if (lbStashHistory.SelectedIndex == lbStashHistory.Items.Count - 1)
                    lbStashHistory.SelectedIndex = 0;
                else
                    lbStashHistory.SelectedIndex++;

                RTC_Restore.SaveRestore();
        }

        private void btnStockpileUP_Click(object sender, EventArgs e)
        {
            if (lbStockpile.SelectedIndex == -1)
                return;

            if (lbStockpile.SelectedIndex == 0)
                lbStockpile.SelectedIndex = lbStockpile.Items.Count - 1;
            else
                lbStockpile.SelectedIndex--;

            RTC_Restore.SaveRestore();
        }

        private void btnStockpileDOWN_Click(object sender, EventArgs e)
        {
            if (lbStockpile.SelectedIndex == -1)
                    return;

            if (lbStockpile.SelectedIndex == lbStockpile.Items.Count - 1)
                lbStockpile.SelectedIndex = 0;
            else
                lbStockpile.SelectedIndex++;

            RTC_Restore.SaveRestore();
        }

        private void RTC_GH_Form_ResizeEnd(object sender, EventArgs e)
        {
            RTC_Restore.SaveRestore();
        }

        private void btnStockpileMoveSelectedUp_Click(object sender, EventArgs e)
        {
            if(lbStockpile.Items.Count < 2)
                return;

            object o = lbStockpile.SelectedItem;
            int pos = lbStockpile.SelectedIndex;
            int count = lbStockpile.Items.Count;
            lbStockpile.Items.RemoveAt(pos);

            DontLoadSelectedStockpile = true;


            if (pos == 0)
            {
                lbStockpile.Items.Add(o);
                lbStockpile.SelectedIndex = count - 1;
            }
            else
            {
                lbStockpile.Items.Insert(pos - 1, o);
                lbStockpile.SelectedIndex = pos-1;
            }
        }

        private void btnStockpileMoveSelectedDown_Click(object sender, EventArgs e)
        {
            if (lbStockpile.Items.Count < 2)
                return;

            object o = lbStockpile.SelectedItem;
            int pos = lbStockpile.SelectedIndex;
            int count = lbStockpile.Items.Count;
            lbStockpile.Items.RemoveAt(pos);

            DontLoadSelectedStockpile = true;

            if (pos == count - 1)
            {
                lbStockpile.Items.Insert(0,o);
                lbStockpile.SelectedIndex = 0;
            }
            else
            {
                lbStockpile.Items.Insert(pos + 1, o);
                lbStockpile.SelectedIndex = pos + 1;
            }
        }

        private void btnStopRender_Click(object sender, EventArgs e)
        {
            StopRender();
        }

        private void nmIntensity_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_Intensity.Value) == Convert.ToInt32(nmIntensity.Value))
                return;

            track_Intensity.Value = Convert.ToInt32(nmIntensity.Value);
            RTC_Core.Intensity = Convert.ToInt32(nmIntensity.Value);

            RTC_Core.coreForm.nmIntensity.Value = nmIntensity.Value;

            RTC_Restore.SaveRestore();
        }

        private void track_Intensity_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_Intensity.Value) == Convert.ToInt32(nmIntensity.Value))
                return;

            nmIntensity.Value = Convert.ToDecimal(track_Intensity.Value);
            RTC_Core.Intensity = Convert.ToInt32(nmIntensity.Value);

            RTC_Core.coreForm.nmIntensity.Value = Convert.ToDecimal(track_Intensity.Value);

            RTC_Restore.SaveRestore();
        }

        private void cbBackupHistory_CheckedChanged(object sender, EventArgs e)
        {
            RTC_Restore.SaveRestore();
        }

        private void btnSelectMultiple_Click(object sender, EventArgs e)
        {

            if (!isStockpileSelectMultiple)
                isStockpileSelectMultiple = true;
            else
                isStockpileSelectMultiple = false;
        }

        private void btnCloudSave_Click(object sender, EventArgs e)
        {
            RTC_RPC.SendToKillSwitch("FREEZE");

            BlastLayer bl;
            StashKey key;

            if (lbStashHistory.SelectedIndex != -1)
                key = (lbStashHistory.SelectedItem as StashKey);
            else if (lbStockpile.SelectedIndex != -1)
                key = (lbStockpile.SelectedItem as StashKey);
            else
                return;


            bl = key.blastlayer;
            bl.CCGD = new CorruptCloudGameData(key);

            tbCorruptCloudCode.Text = RTC_CorruptCloud.CloudSave(bl);

            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private void btnCloudCorrupt_Click(object sender, EventArgs e)
        {
            if (!IsValidCCC())
            {
                GlobalWin.Sound.StopSound();
                MessageBox.Show("The CorruptCloud Code entered below isn't valid");
                GlobalWin.Sound.StartSound();
                return;
            }



            GlobalWin.Sound.StopSound();
            RTC_RPC.SendToKillSwitch("FREEZE");


            BlastLayer bl = RTC_CorruptCloud.CloudLoad(tbCorruptCloudCode.Text);
            if (!bl.CCGD.CheckCompatibility())
            {
                GlobalWin.Sound.StartSound();
                RTC_RPC.SendToKillSwitch("UNFREEZE");
                return;
            }

            if (cbAutoLoadState.Checked)
            {
                bl.CCGD.PutBackSavestate();
                LoadState(bl.CCGD.originalKey, bl.CCGD.originalGameSystem, bl.CCGD.originalGameName);
            }

                if (bl != null)
                    bl.Apply();
                else
                    return;


                if (bl != null)
                    IsCorruptionApplied = true;

                if (cbStashCorrupted.Checked)
                {

                    RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), bl.CCGD.originalKey, bl);

                    DontLoadSelectedStash = true;
                    lbStashHistory.Items.Add(RTC_Core.currentStashkey);
                    lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
                    lbStockpile.ClearSelected();

                }

                if (cbRenderAtCorrupt.Checked)
                    StartRender();

                RTC_RPC.SendToKillSwitch("UNFREEZE");
                GlobalWin.Sound.StartSound();
        }

        private void btnCloudInject_Click(object sender, EventArgs e)
        {
            if (!IsValidCCC())
            {
                GlobalWin.Sound.StopSound();
                MessageBox.Show("The CorruptCloud Code entered below isn't valid");
                GlobalWin.Sound.StartSound();
                return;
            }
            GlobalWin.Sound.StopSound();
            RTC_RPC.SendToKillSwitch("FREEZE");

            BlastLayer bl = RTC_CorruptCloud.CloudLoad(tbCorruptCloudCode.Text);

            if (!bl.CCGD.CheckCompatibility())
            {
                GlobalWin.Sound.StartSound();
                return;
            }

            if (cbAutoLoadState.Checked)
                if (btnParentKeys[Convert.ToInt32(currentSelectedState)] != null)
                    LoadState();
                else
                {
                    GlobalWin.Sound.StopSound();
                    MessageBox.Show("There is no SaveState in the selected box,\nPress 'Switch: Save/Load State' then Press 'SAVE'");
                    GlobalWin.Sound.StartSound();
                    RTC_RPC.SendToKillSwitch("UNFREEZE");
                    return;
                }


            if (bl != null)
                bl.Apply();
            else
            {
                GlobalWin.Sound.StartSound();
                return;
            }

                if (bl != null)
                    IsCorruptionApplied = true;


                if (cbStashInjected.Checked)
                {
                    RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), btnParentKeys[Convert.ToInt32(currentSelectedState)], bl);


                    DontLoadSelectedStash = true;
                    lbStashHistory.Items.Add(RTC_Core.currentStashkey);
                    lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
                    lbStockpile.ClearSelected();

                }

                if (cbRenderAtCorrupt.Checked)
                    StartRender();

                GlobalWin.Sound.StartSound();
                RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        private bool IsValidCCC()
        {
            Guid result = new Guid();
            return (tbCorruptCloudCode.Text != "" && Guid.TryParse(tbCorruptCloudCode.Text, out result));
        }

        public bool ChangeGameWarning(string rom)
        {
            if (Global.Emulator is NullEmulator || GlobalWin.MainForm.CurrentlyOpenRom.ToString().Contains("default.nes"))
                return true;

            if (rom == null)
                return false;

            string currentFilename = GlobalWin.MainForm.CurrentlyOpenRom.ToString();

            if (currentFilename.IndexOf("\\") != -1)
                currentFilename = currentFilename.Substring(currentFilename.LastIndexOf("\\") + 1);

            string btnFilename = rom;

            if (btnFilename.IndexOf("\\") != -1)
                btnFilename = btnFilename.Substring(btnFilename.LastIndexOf("\\") + 1);

            if (btnFilename != currentFilename)
            {

                string cctext =
                    "Loading this savestate will change the game\n" +
                    "\n" +
                                        "Current Rom: " + currentFilename + "\n" +
                    "Target Rom: " + rom + "\n" +
                    "\n" +
                    "Do you wish to continue ? (Yes/No)";


                if (MessageBox.Show(cctext, "Switching to another game", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }

            return true;
        }

        public void btnSendRaw_Click(object sender, EventArgs e)
        {
            string Key = RTC_Core.GetRandomKey();
            RTC_Core.SaveSave(Key + ".timejump");

            BlastLayer bl = new BlastLayer();

            foreach (var item in Global.CheatList)
            {
                string[] disassembleCheat = item.Name.Split('|');

                if (disassembleCheat[0] == "RTC Cheat")
                {
                    string _domain = disassembleCheat[1];
                    long _address = Convert.ToInt64(disassembleCheat[2]);

                    BizHawk.Client.Common.DisplayType _displayType;

                    switch (disassembleCheat[3])
                    {
                        case "Separator":
                            _displayType = BizHawk.Client.Common.DisplayType.Separator;
                            break;
                        case "Signed":
                            _displayType = BizHawk.Client.Common.DisplayType.Signed;
                            break;
                        case "Unsigned":
                            _displayType = BizHawk.Client.Common.DisplayType.Unsigned;
                            break;
                        case "Hex":
                            _displayType = BizHawk.Client.Common.DisplayType.Hex;
                            break;
                        case "Binary":
                            _displayType = BizHawk.Client.Common.DisplayType.Binary;
                            break;
                        case "FixedPoint_12_4":
                            _displayType = BizHawk.Client.Common.DisplayType.FixedPoint_12_4;
                            break;
                        case "FixedPoint_20_12":
                            _displayType = BizHawk.Client.Common.DisplayType.FixedPoint_20_12;
                            break;
                        case "FixedPoint_16_16":
                            _displayType = BizHawk.Client.Common.DisplayType.FixedPoint_16_16;
                            break;
                        case "Float":
                            _displayType = BizHawk.Client.Common.DisplayType.Float;
                            break;
                        default:
                            _displayType = BizHawk.Client.Common.DisplayType.Hex;
                            break;
                    }

                    bool _bigEndian = Convert.ToBoolean(disassembleCheat[4]);
                    int _value = Convert.ToInt32(disassembleCheat[5]);
                    bool _isEnabled = Convert.ToBoolean(disassembleCheat[6]);
                    bool _isFreeze = Convert.ToBoolean(disassembleCheat[7]);

                    bl.Layer.Add(new BlastCheat(_domain, _address, _displayType, _bigEndian, _value, _isEnabled, _isFreeze));
                }
            }

            RTC_Core.currentStashkey = new StashKey(RTC_Core.GetRandomKey(), Key, bl);

            DontLoadSelectedStash = true;
            lbStashHistory.Items.Add(RTC_Core.currentStashkey);
            lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
            lbStockpile.ClearSelected();

            RTC_Restore.SaveRestore();

        }


    }
}
