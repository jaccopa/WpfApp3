using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp3
{
    public class ButtonAdorner : Adorner
    {
        public ButtonAdorner(UIElement ele) : base(ele)
        {

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            FormattedText t = new FormattedText(
             "!!!!!",
             CultureInfo.InstalledUICulture,
             FlowDirection.LeftToRight,
             new Typeface("微软雅黑"),
             15, new SolidColorBrush(Colors.Red)
            );

            drawingContext.DrawText(t, new Point(270, 0));
            base.OnRender(drawingContext);
        }
    }
}
