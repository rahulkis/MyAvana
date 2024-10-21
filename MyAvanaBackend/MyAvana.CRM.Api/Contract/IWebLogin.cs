using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAvanaApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MyAvana.CRM.Api.Contract
{
    public interface IWebLogin
    {
        WebLogin Login(WebLogin webLogin);
        //bool AddNewUser(WebLogin webLogin);
        List<WebLoginDetails> GetUsers(string id);
        WebLoginDetails AddNewUser(WebLoginDetails webLogin);
        WebLoginDetails GetUserByid(WebLoginDetails webLogin);
        bool DeleteUser(WebLogin webLogin);
        //bool DeleteUser(WebLogin webLogin);
        List<UserSalonOwnerModel> GetOwnerSalons();
        bool ResetAdminPassword(ResetPasswordModel resetPassword);

        (JsonResult result, bool success, string error) ForgotAdminPassword(string email);
        (bool success, string error) SetAdminPass(SetPassword setPassword);

        int GetHairStrandNotificationCount();
        List<UserType> GetUserTypeList();
        int GetHairDiaryNotificationCount();
    }
}
