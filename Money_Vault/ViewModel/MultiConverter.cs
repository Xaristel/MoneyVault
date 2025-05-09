using System;
using System.Windows.Data;

namespace Money_Vault.ViewModel
{
    public class MultiConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Tuple<string, string, bool, bool>((string)values[0], (string)values[1], (bool)values[2], (bool)values[3]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { };
        }
    }
}
