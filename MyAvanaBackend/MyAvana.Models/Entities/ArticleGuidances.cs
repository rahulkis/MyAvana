using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class ArticleGuidances
    {
        [Key]
        public int ArticleGuidanceId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("BlogArticleId")]
        public int? BlogArticleId { get; set; }
        public virtual BlogArticle BlogArticle { get; set; }
        [ForeignKey("GuidanceId")]
        public int? GuidanceId { get; set; }
        public virtual Guidances Guidances { get; set; }
    }
}
