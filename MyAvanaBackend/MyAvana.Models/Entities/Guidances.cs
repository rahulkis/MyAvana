using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace MyAvana.Models.Entities
{
    public class Guidances
    {
        [Key]
        public int Id { get; set; }
        public string Guidance { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
