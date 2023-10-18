using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FirstDraft.Controls
{
    /// <summary>
    /// IpAddressBox.xaml 的交互逻辑
    /// </summary>
    public partial class IpAddressBox : UserControl
    {
        public IpAddressBox()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(part1, TextBox_Pasting);
        }

        #region 事件响应

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var vm = this.DataContext as IpAddressDataContext;
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String pastingText = (String)e.DataObject.GetData(typeof(String));
                vm.SetAddress(pastingText);
                part1.Focus();
                e.CancelCommand();
            }

        }

        private void Part4_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back && part4.Text == "")
            {
                part3.CaretIndex = part3.Text.Length;
                part3.Focus();
            }
            if (e.Key == Key.Left && part4.CaretIndex == 0)
            {
                part3.Focus();
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part4.SelectionLength == 0)
                {
                    var vm = this.DataContext as IpAddressDataContext;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        private void Part2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back && part2.Text == "")
            {
                part1.CaretIndex = part1.Text.Length;
                part1.Focus();
            }
            if (e.Key == Key.Right && part2.CaretIndex == part2.Text.Length)
            {
                part3.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Left && part2.CaretIndex == 0)
            {
                part1.Focus();
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part2.SelectionLength == 0)
                {
                    var vm = this.DataContext as IpAddressDataContext;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        private void Part3_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back && part3.Text == "")
            {
                part2.CaretIndex = part2.Text.Length;
                part2.Focus();
            }
            if (e.Key == Key.Right && part3.CaretIndex == part3.Text.Length)
            {
                part4.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Left && part3.CaretIndex == 0)
            {
                part2.Focus();
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part3.SelectionLength == 0)
                {
                    var vm = this.DataContext as IpAddressDataContext;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        private void Part1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right && part1.CaretIndex == part1.Text.Length)
            {
                part2.Focus();
                e.Handled = true;
            }
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part1.SelectionLength == 0)
                {
                    var vm = this.DataContext as IpAddressDataContext;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        #endregion
    }

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            RaisePropertyChanged(PropertyName);
        }
    }

    /// <summary>
    /// IP地址模型
    /// </summary>
    public class IpAddressDataContext : NotifyPropertyChanged
    {
        public event EventHandler AddressChanged;

        public string AddressText
        {
            get { return $"{Part1 ?? "0"}.{Part2 ?? "0"}.{Part3 ?? "0"}.{Part4 ?? "0"}"; }
        }

        /// <summary>
        /// IP字节
        /// </summary>
        public byte[] IpBytes
        {
            get
            {
                byte[] res = new byte[4];
                res[0] = Convert.ToByte(part1);
                res[1] = Convert.ToByte(part2);
                res[2] = Convert.ToByte(part3);
                res[3] = Convert.ToByte(part4);
                return res;
            }
        }

        private bool isPart1Focused;

        public bool IsPart1Focused
        {
            get { return isPart1Focused; }
            set { isPart1Focused = value; OnPropertyChanged(); }
        }

        private string part1;

        public string Part1
        {
            get { return part1; }
            set
            {
                part1 = value;
                SetFocus(true, false, false, false);

                var moveNext = CanMoveNext(ref part1);

                OnPropertyChanged();
                OnPropertyChanged(nameof(AddressText));
                AddressChanged?.Invoke(this, EventArgs.Empty);

                if (moveNext)
                {
                    SetFocus(false, true, false, false);
                }
            }
        }

        private bool isPart2Focused;

        public bool IsPart2Focused
        {
            get { return isPart2Focused; }
            set { isPart2Focused = value; OnPropertyChanged(); }
        }


        private string part2;

        public string Part2
        {
            get { return part2; }
            set
            {
                part2 = value;
                SetFocus(false, true, false, false);

                var moveNext = CanMoveNext(ref part2);

                OnPropertyChanged();
                OnPropertyChanged(nameof(AddressText));
                AddressChanged?.Invoke(this, EventArgs.Empty);

                if (moveNext)
                {
                    SetFocus(false, false, true, false);
                }
            }
        }

        private bool isPart3Focused;

        public bool IsPart3Focused
        {
            get { return isPart3Focused; }
            set { isPart3Focused = value; OnPropertyChanged(); }
        }

        private string part3;

        public string Part3
        {
            get { return part3; }
            set
            {
                part3 = value;
                SetFocus(false, false, true, false);
                var moveNext = CanMoveNext(ref part3);

                OnPropertyChanged();
                OnPropertyChanged(nameof(AddressText));
                AddressChanged?.Invoke(this, EventArgs.Empty);

                if (moveNext)
                {
                    SetFocus(false, false, false, true);
                }
            }
        }

        private bool isPart4Focused;

        public bool IsPart4Focused
        {
            get { return isPart4Focused; }
            set { isPart4Focused = value; OnPropertyChanged(); }
        }

        private string part4;

        public string Part4
        {
            get { return part4; }
            set
            {
                part4 = value;
                SetFocus(false, false, false, true);
                var moveNext = CanMoveNext(ref part4);

                OnPropertyChanged();
                OnPropertyChanged(nameof(AddressText));
                AddressChanged?.Invoke(this, EventArgs.Empty);

            }
        }

        /// <summary>
        /// 设置IP地址
        /// </summary>
        /// <param name="address">地址格式：192.168.1.2</param>
        public void SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return;

            try
            {
                var parts = address.Split('.');

                if (int.TryParse(parts[0], out var num0))
                {
                    part1 = num0.ToString();
                }

                if (int.TryParse(parts[1], out var num1))
                {
                    part2 = parts[1];
                }

                if (int.TryParse(parts[2], out var num2))
                {
                    part3 = parts[2];
                }

                if (int.TryParse(parts[3], out var num3))
                {
                    part4 = parts[3];
                }

                RaisePropertyChanged(nameof(Part1));
                RaisePropertyChanged(nameof(Part2));
                RaisePropertyChanged(nameof(Part3));
                RaisePropertyChanged(nameof(Part4));
                OnPropertyChanged(nameof(AddressText));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设置IP地址，不改变控件的焦点
        /// </summary>
        /// <param name="address"></param>
        public void SetAddress(byte[] address)
        {
            if (address.Length != 4)
                return;

            part1 = address[0].ToString();
            part2 = address[1].ToString();
            part3 = address[2].ToString();
            part4 = address[3].ToString();

            RaisePropertyChanged(nameof(Part1));
            RaisePropertyChanged(nameof(Part2));
            RaisePropertyChanged(nameof(Part3));
            RaisePropertyChanged(nameof(Part4));
            OnPropertyChanged(nameof(AddressText));
        }


        private bool CanMoveNext(ref string part)
        {
            bool moveNext = false;

            if (!string.IsNullOrWhiteSpace(part))
            {
                if (part.Length >= 3)
                {
                    moveNext = true;
                }

                if (part.EndsWith("."))
                {
                    moveNext = true;
                    part = part.Replace(".", "");
                }
            }

            return moveNext;
        }

        private void SetFocus(bool part1, bool part2, bool part3, bool part4)
        {
            IsPart1Focused = part1;
            IsPart2Focused = part2;
            IsPart3Focused = part3;
            IsPart4Focused = part4;
        }

    }

    public class IPRangeValidationRule : ValidationRule
    {
        private int _min;
        private int _max;

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val = 0;
            var strVal = (string)value;
            try
            {
                if (strVal.Length > 0)
                {
                    if (strVal.EndsWith("."))
                    {
                        return CheckRanges(strVal.Replace(".", ""));
                    }

                    // Allow dot character to move to next box
                    return CheckRanges(strVal);
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false,
                  "Please enter the value in the range: " + Min + " - " + Max + ".");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

        private ValidationResult CheckRanges(string strVal)
        {
            if (int.TryParse(strVal, out var res))
            {
                if ((res < Min) || (res > Max))
                {
                    return new ValidationResult(false,
                      "Please enter the value in the range: " + Min + " - " + Max + ".");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            else
            {
                return new ValidationResult(false, "Illegal characters entered");
            }
        }
    }
}
