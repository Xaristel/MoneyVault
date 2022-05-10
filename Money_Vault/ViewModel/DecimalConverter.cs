using System;
using System.Globalization;
using System.Windows.Data;

namespace Money_Vault.ViewModel
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString().Replace(" ", "") == "" || !AdditionalFunctions.CheckAmountFormat(value.ToString()))
            {
                return 0;
            }

            return value.ToString().Replace(",", ".");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString().Replace(" ", "") == "" || !AdditionalFunctions.CheckAmountFormat(value.ToString()))
            {
                return 0;
            }

            return value.ToString().Replace(",", ".");
        }

    }
}
