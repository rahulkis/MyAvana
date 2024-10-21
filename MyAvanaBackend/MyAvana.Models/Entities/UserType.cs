using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class UserType
    {
        public int UserTypeId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
