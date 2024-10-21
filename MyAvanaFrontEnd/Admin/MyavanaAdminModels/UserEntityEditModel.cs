using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class UserEntityEditModel
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Id { get; set; }
        public bool? IsInfluencer { get; set; }



    }
}
