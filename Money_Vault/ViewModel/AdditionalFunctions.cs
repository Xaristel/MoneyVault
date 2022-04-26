using System;

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
    }
}
