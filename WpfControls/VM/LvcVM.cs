using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.Wpf;

namespace WpfControls.VM
{
    public class LvcVM: ViewModelBase
    {
        public LvcVM()
        {
            Load();
        }


        LineSeries Temp = null;

        public void Load()
        {
            Temp = null;

            Temp = new LineSeries
            {
                //Title = "Delta ",
                Values = new ChartValues<double>(),
                ScalesYAt = 0,
                //Foreground = Brushes.Red,
                PointGeometrySize = 1.0f,
                DataLabels = false,
                //AreaLimit = 10.0f,
                Stroke = new SolidColorBrush(Colors.Red),
                LineSmoothness = 0,
            };

            SeriesCollection.Add(Temp);

            double k = 0.0f;

            DateTime dateTimeStart = DateTime.Now;

            DateTime dateTimeNow = DateTime.Now;

            Task.Factory.StartNew(
             ()=> 
             {
                for (int i = 0; i <= 100; i++)
                {
                    Labels.Add(i.ToString());
                }

                while (k <= 100)
                {

                     dateTimeNow = DateTime.Now;

                     Add((dateTimeNow - dateTimeStart).TotalSeconds,k);

                     k += 1.0f;

                     Thread.Sleep(1000);
                }
            });
        }

        public void Add(double x,double y)
        {
            //Labels.Add(x.ToString());
            Console.WriteLine(x.ToString()+" s");

            Temp.Values.Add(x);
        }

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();

        public List<string> Labels { get; set; } = new List<string>();


    }
}
