using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ArticleMoods
    {
        [Key]
        public int ArticleMoodId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("BlogArticleId")]
        public int? BlogArticleId { get; set; }
        public virtual BlogArticle BlogArticle { get; set; }
        [ForeignKey("MoodId")]
        public int? MoodId { get; set; }
        public virtual Moods Moods { get; set; }
    }
}
