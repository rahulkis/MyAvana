using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class BrandTag
    {
        [Key]
        public int BrandTagId { get; set; }

        [ForeignKey("TagsId")]
        public int TagsId { get; set; }
        public virtual Tags Tags { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool? IsActive { get; set; }
    }
}
