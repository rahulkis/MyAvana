using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ClassificationBrand
    {
        [Key]
        public int ClassificationBrandId { get; set; }

        [ForeignKey("BrandClassificationId")]
        public int BrandClassificationId { get; set; }
        public virtual BrandClassification BrandClassification { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool? IsActive { get; set; }
    }
}
