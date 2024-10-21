using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class BrandsBrandRecommendationStatus
    {
        [Key]
        public int BrandsBrandRecommendationStatusId { get; set; }

        [ForeignKey("BrandRecommendationStatusId")]
        public int BrandRecommendationStatusId { get; set; }
        public virtual BrandRecommendationStatus BrandRecommendationStatus { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool IsActive { get; set; }
    }
}
