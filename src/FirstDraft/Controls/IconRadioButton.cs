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

    }
}
