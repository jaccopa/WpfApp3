using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControls.Helper
{
    public enum SonicMode
    {
        B,
        C,
        PW,
        M,
    }

    public enum SonicStatusOfFreeze
    {
        Freeze,
        UnFreeze,
    }

    public class CLanmage
    {
        public static double LengthPerDotX
        {
            get { return _lengthPerDotX; }
            set
            {
                if (value != _lengthPerDotX)
                {
                    _lengthPerDotX = value;
                    //CCmdQToMaster.cmdQToMaster.Enqueue(ControlUserMsg.SelfDefine_Sys_Depth_Change);
                }
            }
        }
        public static double _lengthPerDotX = 0.5f;


        public static double LengthPerDotY
        {
            get { return _lengthPerDotY; }
            set
            {
                if (value != _lengthPerDotY)
                {
                    _lengthPerDotY = value;
                    //CCmdQToMaster.cmdQToMaster.Enqueue(ControlUserMsg.SelfDefine_Sys_Depth_Change);
                }
            }
        }
        private static double _lengthPerDotY = 0.5f;

        public static double StartLevelBaseLen = 2.0f;

        public static double StartLevelBaseDots
        {
            get { return StartLevelBaseLen / LengthPerDotY; }
        }

        public static double Mi = 0.0f;

        public static double Tis = 0.0f;

        private static int _currentDepth = 15;

        public static int CurrentDepth
        {
            get
            {
                return _currentDepth;
            }

            set
            {
                if (_currentDepth != value)
                {
                    _currentDepth = value;
                    //CCmdQToMaster.cmdQToMaster.Enqueue(ControlUserMsg.SelfDefine_Sys_Depth_Change);
                    //log4net.LogManager.GetLogger("loginfo").Info("CLanmage::CurrentDepth::" + CurrentDepth.ToString());
                }
            }
        }

        public static int ProbeID = 0;

        public static string ProbeIDName = "C2";

        public static SonicMode sonicMode = SonicMode.B;

        public static float Power = 50;

        public static bool IsThi = false;

        private static int _Gn = 51;

        public static int Gn
        {
            get
            {
                return _Gn;
            }

            set
            {
                if (_Gn != value)
                {
                    _Gn = value;
                    //log4net.LogManager.GetLogger("loginfo").Info("CLanmage::Gn::" + _Gn.ToString());
                }
            }
        }

        public static double Freq = 3.6;

        public static int FPS = 20;

        public static int FPSFreeze = 20;

        public static DateTime DateTimeFreeze = DateTime.Now;

        public static double FocusPos = 1.0f;

        public static bool DB = false;
        public static int DBCount = 0;

        //public static bool Freeze = false;

        private static SonicStatusOfFreeze sonicStatusOfFreeze = SonicStatusOfFreeze.UnFreeze;

        public static SonicStatusOfFreeze SonicStatusOfFreeze
        {
            get { return sonicStatusOfFreeze; }
            set
            {
                if (sonicStatusOfFreeze != value)
                {
                    sonicStatusOfFreeze = value;
                    //CCmdQToMaster.cmdQToMaster.Enqueue(ControlUserMsg.SelfDefine_Keyboard_Btn_Freeze);
                    if (sonicStatusOfFreeze == SonicStatusOfFreeze.Freeze)
                    {
                        FPSFreeze = FPS;
                        DateTimeFreeze = DateTime.Now;
                    }
                }
            }
        }
    }
}
