using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ArticleProduct
    {
        [Key]
        public int ArticleProductsId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("BlogArticleId")]
        public int? BlogArticleId { get; set; }
        public virtual BlogArticle BlogArticle { get; set; }
        [ForeignKey("Id")]
        public int? ProductEntityId { get; set; }
        public virtual ProductEntity ProductEntity { get; set; }
    }
}
