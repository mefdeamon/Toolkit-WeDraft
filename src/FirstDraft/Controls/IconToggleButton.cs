using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    /// <summary>
    /// 图标
    /// </summary>
    public class IconToggleButton : ToggleButton
    {
        public IconToggleButton()
        {

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
            DependencyProperty.Register(nameof(IconData), typeof(Geometry), typeof(IconToggleButton), new PropertyMetadata(default(Geometry), (s, e) => { }));
    }
}
