using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyAvana.Models.Entities
{
    public class BrandRecommendationStatus
    {
        [Key]
        public int BrandRecommendationStatusId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
