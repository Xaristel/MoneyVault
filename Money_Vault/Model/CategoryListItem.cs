namespace Money_Vault.Model
{
    public class CategoryListItem
    {
        private int _id;
        private string _name;
        private string _note;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }

        public string Note
        {
            get => _note;
            set
            {
                _note = value;
            }
        }
    }
}
