using System;

namespace Money_Vault.Model
{
    public class IncomeCommonListItem
    {
        private int _number;
        private int _id;
        private string _typeName;
        private double _amount;
        private DateTime _date;
        private string _note;

        public int Number { get => _number; set => _number = value; }
        public int Id { get => _id; set => _id = value; }
        public string TypeName { get => _typeName; set => _typeName = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public string Note { get => _note; set => _note = value; }
    }
}
