using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class BrandHairChallenge
    {
        [Key]
        public int BrandHairChallengeId { get; set; }

        [ForeignKey("HairChallengeId")]
        public int? HairChallengeId { get; set; }
        public virtual HairChallenges HairChallenges { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool IsActive { get; set; }
    }
}
