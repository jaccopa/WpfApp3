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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControls.UCs
{
    /// <summary>
    /// NotificationItem.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationItem : UserControl
    {
        public NotificationItem()
        {
            InitializeComponent();
        }

        string _ttitle; 
        string _tcontent;

        int _lifetime = 1;

        public NotificationItem(string ttitle,string tcontent,int lifetime)
        {
            InitializeComponent();

            _ttitle     = ttitle;

            _tcontent   = tcontent;

            _lifetime   = lifetime;

            this.Loaded += NotificationItem_Loaded;
        }

        private void NotificationItem_Loaded(object sender, RoutedEventArgs e)
        {
            //this.xtitle.Content = _ttitle;
            //this.xct.Content = _tcontent;

            //Task.Run
            //(()=> {

            //    Thread.Sleep(_lifetime);

            //    Dispatcher?.Invoke(()=> {
            //        Window parentWindow = Window.GetWindow(this);
            //        this.Visibility     = Visibility.Hidden;
            //        parentWindow.Close();
            //    });

            //});
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.Visibility = Visibility.Hidden;
            parentWindow.Close();
        }
    }
}
