using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class BrandHairGoal
    {
        [Key]
        public int BrandHairGoalId { get; set; }

        [ForeignKey("HairGoalId")]
        public int? HairGoalId { get; set; }
        public virtual HairGoal HairGoal { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool IsActive { get; set; }
    }
}
