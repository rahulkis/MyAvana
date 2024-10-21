using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class Group
    {
        public string HairType { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public bool IsUpdate { get; set; }
        public bool ShowOnMobile { get; set; }
    }

    public class GroupsModel
    {
        public string HairType { get; set; }
        public List<Users> Users { get; set; }
        public bool ShowOnMobile { get; set; }
        public bool IsPublic { get; set; }
    }

    public class Users
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int? CustomerTypeId { get; set; }
    }
    public class GroupRequestModel
    {
        public string HairType { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool AccessRequested { get; set; }
        public int Id { get; set; }
    }
    public class RequestApproveModel
    {
        public int Id { get; set; }
        public int? UpdatedByUserId { get; set; }
    }

    public enum CustomerTypeEnum
    {
        DigitalAnalysis = 1, HairKit = 2, HairKitPlus = 3, Legacy = 4
    }
}
