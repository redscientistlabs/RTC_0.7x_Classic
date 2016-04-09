using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace RTC
{
    public static class RTC_TimeStack
    {
        public static TimeStack ts = new TimeStack();
        public static Queue<string> ts_buffer = new Queue<string>();
        public static int TimeStackDelay = 4;
        static Timer t = null;

        public static void Start()
        {
            t = new Timer();
            TimeStackDelay = Convert.ToInt32(RTC_Core.coreForm.nmTimeStackDelay.Value);
            t.Interval = TimeStackDelay * 1000;
            t.Tick += new EventHandler(Tick);
            t.Start();
            RTC_Core.coreForm.btnTimeStackJump.ForeColor = Color.FromArgb(192, 255, 192);
        }

        public static void Stop()
        {
            if (t != null)
            {
                t.Stop();
                t = null;
            }
            RTC_Core.coreForm.btnTimeStackJump.ForeColor = Color.Silver;
        }

        public static void Reset()
        {
            ts.Clear();
            RTC_Core.tfForm.RefreshJumpLabel();
        }

        
        public static void Jump()
        {
            if (t == null)
                return;

            string key;

            if (ts.Count() > 0)
                key = ts.Pop();
            else
                return;

            RTC_TimeFlow.State = TimeState.Jump;

            if (key != null)
            {

                t.Stop();
                t.Start();

                ts_buffer.Enqueue(key);
                key = ts_buffer.Dequeue();

                Bitmap bmp = MainForm.MakeScreenshotImage().ToSysdrawingBitmap();

                for(int y = 0; y<bmp.Size.Height;y++)
                    for (int x = 0; x < bmp.Size.Width; x++)
                    {
                        Color pix = bmp.GetPixel(x,y);
                        Color pix2 = Color.FromArgb(Convert.ToInt32(pix.R*0.2),Convert.ToInt32(pix.G*0.2),Convert.ToInt32(pix.B*0.2));
                        bmp.SetPixel(x, y, pix2);
                    }

                GlobalWin.MainForm.BackgroundImageLayout = ImageLayout.Center;

                double factor = Convert.ToDouble(GlobalWin.MainForm.PresentationPanel.Control.Size.Height) / Convert.ToDouble(bmp.Height);

                Size bmpsize = new Size(Convert.ToInt32(bmp.Width * factor), Convert.ToInt32(bmp.Height * factor));

                GlobalWin.MainForm.BackColor = Color.Black;
                GlobalWin.MainForm.BackgroundImage = new Bitmap(bmp, bmpsize);


                GlobalWin.MainForm.PresentationPanel.Control.Visible = false;
                    

                if (ts_buffer.Count == 0)
                {
                    

                    RTC_RPC.SendToKillSwitch("FREEZE");
                    RTC_Core.LoadStateCorruptorSafe(key + ".timestack.timejump", null);
                    RTC_RPC.SendToKillSwitch("UNFREEZE");

                    
                    GlobalWin.MainForm.BackColor = System.Drawing.SystemColors.Control;
                    GlobalWin.MainForm.BackgroundImage = null;

                    GlobalWin.MainForm.PresentationPanel.Control.Visible = true;
                    GlobalWin.MainForm.Activate();
                    GlobalWin.MainForm.Focus();
                }

                RTC_TimeFlow.JumpStep();
                RTC_Core.tfForm.RefreshJumpLabel();

            }






        }
        

        static void Tick(object Sender, EventArgs e)
        {
            if (Global.Emulator is NullEmulator)
                return;

            string key = RTC_Core.GetRandomKey();
            RTC_Core.SaveSave(key + ".timestack.timejump");
            ts.Push(key);

            RTC_TimeFlow.AddTimeJumpBar();
            RTC_Core.tfForm.RefreshJumpLabel();
        }

        public static void SaveTimeStack()
        {

            DeleteTimeStack();

            FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();
            FS = File.Open(RTC_Core.rtcDir + "\\SESSION\\TimeStack.dat", FileMode.OpenOrCreate);
            bformatter.Serialize(FS, ts);
            FS.Close();

            

        }

        public static void LoadTimeStack()
        {
            FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();
            FS = File.Open(RTC_Core.rtcDir + "\\SESSION\\TimeStack.dat", FileMode.OpenOrCreate);
            ts = (TimeStack)bformatter.Deserialize(FS);
            FS.Close();

            Jump();
        }

        public static void DeleteTimeStack()
        {
            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\TimeStack.dat"))
                File.Delete(RTC_Core.rtcDir + "\\SESSION\\TimeStack.dat");
        }

        static void TimeStackIsEmpty(){
        }
    }

    [Serializable()]
    public class TimeStack
    {
        Stack<string> ts = new Stack<string>();

        public string Pop()
        {
            if (ts.Count > 0)
                return ts.Pop();
            else
                return null;
        }
        public void Push(string key)
        {
            ts.Push(key);
            RTC_TimeStack.SaveTimeStack();
        }
        public void Clear()
        {
            ts.Clear();
        }

        public int Count()
        {
            return ts.Count;
        }
    }
}
