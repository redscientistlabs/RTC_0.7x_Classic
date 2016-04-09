using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using System.Windows.Forms;
using System.Threading;

namespace RTC
{

    public static class RTC_MemoryZones
    {
        //To be refactored in RTC_MemoryZones
        public static Hashtable MemoryDomains = new Hashtable();
        public static List<string> SelectedDomains = new List<string>();
        public static List<string> pendingSelectedDomains = null;

        public static List<string> getSelectedDomains()
        {
            return SelectedDomains;
        }

        public static void setSelectedDomains(List<string> _domains)
        {
            RefreshDomains();

            if (_domains == null || _domains.Count == 0)
                return;

            foreach (string _domain in _domains)
            {
                for(int i = 0; i< RTC_Core.coreForm.lbMemoryZones.Items.Count;i++)
                    if ((string)RTC_Core.coreForm.lbMemoryZones.Items[i] == _domain)
                    {
                        RTC_Core.coreForm.lbMemoryZones.SetSelected(i, true);
                        break;
                    }
            }
        }

        public static void SelectDomains()
        {
            if (RTC_Core.coreForm != null)
            {
                SelectedDomains.Clear();
                foreach (object item in RTC_Core.coreForm.lbMemoryZones.SelectedItems)
                    SelectedDomains.Add((string)item);
            }
        }

        public static void AutoSelectDomains()
        {
            // Auto Select the Zones that are time-safe
            // Time-safe means that the rewinder can rewind past before
            // game-crashing and can reprocess these frames differently 

            RefreshDomains();
            string thisSystem = Global.Game.System.ToString().ToUpper();
            List<string> ChipBlacklist = new List<string>();

            switch (thisSystem)
            {

                case "NES":     //Nintendo Entertainment system

                    ChipBlacklist.Add("System Bus");
                    ChipBlacklist.Add("PRG ROM");
                    ChipBlacklist.Add("CHR VROM"); //Cartridge
                    ChipBlacklist.Add("Battery RAM"); //Cartridge Save Data
                    break;


                case "GB":      //Gameboy
                case "GBC":     //Gameboy Color
                    ChipBlacklist.Add("ROM"); //Cartridge
                    ChipBlacklist.Add("System Bus");
                    break;

                case "SNES":    //Super Nintendo

                    ChipBlacklist.Add("CARTROM"); //Cartridge
                    ChipBlacklist.Add("CARTRAM"); //Cartridge Save data
                    ChipBlacklist.Add("APURAM"); //SPC700 memory
                    ChipBlacklist.Add("CGRAM"); //Color Memory (Useless and disgusting)
                    break;

                case "N64":     //Nintendo 64
                    ChipBlacklist.Add("System Bus");
                    ChipBlacklist.Add("PI Register");
                    ChipBlacklist.Add("EEPROM");
                    ChipBlacklist.Add("ROM");
                    ChipBlacklist.Add("SI Register");
                    break;

                case "PCE":     //PC Engine / Turbo Grafx
                    ChipBlacklist.Add("ROM");
                    break;


                case "GBA":     //Gameboy Advance
                    ChipBlacklist.Add("OAM");
                    ChipBlacklist.Add("BIOS");
                    ChipBlacklist.Add("PALRAM");
                    ChipBlacklist.Add("ROM");
                    break;

                case "SG":      //Sega SG-1000
                    //everything okay
                    break;

                case "SMS":     //Sega Master System
                    ChipBlacklist.Add("System Bus"); // the game cartridge appears to be on the system bus
                    break;

                case "GG":      //Sega GameGear
                    //everything okay
                    break;

                case "GEN":     //Sega Genesis and CD
                    ChipBlacklist.Add("MD CART");
                    break;


                case "PSX":     //Sony Playstation 1
                    ChipBlacklist.Add("MainRAM");
                    ChipBlacklist.Add("BiosROM");
                    ChipBlacklist.Add("PIOMem");
                    break;

                case "A26":     //Atari 2600
                    break;

                case "A78":     //Atari 7800
                    ChipBlacklist.Add("BIOS ROM");
                    ChipBlacklist.Add("HSC ROM");
                    break;

                case "LYNX":    //Atari Lynx
                    ChipBlacklist.Add("Save RAM");
                    ChipBlacklist.Add("Cart B");
                    ChipBlacklist.Add("Cart A");
                    break;


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
                    MessageBox.Show("WARNING: The selected system appears to be supported by Bizhawk Emulator.\n " +
                    "However, no corruption Template is available yet for this system.\n " +
                    "You'll have to manually select the Memory Zones to corrupt.");
                    break;

                    //TODO: Add more zones like gamegear, atari, turbo graphx
            }


            // Setting the Listbox
            if (RTC_Core.coreForm != null)
            {
                for (int i = 0; i < RTC_Core.coreForm.lbMemoryZones.Items.Count; i++)
                {
                    object OneItem = RTC_Core.coreForm.lbMemoryZones.Items[i];
                    bool Stringfound = false;

                    foreach (string OneString in ChipBlacklist)
                        if (OneItem.ToString() == OneString)
                            Stringfound = true;

                    if (!Stringfound)
                        RTC_Core.coreForm.lbMemoryZones.SetSelected(i, true);
                }
            }


        }

        public static void RefreshAndKeepDomains()
        {
            List<string> MemoryBanks = new List<string>(); //Saves current targetted zones
            foreach (object oneItem in RTC_MemoryZones.SelectedDomains)
                MemoryBanks.Add(oneItem.ToString());

            RTC_MemoryZones.RefreshDomains(); //refresh and reload zones

            int nbZones = RTC_Core.coreForm.lbMemoryZones.Items.Count;

            for (int i = 0; i < nbZones; i++)
                foreach (string SelectedItem in MemoryBanks)
                    if (RTC_Core.coreForm.lbMemoryZones.Items[i].ToString() == SelectedItem)
                    {
                        RTC_Core.coreForm.lbMemoryZones.SetSelected(i, true);
                        break;
                    }
        }


        public static void RefreshDomains()
        {

            MemoryDomains.Clear();
            SelectedDomains.Clear();

            if (Global.Emulator is NullEmulator)
                return;

            int maxtries = 0;

            while (RTC_Core.hexeditor.MemoryDomains == null && maxtries < 20)
            {
                Thread.Sleep(300);
                maxtries++;
            }

            if(maxtries >= 20)
            {
                MessageBox.Show("Something horrible happenned while RTC tried to query the Memory Zones from Bizhawk. You may need to refresh the zones in a few seconds or even restart the whole thing.");
                return;
            }

            foreach (MemoryDomain _domain in RTC_Core.hexeditor.MemoryDomains)
                MemoryDomains.Add(_domain.ToString(), _domain);

            if (RTC_Core.coreForm != null)
            {
                RTC_Core.coreForm.lbMemoryZones.Items.Clear();
                foreach (object item in RTC_MemoryZones.MemoryDomains.Values)
                    RTC_Core.coreForm.lbMemoryZones.Items.Add(item.ToString());
            }
        }

        public static void Clear()
        {
            MemoryDomains.Clear();
            SelectedDomains.Clear();
            RTC_Core.coreForm.lbMemoryZones.Items.Clear();

        }

        public static MemoryDomain getDomain(string _domain)
        {

            MemoryDomain md;
            //Tries to query a domain object from the memorydomain pool using a string
            try { md = (MemoryDomain)MemoryDomains[_domain]; }
            catch { return null; } //domain wasn't found in the current loaded domains

            return md;
        }

    }
}
