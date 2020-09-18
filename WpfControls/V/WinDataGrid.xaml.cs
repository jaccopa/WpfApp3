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

            this.DataContext = new DataGridVM();

            this.Loaded += WinDataGrid_Loaded;
        }

        Storyboard s = null;

        private void WinDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            s = (Storyboard)TryFindResource("ani");
            s.Begin();
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
    }
}
