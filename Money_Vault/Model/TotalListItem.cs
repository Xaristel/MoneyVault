﻿using System;

namespace Money_Vault.ViewModel
{
    public class TotalListItem : IComparable<TotalListItem>
    {
        private string _typeName;
        private double _totalAmount;

        public string TypeName
        {
            get => _typeName;
            set
            {
                _typeName = value;
            }
        }
        public double TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
            }
        }

        public int CompareTo(TotalListItem other)
        {
            return other.TotalAmount.CompareTo(TotalAmount);
        }
    }
}
