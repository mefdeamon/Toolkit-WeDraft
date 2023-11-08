using System;
using System.Windows;

namespace FirstDraft.AttachedProperties
{
    public class ButtonAttProp
    {
        public static Boolean GetIsRunning(DependencyObject obj)
        {
            return (Boolean)obj.GetValue(IsRunningProperty);
        }

        public static void SetIsRunning(DependencyObject obj, Boolean value)
        {
            obj.SetValue(IsRunningProperty, value);
        }

        /// <summary>
        /// 提供布尔值状态附加属性
        /// 需要动画过度按钮附加属性，过度执行过程中
        /// </summary>
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.RegisterAttached("IsRunning", typeof(Boolean), typeof(ButtonAttProp), new PropertyMetadata(false));
    }
}
