using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp3
{
    public class AdornerHelper
    {
        public static readonly DependencyProperty IsShowAdonerProperty =
            DependencyProperty.RegisterAttached("IsShowAdorner",typeof(bool),typeof(AdornerHelper),new PropertyMetadata(true, IsShowChangedCallBack));

        private static void IsShowChangedCallBack(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(element);
                if (adornerLayer != null)
                {
                    var adorners = adornerLayer.GetAdorners(element);
                    if (adorners != null && adorners.Count() != 0)
                    {
                        var adorner = adorners.FirstOrDefault();
                        if (adorner == null)
                        {
                            return;
                        }

                        adorner.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
                    }
                }
            }
        }


        public static void SetAdorner(DependencyObject obj, Adorner adorner)
        {
            var element = obj as Visual;
            if (element != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(element);
                if (adornerLayer != null)
                {
                    adornerLayer.Add(adorner);
                }
            }
        }
    }
}
