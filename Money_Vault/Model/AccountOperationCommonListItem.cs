namespace Money_Vault.Model
{
    public class AccountOperationCommonListItem
    {
        private int _number;
        private int _id;
        private int _account_Id;
        private string _type;
        private double _amount;
        private string _date;

        public int Number { get => _number; set => _number = value; }
        public int Id { get => _id; set => _id = value; }
        public int Account_Id { get => _account_Id; set => _account_Id = value; }
        public string Type { get => _type; set => _type = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public string Date { get => _date; set => _date = value; }
    }
}
