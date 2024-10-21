using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }
        public string Country { get; set; }
    }
}
