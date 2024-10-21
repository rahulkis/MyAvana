using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class HairChallengeVideoMapping
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MediaLinkEntityId")]
        public int MediaLinkEntityId { get; set; }
        public virtual MediaLinkEntity MediaLinkEntity { get; set; }
        [ForeignKey("HairChallengeId")]
        public int HairChallengeId { get; set; }
        public virtual HairChallenges HairChallenges { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
