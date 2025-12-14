using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace FirstDraft.AttachedProperties
{
    public static class TextBoxAttProp
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
                "IsFocused", typeof(bool), typeof(TextBoxAttProp),
                new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (TextBoxBase)d;
            if ((bool)e.NewValue)
            {
                control.Focus();
            }
        }

        #region 选中

        /// <summary>
        /// 当 TextBoxBase获得焦点的时候，自动全部选择文字。附加属性为SelectAllWhenGotFocus，类型为bool.
        /// </summary>
        public static readonly DependencyProperty SelectAllWhenGotFocusProperty = DependencyProperty.RegisterAttached("SelectAllWhenGotFocus",
                          typeof(bool), typeof(TextBoxAttProp),
                          new FrameworkPropertyMetadata((bool)false, new PropertyChangedCallback(OnSelectAllWhenGotFocusChanged)));

        public static bool GetSelectAllWhenGotFocus(TextBoxBase d)
        {
            return (bool)d.GetValue(SelectAllWhenGotFocusProperty);
        }
        public static void SetSelectAllWhenGotFocus(TextBoxBase d, bool value)
        {
            d.SetValue(SelectAllWhenGotFocusProperty, value);
        }

        private static void OnSelectAllWhenGotFocusChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs e)
        {
            if (dependency is TextBoxBase tBox)
            {
                var isSelectedAllWhenGotFocus = (bool)e.NewValue;
                if (isSelectedAllWhenGotFocus)
                {
                    tBox.PreviewMouseDown += TextBoxPreviewMouseDown;
                    tBox.GotFocus += TextBoxOnGotFocus;
                    tBox.LostFocus += TextBoxOnLostFocus;
                    tBox.PreviewTextInput += TxtInput;
                }
                else
                {
                    tBox.PreviewMouseDown -= TextBoxPreviewMouseDown;
                    tBox.GotFocus -= TextBoxOnGotFocus;
                    tBox.LostFocus -= TextBoxOnLostFocus;
                    tBox.PreviewTextInput -= TxtInput;
                }
            }
        }
        private static void TextBoxOnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.SelectAll();
                tBox.PreviewMouseDown -= TextBoxPreviewMouseDown;
            }

        }
        private static void TextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.Focus();
                e.Handled = true;
            }
        }

        private static void TxtInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
            }
        }

        private static void TextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {

            if (sender is TextBoxBase tBox)
            {
                tBox.PreviewMouseDown += TextBoxPreviewMouseDown;
            }
        }

        #endregion



        #region 按Enter结束输入

        public static readonly DependencyProperty IsEnterKeyLostFocusProperty =
            DependencyProperty.RegisterAttached(
                "IsEnterKeyLostFocus",
                typeof(bool),
                typeof(TextBoxAttProp),
                new PropertyMetadata(false, OnIsEnterKeyLostFocusChanged));

        public static bool GetIsEnterKeyLostFocus(DependencyObject obj)
            => (bool)obj.GetValue(IsEnterKeyLostFocusProperty);

        public static void SetIsEnterKeyLostFocus(DependencyObject obj, bool value)
            => obj.SetValue(IsEnterKeyLostFocusProperty, value);

        private static void OnIsEnterKeyLostFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                    textBox.LostFocus += TextBox_LostFocus;
                }
                else
                {
                    textBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                    textBox.LostFocus -= TextBox_LostFocus;
                }
            }
        }

        private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;

                // 更新绑定源
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

                // 调用提交命令（如果有）
                var command = GetSubmitCommand(textBox);
                var commandParameter = GetSubmitCommandParameter(textBox) ?? textBox.Text;

                if (command?.CanExecute(commandParameter) == true)
                {
                    command.Execute(commandParameter);
                }

                // 失去焦点
                Keyboard.ClearFocus();



                //int caretIndex = textBox.CaretIndex;

                //// 移动到下一个控件
                //textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                //// 等待调度器处理完所有高优先级任务（包括绑定更新）
                //await Dispatcher.Yield(DispatcherPriority.ApplicationIdle);

                //// 返回焦点
                //textBox.Focus();
                //textBox.CaretIndex = caretIndex;
                //textBox.SelectAll(); // 全选文本，用户可直接输入覆盖


                e.Handled = true;
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        #region 绑定命令触发（按下Enter执行命令）

        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.RegisterAttached(
                "SubmitCommand",
                typeof(ICommand),
                typeof(TextBoxAttProp),
                new PropertyMetadata(null));

        public static ICommand GetSubmitCommand(DependencyObject obj)
            => (ICommand)obj.GetValue(SubmitCommandProperty);

        public static void SetSubmitCommand(DependencyObject obj, ICommand value)
            => obj.SetValue(SubmitCommandProperty, value);

        public static readonly DependencyProperty SubmitCommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "SubmitCommandParameter",
                typeof(object),
                typeof(TextBoxAttProp),
                new PropertyMetadata(null));

        public static object GetSubmitCommandParameter(DependencyObject obj)
            => obj.GetValue(SubmitCommandParameterProperty);

        public static void SetSubmitCommandParameter(DependencyObject obj, object value)
            => obj.SetValue(SubmitCommandParameterProperty, value);

        #endregion

        #endregion

    }
}
