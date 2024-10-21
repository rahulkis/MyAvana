using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class MappingHairGoalAndProductTag
    {
        [Key]
        public int Id { get; set; }
        public string GoalDescription { get; set; }
        public int ProductTagsId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
