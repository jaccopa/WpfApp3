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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var adornerLayer = AdornerLayer.GetAdornerLayer(btn);
            //adornerLayer.Add(new SimpleCircleAdorner(btn));

            this.inkcan.IsEnabled =false;
        }

        private void mythumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;

            //Dispatcher.Invoke(()=> {

            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange );

            //});
            Canvas.SetLeft(rect, Canvas.GetLeft(thumb) + e.HorizontalChange + 10);
            Canvas.SetTop(rect, Canvas.GetTop(thumb) + e.VerticalChange + 10);


            rect.Width = Math.Abs(Canvas.GetLeft(mythumb3) - Canvas.GetLeft(mythumb1));

            rect.Height = Math.Abs(Canvas.GetTop(mythumb3) - Canvas.GetTop(mythumb1));

            Console.WriteLine(e.HorizontalChange.ToString() + " "+ e.VerticalChange.ToString());

        }

        private void mythumb_DragDelta2(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;

            //Dispatcher.Invoke(()=> {

            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);

            //});


            Console.WriteLine(e.HorizontalChange.ToString() + " " + e.VerticalChange.ToString());

        }

        private void mythumb_DragDelta3(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;

            //Dispatcher.Invoke(()=> {

            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);

            //});
            //Canvas.SetRight(rect, Canvas.GetLeft(thumb) + e.HorizontalChange);

            //Canvas.SetBottom(rect, Canvas.GetLeft(thumb) + e.HorizontalChange);


            rect.Width = Math.Abs(Canvas.GetLeft(mythumb3) - Canvas.GetLeft(mythumb1));

            rect.Height = Math.Abs(Canvas.GetTop(mythumb3) - Canvas.GetTop(mythumb1));


            Console.WriteLine(e.HorizontalChange.ToString() + " " + e.VerticalChange.ToString());

        }

        private void mythumb_DragDelta4(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;

            //Dispatcher.Invoke(()=> {

            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);

            //});


            Console.WriteLine(e.HorizontalChange.ToString() + " " + e.VerticalChange.ToString());

        }

        private void mythumb_DragOver(object sender, DragEventArgs e)
        {
           

        }

        private void mythumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Console.WriteLine("drag over...");
            double StartXCurrent = Canvas.GetLeft(mythumb1);
            double StartYCurrent = Canvas.GetLeft(mythumb1);
            if (StartXCurrent > 500 || StartYCurrent > 500)
            {
                Canvas.SetLeft(mythumb1,StartX);
                Canvas.SetTop(mythumb1, StartY);
            }


            Canvas.SetLeft(rect, Canvas.GetLeft(mythumb1) + 10 );
            Canvas.SetTop(rect, Canvas.GetTop(mythumb1) + 10 );




            rect.Width = Math.Abs(Canvas.GetLeft(mythumb3) - Canvas.GetLeft(mythumb1));
            rect.Height = Math.Abs(Canvas.GetTop(mythumb3) - Canvas.GetTop(mythumb1));



            
        }


        double StartX = 0;

        double StartY = 0;

        private void mythumb1_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            StartX = Canvas.GetLeft(thumb);
            StartY = Canvas.GetTop(thumb);
        }
    }
}
