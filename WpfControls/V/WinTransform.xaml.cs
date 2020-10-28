using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WPFNotification;

namespace WpfControls.V
{
    /// <summary>
    /// WinTransform.xaml 的交互逻辑
    /// </summary>
    public partial class WinTransform : Window
    {
        public WinTransform()
        {
            InitializeComponent();

            this.Loaded += WinTransform_Loaded;
        }

        private void WinTransform_Loaded(object sender, RoutedEventArgs e)
        {

            Thread thread = new Thread(DialogTask);
            thread.Start();
            //Task.Run
            //(
            //    ()=> 
            //    {
            //    for (; ; )
            //    {
            //        Dispatcher.Invoke(() => {

            //            WinDialog.ShowDialog2("tip", "abc");
            //        });

            //            Thread.Sleep(100);
            //    }
            //});
        }

        private void DialogTask()
        {
            for (; ; )
            {
                App.Current.Dispatcher?.Invoke(() =>
                {

                    WinDialog.ShowDialog2("tip", "abc");
                });

                Thread.Sleep(100);
            }
        }
    }
}
