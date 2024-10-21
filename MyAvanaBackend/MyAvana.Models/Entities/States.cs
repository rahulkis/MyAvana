using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class States
    {
        [Key]
        public int StateId { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public virtual Countries Countries { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
    }
}
