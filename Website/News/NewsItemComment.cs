using HowLeakyModels.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HowLeakyModels.News
{
    [Table("PostComments")]
    public partial class NewsItemComment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public System.DateTime DateTime { get; set; }

        [Required]
        public string Body { get; set; }

        public virtual ApplicationUser AspNetUser { get; set; }
        public virtual NewsItem Post { get; set; }
    }
}
