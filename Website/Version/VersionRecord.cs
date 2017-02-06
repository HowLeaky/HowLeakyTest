using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HowLeakyModels.Version
{
    [Table("VersionRecords")]
    public class VersionRecord
    {
        public VersionRecord()
        {

        }

        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Version")]
        public string Version { get; set; }

        [Required]
        [Display(Name = "Public Description")]
        public string PublicDescription { get; set; }

        [Required]
        [Display(Name = "Developer Description")]
        public string DeveloperDescription { get; set; }
    }
}