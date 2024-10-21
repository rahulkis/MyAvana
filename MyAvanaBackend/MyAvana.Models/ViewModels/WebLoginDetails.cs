using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class WebLoginDetails
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? UserType { get; set; }
        public List<UserSalonOwnerModel> userSalons { get; set; }
        public string SalonOwner { get; set; }
        public int? UserTypeId { get; set; }
        public int LoggedInUserId { get; set; }

    }

}
