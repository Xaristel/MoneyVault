﻿using System;
using System.Windows.Data;

namespace Money_Vault.ViewModel
{
    public class MultiConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Tuple<string, string, bool> tuple = new Tuple<string, string, bool>((string)values[0], (string)values[1], (bool)values[2]);
            return tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { };
        }
    }
}
