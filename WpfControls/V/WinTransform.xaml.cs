﻿using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using WpfControls.Helper;
using WpfControls.UCs;
using WPFNotification;
using WPFNotification.Core.Configuration;
using WPFNotification.Model;
using WPFNotification.Services;

namespace WpfControls.V
{
    public enum ArmModeStatus
    {
        MAuto,
        MKeyboard,
        MJStk,
    }

    public class ArmStatus
    {
        public static ArmModeStatus armModeStatus = ArmModeStatus.MAuto;

        private static List<DateTime> DateTimeList = new List<DateTime>(3) { DateTime.Now, DateTime.Now, DateTime.Now };

        //public static bool Running { get { return !CControlCenter.RobotIsVacant; } }

        public static bool IsCheckOK(ArmModeStatus armModeStatus, int tm)
        {
            DateTime dateTime = DateTime.Now;
            TimeSpan ts = dateTime - DateTimeList[(int)armModeStatus];
            if (ts.TotalSeconds >= tm)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Update(ArmModeStatus armModeStatus)
        {
            DateTimeList[(int)armModeStatus] = DateTime.Now;
        }
    }

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
            List<DateTime> DateTimeList = new List<DateTime>(3) { DateTime.Now, DateTime.Now, DateTime.Now };
            //Console.Read();

            Thread.Sleep(2 * 1000);

            ArmStatus.Update(ArmModeStatus.MAuto);

            Thread.Sleep(2 * 1000);

            ArmStatus.Update(ArmModeStatus.MJStk);

            bool ret = ArmStatus.IsCheckOK(ArmModeStatus.MAuto,2);

            //InvokeTest();

            //Thread thread = new Thread(ImageArrayCmp);
            //thread.Start();

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

        private void InvokeTest()
        {
            Thread.Sleep(1000);

            WinDialog.ShowDialog2("abc","123");
        }

        private void ImageArrayCmp()
        {
            Bitmap bitmap = new Bitmap("blue3.jpg");
            byte[] vs1 = ImageHelper.Instance.ImageToArrayFast(bitmap);
            byte[] vs2 = ImageHelper.Instance.ImageToArray(bitmap);

            var s1 = vs1.ToString();
            var s2 = vs2.ToString();

            int k = string.Compare(s1,s2);
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
                App.Current?.Dispatcher?.Invoke(() =>
                {
                    _dailogService.ShowNotificationWindow(newNotification, configuration);
                });

                Thread.Sleep(4 * 1000);
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
