using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace FirstDraft.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class NumericBox : IconControl
    {
        static NumericBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericBox),
                new FrameworkPropertyMetadata(typeof(NumericBox)));
        }
        public NumericBox()
        {
            // 设置控件的键盘导航行为
            KeyboardNavigation.SetTabNavigation(this, KeyboardNavigationMode.Once);
            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.None);

            // 确保只有 TextBox 接收焦点
            GotFocus += (sender, e) =>
            {
                if (_textBox != null && !_textBox.IsFocused)
                {
                    _textBox.Focus();
                    e.Handled = true;
                }
            };
        }

        #region 依赖属性定义

        // 当前值
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(double),
                typeof(NumericBox),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged,
                    CoerceValue));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // 最小值
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(double.MinValue, OnLimitChanged));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        // 最大值
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(double.MaxValue, OnLimitChanged));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        // 增量值
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(
                "Increment",
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(1.0));

        public double Increment
        {
            get => (double)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        // 数值类型
        public static readonly DependencyProperty NumericTypeProperty =
            DependencyProperty.Register(
                "NumericType",
                typeof(NumericType),
                typeof(NumericBox),
                new PropertyMetadata(NumericType.Double));

        public NumericType NumericType
        {
            get => (NumericType)GetValue(NumericTypeProperty);
            set => SetValue(NumericTypeProperty, value);
        }

        // 是否允许手动输入
        public static readonly DependencyProperty AllowTextInputProperty =
            DependencyProperty.Register(
                "AllowTextInput",
                typeof(bool),
                typeof(NumericBox),
                new PropertyMetadata(true));

        public bool AllowTextInput
        {
            get => (bool)GetValue(AllowTextInputProperty);
            set => SetValue(AllowTextInputProperty, value);
        }

        // 小数位数（仅对Double类型有效）
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(
                "DecimalPlaces",
                typeof(int),
                typeof(NumericBox),
                new PropertyMetadata(2));

        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }

        #endregion

        private TextBox _textBox;
        private RepeatButton _increaseButton;
        private RepeatButton _decreaseButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 获取模板中的控件
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            _increaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            _decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;

            // 绑定事件
            if (_increaseButton != null)
            {
                _increaseButton.Click += IncreaseButton_Click;
                _increaseButton.Focusable = false;
                _increaseButton.IsTabStop = false;
            }

            if (_decreaseButton != null)
            {
                _decreaseButton.Click += DecreaseButton_Click;
                _decreaseButton.Focusable = false;
                _decreaseButton.IsTabStop = false;
            }

            if (_textBox != null)
            {
                _textBox.TextChanged += TextBox_TextChanged;
                _textBox.LostFocus += TextBox_LostFocus;
                _textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                _textBox.PreviewTextInput += TextBox_PreviewTextInput;  
            }

            UpdateTextBox();
            UpdateButtonStates();
        }

        #region 事件处理


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!AllowTextInput)
            {
                e.Handled = true;
                return;
            }

            var textBox = (TextBox)sender;
            string currentText = textBox.Text;
            string newText = currentText.Substring(0, textBox.SelectionStart) +
                             e.Text +
                             currentText.Substring(textBox.SelectionStart + textBox.SelectionLength);

            if (string.IsNullOrEmpty(newText))
            {
                return;
            }

            if (!IsValidNumberInput(newText))
            {
                e.Handled = true;
                return;
            }

            if (double.TryParse(newText, out double newValue))
            {
                if (newValue < Minimum || newValue > Maximum)
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private bool IsValidNumberInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            if (NumericType == NumericType.Int)
            {
                foreach (char c in input)
                {
                    if (!char.IsDigit(c) && c != '-' && c != '.')
                        return false;
                }
            }
            else
            {
                foreach (char c in input)
                {
                    if (!char.IsDigit(c) && c != '-' && c != '.')
                        return false;
                }
            }

            if (input.Contains("-") && input.IndexOf("-") > 0)
                return false;

            if (NumericType == NumericType.Double)
            {
                int dotCount = input.Count(c => c == '.');
                if (dotCount > 1)
                    return false;
            }
            else
            {
                if (input.Contains("."))
                    return false;
            }

            return true;
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Value = Math.Min(Value + Increment, Maximum);
            _textBox.Focus();
            _textBox.CaretIndex = _textBox.Text.Length;
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Value = Math.Max(Value - Increment, Minimum);
            _textBox.Focus();
            _textBox.CaretIndex = _textBox.Text.Length;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!AllowTextInput || _textBox == null)
                return;

            if (string.IsNullOrEmpty(_textBox.Text))
                return;

            if (double.TryParse(_textBox.Text, out double newValue))
            {
                newValue = (double)CoerceValue(this, newValue);

                if (Math.Abs(newValue - Value) > double.Epsilon)
                {
                    Value = newValue;
                }
                else
                {
                    UpdateTextBox();
                }
            }
            else
            {
                UpdateTextBox();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateTextBox();

            if (_textBox != null && !string.IsNullOrEmpty(_textBox.Text))
            {
                if (!double.TryParse(_textBox.Text, out double parsedValue) ||
                    parsedValue < Minimum || parsedValue > Maximum)
                {
                    UpdateTextBox();
                }
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                Value = Math.Min(Value + Increment, Maximum);
                _textBox.CaretIndex = _textBox.Text.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                Value = Math.Max(Value - Increment, Minimum);
                _textBox.CaretIndex = _textBox.Text.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                _textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

                Keyboard.ClearFocus();
                _textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                Dispatcher.Yield(DispatcherPriority.ApplicationIdle);

                _textBox.Focus();
                _textBox.SelectAll();
            }
        }

        #endregion

        #region 私有方法

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericBox)d;
            control.UpdateTextBox();
            control.UpdateButtonStates();
        }

        private static void OnLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericBox)d;
            control.Value = control.CoerceValue(control.Value);
        }

        private static object CoerceValue(DependencyObject d, object value)
        {
            var control = (NumericBox)d;
            double currentValue = (double)value;
            return control.CoerceValue(currentValue);
        }

        private double CoerceValue(double value)
        {
            value = Math.Max(Minimum, Math.Min(Maximum, value));

            // 如果是整数类型，进行四舍五入
            if (NumericType == NumericType.Int)
                value = Math.Round(value);

            return value;
        }

        private void UpdateTextBox()
        {
            if (_textBox == null)
                return;

            string format = NumericType == NumericType.Int ? "F0" : $"F{DecimalPlaces}";
            _textBox.Text = Value.ToString(format);
        }

        private void UpdateButtonStates()
        {
            if (_increaseButton != null)
                _increaseButton.IsEnabled = Value < Maximum;

            if (_decreaseButton != null)
                _decreaseButton.IsEnabled = Value > Minimum;
        }

        #endregion
    }
    public enum NumericType
    {
        Int,
        Double
    }
}
