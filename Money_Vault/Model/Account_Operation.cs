namespace Money_Vault.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Account_Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Account_Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int? Amount { get; set; }

        public virtual Account Account { get; set; }
    }
}
