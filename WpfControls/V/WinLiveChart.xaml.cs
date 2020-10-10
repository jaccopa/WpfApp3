using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfControls.VM;

namespace WpfControls.V
{
    /// <summary>
    /// WinLiveChart.xaml 的交互逻辑
    /// </summary>
    public partial class WinLiveChart : Window
    {
        public WinLiveChart()
        {
            InitializeComponent();

            byte[] ak = BitConverter.GetBytes(0x1234);

            this.Loaded += WinLiveChart_Loaded;
        }

        private void WinLiveChart_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LvcVM();
        }
    }
}
