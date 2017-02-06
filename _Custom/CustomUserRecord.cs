using HowLeakyModels.Accounts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HowLeakyModels._Custom
{
     [Table("UserRecord")]
    public abstract class CustomUserRecord
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public virtual ApplicationUser AppUser { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}