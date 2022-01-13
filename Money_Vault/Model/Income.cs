namespace Money_Vault.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Income")]
    public partial class Income
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Income_Type_Id { get; set; }

        public int Total_Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string Note { get; set; }

        public virtual Income_Type Income_Type { get; set; }

        public virtual User User { get; set; }
    }
}
