using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class BrandHairState
    {
        [Key]
        public int BrandHairStateId { get; set; }

        [ForeignKey("HairStateId")]
        public int HairStateId { get; set; }
        public virtual HairState HairState { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool IsActive { get; set; }
    }
}
