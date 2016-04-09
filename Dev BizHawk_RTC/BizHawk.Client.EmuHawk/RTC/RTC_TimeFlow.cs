using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BizHawk.Client.EmuHawk;
using System.Threading;

namespace RTC
{
    public enum TimeState
    {
        Forward,
        FastForward,
        Rewind,
        Jump,
        Paused,
        Paradox
    }

    public static class RTC_TimeFlow
    {
        public static volatile Bitmap FullMap = null;

        public static Image imgTimeFlowJump = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\jump.png");
        public static Image imgTimeFlowRewind = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\rewind.png");
        public static Image imgTimeFlowForward = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\forward.png");
        public static Image imgTimeFlowFastForward = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\fastforward.png");
        public static Image imgTimeFlowParadox = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\paradox.png");
        public static Image imgTimeFlowPaused = Bitmap.FromFile(RTC_Core.rtcDir + "\\ASSETS\\paused.png");

        public static Stack<int> TimeFlowGame;
        public static Stack<int> UniverseFlow;
        public static Stack<int> UniverseLayer;

        public static int LastTimeGame = 0;
        public static int timeGame = 0;

        public static int timeParadox = -1;
        public static bool inParadox = false;

        public static int Universe = 1;
        public static int TotalUniverse = 1;

        public static volatile bool Running = false;

        static TimeState _state = TimeState.Paused;
        public static TimeState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    switch (value)
                    {
                        case TimeState.Forward:
                            RTC_Core.tfForm.pbTimeFlow.Image = imgTimeFlowForward;
                            break;
                        case TimeState.FastForward:
                            RTC_Core.tfForm.pbTimeFlow.Image = imgTimeFlowFastForward;
                            break;
                        case TimeState.Rewind:
                            RTC_Core.tfForm.pbTimeFlow.Image = imgTimeFlowRewind;
                            break;
                        case TimeState.Jump:
                            RTC_Core.tfForm.pbTimeFlow.Image = imgTimeFlowJump;
                            RTC_Core.tfForm.pbTimeFlow.Refresh();

                            break;
                        case TimeState.Paradox:
                            RTC_Core.tfForm.pbTimeFlow.Image = imgTimeFlowParadox;
                            break;
                        case TimeState.Paused:
                            RTC_Core.tfForm.pbTimeFlow.Image = imgTimeFlowPaused;
                            break;
                    }

