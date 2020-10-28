using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControls.Helper
{
    public class CTargetPara
    {

        /*
         * 
         *               ^Z
         *               |  
         *               | 
         *               |
         *    x          o--------->x
         *              /
         *             /
         *            /
         *            Y
         * 
         * 
         */
        //public static double BaseTarget = 142.261f;
        public static double BaseTarget = 142.261f;

        public static double BaseTargetSeparatePlus = 51.0f;

        public static double BaseTargetActualZ = BaseTarget + CLanmage.StartLevelBaseLen + Target_Tall / 2;

        public static double BaseTargetDotsZ
        {
            get { return BaseTargetActualZ / CLanmage.LengthPerDotY; }
        }

        public static double BaseTargetActualXY
        {
            get { return BaseTargetDotsXY * CLanmage.LengthPerDotX; }
        }

        public static double BaseTargetDotsXY = 480 / 2;




        public static double Target_Tall = 13.0f;

        public static double Target_Round = 2.0f;

        public double RoundL = 3.0f;

        public double RoundR = 3.0f;

        public static int offset = 0;

        public static int total = 0;

        public static void Init()
        {
            offset = 0;

            //total = GetTargets().Count;
        }

        public static double GetPercentage()
        {
            if (total > 0 && offset >= 1)
            {
                return ((offset - 1) * 100) / total;
            }
            return 100.0f;
        }

        public class XYZ
        {
            public XYZ(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public int X = 0;
            public int Y = 0;
            public int Z = 0;
        }

        private static List<XYZ> XYZE(int R, int C, int L)
        {
            int rows = R;
            int cols = C;
            int layers = L;

            List<XYZ> xes = new List<XYZ>();

            //add X
            bool flagX = true;
            for (int i = 0; i < rows * layers; i++)
            {
                flagX = !flagX;
                for (int j = 0; j < cols - 1; j++)
                {
                    xes.Add(new XYZ(((flagX == true) ? 1 : -1) * 1, 0, 0));
                }
            }


            //add Y
            List<XYZ> yess = new List<XYZ>();
            bool flagY = true;
            for (int i = 0; i < layers; i++)
            {
                flagY = !flagY;
                for (int j = 0; j < rows - 1; j++)
                {
                    yess.Add(new XYZ(0, ((flagY == true) ? 1 : -1) * 1, 0));
                }
            }

            //add Z
            List<XYZ> zess = new List<XYZ>();
            for (int i = 0; i < layers - 1; i++)
            {
                zess.Add(new XYZ(0, 0, 1));
            }

            List<XYZ> xyze = new List<XYZ>();

            int offSetX = 0;
            int offSetY = 0;
            int offSetZ = 0;


            //sort
            for (int i = 0; i < rows * cols * layers; i++)
            {
                if (i == 0)
                {
                    xyze.Add(new XYZ(0, 0, 0));
                    continue;
                }
                if (i % (rows * cols) == 0)
                {
                    xyze.Add(zess.ElementAt(offSetZ));
                    offSetZ++;
                    continue;
                }
                if (i % cols == 0)
                {
                    xyze.Add(yess.ElementAt(offSetY));
                    offSetY++;
                    continue;
                }
                xyze.Add(xes.ElementAt(offSetX));
                offSetX++;
            }

            return xyze;
        }

        //fake
        //public static List<double[]> GetTargets()
        //{
        //    List<double[]> lTargets = new List<double[]>();

        //    List<XYZ> xyzList = XYZE(
        //        CStatusQToView.PeriodDataInfo.TreatInfo.LayerEnd - CStatusQToView.PeriodDataInfo.TreatInfo.LayerStart + 1,
        //        CStatusQToView.PeriodDataInfo.Cols,
        //        CStatusQToView.PeriodDataInfo.Rows
        //        );

        //    //log4net.LogManager.GetLogger("loginfo").Info(string.Format("GetTargets():Layers:{0}", CStatusQToView.PeriodDataInfo.TreatInfo.LayerEnd - CStatusQToView.PeriodDataInfo.TreatInfo.LayerStart + 1));

        //    //log4net.LogManager.GetLogger("loginfo").Info(string.Format("GetTargets();Cols:{0}", CStatusQToView.PeriodDataInfo.Cols));

        //    //log4net.LogManager.GetLogger("loginfo").Info(string.Format("GetTargets();Rows:{0}", CStatusQToView.PeriodDataInfo.Rows));

        //    for (int i = 0; i < (CStatusQToView.PeriodDataInfo.TreatInfo.LayerEnd - CStatusQToView.PeriodDataInfo.TreatInfo.LayerStart + 1) * CStatusQToView.PeriodDataInfo.Cols * CStatusQToView.PeriodDataInfo.Rows;
        //        i++)
        //    {
        //        lTargets.Add(new double[] { (double)xyzList.ElementAt(i).X, (double)xyzList.ElementAt(i).Y, (double)xyzList.ElementAt(i).Z });
        //    }

        //    PrintLIST(xyzList);

        //    //delete 1st point
        //    //lTargets.RemoveAt(0);

        //    return lTargets;
        //}

        //public static double[] GetCurrrentTargets()
        //{
        //    if (GetTargets().Count > 0 && offset < GetTargets().Count)
        //    {
        //        double[] dl = GetTargets().ElementAt(offset);
        //        Console.WriteLine("GetCurrrentTargets::offset::" + offset.ToString());
        //        offset++;
        //        return dl;
        //    }
        //    return null;
        //}

        //private static void PrintLIST(List<XYZ> zYXes)
        //{
        //    foreach (var p in zYXes)
        //    {
        //        Console.WriteLine(string.Format("i:{0}, j:{1}, k:{2}", p.X, p.Y, p.Z));
        //    }

        //    Console.WriteLine(string.Format("offset:{0}", offset));

        //}
    }
}
