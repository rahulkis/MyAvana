using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class BrandHairType
    {
        [Key]
        public int BrandHairTypeId { get; set; }

        [ForeignKey("HairTypeId")]
        public int HairTypeId { get; set; }
        public virtual HairType HairType { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool IsActive { get; set; }
    }
}
