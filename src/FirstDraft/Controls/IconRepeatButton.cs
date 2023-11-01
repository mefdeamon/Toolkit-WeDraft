using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    public class IconRepeatButton : RepeatButton
    {
        public IconRepeatButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
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
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(IconRepeatButton), new PropertyMetadata(default(Geometry), (s, e) => { }));


        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(IconRepeatButton), new PropertyMetadata(12D));



    }
}
