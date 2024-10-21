using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class CustomerType
    {
        [Key]
        public int CustomerTypeId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
