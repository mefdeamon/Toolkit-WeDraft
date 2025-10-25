using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace FirstDraft.Converters
{
    public class Int012ToBooleaneNullTrueFalsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                if (intValue == 0)
                    return null;
                else if (intValue == 1)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool tValue && EqualityComparer<bool>.Default.Equals(tValue, true);
        }

    }
}
