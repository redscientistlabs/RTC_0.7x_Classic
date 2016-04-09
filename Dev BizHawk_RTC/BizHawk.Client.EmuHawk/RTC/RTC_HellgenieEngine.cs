using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;

namespace RTC
{
    public static class RTC_HellgenieEngine
    {
        public static int MaxCheats = 50;


        public static BlastCheat GenerateUnit(string _domain, long _address)
        {
            try
            {
                BizHawk.Client.Common.DisplayType _displaytype;
                switch (RTC_Core.RND.Next(1, 9))
                {
                    case 1:
                        _displaytype = BizHawk.Client.Common.DisplayType.Binary;
                        break;
                    case 2:
                        _displaytype = BizHawk.Client.Common.DisplayType.FixedPoint_12_4;
                        break;
                    case 3:
                        _displaytype = BizHawk.Client.Common.DisplayType.FixedPoint_20_12;
                        break;
                    case 4:
                        _displaytype = BizHawk.Client.Common.DisplayType.Float;
                        break;
                    case 5:
                        _displaytype = BizHawk.Client.Common.DisplayType.Hex;
                        break;
                    case 6:
                        _displaytype = BizHawk.Client.Common.DisplayType.Separator;
                        break;
                    case 7:
                        _displaytype = BizHawk.Client.Common.DisplayType.Signed;
                        break;
                    case 8:
                        _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;
                        break;
                    default:
                        MessageBox.Show("Random returned an unexpected value (RTC_HellGenie switch for displaytype");
                        return null;
                }

                int biggy = RTC_Core.RND.Next(0, 2);
                bool _bigEndian;

                if (biggy == 0)
                    _bigEndian = true;
                else
                    _bigEndian = false;

                int _value = RTC_Core.RND.Next(255);

                return new BlastCheat(_domain, _address, _displaytype, _bigEndian, _value, true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Hellgenie Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }

        public static void RemoveExcessCheats()
        {
            while (Global.CheatList.Count > MaxCheats)
                Global.CheatList.Remove(Global.CheatList[0]);
        }

        public static void ClearCheats()
        {
            Global.CheatList.Clear();
        }
    }
}
