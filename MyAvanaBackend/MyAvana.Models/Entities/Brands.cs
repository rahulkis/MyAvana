using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyAvana.Models.Entities;

namespace MyAvana.Models.Entities
{
    public class Brands
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? HideInSearch { get; set; }
        public string FeaturedIngredients { get; set; }
        public int? Rank { get; set; }
    }
}
