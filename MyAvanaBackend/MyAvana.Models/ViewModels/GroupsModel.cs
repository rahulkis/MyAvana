using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class GroupsModel
    {
        public string HairType { get; set; }
        public int Id { get; set; }
        public bool IsGroupMember { get; set; }
        public List<GpUsers> Users { get; set; }
        public bool ShowOnMobile { get; set; }
        public bool IsPublic { get; set; }
    }

    public class GpUsers
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public class GroupModel
    {
        public string HairType { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdate { get; set; }
        public bool AccessRequested { get; set; }
        public int Id { get; set; }
        public DateTime? UpdatedOn { get; set; }
       
        public int? UpdatedByUserId { get; set; }
    }
}
