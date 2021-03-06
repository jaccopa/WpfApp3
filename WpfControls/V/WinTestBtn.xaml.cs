﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfControls.VM;

namespace WpfControls
{
    /// <summary>
    /// WinTestBtn.xaml 的交互逻辑
    /// </summary>
    public partial class WinTestBtn : Window
    {
        public WinTestBtn()
        {
            InitializeComponent();

            DataContext = new TstVM();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        //https://www.cnblogs.com/hyunbar/p/10083532.html
            //Regex re = new Regex("[^0-9.\\-]+");
            Regex re = new Regex("^(([1-9]*)(0*)(\\.\\d{2}))|(0(\\.\\d{2}))$");

            if (txtregmatch.Text.StartsWith("0") && e.Text == "0")
            {
                txtregmatch.Text.Remove(0,1);
                txtregmatch.SelectionStart = txtregmatch.Text.Length;

                e.Handled = true;
                return;
            }

            if (txtregmatch.Text.Contains(".") && e.Text == ".")
            {
                e.Handled = true;
                return;
            }

            e.Handled = re.IsMatch(txtregmatch.Text);

            //else
            //{
            //    App.Current?.Dispatcher.Invoke(
            //    () =>
            //    {

            //        txtregmatch.Clear();
            //    }

            //    );
            //}

            //e.Handled = 

            //    //&& re.IsMatch(e.Text);


            //Console.WriteLine(txtregmatch.Text);

            //if (e.Handled == false)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        App.Current?.Dispatcher.Invoke(
            //            () =>
            //            {

            //                txtregmatch.Clear();
            //            }

            //            );
            //    });

            //}

        }

        private void txtregmatch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (!char.IsDigit(e.Key.))
            //{
            //    e.Handled = true;
            //}
        }

        private void txtregmatch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp = "";
            bool ret = false;
            foreach (var p in txtregmatch.Text)
            {
                if (char.IsDigit(p) || p == '.')
                {
                    tmp += p;
                    continue;
                }
                else
                {
                    ret = true;
                    break;
                }
            }
            if (ret)
            {
                txtregmatch.Text = tmp;
                txtregmatch.SelectionStart = txtregmatch.Text.Length;

            }
        }

        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            DatePicker datepicker = (DatePicker)sender;
            Popup popup = (Popup)datepicker.Template.FindName("PART_Popup", datepicker);
            System.Windows.Controls.Calendar cal = (System.Windows.Controls.Calendar)popup.Child;
            cal.DisplayMode = System.Windows.Controls.CalendarMode.Decade;
        }
    }
}
