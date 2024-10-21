using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MyAvana.CRM.Api.Services
{
    public class WebService : IWebLogin
    {
        private readonly AvanaContext _context;
        private readonly IEmailService _emailService;
        private readonly Logger.Contract.ILogger _logger;
        public WebService(AvanaContext avanaContext, IEmailService emailService,Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _emailService = emailService;
            _logger = logger;
        }
        public WebLogin Login(WebLogin webLogin)
        {
            try
            {
                WebLogin objWeb = _context.WebLogins.Where(x => x.UserEmail == webLogin.UserEmail && x.Password == webLogin.Password && x.IsActive == true).FirstOrDefault();
                if (objWeb != null)
                {
                    webLogin.UserId = objWeb.UserId;
                    webLogin.UserEmail = objWeb.UserEmail;
                    webLogin.Password = objWeb.Password;
                    webLogin.IsActive = objWeb.IsActive;
                    //webLogin.OwnerSalonId = objWeb.OwnerSalonId;

                    return objWeb;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: Login, Email:" + webLogin.UserEmail + ", Error: " + ex.Message, ex);
                WebLogin objWeb = new WebLogin();
                objWeb.UserEmail = ex.Message;
                return objWeb;
            }
        }

        public List<WebLoginDetails> GetUsers(string id)
        {
            try
            {
                var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == Convert.ToInt32(id)).UserTypeId;
                if (userType == (int)UserTypeEnum.B2B)
                {
                    var salonIds = _context.SalonsOwners.Where(x => x.UserId == Convert.ToInt32(id) && x.IsActive == true).Select(x => x.SalonId).ToArray();
                    var userIds = _context.SalonsOwners.Where(u => salonIds.Any(x => x == u.SalonId) && u.IsActive == true).Select(x => x.UserId).ToArray();
                    List<WebLoginDetails> webLogins = _context.WebLogins.Where(x => x.IsActive == true).Select(x => new WebLoginDetails
                    {
                        UserId = x.UserId,
                        UserEmail = x.UserEmail,
                        Password = x.Password,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        IsActive = x.IsActive,
                        UserType = x.UserType,
                        UserTypeId = x.UserTypeId,
                        SalonOwner = string.Join(",", _context.SalonsOwners.Where(z => z.IsActive == true && z.UserId == x.UserId).Join(
                    _context.Salons,
                    SalonsOwner => SalonsOwner.SalonId,
                    Salon => Salon.SalonId,
                    (SalonsOwner, Salon) => new UserSalonOwnerModel
                    {
                        SalonName = Salon.SalonName
                    }).Select(z => z.SalonName).ToArray())
                    }).Where(u => userIds.Any(x => x == u.UserId)).OrderByDescending(x => x.CreatedOn).ToList();

                    
                    return webLogins;
                }
                else
                {
                    List<WebLoginDetails> webLogins = _context.WebLogins.Where(x => x.IsActive == true).Select(x => new WebLoginDetails
                    {
                        UserId = x.UserId,
                        UserEmail = x.UserEmail,
                        Password = x.Password,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        IsActive = x.IsActive,
                        UserType = x.UserType,
                        UserTypeId = x.UserTypeId,
                        SalonOwner = string.Join(",", _context.SalonsOwners.Where(z => z.IsActive == true && z.UserId == x.UserId).Join(
                    _context.Salons,
                    SalonsOwner => SalonsOwner.SalonId,
                    Salon => Salon.SalonId,
                    (SalonsOwner, Salon) => new UserSalonOwnerModel
                    {
                        SalonName = Salon.SalonName
                    }).Select(z => z.SalonName).ToArray())
                    }).OrderByDescending(x => x.CreatedOn).ToList();

                    return webLogins;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetUsers, UserId:" + id + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public WebLoginDetails GetUserByid(WebLoginDetails webLogin)
        {
            try
            {
                WebLoginDetails getUser = _context.WebLogins.Where(x => x.UserId == webLogin.UserId).Select(x => new WebLoginDetails
                {
                    UserEmail = x.UserEmail,
                    UserId = x.UserId,
                    UserType = x.UserType,
                    UserTypeId = x.UserTypeId,
                    userSalons = _context.SalonsOwners.Where(s => s.IsActive == true && s.UserId == x.UserId).Select(s => new UserSalonOwnerModel
                    {
                        SalonId = s.SalonId,
                        SalonName = _context.Salons.Where(p => p.SalonId == s.SalonId).Select(p => p.SalonName).FirstOrDefault(),
                        SalonOwnerId = s.SalonOwnerId
                    }).ToList()
                }).FirstOrDefault();
                return getUser;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetUserByid, UserId:" + webLogin.UserId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "@#$abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
        private static string CreateRandomCode(int passwordLength)
        {
            string allowedChars = "0123456789";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
        public WebLoginDetails AddNewUser(WebLoginDetails webLogin)
        {
            try
            {
                EmailInformation emailInformation = new EmailInformation();
                var isNewUser = false;
                var salonIds = webLogin.userSalons?.Select(x => x.SalonId);
                if (webLogin.UserId != 0)
                {
                    var objuser = _context.WebLogins.Where(x => x.UserId == webLogin.UserId).Select(x => new WebLoginDetails
                    {
                        UserEmail = x.UserEmail,
                        UserType = x.UserType,
                        UserTypeId=x.UserTypeId

                    }).FirstOrDefault();
                    if(objuser.UserEmail != webLogin.UserEmail)
                    {
                        var web = _context.WebLogins.Where(x => x.UserId == webLogin.UserId).FirstOrDefault();
                        if (web != null)
                        {
                            web.UserEmail = webLogin.UserEmail;
                            _context.SaveChanges();
                        }
                    }
                    if (objuser.UserTypeId != webLogin.UserTypeId)
                    {
                        var web = _context.WebLogins.Where(x => x.UserId == webLogin.UserId).FirstOrDefault();
                        if (web != null)
                        {
                            web.UserTypeId = webLogin.UserTypeId;
                            _context.SaveChanges();
                        }
                    }

                    if (salonIds?.Count() > 0)//  && objuser.UserTypeId == (int)UserTypeEnum.B2B) 
                    {
                        var userSalons = _context.SalonsOwners.Where(s => s.IsActive == true && s.UserId == webLogin.UserId).ToList();
                        userSalons.ForEach(x => x.IsActive = false);
                        _context.SalonsOwners.UpdateRange(userSalons);
                        if (webLogin.UserTypeId == (int)UserTypeEnum.B2B)
                        {
                            foreach (var userSalon in salonIds)
                            {
                                SalonsOwner objSalonOwner = new SalonsOwner();
                                objSalonOwner.SalonId = userSalon;
                                objSalonOwner.IsActive = true;
                                objSalonOwner.UserId = webLogin.UserId;
                                _context.SalonsOwners.Add(objSalonOwner);
                            }
                        }
                    }
                    _context.SaveChanges();
                }
                else
                {
                    var password = CreateRandomPassword(9);
                    WebLogin objWeb = new WebLogin();
                    objWeb.UserEmail = webLogin.UserEmail;
                    objWeb.Password = password;
                    objWeb.CreatedBy = "Admin";
                    objWeb.IsActive = true;
                    objWeb.CreatedOn = DateTime.UtcNow;
                   
                    objWeb.UserTypeId = webLogin.UserTypeId;
                    _context.WebLogins.Add(objWeb);
                    _context.SaveChanges();
                    if (salonIds == null || salonIds?.Count() == 0)
                    {
                        salonIds = _context.SalonsOwners.Where(x => x.UserId == webLogin.LoggedInUserId && x.IsActive == true).Select(x => x.SalonId).ToArray();
                    }
                    
                    if (salonIds?.Count() > 0 && webLogin.UserTypeId == (int)UserTypeEnum.B2B)
                    {
                        foreach (var userSalon in salonIds)
                        {
                            SalonsOwner objSalonOwner = new SalonsOwner();
                            objSalonOwner.SalonId = userSalon;
                            objSalonOwner.IsActive = true;
                            objSalonOwner.UserId = objWeb.UserId;
                            _context.SalonsOwners.Add(objSalonOwner);
                        }
                    }
                    emailInformation.Email = webLogin.UserEmail;
                    emailInformation.Name = webLogin.UserTypeId == 1 ? "Admin" : (webLogin.UserTypeId == 2 ? "B2B" : (webLogin.UserTypeId == 3 ? "DataEntry" : ""));
                    //emailInformation.Name = webLogin.UserType == true ? "Admin" : "B2B";
                    emailInformation.Code = password;
                    isNewUser = true;
                }
                _context.SaveChanges();

                if (isNewUser)
                    _emailService.SendEmail("REGUSER", emailInformation);

                return webLogin;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AddNewUser, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteUser(WebLogin webLogin)
        {
            try
            {
                var objUser = _context.WebLogins.FirstOrDefault(x => x.UserId == webLogin.UserId);
                {
                    if (objUser != null)
                    {
                        objUser.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteUser, UserId:" + webLogin.UserId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<UserSalonOwnerModel> GetOwnerSalons()
        {
            try
            {
                List<UserSalonOwnerModel> userSalons = _context.SalonsOwners.Where(x => x.IsActive == true).Select(x => new UserSalonOwnerModel
                {
                    SalonId = x.SalonId,
                    SalonOwnerId = x.SalonOwnerId,
                    SalonName = _context.Salons.Where(s => s.SalonId == x.SalonId).Select(s => s.SalonName).FirstOrDefault()
                }).OrderByDescending(x => x.SalonOwnerId).ToList();
                return userSalons;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetOwnerSalons, Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool  ResetAdminPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                var objUser = _context.WebLogins.FirstOrDefault(x => x.UserId == resetPassword.UserId);
                if (objUser != null)
                {
                    objUser.Password = resetPassword.Password;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: ResetAdminPassword, UserId:" + resetPassword.UserId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }


        public (JsonResult result, bool success, string error) ForgotAdminPassword(string email)
        {
            try
            {
                WebLoginDetails getUser = _context.WebLogins.Where(x => x.UserEmail == email).Select(x => new WebLoginDetails
                {
                    UserEmail = x.UserEmail,
                    UserId = x.UserId,
                    UserType = x.UserType,
                    UserTypeId = x.UserTypeId,
                    Password=x.Password
                   
                }).FirstOrDefault();

                if (getUser != null)
                {
                    var emailRes = SendForgotPasswordEmail(getUser);
                    if (!emailRes.success)
                        _logger.LogError("Method: ForgotAdminPassword, Email:" + email + ", Error: " + emailRes.error);

                    return (new JsonResult("We have sent you a secure code on your email. Please use that to reset your password.")
                    { StatusCode = (int)HttpStatusCode.OK }, true, "");

                }
                else
                {
                    _logger.LogError("Method: ForgotAdminPassword, Email:" + email + ", Error: Email id does not exist in application!");
                    return (new JsonResult(""), false, "Email id does not exist in application!");
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: ForgotAdminPassword, Email:" + email + ", Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Some server error occured. Please try again!!");
            }
        }
        private (bool success, string error) SendForgotPasswordEmail(WebLoginDetails entity)
        {
            try
            {
                string activationCode = CreateRandomCode(4);
                _context.UserCodes.Where(s => s.Email == entity.UserEmail).ToList().ForEach(s => s.IsActive = false);
                _context.SaveChanges();
                _context.UserCodes.Add(new UserCodes() { Email = entity.UserEmail, Code = activationCode, IsActive = true, CreatedDate = DateTime.UtcNow });
                _context.SaveChanges();

                EmailInformation emailInformation = new EmailInformation
                {
                    Code = activationCode,
                    Email = entity.UserEmail,
                    Name = entity.UserTypeId == (int)UserTypeEnum.Admin ? "Admin" : "B2B"
                };

                var emailRes = _emailService.SendEmail("FGTADMINPASS", emailInformation);
                return emailRes;
            }
            catch(Exception Ex)
            {
                _logger.LogError("Method: SendForgotPasswordEmail, Email:" + entity.UserEmail + ", Error: " + Ex.Message, Ex);
                return (false, "");
            }

        }
        public  (bool success, string error) SetAdminPass(SetPassword setPassword)
        {
            try
            {
                var objUser = _context.WebLogins.FirstOrDefault(x => x.UserEmail == setPassword.Email);
                if (objUser!=null)
                {
                    var result = _context.UserCodes.Where(s => s.Email == objUser.UserEmail && s.IsActive && s.Code == setPassword.Code.Trim()).FirstOrDefault();
                    if (result != null)
                    { ResetPasswordModel obj = new ResetPasswordModel();
                        obj.Password = setPassword.Password;
                        obj.UserId = objUser.UserId;
                       var success= ResetAdminPassword(obj);
                        if(!success)
                        {
                            _logger.LogError("Method: SetAdminPass, Email:" + setPassword.Email + ", Error: Error in updating the password.");
                            return (false, "Error in updating the password.");
                        }
                        else
                        {
                            return (true, "");
                        }
                    }
                    else
                    {
                        _logger.LogError("Method: SetAdminPass, Email:" + setPassword.Email + ", Error: Invalid reset code.");
                        return (false, "Invalid reset code.");
                    }
                }
                else
                {
                    _logger.LogError("Method: SetAdminPass, Email:" + setPassword.Email + ", Error: Invalid email address.");
                    return (false, "Invalid email address.");
                }
               
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SetAdminPass, Email:" + setPassword.Email + ", Error: " + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }

        public int GetHairStrandNotificationCount()
        {
            try
            {
                int count = _context.HairStrandUploadNotification.Count(x => x.IsRead == false);
                return count;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairStrandNotificationCount, Error:" + Ex.Message, Ex);
                return 0;
            }
        }
        public List<UserType> GetUserTypeList()
        {
            try
            {
                List<UserType> lstUserType = _context.UserTypes.Where(x => x.IsActive == true).ToList();
                return lstUserType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetUserTypeList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public int GetHairDiaryNotificationCount()
        {
            try
            {
                int count = _context.DailyRoutineTracker.Count(x => x.IsRead == false);
                return count;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairDiaryNotificationCount, Error:" + Ex.Message, Ex);
                return 0;
            }
        }

    }
}
