using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    public class IconTextBox : TextBox
    {
        public IconTextBox()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTextBox), new FrameworkPropertyMetadata(typeof(IconTextBox)));
        }

        /// <summary>
        /// 图标数据
        /// </summary>
        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// <see cref="Icon"/>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(IconTextBox), new PropertyMetadata(default(Geometry), (s, e) => { }));

        /// <summary>
        /// 图标大小
        /// </summary>
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(IconTextBox), new PropertyMetadata(12D));
    }
}
