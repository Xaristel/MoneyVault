namespace Money_Vault.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Goal")]
    public partial class Goal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? User_Id { get; set; }

        public int Account_Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int Required_Amount { get; set; }

        public virtual Account Account { get; set; }

        public virtual User User { get; set; }
    }
}
