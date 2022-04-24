namespace Money_Vault.Model
{
    public class IncomeCommonListItem
    {
        private int _id;
        private string _typeName;
        private string _amount;
        private string _date;
        private string _note;

        public int Id { get => _id; set => _id = value; }
        public string TypeName { get => _typeName; set => _typeName = value; }
        public string Amount { get => _amount; set => _amount = value; }
        public string Date { get => _date; set => _date = value; }
        public string Note { get => _note; set => _note = value; }
    }
}
