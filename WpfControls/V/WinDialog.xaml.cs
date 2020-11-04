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

namespace WpfControls.V
{
    /// <summary>
    /// WinDialog.xaml 的交互逻辑
    /// </summary>
    public partial class WinDialog : Window
    {
        public WinDialog()
        {
            InitializeComponent();

            this.Closed += WinDialog_Closed;

            this.Activated += WinDialog_Activated;
        }

        public static bool? ShowDialog2(string title, string content, string btn1Content = "确定", string btn2Content = "取消")
        {
            WinDialog win = new WinDialog(title, content, btn1Content, btn2Content);
            win.Owner = Application.Current.MainWindow;
            win.ShowDialog();
            return win.DialogResult;
        }

        public static bool? ShowDialogRightPos(string title, string content, string btn1Content = "确定", string btn2Content = "取消")
        {
            WinDialog win = new WinDialog(title, content, btn1Content, btn2Content);
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.Manual;
            win.Left = 1400;
            win.Top = 400;
            win.ShowDialog();
            return win.DialogResult;
        }

        public WinDialog(string title, string content, string btn1Content = "确定", string btn2Content = "取消") : this()
        {

            lblTitle.Content = title;
            lblContent.Text = content;
            btnOK.Content = btn1Content;
            btnCancel.Content = btn2Content;
        }

        private void WinDialog_Activated(object sender, EventArgs e)
        {
            Task.Run(
                () => {
                    Thread.Sleep(100);
                    Dispatcher.Invoke(
                    ()=>
                    {
                        //this.btnOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, btnOK));
                    }
                    );
                }
                
                );
            
        }

        private void WinDialog_Closed(object sender, EventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
