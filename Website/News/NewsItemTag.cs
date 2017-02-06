using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HowLeakyModels.News
{
    [Table("PostTags")]
    public class NewsItemTag
    {
        public NewsItemTag()
        {
            this.Posts = new HashSet<NewsItem>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<NewsItem> Posts { get; set; }
    }
}
