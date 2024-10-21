﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductIndicator
    {
        [Key]
        public int ProductIndicatorId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
