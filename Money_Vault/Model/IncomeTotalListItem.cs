namespace Money_Vault.ViewModel
{
    public class IncomeTotalListItem
    {
        private string _typeName;
        private string _totalAmount;

        public string TypeName
        {
            get => _typeName;
            set
            {
                _typeName = value;
            }
        }
        public string TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
            }
        }
    }
}
