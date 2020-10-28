using CommonServiceLocator;
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
using WpfControls.UCs;
using WPFNotification;
using WPFNotification.Core.Configuration;
using WPFNotification.Model;
using WPFNotification.Services;

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


        private readonly INotificationDialogService _dailogService = new  NotificationDialogService();

        private void WinTransform_Loaded(object sender, RoutedEventArgs e)
        {

            Thread thread = new Thread(MyToastTask);
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

        private void ToastTask()
        {
            var newNotification = new Notification()
            {
                //ImgURL = "Resources\\blue3.jpg",
                Title = "Machine error",
                Message = "Error!! Please check your Machine Code and Try Again"
            };

            for (; ; )
            {
                App.Current.Dispatcher?.Invoke(() =>
                {
                    _dailogService.ShowNotificationWindow(newNotification);
                });

                Thread.Sleep(5 * 1000);
            }
        }


        private void MyToastTask()
        {
            var newNotification = new MailNotification()
            {
                Title = "Vacation Request",
                Sender = "Mohamed Magdy",
                Content = "I would like to request for vacation from 20 / 12 / 2015 to 30 / 12 / 2015............."
            };

            var configuration = new NotificationConfiguration(new TimeSpan(0,0,5), null,
                    null, "MailNotificationTemplate",NotificationFlowDirection.RightBottom);

            for (; ; )
            {
                App.Current.Dispatcher?.Invoke(() =>
                {
                    _dailogService.ShowNotificationWindow(newNotification, configuration);
                });

                Thread.Sleep(5 * 1000);
            }
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
