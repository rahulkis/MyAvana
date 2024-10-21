using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class WebLogin
    {
        [JsonProperty(PropertyName = "UserId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "UserEmail")]
        public string UserEmail { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        [JsonProperty(PropertyName = "CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "IsActive")]
        public bool? IsActive { get; set; }

        [JsonProperty(PropertyName = "UserType")]
        public bool? UserType { get; set; }
        [JsonProperty(PropertyName = "UserTypeId")]
        public int? UserTypeId { get; set; }

        [JsonProperty(PropertyName = "OwnerSalonId")]
        public int? OwnerSalonId { get; set; }

        [JsonProperty(PropertyName = "SalonOwner")]
        public string SalonOwner { get; set; }

        [JsonProperty(PropertyName = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class Imagerequest
    {
        public string fileData { get; set; }

    }

    public class fileData
    {
        public string access_token { get; set; }
        public string ImageURL { get; set; }
        public string user_name { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public bool TwoFactor { get; set; }
        public string HairType { get; set; }
    }

    public class UserSalonOwnerModel
    {
        public int SalonOwnerId { get; set; }
        public int SalonId { get; set; }
        public string SalonName { get; set; }
    }
    public class WebLoginModel
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public bool? UserType { get; set; }
        public int LoggedInUserId { get; set; }

        public int? UserTypeId { get; set; }
        public List<UserSalonOwnerModel> userSalons { get; set; }

    }
    public class ResetPasswordModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }


    }

    public class ForgotPassword
    {
        public string contentType { get; set; }
        public string serializerSettings { get; set; }
        public int statusCode { get; set; }
        public string value { get; set; }
    }
    public class SetPasswordAdmin
    {
        public string Email { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }
    }

    public class UserType
    {
        public int UserTypeId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public enum UserTypeEnum
    {
        Admin = 1, B2B = 2, DataEntry = 3
    }
}
