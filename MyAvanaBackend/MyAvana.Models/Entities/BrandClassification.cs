using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class BrandClassification
    {
        [Key]
        public int BrandClassificationId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
