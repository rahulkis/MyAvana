using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyAvana.Models.Entities
{
    public class HairGoalVideoMapping
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MediaLinkEntityId")]
        public int MediaLinkEntityId { get; set; }
        public virtual MediaLinkEntity MediaLinkEntity { get; set; }
        [ForeignKey("HairGoalId")]
        public int HairGoalId { get; set; }
        public virtual HairGoal HairGoal { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
