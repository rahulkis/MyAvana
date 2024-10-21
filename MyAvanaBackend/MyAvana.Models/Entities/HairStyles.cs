using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyAvana.Models.Entities
{
    public class HairStyles
    {
        [Key]
        public int Id { get; set; }
        public string Style { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
