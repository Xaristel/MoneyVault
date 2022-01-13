namespace Money_Vault.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Expense")]
    public partial class Expense
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Expense()
        {
            Product_List = new HashSet<Product_List>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Expense_Type_Id { get; set; }

        public int Shop_Id { get; set; }

        public int Total_Price { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string Note { get; set; }

        public virtual User User { get; set; }

        public virtual Expense_Type Expense_Type { get; set; }

        public virtual Shop Shop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_List> Product_List { get; set; }
    }
}
