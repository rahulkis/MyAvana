using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductTypeHairChallenge
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        [ForeignKey("HairChallengeId")]
        public int HairChallengeId { get; set; }
        public virtual HairChallenges HairChallenges { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
