using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    public class IconButton : Button
    {
        public IconButton()
        {
            //((StreamGeometry)IconData).FillRule = FillRule.EvenOdd;
        }
        /// <summary>
        /// 图标数据
        /// </summary>
        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        /// <summary>
        /// <see cref="IconData"/>
        /// </summary>
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register(nameof(IconData), typeof(Geometry), typeof(IconButton), new PropertyMetadata(default(Geometry), (s, e) => { }));


        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(IconButton), new PropertyMetadata(default(double)));



    }
}
