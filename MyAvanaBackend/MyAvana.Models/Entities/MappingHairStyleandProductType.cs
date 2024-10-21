using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class MappingHairStyleandProductType
    {
        [Key]
        public int Id { get; set; }
        public int HairStyleAnswerId{ get; set; }
        public int QuestionId { get; set; }
        public int ProductTypeId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
