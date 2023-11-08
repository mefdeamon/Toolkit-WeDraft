using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FirstDraft.Converters
{
    /// <summary>
    /// <see cref="Boolean"/> 值转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BooleanConverter<T> : IValueConverter
    {
        public BooleanConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public T TrueValue { get; set; }
        public T FalseValue { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool boolValue && boolValue ? TrueValue : FalseValue;

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is T tValue && EqualityComparer<T>.Default.Equals(tValue, TrueValue);
    }

    /// <summary>
    /// <see cref="Boolean"/> 值 反向转换
    /// </summary>
    public class InvertBooleanConverter : BooleanConverter<bool>
    {
        public InvertBooleanConverter()
            : base(false, true)
        {
        }
    }

    /// <summary>
    /// <see cref="Boolean"/> 值与 <see cref="Visibility"/> 值 正向转换
    /// </summary>
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }

    /// <summary>
    /// <see cref="Boolean"/> 值与 <see cref="Visibility"/> 值 反向向转换
    /// </summary>
    public sealed class InvertBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InvertBooleanToVisibilityConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        { }
    }


    /// <summary>
    /// <see cref="Boolean"/> 值与 <see cref="Visibility"/> 值 正向转换
    /// </summary>
    public sealed class BooleanToVisibilityHiddenConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityHiddenConverter() :
            base(Visibility.Visible, Visibility.Hidden)
        { }
    }

    /// <summary>
    /// <see cref="Boolean"/> 值与 <see cref="Visibility"/> 值 反向向转换
    /// </summary>
    public sealed class InvertBooleanToVisibilityHiddenConverter : BooleanConverter<Visibility>
    {
        public InvertBooleanToVisibilityHiddenConverter() :
            base(Visibility.Hidden, Visibility.Visible)
        { }
    }

    /// <summary>
    /// <see cref="Boolean"/> 值与 <see cref="int"/> 值(0,1) 反向向转换
    /// </summary>
    public class BooleanToInt01Converter : BooleanConverter<int>
    {
        public BooleanToInt01Converter() : base(1, 0)
        {
        }
    }
}
