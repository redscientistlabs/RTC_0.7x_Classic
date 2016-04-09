using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;

namespace RTC
{


    public static class RTC_RPC
    {
        private static string ip = "127.0.0.1";
        private static int pluginPortOUT = 56664;
        private static int pluginPortIN = 56665;
        private static int bridgePortIN = 56666;
        private static int killswitchPortOUT = 56667;
        private static System.Windows.Forms.Timer time;
        private static Thread bridgeThread;
        private static Thread pluginThread;
        private static volatile Queue<String> messages = new Queue<string>();
        private static UdpClient pluginSender = new UdpClient(ip, pluginPortOUT);
        private static UdpClient killswitchSender = new UdpClient(ip, killswitchPortOUT);
        private static volatile bool Running = false;

        public static void Start()
        {
            Running = true;

            time = new System.Windows.Forms.Timer();
            time.Interval = 200;
            time.Tick += new EventHandler(CheckMessages);
            time.Start();

            bridgeThread = new Thread(new ThreadStart(ListenToBridge));
            bridgeThread.IsBackground = true;
            bridgeThread.Start();

            pluginThread = new Thread(new ThreadStart(ListenToPlugin));
            pluginThread.IsBackground = true;
            pluginThread.Start();

            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        public static void Stop()
        {
            Running = false;
        }

        private static void ListenToBridge()
        {
            bool done = false;

            UdpClient Listener = new UdpClient(bridgePortIN);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(ip), bridgePortIN);

            try
            {
                while (!done)
                {
                    if (!Running)
                        break;

                    byte[] bytes = Listener.Receive(ref groupEP);

                    messages.Enqueue(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Listener.Close();
            }
        }
        private static void ListenToPlugin()
        {
            bool done = false;

            UdpClient Listener = new UdpClient(pluginPortIN);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(ip), pluginPortIN);

            try
            {
                while (!done)
                {
                    if (!Running)
                        break;

                    byte[] bytes = Listener.Receive(ref groupEP);

                    messages.Enqueue(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Listener.Close();
            }
        }

        public static void CheckMessages(object sender, EventArgs e)
        {
            string msg = "";
            while (messages.Count != 0)
            {
                msg = messages.Dequeue();
                string[] splits = msg.Split('|');

                switch (splits[0])
                {
                    default:
                        break;

                    case "RTC":
                        switch (splits[1])
                        {
                            default:
                                break;

                            case "CORRUPT":
                                RTC_Core.ghForm.btnCorrupt_Click(new object(), new EventArgs());
                                break;
                            case "ASK_PLUGIN_SET":
                                if (GlobalWin.MainForm.CurrentlyOpenRom != null)
                                    RefreshPlugin();
                                break;
                        }
                    break;

                }
            }


            SendHeartbeat();
        }

        public static void RefreshPlugin()
        {
            SendToPlugin("RTC_Plugin|SET|" + GlobalWin.MainForm.CurrentlyOpenRom + "|" + RTC_Core.bizhawkDir + "\\CorruptedROM.rom" + "|" + RTC_Core.bizhawkDir + "\\ExternalCorrupt.exe");
        }
        public static void ClosePlugin()
        {
            SendToPlugin("RTC_Plugin|CLOSE");
        }
        public static void SendToPlugin(string msg)
        {
            if (!Running)
                return;

            Byte[] sdata = Encoding.ASCII.GetBytes(msg);
            pluginSender.Send(sdata, sdata.Length);
        }
        public static void SendHeartbeat()
        {
            SendToKillSwitch("TICK");
        }
        public static void SendToKillSwitch(string extra)
        {
            if (!Running)
                return;

            string message = "RTC_Heartbeat";

            if (extra != null)
                message += "|" + extra;

            Byte[] sdata = Encoding.ASCII.GetBytes(message);
            killswitchSender.Send(sdata, sdata.Length);
        }
    }
}
