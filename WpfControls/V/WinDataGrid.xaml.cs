﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfControls.VM;

namespace WpfControls.V
{
    /// <summary>
    /// WinDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class WinDataGrid : Window
    {
        public WinDataGrid()
        {
            InitializeComponent();

            int sk = Screen.AllScreens.Count();

            this.DataContext = new DataGridVM();

            this.Loaded += WinDataGrid_Loaded;
        }

        Storyboard s = null;

        private void WinDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            s = (Storyboard)TryFindResource("ani");
            s.Begin();


            int ak = 0x0102;
            byte[] arr = BitConverter.GetBytes(ak);

        }

        bool flag = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flag = !flag;
            if (flag)
                s.Begin();
            else
                s.Stop();
        }

        private void DatePickerTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.dp.IsDropDownOpen = true;
        }
    }
}
