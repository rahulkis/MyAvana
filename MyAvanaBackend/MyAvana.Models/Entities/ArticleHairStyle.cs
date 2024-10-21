using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ArticleHairStyle
    {
        [Key]
        public int ArticleHairStyleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("BlogArticleId")]
        public int? BlogArticleId { get; set; }
        public virtual BlogArticle BlogArticle { get; set; }
        [ForeignKey("HairStylesId")]
        public int? HairStylesId { get; set; }
        public virtual HairStyles HairStyles { get; set; }
    }
}
