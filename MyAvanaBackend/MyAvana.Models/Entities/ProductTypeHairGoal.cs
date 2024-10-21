using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductTypeHairGoal
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        [ForeignKey("HairGoalId")]
        public int HairGoalId { get; set; }
        public virtual HairGoal HairGoal { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
