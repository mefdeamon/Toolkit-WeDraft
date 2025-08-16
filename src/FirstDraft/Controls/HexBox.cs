using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    public class HexBox : TextBox
    {
        private const string HexPattern = "^[0-9A-F]*$";
        private bool _isFormatting;
        static HexBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(HexBox),
                new FrameworkPropertyMetadata(typeof(HexBox)));

        }

        #region override

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            // 获取原始文本（不含空格）
            var raw = GetRawText();

            // 计算在原始文本中的插入位置
            int rawCaretPos = GetRawCaretPosition(this.CaretIndex);

            // 检查插入位置是否有效
            if (rawCaretPos < 0 || rawCaretPos > raw.Length)
            {
                e.Handled = true;
                return;
            }

            // 构建新文本并验证
            string newText = raw.Insert(rawCaretPos, e.Text.ToUpper());
            e.Handled = !Regex.IsMatch(newText, HexPattern);

            base.OnPreviewTextInput(e);
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_isFormatting) return;

            try
            {
                _isFormatting = true;

                int originalCaretPos = GetRawCaretPosition(this.CaretIndex);
                bool wasAtEnd = originalCaretPos >= GetRawText().Length;

                string rawText = GetRawText().ToUpper();

                string formattedText = FormatHexString(rawText);

                this.Text = formattedText;

                int newCaretPos;
                if (wasAtEnd)
                {
                    newCaretPos = formattedText.Length;
                }
                else
                {
                    newCaretPos = GetFormattedCaretPosition(originalCaretPos);
                }

                this.CaretIndex = Math.Min(newCaretPos, formattedText.Length);

            }
            finally
            {
                _isFormatting = false;
            }

            base.OnTextChanged(e);
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        #endregion

        #region override helper function

        private string GetRawText()
        {
            return this.Text?.Replace(" ", "") ?? "";
        }

        private string FormatHexString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i > 0 && i % (Byte)HexMode == 0)
                {
                    sb.Append(" ");
                }
                sb.Append(input[i]);
            }
            return sb.ToString();
        }

        private int GetFormattedCaretPosition(int rawPosition)
        {
            if (rawPosition <= 0) return 0;

            int spaces = (rawPosition - 1) / (Byte)HexMode;
            return rawPosition + spaces;
        }

        private int GetRawCaretPosition(int formattedPosition)
        {
            if (string.IsNullOrEmpty(this.Text)) return 0;

            int rawPos = 0;
            for (int i = 0; i < Math.Min(formattedPosition, this.Text.Length); i++)
            {
                if (this.Text[i] != ' ')
                    rawPos++;
            }
            return rawPos;
        }

        #endregion

        #region dependency properties

        #region hex view mode

        public HexMode HexMode
        {
            get { return (HexMode)GetValue(HexModeProperty); }
            set { SetValue(HexModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HexMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HexModeProperty =
            DependencyProperty.Register("HexMode", typeof(HexMode), typeof(HexBox), new PropertyMetadata(HexMode.BYTE, (s, e) =>
            {
                if (s is HexBox box)
                {
                    box.Text = box.FormatHexString(box.GetRawText());
                }
            }));

        #endregion

        #region icon

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
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(HexBox), new PropertyMetadata(default(Geometry), (s, e) => { }));

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
            DependencyProperty.Register("IconSize", typeof(double), typeof(HexBox), new PropertyMetadata(12D));

        #endregion

        #endregion


        #region 公共方法

        /// <summary>
        /// 获取文本框中的16进制值作为UInt数组（按4字节分割）
        /// </summary>
        /// <returns>解析成功的UInt数组，解析失败的部分默认为0</returns>
        public uint[] ToArray()
        {
            string rawText = GetRawText();
            if (string.IsNullOrEmpty(rawText))
                return Array.Empty<uint>();

            // 按每8个字符分割（每个uint对应4字节，即8个16进制字符）
            int chunkSize = 8;
            int arraySize = (rawText.Length + chunkSize - 1) / chunkSize;
            uint[] result = new uint[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                int startIndex = i * chunkSize;
                int length = Math.Min(chunkSize, rawText.Length - startIndex);
                string chunk = rawText.Substring(startIndex, length);

                try
                {
                    result[i] = Convert.ToUInt32(chunk, 16);
                }
                catch
                {
                    result[i] = 0; // 解析失败的部分设为0
                }
            }

            return result;
        }

        /// <summary>
        /// 设置UInt数组值（将自动格式化为16进制字符串）
        /// </summary>
        /// <param name="values">要设置的UInt数组</param>
        public void SetArray(uint[] values)
        {
            if (values == null || values.Length == 0)
            {
                this.Text = string.Empty;
                return;
            }

            HexMode = HexMode.UINT32; // 设置为UINT32模式以确保每个值占8个字符
            StringBuilder sb = new StringBuilder();
            foreach (uint value in values)
            {
                sb.Append(value.ToString("X8")); // 每个uint固定8位16进制
            }

            this.Text = FormatHexString(sb.ToString());
        }

        public void SetArray(ushort[] values)
        {
            if (values == null || values.Length == 0)
            {
                this.Text = string.Empty;
                return;
            }
            HexMode = HexMode.UINT16; // 设置为UINT16模式以确保每个值占4个字符
            StringBuilder sb = new StringBuilder();
            foreach (ushort value in values)
            {
                sb.Append(value.ToString("X4")); // 每个ushort固定4位16进制
            }
            this.Text = FormatHexString(sb.ToString());
        }

        public void SetArray(byte[] values)
        {
            if (values == null || values.Length == 0)
            {
                this.Text = string.Empty;
                return;
            }
            HexMode = HexMode.BYTE; // 设置为BYTE模式以确保每个值占2个字符
            StringBuilder sb = new StringBuilder();
            foreach (byte value in values)
            {
                sb.Append(value.ToString("X2")); // 每个byte固定2位16进制
            }
            this.Text = FormatHexString(sb.ToString());
        }

        #endregion

        #region public static helper function

        public static byte[] GetBytes(string text)
        {
            if (text == null || string.IsNullOrEmpty(text))
                return Array.Empty<byte>();
            return text.Split(' ').Select(x => Convert.ToByte(x, 16)).ToArray();
        }

        public static ushort[] GetUInt16(string text)
        {
            var bytes = GetBytes(text);
            if (bytes.Length < 1) return Array.Empty<ushort>();

            return Enumerable.Range(0, bytes.Length / 2)
                             .Select(i => BitConverter.ToUInt16(bytes, i * 2))
                             .ToArray();
        }

        #endregion
    }


    public enum HexMode : Byte
    {
        BYTE = 2,
        UINT16 = 4,
        UINT32 = 8,
    }

}