                    _state = value;
                }
            }
        }

        public static TimeState LastState = State;

        public static Color RewindColor = Color.SteelBlue;
        public static Color ForwardColor = Color.Lime;
        public static Color FastForwardColor = Color.Orange;
        public static Color JumpColor = Color.Red;
        public static Color ParadoxColor = Color.Gray;

        public static int getLoopUniverse(int thisUniverse)
        {
            return thisUniverse % (RTC_Core.tfForm.pbFullMap.Height);
        }

        public static Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight)
        {

            Rectangle rect;

            if (cropX + cropWidth > bitmap.Width)
                rect = new Rectangle(bitmap.Width-cropWidth, cropY, cropWidth, cropHeight);
            else
                rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);

            return cropped;
        }

        private static void initFullMap()
        {
            RTC_RPC.SendToKillSwitch("FREEZE");
            GlobalWin.Sound.StopSound();

            new Thread(() =>
                {
                    Bitmap newMap = new Bitmap(204800, RTC_Core.tfForm.pbFullMap.Height);

                    
                    for (int x = 0; x < newMap.Width; x++)
                        if (x % 30 == 0)
                            for (int y = 0; y < newMap.Height; y++)
                                SafeSetPixel(newMap, x, y, Color.FromArgb(32, 32, 32));
                    

                    FullMap = newMap;

                    Running = true;
                }
            ).Start();


            timeGame = 0;
            LastTimeGame = 0;
            State = TimeState.Forward;

            TimeFlowGame.Clear();
            UniverseFlow.Clear();
            UniverseLayer.Clear();

            GlobalWin.Sound.StartSound();
            RTC_RPC.SendToKillSwitch("UNFREEZE");
        }

        public static void DoRewindBranch()
        {
            int LastUniverse = Universe;

            if (UniverseLayer.Count > 0)
                Universe = UniverseLayer.Pop();

            int diffUniverse = LastUniverse - Universe;


            for (int i = 0; i < diffUniverse; i++)
            {
                SafeSetPixel(UniverseFlow.Peek(), getLoopUniverse((Universe + i) * 5), RewindColor);
                SafeSetPixel(UniverseFlow.Peek(), getLoopUniverse((Universe + i) * 5 + 1), RewindColor);
                SafeSetPixel(UniverseFlow.Peek(), getLoopUniverse((Universe + i) * 5 + 2), RewindColor);
                SafeSetPixel(UniverseFlow.Peek(), getLoopUniverse((Universe + i) * 5 + 3), RewindColor);
                SafeSetPixel(UniverseFlow.Peek(), getLoopUniverse((Universe + i) * 5 + 4), RewindColor);
                SafeSetPixel(UniverseFlow.Peek(), getLoopUniverse((Universe + i) * 5 + 5), RewindColor);

            }

            if (UniverseFlow.Count > 0)
            UniverseFlow.Pop();
        }

        public static void DoForwardBranch()
        {

            int diffUniverse = TotalUniverse - Universe;
            diffUniverse++;

            for (int i = 0; i < diffUniverse; i++)
            {
                SafeSetPixel(timeGame, getLoopUniverse((Universe + i) * 5), Color.Lime);
                SafeSetPixel(timeGame, getLoopUniverse(((Universe + i) * 5) + 1), Color.Lime);
                SafeSetPixel(timeGame, getLoopUniverse(((Universe + i) * 5) + 2), Color.Lime);
                SafeSetPixel(timeGame, getLoopUniverse(((Universe + i) * 5) + 3), Color.Lime);
                SafeSetPixel(timeGame, getLoopUniverse(((Universe + i) * 5) + 4), Color.Lime);
                SafeSetPixel(timeGame, getLoopUniverse(((Universe + i) * 5) + 5), Color.Lime);
            }

            UniverseFlow.Push(timeGame); // pushing a Timestamp of timeGame for when the rewind will have to go up.
            UniverseLayer.Push(Universe);

            TotalUniverse++;
            Universe = TotalUniverse;

        }

        public static void PausedStep()
        {
            State = TimeState.Paused;

            if (!Running)
                return;

            LastState = State;
        }

        public static void ForwardStep()
        {
            State = TimeState.Forward;

            if (!Running)
                return;

            inParadox = false;

            timeGame++;

            switch (LastState)
            {
                case TimeState.Rewind:
                    DoForwardBranch();
                    break;
                case TimeState.Paradox:
                    timeGame = 0;
                    TotalUniverse++;
                    Universe = TotalUniverse;
                    break;
            }


            SafeSetPixel(timeGame, getLoopUniverse(Universe * 5), ForwardColor);

            LastState = State;
        }

        public static void RewindStep()
        {
            State = TimeState.Rewind;

            if (!Running)
                return;

            timeGame -= 2;

            if (timeGame < 0)
                timeGame = 0;

            if (timeGame < timeParadox)
            {
                timeParadox = 0;
                inParadox = true;
            }

            if (inParadox)
            {
                SafeSetPixel(timeGame + 2, getLoopUniverse(Universe * 5), ParadoxColor);
                //SafeSetPixel(timeGame + 1, getLoopUniverse(Universe * 5), ParadoxColor);
                State = TimeState.Paradox;
            }
            else
            {
                SafeSetPixel(timeGame + 2, getLoopUniverse(Universe * 5), RewindColor);
                SafeSetPixel(timeGame + 1, getLoopUniverse(Universe * 5), RewindColor);
            }


            if (UniverseFlow.Count > 0 && (timeGame <= UniverseFlow.Peek())) // RollBack 1 Universe
                DoRewindBranch();


            LastState = State;
        }

        public static void FastForwardStep()
        {
            State = TimeState.FastForward;

            if (!Running)
                return;

            timeGame++;

            SafeSetPixel(timeGame, getLoopUniverse(Universe * 5), FastForwardColor);

            LastState = State;
        }

        public static void JumpStep()
        {
            State = TimeState.Jump;

            if (!Running)
                return;

            int beforeTimeGame = timeGame;
            int timeGameStamp;

            if (TimeFlowGame.Count > 0)
            {
                timeGameStamp = TimeFlowGame.Peek();
                timeGame = TimeFlowGame.Pop();
            }
            else
                timeGameStamp = 0;

            if (beforeTimeGame > timeGameStamp)
            {
                for (int i = beforeTimeGame; i >= timeGameStamp; i--)
                {
                    SafeSetPixel(i, getLoopUniverse(Universe * 5), Color.Red);
                }
            }

            if (beforeTimeGame < timeGameStamp)
            {
                for (int i = timeGameStamp; i >= beforeTimeGame; i--)
                {
                    SafeSetPixel(i, getLoopUniverse(Universe * 5), Color.Red);
                }
            }

            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) + 1), Color.Red);
            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) + 2), Color.Red);
            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) + 3), Color.Red);
            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) + 4), Color.Red);
            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) + 5), Color.Red);


            timeParadox = timeGame;

            Universe++;
            TotalUniverse++;


            LastState = State;

            RTC_Core.tfForm.RefreshMap(null, null);
            RTC_Core.tfForm.pbFullMap.Refresh();
        }

        public static void AddTimeJumpBar()
        {
            if (!Running)
                return;

            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) - 1), Color.Red);
            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5)), Color.Red);
            SafeSetPixel(timeGame, getLoopUniverse((Universe * 5) + 1), Color.Red);

            TimeFlowGame.Push(timeGame);

        }


        public static void Start()
        {

            TimeFlowGame = new Stack<int>();
            UniverseFlow = new Stack<int>();
            UniverseLayer = new Stack<int>();

            initFullMap();
            RTC_Core.tfForm.Show();


        }

        public static void Stop()
        {
            Running = false;
            RTC_Core.tfForm.Hide();
            
        }

        public static void SafeSetPixel(int x, int y, Color color)
        {
            SafeSetPixel(FullMap, x, y, color);
        }

        public static void SafeSetPixel(Bitmap bmp, int x, int y, Color color)
        {

            if (x < 0 || x >= bmp.Width)
                return;

            bool done = false;
            while (!done)
            {
                try
                {
                    bmp.SetPixel(x, y, color);
                    done = true;
                }
                catch
                {
                }
            }
        }


    }

}
