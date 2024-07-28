using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    /// <summary>
    /// 边矩形图标单选按钮
    /// </summary>
    public class IconRadioButton : RadioButton
    {
        public IconRadioButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(IconRadioButton), new FrameworkPropertyMetadata(typeof(IconRadioButton)));
        }
        /// <summary>
        /// 图标
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
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(IconRadioButton), new PropertyMetadata(default(Geometry)));


        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(IconRadioButton), new PropertyMetadata(12D));




        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(IconRadioButton), new PropertyMetadata(new CornerRadius(0)));

    }
}
