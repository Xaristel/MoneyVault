using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public static class AdditionalFunctions
    {
        public static bool FindLessDate(string firstDate, string secondDate)
        {
            if (Convert.ToInt32(firstDate.Split('.')[2]) < Convert.ToInt32(secondDate.Split('.')[2])) //year
            {
                return true;
            }
            else if (Convert.ToInt32(firstDate.Split('.')[2]) == Convert.ToInt32(secondDate.Split('.')[2])) //year
            {
                if (Convert.ToInt32(firstDate.Split('.')[1]) < Convert.ToInt32(secondDate.Split('.')[1])) //month
                {
                    return true;
                }
                else if (Convert.ToInt32(firstDate.Split('.')[1]) == Convert.ToInt32(secondDate.Split('.')[1])) //month
                {
                    if (Convert.ToInt32(firstDate.Split('.')[0]) < Convert.ToInt32(secondDate.Split('.')[0])) //day
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static double ConvertToCurrencyFormat(int value)
        {
            if (value == 0)
            {
                return Convert.ToDouble(value.ToString().Insert(1, ",00"));
            }
            else
            {
                return Convert.ToDouble(value.ToString().Insert(value.ToString().Length - 2, ","));
            }
        }

        public static int ConvertFromCurrencyFormat(string value)
        {
            if (value.Contains(",") || value.Contains("."))
            {
                if (value.Replace(",", ".").IndexOf('.') == value.Length - 3)
                {
                    return Convert.ToInt32(value.ToString().Replace(".", string.Empty).Replace(",", string.Empty));
                }
                else
                {
                    string tempValue = value.ToString().Replace(".", string.Empty).Replace(",", string.Empty);
                    return Convert.ToInt32(tempValue.Insert(tempValue.ToString().Length, "0"));
                }
            }
            else
            {
                return Convert.ToInt32(value.ToString().Insert(value.ToString().Length, "00"));
            }
        }

        public static bool CheckAmountFormat(string amount)
        {
            if (int.TryParse(amount.Replace(".", string.Empty).Replace(",", string.Empty), out int result) && result > 0)
            {
                if (amount.Contains(",") || amount.Contains("."))
                {
                    if (amount.Replace(",", ".").IndexOf('.') == amount.Length - 3 || amount.Replace(",", ".").IndexOf('.') == amount.Length - 2)
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        public static void SetColorForActiveButton(ObservableCollection<Brush> buttonsColorList, List<string> pathesList, string path)
        {
            for (int i = 0; i < buttonsColorList.Count(); i++)
            {
                buttonsColorList[i] = Brushes.White;
            }
            buttonsColorList[pathesList.IndexOf(path)] = Brushes.DeepSkyBlue;
        }
    }
}
