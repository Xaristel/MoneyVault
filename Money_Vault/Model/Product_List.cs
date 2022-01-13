namespace Money_Vault.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_List
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Expense_Id { get; set; }

        public int Product_Id { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public virtual Expense Expense { get; set; }

        public virtual Product Product { get; set; }
    }
}
