using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.Auth.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.IServices;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyAvana.Auth.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly Logger.Contract.ILogger _logger;
        private readonly AvanaContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly ICryptoService _cryptoService;
        private readonly IOptions<Audience> _settings;
        private readonly HubSpotApi _hubSpotApi;
        public AccountService(IConfiguration configuration,
                                Logger.Contract.ILogger logger,
                                AvanaContext context,
                                UserManager<UserEntity> userManager,
                                SignInManager<UserEntity> signInManager,
                                ICryptoService cryptoService,
                                IHttpClientFactory httpClientFactory,
                                IEmailService emailService,
                                IOptions<Audience> settings)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
            _settings = settings;
            _cryptoService = cryptoService;
            _configuration = configuration;
            _hubSpotApi = new HubSpotApi(_configuration.GetSection("Hubspot:Key").Value);
        }

        public (JsonResult result, bool success, string error) SignIn(Authentication authentication)
        {
            try
            {
                var result = _signInManager.PasswordSignInAsync(authentication.UserName, authentication.Password, false, false).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = _userManager.FindByEmailAsync(authentication.UserName).GetAwaiter().GetResult();
                    if (user.Active)
                    {
                        var Token = GenerateToken(user);
                        return (Token, result.Succeeded, "");
                    }
                    _logger.LogError("Method:SignIn, UserName:" + authentication.UserName + ", Error:Please activate your account.");
                    return (new JsonResult(""), false, "Please activate your account.");
                }
                _logger.LogError("Method: SignIn, UserName:" + authentication.UserName + ", Error: Invalid Credentials");
                return (new JsonResult(""), false, "Invalid Credentials");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SignIn, UserName:" + authentication.UserName + ", Error: " + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }
        public (bool success, string error) SignUp(Signup signup)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    UserEntity entity;
                    Claim[] claims;

                    IdentityResult result = CreateUser(signup, out entity, out claims);
                    if (!result.Succeeded)
                    {
                        var firstError = result.Errors.FirstOrDefault()?.Description;
                        return (result.Succeeded, firstError);
                    }
                    else
                    {
                        _userManager.AddToRoleAsync(entity, "User").Wait();
                        _userManager.AddClaimsAsync(entity, claims).Wait();
                        _userManager.UpdateAsync(entity).Wait();
                        CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                        customerTypeHistory.CustomerId = entity.Id;
                        customerTypeHistory.OldCustomerTypeId = null;
                        customerTypeHistory.NewCustomerTypeId = (int)(entity.CustomerTypeId);
                        customerTypeHistory.CreatedOn = DateTime.Now;
                        customerTypeHistory.IsActive = true;
                        customerTypeHistory.UpdatedByUserId = null;
                        customerTypeHistory.Comment = "New customer signup from mobile.";
                        _context.CustomerTypeHistory.Add(customerTypeHistory);
                        _context.SaveChanges();
                        _context.SaveChanges();
                    }


                    var emailRes = SendEmail(entity, Operation.CodeVerify, "REG");
                    if (entity.BuyHairKit == true)
                    {
                        AddHairAnalysisStatus(entity.Id, signup.KitSerialNumber);
                    }

                    if (!emailRes.success)
                        _logger.LogError("Method: SignUp, Error: " + emailRes.error);
                    dbContextTransaction.Commit();
                    return (result.Succeeded, "");

                }
                catch (Exception Ex)
                {
                    _logger.LogError("Method: SignUp, Error: " + Ex.Message, Ex);
                    return (false, "Something went wrong. Please try again later.");
                }
            }
        }
        public (JsonResult result, bool success, string error) ForgetPass(string email)
        {
            try
            {
                var result = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
                if (result != null)
                {
                    var emailRes = SendEmail(result, Operation.ForgetPassword, "FGTPASS");
                    if (!emailRes.success)
                        _logger.LogError("Method: ForgetPass, Email: " + email + ", Error: " + emailRes.error);
                    return (new JsonResult("We have sent you a secure code on your email. Please use that to reset your password.")
                    { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                else
                    _logger.LogError("Method: ForgetPass, Email: " + email + ", Error: Email id not exist in application!!!");
                return (new JsonResult(""), false, "Email id not exist in application!!!");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: ForgetPass, Email: " + email + ", Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Some server error occured. Please try again!!");
            }
        }
        public (JsonResult result, bool success, string error) ActivateUser(CodeVerify codeVerify)
        {
            try
            {
                var codeResponse = GetCodeEntity(codeVerify, Operation.CodeVerify);
                if (codeResponse.CodeEntity != null)
                {
                    if (codeResponse.CodeEntity.Code.ToLower() == codeVerify.Code.ToLower())
                    {
                        if (string.IsNullOrEmpty(codeResponse.entity.HubSpotContactId))
                        {
                            var res = CreateContact(codeResponse.entity);
                        }
                        codeResponse.CodeEntity.IsActive = false;
                        codeResponse.entity.Active = true;
                        codeResponse.entity.EmailConfirmed = true;
                        _context.SaveChanges();
                        return (GenerateToken(codeResponse.entity), true, "");
                    }
                    _logger.LogError("Method: ActivateUser, Error: Invalid Code.");
                    return (new JsonResult(""), false, "Invalid Code.");
                }
                _logger.LogError("Method: ActivateUser, Error: " + codeResponse.error);
                return (new JsonResult(""), false, codeResponse.error);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: ActivateUser, Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again later.");
            }
        }

        private bool CreateContact(UserEntity entity)
        {
            try
            {

                var contact = _hubSpotApi.Contact.CreateOrUpdate(new ContactHubSpotModel()
                {
                    Email = entity.Email,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Phone = entity.PhoneNumber
                });
                entity.HubSpotContactId = contact.Id.ToString();
                return true;
            }
            catch (HubSpot.NET.Core.HubSpotException Ex)
            {
                if (!string.IsNullOrEmpty(Ex.RawJsonResponse))
                {
                    var response = JsonConvert.DeserializeObject<MyAvanaApi.Models.ViewModels.HubSpotException>(Ex.RawJsonResponse);
                    if (response.message == "Contact already exists")
                    {
                        entity.HubSpotContactId = response.identityProfile.vid.ToString();
                    }
                }
                _logger.LogError("Method: CreateContact, Email:" + entity.Email + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }


        private IdentityResult CreateUser(Signup signUp, out UserEntity entity, out Claim[] claims)
        {
            try
            {
                string accountNo = signUp.FirstName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                entity = new UserEntity
                {
                    AccountNo = accountNo,
                    Email = signUp.Email,
                    UserName = signUp.Email,
                    Active = false,
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    EmailConfirmed = false,
                    PhoneNumber = signUp.PhoneNo,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CountryCode = signUp.CountryCode,
                    IsProCustomer = true,
                    IsPaid = false,
                    CustomerTypeId = (int)CustomerTypeEnum.HairKit,
                    BuyHairKit = signUp.BuyHairKit
                };
                claims = new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
                };
                return _userManager.CreateAsync(entity, signUp.Password).GetAwaiter().GetResult();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateUser, Error: " + Ex.Message, Ex);
                entity = null;
                claims = null;
                return null;
                //return IdentityResult.Failed(new IdentityError { Description = "Something went wrong. Please try again later." });
            }
        }
        private (bool success, string error) SendEmail(UserEntity entity, Operation operation, string template)
        {
            try
            {
                string activationCode = _cryptoService.GetRandomKey(4);
                _context.CodeEntities.Where(s => s.AccountId == entity.AccountNo && s.OpCode == operation).ToList().ForEach(s => s.IsActive = false);
                _context.SaveChanges();
                _context.CodeEntities.Add(new CodeEntity() { Id = Guid.NewGuid(), AccountId = entity.AccountNo, Code = activationCode, OpCode = operation, IsActive = true, CreatedDate = DateTime.UtcNow });
                _context.SaveChanges();

                EmailInformation emailInformation = new EmailInformation
                {
                    Code = activationCode,
                    Email = entity.Email,
                    Name = entity.FirstName + " " + entity.LastName,

                };

                var emailRes = _emailService.SendEmail(template, emailInformation);
                return emailRes;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SendEmail, Error: " + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }

        }
        private JsonResult GenerateToken(UserEntity user)
        {
            try
            {
                var now = DateTime.UtcNow;


                var claims = new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Secret));

                var jwt = new JwtSecurityToken(
                        issuer: _settings.Value.Iss,
                        audience: _settings.Value.Aud,
                        claims: claims,
                        notBefore: now,
                        expires: now.Add(TimeSpan.FromDays(30)),
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                bool? isOnTrial = false;
                var trialDate = user.TrialExpiredOn;
                if (user.IsOnTrial == true && user.TrialExpiredOn >= DateTime.Now)
                {
                    isOnTrial = user.IsOnTrial;
                }

                var response = new JsonResult(new Dictionary<string, object>
            {
                { "access_token" , encodedJwt },
                { "expires_in" , (int) TimeSpan.FromDays(30).TotalSeconds },
                {"user_name",user.UserName },
                {"Email",user.Email },
                {"Name",user.FirstName +" "+ user.LastName },
                {"AccountNo",user.AccountNo },
                {"TwoFactor",user.TwoFactorEnabled },
                {"hairType",user.HairType },
                {"imageURL",user.ImageURL },
                {"Id", user.Id },
                {"IsProCustomer", user.IsProCustomer },
                {"IsPaid", user.IsPaid },
                {"CustomerTypeId", user.CustomerTypeId },
                {"IsOnTrial", isOnTrial },
                {"CountryCode", user.CountryCode },
                {"PhoneNumber", user.PhoneNumber },
                {"IsInfluencer", user.IsInfluencer },
               // {"SubscriptionType", user.SubscriptionType ?? 1 },
                //{"IsHairAIAllowed", user.IsHairAIAllowed ?? false},
            });

                return response;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GenerateToken, Error: " + Ex.Message, Ex);
                return null;
            }

        }
        private (CodeEntity CodeEntity, string error, UserEntity entity) GetCodeEntity(CodeVerify codeVerify, Operation operation)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == codeVerify.Email.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    var result = _context.CodeEntities.Where(s => s.AccountId == user.AccountNo && s.IsActive && s.OpCode == operation && s.Code == codeVerify.Code.Trim()).FirstOrDefault();
                    if (result != null)
                        return (result, "", user);
                }
                _logger.LogError("Method: GetCodeEntity, Email:" + codeVerify.Email + " , Error: Invalid email address.");
                return (null, "Invalid email address.", null);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetCodeEntity, Error: " + Ex.Message, Ex);
                return (null, "Something went wrong. Please try again later.", null);
            }
        }
        public async Task<(bool success, string error)> SetPass(SetPassword setPassword)
        {
            try
            {
                var codeResponse = GetCodeEntity(new CodeVerify() { Email = setPassword.Email, Code = setPassword.Code }, Operation.ForgetPassword);
                if (codeResponse.CodeEntity != null)
                {
                    var user = await _userManager.FindByEmailAsync(setPassword.Email);
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, setPassword.Password);
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return (true, "");
                    _logger.LogError("Method: SetPass, Email:" + setPassword.Email + " , Error: Error in updating the password.");
                    return (false, "Error in updating the password.");

                }
                return (false, codeResponse.error);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SetPass, Email:" + setPassword.Email + " , Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
        public async Task<(bool success, string error)> ChangePass(SetPassword setPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(setPassword.Email);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, setPassword.Password);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return (true, "");
                _logger.LogError("Method: ChangePass, Email:" + setPassword.Email + " , Error: Error in updating the password.");
                return (false, "Error in updating the password.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: ChangePass, Email:" + setPassword.Email + " , Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
        public UserEntity GetAccountNo(ClaimsPrincipal user)
        {
            try
            {
                Claim claim = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                UserEntity usr = _userManager.GetUsersForClaimAsync(claim).GetAwaiter().GetResult().FirstOrDefault();
                if (usr == null)
                {
                    var email = user.Identities.FirstOrDefault().Name;
                    usr = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
                }
                return usr;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetAccountNo, Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public string GetHailAnalysisIemage(Guid UserId)
        {
            try
            {
                var imageQuestionaire = _context.Questionaires.Where(x => x.QuestionId == 22 && x.UserId == UserId.ToString() && x.IsActive == true).LastOrDefault();
                if (imageQuestionaire != null)
                {

                    return imageQuestionaire.DescriptiveAnswer;
                }
                else
                    return null;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHailAnalysisIemage, Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public (JsonResult result, bool success, string error) ResendCode(string email)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    if (!user.Active)
                    {
                        var mailStatus = SendEmail(user, Operation.CodeVerify, "REG");
                        if (mailStatus.success)
                            return (new JsonResult("Activation code has been sent to your email address.") { StatusCode = (int)HttpStatusCode.OK }, true, "");
                        _logger.LogError("Method: ResendCode, Email:" + email + " , Error: Error in sending the emails.");
                        return (new JsonResult(""), false, "Error in sending the emails.");
                    }
                    _logger.LogError("Method: ResendCode, Email:" + email + " , Error: User is already activated. Please use forget password link, If you forget your password.");
                    return (new JsonResult(""), false, "User is already activated. Please use forget password link, If you forget your password.");
                }
                _logger.LogError("Method: ResendCode, Email:" + email + " , Error: Invalid user.");
                return (new JsonResult(""), false, "Invalid user.");

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: ResendCode, Email:" + email + " , Error:" + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again later.");
            }
        }




        public (UserEntity user, bool success, string error) GetAccountNo(string email)
        {
            try
            {
                var resValue = _context.Users.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefault();
                return (resValue, true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetAccountNo, Error: " + Ex.Message, Ex);
                return (null, false, "");
            }
        }

        public (string userId, string error) WebSignup(Signup signup)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(signup.Password))
                    {
                        string str = RandomPassword();
                        signup.Password = str;
                    }
                    UserEntity entity;
                    Claim[] claims;

                    IdentityResult result = CreateWebUser(signup, out entity, out claims);
                    if (!result.Succeeded)
                    {
                        var firstError = result.Errors.FirstOrDefault()?.Description;
                        return (null, firstError);
                    }
                    else
                    {
                        _userManager.AddToRoleAsync(entity, "User").Wait();
                        _userManager.AddClaimsAsync(entity, claims).Wait();
                        _userManager.UpdateAsync(entity).Wait();
                        CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                        customerTypeHistory.CustomerId = entity.Id;
                        customerTypeHistory.OldCustomerTypeId = null;
                        customerTypeHistory.NewCustomerTypeId = (int)(entity.CustomerTypeId);
                        customerTypeHistory.CreatedOn = DateTime.Now;
                        customerTypeHistory.IsActive = true;
                        customerTypeHistory.UpdatedByUserId = signup.CreatedByUserId != 0 ? signup.CreatedByUserId : null;
                        customerTypeHistory.Comment = "New customer signup from customer portal.";
                        _context.CustomerTypeHistory.Add(customerTypeHistory);
                        _context.SaveChanges();
                    }


                    //var emailRes = SendEmail(entity, Operation.CodeVerify, "REG");
                  //  var emailRes = SendEmail(entity, Operation.ForgetPassword, "REGHAIRKITUSER");
                    var emailRes = SendEmail(entity, Operation.ForgetPassword, "REGISTERUSER");
                    if (!emailRes.success)
                        _logger.LogError("Method: WebSignup, Email:" + signup.Email + " , Error:" + emailRes.error);


                    //EmailInformation emailInformation = new EmailInformation
                    //{
                    //    Email = entity.Email,
                    //    Name = entity.FirstName + " " + entity.LastName,
                    //    Code = signup.Password
                    //};

                    //var emailRes = _emailService.SendEmail("REGHAIRKITUSER", emailInformation);
                   
                    dbContextTransaction.Commit();
                    var user = _userManager.FindByEmailAsync(signup.Email);
                    if (signup.BuyHairKit == true)
                    {
                        AddHairAnalysisStatus(user.Result.Id, signup.KitSerialNumber);
                    }

                    return (user.Result.Id.ToString(), "");

                }
                catch (Exception Ex)
                {
                    _logger.LogError("Method: WebSignup, Email:" + signup.Email + " , Error:" + Ex.Message, Ex);
                    return (null, "Something went wrong. Please try again later.");
                }
            }
        }
        public bool AddHairAnalysisStatus(Guid userId, string serNo)
        {
            try
            {
                StatusTracker obj = new StatusTracker();
                obj.CustomerId = userId;
                obj.RegistrationDate = DateTime.Now;
                obj.KitSerialNumber = serNo;
                obj.HairAnalysisStatusId = 1;
                _context.StatusTracker.Add(obj);
                _context.SaveChanges();

                HairAnalysisStatusHistory hairAnalysisStatusHistory = new HairAnalysisStatusHistory
                {
                    NewHairAnalysisStatusId = obj.HairAnalysisStatusId,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    CustomerId = obj.CustomerId
                };
                _context.HairAnalysisStatusHistory.Add(hairAnalysisStatusHistory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AddHairAnalysisStatus, UserId:" + userId.ToString() + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        private IdentityResult CreateWebUser(Signup signUp, out UserEntity entity, out Claim[] claims)
        {
            try
            {
                string accountNo = signUp.FirstName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                entity = new UserEntity
                {
                    AccountNo = accountNo,
                    Email = signUp.Email,
                    UserName = signUp.Email,
                    Active = true,
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    EmailConfirmed = false,
                    PhoneNumber = signUp.PhoneNo,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CountryCode = signUp.CountryCode,
                    CustomerType = signUp.CustomerType == null ? true : false,
                    IsProCustomer = signUp.IsProCustomer,
                    IsPaid = signUp.IsPaid,
                    CustomerTypeId = signUp.CustomerTypeId > 0 ? signUp.CustomerTypeId : signUp.BuyHairKit == true ? (int)CustomerTypeEnum.HairKit : (int)CustomerTypeEnum.HairKit,
                    BuyHairKit = signUp.BuyHairKit,
                    SalonId = signUp.SalonId != null ? signUp.SalonId : null,
                    IsInfluencer=signUp.IsInfluencer
                };
                claims = new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
                };
                var response = _userManager.CreateAsync(entity, signUp.Password).GetAwaiter().GetResult();
                return response;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateWebUser, Error: " + Ex.Message, Ex);
                entity = null;
                claims = null;
                return null;
            }
        }
        //public (bool success, string error) SaveHairAnalysisStatus(StatusTracker tracker)
        //{
        //    try
        //    {
        //        var statusTrackerModel = _context.StatusTracker.FirstOrDefault(x => x.CustomerId == tracker.CustomerId);
        //        if (statusTrackerModel != null)
        //        {
        //            statusTrackerModel.HairAnalysisStatusId = tracker.HairAnalysisStatusId;
        //            statusTrackerModel.LastUpdatedOn = DateTime.Now;
        //            statusTrackerModel.LastModifiedBy = tracker.LastModifiedBy;
        //            // statusTrackerModel.KitSerialNumber = tracker.KitSerialNumber;

        //        }
        //        else
        //        {
        //            StatusTracker obj = new StatusTracker();
        //            obj.CustomerId = tracker.CustomerId;
        //            obj.RegistrationDate = DateTime.Now;
        //            obj.KitSerialNumber = tracker.KitSerialNumber;
        //            obj.HairAnalysisStatusId = 1;
        //            _context.Add(obj);
        //        }
        //        _context.SaveChanges();
        //        return (true, "");
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogError("Method: SaveHairAnalysisStatus, UserId:" + tracker.CustomerId + ", Error: " + Ex.Message, Ex);
        //        return (false, "Something went wrong. Please try again.");
        //    }
        //}
        private IdentityResult CreateWebUserWithPayment(SignupAndPayment signUp, out UserEntity entity, out Claim[] claims)
        {
            string accountNo = signUp.FirstName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            entity = new UserEntity
            {
                AccountNo = accountNo,
                Email = signUp.Email,
                UserName = signUp.Email,
                Active = true,
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                EmailConfirmed = false,
                PhoneNumber = signUp.PhoneNo,
                CreatedAt = DateTimeOffset.UtcNow,
                CountryCode = signUp.CountryCode,
                CustomerType = signUp.CustomerType == null ? true : false,
                IsProCustomer = signUp.IsProCustomer,
                IsPaid = signUp.IsPaid,
                CustomerTypeId = signUp.CustomerTypeId > 0 ? signUp.CustomerTypeId : signUp.BuyHairKit == true ? (int)CustomerTypeEnum.HairKit : (int)CustomerTypeEnum.HairKit,
                BuyHairKit = signUp.BuyHairKit,
                SalonId = signUp.SalonId != null ? signUp.SalonId : null,
                //SubscriptionType = signUp.IsSubscriptionPayment == true ? 1 : 2
            };
            claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
            };
            var response = _userManager.CreateAsync(entity, signUp.Password).GetAwaiter().GetResult();
            return response;
        }
        public bool updateDeviceId(NewUserEntityModel userEntity, string deviceId)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == userEntity.UserName.ToLower()).FirstOrDefault();
                user.DeviceId = deviceId;
                _context.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: updateDeviceId, Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool saveAIResult(UserEntity userEntity, string aiResult)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == userEntity.UserName.ToLower()).FirstOrDefault();
                user.AIResult = aiResult;
                _context.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: saveAIResult, Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public async Task<(bool success, string error)> SetCustomerPass(SetCustomerPassword setPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(setPassword.Email);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, setPassword.Password);
                user.LoginAlert = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    EmailInformation emailInformation = new EmailInformation
                    {
                        Email = setPassword.Email,
                        Name = user.FirstName + " " + user.LastName,
                        Code = setPassword.Password
                    };

                    var emailRes = _emailService.SendEmail("RESETPWD", emailInformation);
                    return (true, "");
                }
                _logger.LogError("Method: SetCustomerPass, Email:" + setPassword.Email + " , Error:Error in updating the password.");
                return (false, "Error in updating the password.");

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SetCustomerPass, Email:" + setPassword.Email + " , Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }

        public (string code, bool success, string error) AlexaLinking(AlexaLinkingModel alexaLinkingModel)
        {
            try
            {
                var result = _signInManager.PasswordSignInAsync(alexaLinkingModel.UserName, alexaLinkingModel.Password, false, false).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = _userManager.FindByEmailAsync(alexaLinkingModel.UserName).GetAwaiter().GetResult();
                    if (user.Active)
                    {
                        //var code = _userManager.GenerateEmailConfirmationTokenAsync(user).GetAwaiter().GetResult();
                        //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        //var Token = GenerateToken(user);ss
                        //var c = _userManager.GetAuthenticatorKeyAsync(user).GetAwaiter().GetResult();
                        return (user.Id.ToString(), result.Succeeded, "");
                    }
                    _logger.LogError("Method: AlexaLinking, UserName:" + alexaLinkingModel.UserName + " , Error:Please activate your account.");
                    return ("", false, "Please activate your account.");
                }
                _logger.LogError("Method: AlexaLinking, UserName:" + alexaLinkingModel.UserName + " , Error:Invalid Credentials");
                return ("", false, "Invalid Credentials");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AlexaLinking, UserName:" + alexaLinkingModel.UserName + " , Error:" + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }

        public (JsonResult token, bool success, string error) GetToken(string code)
        {
            try
            {
                var user = _userManager.FindByIdAsync(code).GetAwaiter().GetResult();
                if (user.Active)
                {
                    var Token = GenerateToken(user);
                    return (Token, true, "");
                }
                _logger.LogError("Method: GetToken , Error:Please activate your account");
                return (new JsonResult(""), false, "Please activate your account.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetToken , Error:" + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }

        public (bool success, string error) DeleteCustomer(UserEntity userModel)
        {
            try
            {
                var user = _context.Users.Where(s => s.Id == userModel.Id).FirstOrDefault();
                if (user != null)
                {
                    var lstAIResults = _context.CustomerAIResults.Where(c => c.UserId == user.Id).ToList();
                    _context.CustomerAIResults.RemoveRange(lstAIResults);

                    var lstQuestionnaire = _context.Questionaires.Where(q => q.UserId == user.Id.ToString()).ToList();
                    _context.Questionaires.RemoveRange(lstQuestionnaire);

                    var lstGroup = _context.Groups.Where(g => g.UserEmail == user.Email).ToList();
                    _context.Groups.RemoveRange(lstGroup);

                    var lstGroupPost = _context.GroupPosts.Where(gp => gp.UserEmail == user.Email).ToList();
                    _context.GroupPosts.RemoveRange(lstGroupPost);

                    var lstHairProfiles = _context.HairProfiles.Where(h => h.UserId == user.Email).ToList();
                    _context.HairProfiles.RemoveRange(lstHairProfiles);

                    var lstCustomerTypeHistory = _context.CustomerTypeHistory.Where(c => c.CustomerId == user.Id).ToList();
                    _context.CustomerTypeHistory.RemoveRange(lstCustomerTypeHistory);

                    _context.SaveChanges();
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }

                return (true, "You account has been deleted.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteCustomer, Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }
        }
        public bool saveAIResultV2(UserEntity userEntity, string aiResult, string hairType)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == userEntity.UserName.ToLower()).FirstOrDefault();
                user.AIResult = aiResult;
                user.IsAIV2Mobile = true;
                user.HairType = hairType;
                _context.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: saveAIResultV2, Error: " + Ex.Message, Ex);
                return false;
            }
        }

        #region HairKit User
        public (bool success, string error) SignUpShopifyOneTime(ShopifyOrder signup)
        {
            if (signup.line_items != null && signup.line_items.Any(item => item.product_id == 7359177162788))
            {
                // Get the final price of the item with 'product_id' equal to "7359177162788"
                decimal finalPrice = signup.line_items
                    .Where(item => item.product_id == 7359177162788)
                    .Select(item => Convert.ToDecimal(item.price))
                    .FirstOrDefault();
                var existUser = _userManager.FindByEmailAsync(signup.customer.email).GetAwaiter().GetResult();
                if (existUser != null)
                {
                    
                    //---make an entry in shopify request table
                    var obj = new ShopifyRequest();
                    obj.Email = existUser.Email;
                    obj.Payload = JsonConvert.SerializeObject(signup);
                    obj.RequestDate = DateTime.UtcNow;
                    obj.SubscriptionType = (int)SubscriptionTypeEnum.OneTime;
                    obj.IsExistingCustomer = true;
                    obj.CustomerId = existUser.Id;
                    obj.AlreadyActiveSubscription = existUser.IsPaid;
                    _context.ShopifyRequest.Add(obj);
                    _context.SaveChanges();

                    //if user have active subscritption of anytype one time or subscritption or user has no subscription i.e new user
                    _context.PaymentEntities.Add(new PaymentEntity()
                    {
                        EmailAddress = signup.customer.email,
                        CCNumber = "",
                        CreatedDate = DateTime.UtcNow,
                        PaymentAmount =finalPrice.ToString(), //signup.total_price,
                        PaymentId = Guid.NewGuid(),
                        SubscriptionId = "",
                        ProviderName = "OneTime",
                        ExpirationDate = DateTime.UtcNow.AddMonths(1).AddDays(-1)
                    });
                    _context.SaveChanges();

                    EmailInformation emailInformation = new EmailInformation
                    {
                        Email = existUser.Email,
                        Name = existUser.FirstName + " " + existUser.LastName,
                        Code = "One Time Access"
                    };

                    var emailRes = _emailService.SendEmail("HAIRAIPURCHASE", emailInformation);
                    return (UpdateHairKitUser(existUser), "");
                }
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var password = RandomPassword();
                        UserEntity entity;
                        Claim[] claims;

                        IdentityResult result = CreateHairKitUser(signup, out entity, out claims, password);
                        if (!result.Succeeded)
                        {
                            var firstError = result.Errors.FirstOrDefault()?.Description;
                            return (result.Succeeded, firstError);
                        }
                        else
                        {
                            _userManager.AddToRoleAsync(entity, "User").Wait();
                            _userManager.AddClaimsAsync(entity, claims).Wait();
                            _userManager.UpdateAsync(entity).Wait();

                            //---make an entry in shopify request table for new customer
                            var obj = new ShopifyRequest();
                            obj.Email = signup.customer.email;
                            obj.Payload = JsonConvert.SerializeObject(signup);
                            obj.RequestDate = DateTime.UtcNow;
                            obj.SubscriptionType = (int)SubscriptionTypeEnum.OneTime;
                            obj.IsExistingCustomer = false;
                            obj.CustomerId = entity.Id;
                            obj.AlreadyActiveSubscription = false;
                            _context.ShopifyRequest.Add(obj);
                            _context.SaveChanges();


                            //---------Make an entry in PaymentEntityTable for new customer
                            _context.PaymentEntities.Add(new PaymentEntity()
                            {
                                EmailAddress = signup.customer.email,
                                CCNumber = "",
                                CreatedDate = DateTime.UtcNow,
                                PaymentAmount =finalPrice.ToString(), //signup.total_price,
                                PaymentId = Guid.NewGuid(),
                                SubscriptionId = "",
                                ProviderName = "OneTime",
                                ExpirationDate = DateTime.UtcNow.AddMonths(1).AddDays(-1)
                            });
                            _context.SaveChanges();


                            CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                            customerTypeHistory.CustomerId = entity.Id;
                            customerTypeHistory.OldCustomerTypeId = null;
                            customerTypeHistory.NewCustomerTypeId = (int)(entity.CustomerTypeId);
                            customerTypeHistory.CreatedOn = entity.CreatedAt?.DateTime ?? DateTime.MinValue;
                            customerTypeHistory.IsActive = true;
                            customerTypeHistory.UpdatedByUserId = null;
                            customerTypeHistory.Comment = "New customer signup from shopify";
                            _context.CustomerTypeHistory.Add(customerTypeHistory);
                            _context.SaveChanges();
                        }

                        var emailRes = SendEmail(entity, Operation.ForgetPassword, "REGHAIRKITUSER");
                        dbContextTransaction.Commit();
                        return (result.Succeeded, "");

                    }
                    catch (Exception Ex)
                    {
                        _logger.LogError("Method: SignUpShopifyOneTime, Email:" + signup.customer.email + " , Error:" + Ex.Message, Ex);
                        return (false, "Something went wrong. Please try again later.");
                    }
                }
            }
            else
            {
                return (true, "");
            }
        }
        private IdentityResult CreateHairKitSubscriptionUser(ShopifyOrderSubscription signUp, out UserEntity entity, out Claim[] claims, string password)
        {
            try
            {
                
                string accountNo = signUp.customer.firstname + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                
                entity = new UserEntity
                {
                    AccountNo = accountNo,
                    Email = signUp.customer.email,
                    UserName = signUp.customer.email,
                    Active = true,
                    FirstName = signUp.customer.firstname,
                    LastName = signUp.customer.lastname,
                    EmailConfirmed = false,
                    PhoneNumber = signUp.customer.phone,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CountryCode = 1,
                    CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus,
                    IsPaid = true, //changed to true
                    IsProCustomer = false,
                   // SubscriptionType = (int)SubscriptionTypeEnum.AnnualSubscription,
                    //IsHairAIAllowed = true
                };
                claims = new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
                };
                return _userManager.CreateAsync(entity, password).GetAwaiter().GetResult();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHairKitSubscriptionUser, Error: " + Ex.Message, Ex);
                entity = null;
                claims = null;
                return null;
            }

        }


        private IdentityResult CreateHairKitSubUserNew(ShopifyOrderNew signUp, out UserEntity entity, out Claim[] claims, string password)
        {
            try
            {

                string accountNo = signUp.first_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                entity = new UserEntity
                {
                    AccountNo = accountNo,
                    Email = signUp.email,
                    UserName = signUp.email,
                    Active = true,
                    FirstName = signUp.first_name,
                    LastName = signUp.last_name,
                    EmailConfirmed = false,
                    PhoneNumber = signUp.s_phone,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CountryCode = 1,
                    CustomerTypeId = (int)CustomerTypeEnum.DigitalAnalysis,
                    IsPaid = true, //changed to true
                    IsProCustomer = false,
                    
                };
                claims = new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
                };
                return _userManager.CreateAsync(entity, password).GetAwaiter().GetResult();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHairKitSubUserNew, Error: " + Ex.Message, Ex);
                entity = null;
                claims = null;
                return null;
            }

        }

        private bool UpdateHairKitUser(UserEntity userEntity)
        {
            try
            {
                UserEntity accountNo = _context.Users.Where(x => x.Id == userEntity.Id).FirstOrDefault();
                if (accountNo != null)
                {
                    //For existing customer when he comes from shopify payment and earlier was not a DigitalAnalysisCustomer
                    if (accountNo.CustomerTypeId != (int)CustomerTypeEnum.DigitalAnalysis)
                    {
                        CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                        customerTypeHistory.CustomerId = accountNo.Id;
                        customerTypeHistory.OldCustomerTypeId = accountNo.CustomerTypeId;
                        customerTypeHistory.CreatedOn = accountNo.CreatedAt?.UtcDateTime ?? DateTime.MinValue;
                        customerTypeHistory.IsActive = true;
                        customerTypeHistory.UpdatedByUserId = null;
                        customerTypeHistory.Comment = "Update customer from shopify.";
                        if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKit && accountNo.BuyHairKit == true)
                        {
                            customerTypeHistory.NewCustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                        }
                        else if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKitPlus)
                        {
                            customerTypeHistory.NewCustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                        }
                        else
                        {
                            customerTypeHistory.NewCustomerTypeId = (int)CustomerTypeEnum.DigitalAnalysis;
                        }
                       
                        _context.CustomerTypeHistory.Add(customerTypeHistory);
                        _context.SaveChanges();
                    }
                    accountNo.IsPaid = true;
                    if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKit && accountNo.BuyHairKit == true)
                    {
                        accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                    }
                    else if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKitPlus)
                    {
                        accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                    }
                    else
                    {
                        accountNo.CustomerTypeId = (int)CustomerTypeEnum.DigitalAnalysis;
                    }
                    //accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                    _context.UserEntity.Update(accountNo);
                    _context.SaveChanges();
                    return (true);
                    
                }
                return (false);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateHairKitUser, Error:" + Ex.Message, Ex);
                return (false);
            }
        }
        private IdentityResult CreateHairKitUser(ShopifyOrder signUp, out UserEntity entity, out Claim[] claims, string password)
        {
            try
            {   //int customerTypeId = (int)CustomerTypeEnum.HairKit;
                string accountNo = signUp.customer.first_name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //if (signUp.line_items != null && signUp.line_items.Any(item => item.name == "MYAVANA Hair Analysis Kit"))
                //{
                //    customerTypeId=(int)CustomerTypeEnum.HairKitPlus;
                //}
                entity = new UserEntity
                {
                    AccountNo = accountNo,
                    Email = signUp.customer.email,
                    UserName = signUp.customer.email,
                    Active = true,
                    FirstName = signUp.customer.first_name,
                    LastName = signUp.customer.last_name,
                    EmailConfirmed = false,
                    PhoneNumber = signUp.customer.phone,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CountryCode = 1,
                    CustomerTypeId = (int)CustomerTypeEnum.DigitalAnalysis,
                    IsPaid = true, //changed to true
                    IsProCustomer = false,
                    
                };
                claims = new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
                };
                return _userManager.CreateAsync(entity, password).GetAwaiter().GetResult();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHairKitUser, Error: " + Ex.Message, Ex);
                entity = null;
                claims = null;
                return null;
            }

        }
        private void AddCustomerSubscriptionHistory(ShopifyOrderNew signup)
        {
            _context.CustomerSubscriptionHistory.Add(new CustomerSubscriptionHistory
            {
                EmailAddress = signup.email,
                Status = signup.status.ToLower(),
                ProductId = signup.items.FirstOrDefault()?.product_id,
                Date = signup.status.ToLower() == "paused" ? signup.paused_on : (signup.status.ToLower() == "cancelled" ? signup.cancelled_on : signup.order_placed),
                CreatedOn = DateTime.UtcNow
            });

            _context.SaveChanges();
        }


        public (bool success, string error) SignUpShopifySubscription(ShopifyOrderNew signup)
        {
            if (signup.items != null && signup.items.Any(item => item.product_id == "7359177162788"))
            {
                decimal finalPrice = signup.items
                      .Where(item => item.product_id == "7359177162788")
                      .Select(item => Convert.ToDecimal(item.final_price))
                      .FirstOrDefault();
                var recentAttempt = signup.billing_attempts
                .Where(attempt => !string.IsNullOrWhiteSpace(attempt.status))
                .OrderByDescending(attempt => attempt.date).FirstOrDefault();

                bool IsMyavanaExistingCustomer = false;

                var existUser = _userManager.FindByEmailAsync(signup.email).GetAwaiter().GetResult();
                if (existUser != null)
                {
                    var shopifyCust = _context.ShopifyRequest.Where(x => x.Email == signup.email).FirstOrDefault(); //check customer is exiting myavana customer or shopifycustomer
                    if (shopifyCust == null) // if the customer is from myavana
                    {
                        IsMyavanaExistingCustomer = true;
                    }

                    var existingSubscriptionStatus = _context.CustomerSubscriptionHistory.Where(x => x.EmailAddress == signup.email).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    //---make an entry in shopify request table for existing customer
                    var obj = new ShopifyRequest();
                    obj.Email = existUser.Email;
                    obj.Payload = JsonConvert.SerializeObject(signup);
                    obj.RequestDate = DateTime.UtcNow;
                    obj.SubscriptionType = (int)SubscriptionTypeEnum.AnnualSubscription;
                    obj.IsExistingCustomer = true;
                    obj.CustomerId = existUser.Id;
                    obj.AlreadyActiveSubscription = existUser.IsPaid;
                    _context.ShopifyRequest.Add(obj);
                    _context.SaveChanges();


                    if ((!IsMyavanaExistingCustomer && (signup.status.ToLower() == "cancelled" || signup.status.ToLower() == "paused") && existingSubscriptionStatus.Status.ToLower() == signup.status.ToLower() && existingSubscriptionStatus.Date == (signup.status.ToLower() == "paused" ? signup.paused_on : (signup.status.ToLower() == "cancelled" ? signup.cancelled_on : signup.order_placed))) || IsMyavanaExistingCustomer)   // skip same  multiple request
                    {
                        return (true, ""); //skip adding to history if repeated payloads
                    }
                    else
                    {
                        var paymentExists = _context.PaymentEntities.Where(x => x.EmailAddress == signup.email && x.ProviderName != "OneTime").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (signup.status.ToLower() == "paused" || signup.status.ToLower() == "cancelled")
                        {
                            AddCustomerSubscriptionHistory(signup);
                        }
                        //For second time or existing customer, if active payload come then we have to check billing attempt
                        if ((recentAttempt != null && recentAttempt.status != "error" && recentAttempt.status != "" && signup.status.ToLower() == "active" && !IsMyavanaExistingCustomer) || (IsMyavanaExistingCustomer && signup.status.ToLower() == "active"))
                        {

                            if (paymentExists == null)
                            {
                                _context.PaymentEntities.Add(new PaymentEntity()
                                {
                                    EmailAddress = signup.email,
                                    CCNumber = "",
                                    CreatedDate = signup.order_placed,
                                    PaymentAmount = finalPrice.ToString(),//signup.total_value,
                                    ExpirationDate = signup.order_placed.AddMonths(1).AddDays(-1),
                                    PaymentId = Guid.NewGuid(),
                                    SubscriptionId = "",
                                    ProviderName = "Subscription"
                                });
                                AddCustomerSubscriptionHistory(signup);
                                EmailInformation emailInformation = new EmailInformation
                                {
                                    Email = existUser.Email,
                                    Name = existUser.FirstName + " " + existUser.LastName,
                                    Code = "Subscription"
                                };

                                var emailRes = _emailService.SendEmail("HAIRAIPURCHASE", emailInformation);
                            }
                            else
                            {
                                if (paymentExists.ExpirationDate < DateTime.Now) //expired payment
                                {
                                    //if (existingSubscriptionStatus.Status.ToLower() == "cancelled")
                                    //{  remove cancelled check whether we receive renewal or reactivate payload(i.e. any payload with active status, then we will extend the expiry date by 1 year if already expired
                                    paymentExists.ExpirationDate = DateTime.UtcNow.AddMonths(1).AddDays(-1);
                                    _context.SaveChanges();
                                    AddCustomerSubscriptionHistory(signup);
                                    EmailInformation emailInformation = new EmailInformation
                                    {
                                        Email = existUser.Email,
                                        Name = existUser.FirstName + " " + existUser.LastName,
                                        Code = "Subscription"
                                    };

                                    var emailRes = _emailService.SendEmail("HAIRAIPURCHASE", emailInformation);
                                }
                                else // create history for active if we receive active payload after cancel or pause but the subscription is not yet expired as we don't do anything in the payment entity then
                                {
                                    if (existingSubscriptionStatus.Status.ToLower() != signup.status.ToLower())
                                    {
                                        AddCustomerSubscriptionHistory(signup);

                                    }
                                }
                            }

                            return (UpdateHairKitUser(existUser), "");
                        }
                        else
                        {
                            if (signup.status.ToLower() != "active")
                            {
                                return (true, "Subscription is " + signup.status);
                            }
                            else
                            {
                                return (true, "Billing attempt failed");
                            }
                        }
                    }
                }
                else
                {
                    //new customer- grant or renew access to customers based on their subscription status and billing — 'active' and 'completed' for current customers, and 'active' for new ones
                    if (signup.status.ToLower() == "active")  //create customer if payload status is active
                    {
                        using (var dbContextTransaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var password = RandomPassword();
                                UserEntity entity;
                                Claim[] claims;


                                IdentityResult result = CreateHairKitSubUserNew(signup, out entity, out claims, password);
                                if (!result.Succeeded)
                                {
                                    var firstError = result.Errors.FirstOrDefault()?.Description;
                                    return (result.Succeeded, firstError);
                                }
                                else
                                {
                                    _userManager.AddToRoleAsync(entity, "User").Wait();
                                    _userManager.AddClaimsAsync(entity, claims).Wait();
                                    _userManager.UpdateAsync(entity).Wait();

                                    //---make an entry in shopify request table for new customer
                                    var obj = new ShopifyRequest();
                                    obj.Email = signup.email;
                                    obj.Payload = JsonConvert.SerializeObject(signup);
                                    obj.RequestDate = DateTime.UtcNow;
                                    obj.SubscriptionType = (int)SubscriptionTypeEnum.AnnualSubscription; //monthly subscription
                                    obj.IsExistingCustomer = false;
                                    obj.CustomerId = entity.Id;
                                    obj.AlreadyActiveSubscription = false;
                                    _context.ShopifyRequest.Add(obj);
                                    _context.SaveChanges();

                                    AddCustomerSubscriptionHistory(signup);


                                    //---------Make an entry in PaymentEntityTable for new customer
                                    _context.PaymentEntities.Add(new PaymentEntity()
                                    {
                                        EmailAddress = signup.email,
                                        CCNumber = "",
                                        CreatedDate = signup.order_placed,
                                        ExpirationDate = signup.order_placed.AddMonths(1).AddDays(-1),
                                        PaymentAmount = finalPrice.ToString(), //signup.total_value,
                                        PaymentId = Guid.NewGuid(),
                                        SubscriptionId = "",
                                        ProviderName = "Subscription"
                                    });
                                    _context.SaveChanges();

                                    CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                                    customerTypeHistory.CustomerId = entity.Id;
                                    customerTypeHistory.OldCustomerTypeId = null;
                                    customerTypeHistory.NewCustomerTypeId = (int)(entity.CustomerTypeId);
                                    customerTypeHistory.CreatedOn = entity.CreatedAt?.DateTime ?? DateTime.MinValue;
                                    customerTypeHistory.IsActive = true;
                                    customerTypeHistory.UpdatedByUserId = null;
                                    customerTypeHistory.Comment = "New customer signup from shopify";
                                    _context.CustomerTypeHistory.Add(customerTypeHistory);
                                    _context.SaveChanges();
                                }

                                var emailRes = SendEmail(entity, Operation.ForgetPassword, "REGHAIRKITUSER");
                                dbContextTransaction.Commit();
                                return (result.Succeeded, "");

                            }
                            catch (Exception Ex)
                            {
                                _logger.LogError("Method: SignUpShopifySubscription, Email:" + signup.email + " , Error:" + Ex.Message, Ex);
                                return (false, "Something went wrong. Please try again later.");
                            }
                        }
                    }
                    else
                    {

                        var obj = new ShopifyRequest();
                        obj.Email = signup.email;
                        obj.Payload = JsonConvert.SerializeObject(signup);
                        obj.RequestDate = DateTime.UtcNow;
                        obj.SubscriptionType = (int)SubscriptionTypeEnum.AnnualSubscription; //monthly subscription
                        obj.IsExistingCustomer = false;
                        obj.CustomerId = null;
                        obj.AlreadyActiveSubscription = false;
                        _context.ShopifyRequest.Add(obj);
                        _context.SaveChanges();
                        return (false, "User not registerd as billing attempt failed");
                    }
                }
            }
            else
            {
                return (true, "");
            }
        }
        public (JsonResult result, bool success, string error) GetCustomerNonce(string email)
        {
            try
            {
                var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
                if (user.Active)
                {
                    var existNonce = _context.CustomerAuthentications.FirstOrDefault(x => x.UserId == user.Id && x.IsActive == true);
                    if (existNonce != null)
                    {
                        existNonce.IsActive = false;
                        _context.CustomerAuthentications.Update(existNonce);
                    }
                    var obj = new CustomerAuthentication();
                    obj.token = Guid.NewGuid();
                    obj.UserId = user.Id;
                    obj.CreatedOn = DateTime.UtcNow;
                    obj.ExpireOn = DateTime.UtcNow.AddHours(2);
                    obj.IsActive = true;
                    _context.CustomerAuthentications.Add(obj);
                    _context.SaveChanges();

                    return (new JsonResult(obj.token), true, "");
                }
                _logger.LogError("Method: GetCustomerNonce, Email:" + email + " , Error:Please activate your account.");
                return (new JsonResult(""), false, "Please activate your account.");

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetCustomerNonce, Email:" + email + " , Error:" + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }

        public (JsonResult result, bool success, string error) AuthenticateCustomerNonce(string nonce)
        {
            try
            {
                var existNonce = _context.CustomerAuthentications.FirstOrDefault(x => x.token.ToString() == nonce && x.IsActive == true && x.ExpireOn > DateTime.UtcNow);
                if (existNonce != null)
                {
                    var user = _userManager.FindByIdAsync(existNonce.UserId.ToString()).GetAwaiter().GetResult();
                    if (user.Active)
                    {
                        var Token = GenerateToken(user);
                        return (Token, true, "");
                    }
                    _logger.LogError("Method: AuthenticateCustomerNonce, Error:Please activate your account.");
                    return (new JsonResult(""), false, "Please activate your account.");
                }
                _logger.LogError("Method: AuthenticateCustomerNonce, Error:Your token is expired or used.");
                return (new JsonResult(""), false, "Your token is expired or used.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AuthenticateCustomerNonce, Error:" + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }
        #endregion

        #region common

        public string RandomString(int size, bool lowerCase)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RandomString, Error:" + Ex.Message, Ex);
                return "";
            }
        }

        public int RandomNumber(int min, int max)
        {
            try
            {
                Random random = new Random();
                return random.Next(min, max);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RandomNumber, Error:" + Ex.Message, Ex);
                return 0;
            }
        }

        public string RandomPassword(int size = 0)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                return builder.ToString();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RandomPassword, Error:" + Ex.Message, Ex);
                return "";
            }
        }

        public (bool success, string error) ShopifySample(ShopifyOrder signup)
        {
            try
            {
                var obj = new ShopifySampleData();
                obj.DataModel = JsonConvert.SerializeObject(signup);
                obj.CreatedOn = DateTime.UtcNow;
                _context.ShopifySampleDatas.Add(obj);
                _context.SaveChanges();

                // send email
                sendSampleEmail("Just check the shopify request");

                return (true, "");
            }
            catch (Exception Ex)
            {
                sendSampleEmail("Exception" + Ex.Message);
                _logger.LogError("Method: ShopifySample, Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }
        }

        public void sendSampleEmail(string body)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.sendgrid.net",
                    Port = 587,
                    EnableSsl = true,//result.EnableSSL,

                    Credentials = new System.Net.NetworkCredential("apikey", "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"),
                };
                MailMessage message = new MailMessage("Support@myavana.com", "rahulb.kis@gmail.com", "Shopify Test", body);

                message.From = new MailAddress("Support@myavana.com", "MYAVANA");
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: sendSampleEmail, Error:" + Ex.Message, Ex);
            }
        }

        public (UserEntity user, bool success, string error) GetUserById(string userId)
        {
            try
            {
                var resValue = _context.Users.Where(s => s.Id.ToString() == userId).FirstOrDefault();
                return (resValue, true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetUserById, UserId:" + userId + ", Error:" + Ex.Message, Ex);
                return (null, false, "");
            }
        }

        public bool CheckPromotionalPeriod()
        {
            try
            {
                bool isOnTrial = false;
                var trialDate = _context.DigitalAnalysisTrial.FirstOrDefault().TrialEndDate;
                DateTimeOffset? expiryDate = DateTime.Now;
                if (trialDate >= DateTime.Now)
                {
                    isOnTrial = true;
                }
                return isOnTrial;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CheckPromotionalPeriod, Error:" + Ex.Message, Ex);
                return false;
            }
        }

        public int GetSalonIdByUserId(int UserId)
        {
            try
            {
                int salonId = _context.SalonsOwners.Where(x => x.UserId == UserId && x.IsActive == true).Select(x => x.SalonId).FirstOrDefault();
                return salonId;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetSalonIdByUserId, UserId:" + UserId + ", Error:" + Ex.Message, Ex);
                return 0;
            }
        }
        public (SalonLoginDetail result, bool success) GetSalonNameByUserId(int UserId)
        {
            try
            {
                var salon =
                                   (from Salons in _context.Salons
                                    join Salonowner in _context.SalonsOwners.Where(x => x.UserId == UserId && x.IsActive == true)
                                     on Salons.SalonId equals Salonowner.SalonId
                                    select new SalonLoginDetail { 
                                        SalonName=Salons.SalonName,
                                        SalonLogo=Salons.SalonLogo
                                    }).FirstOrDefault();


                return (salon, true);
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetSalonNameByUserId, UserId:" + UserId + ", Error:" + Ex.Message, Ex);
                return (null, false);
            }
        }

        public (bool success, string error) GetTrialDigitalAssessment(DigitalAssessmentMarketModel digitalAssessmentMarketModel)
        {
            try
            {
                var user = _context.Users.Where(s => s.Id.ToString() == digitalAssessmentMarketModel.userId).FirstOrDefault();
                if (user != null)
                {
                    user.IsOnTrial = true;
                    user.TrialExpiredOn = DateTime.Now.AddDays(30);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return (true, "updated successfully");
                }
                return (false, "user not found");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetTrialDigitalAssessment, UserId:" + digitalAssessmentMarketModel.userId + ", Error:" + Ex.Message, Ex);
                return (false, "something went's wrong");
            }
        }
        public (string userId, string error) WebSignupWithPayment(SignupAndPayment signup)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(signup.Password))
                    {
                        string str = RandomPassword();
                        signup.Password = str;
                    }
                    UserEntity entity;
                    Claim[] claims;

                    IdentityResult result = CreateWebUserWithPayment(signup, out entity, out claims);
                    if (!result.Succeeded)
                    {
                        var firstError = result.Errors.FirstOrDefault()?.Description;
                        return (null, firstError);
                    }
                    else
                    {
                        _userManager.AddToRoleAsync(entity, "User").Wait();
                        _userManager.AddClaimsAsync(entity, claims).Wait();
                        _userManager.UpdateAsync(entity).Wait();
                        CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                        customerTypeHistory.CustomerId = entity.Id;
                        customerTypeHistory.OldCustomerTypeId = null;
                        customerTypeHistory.NewCustomerTypeId = (int)(entity.CustomerTypeId);
                        customerTypeHistory.CreatedOn = DateTime.Now;
                        customerTypeHistory.IsActive = true;
                        customerTypeHistory.UpdatedByUserId = signup.CreatedByUserId != 0 ? signup.CreatedByUserId : null;
                        customerTypeHistory.Comment = "New customer signup from customer portal.";
                        _context.CustomerTypeHistory.Add(customerTypeHistory);
                        _context.SaveChanges();
                    }




                    var user = _userManager.FindByEmailAsync(signup.Email);
                    signup.userId = user.Result.Id.ToString();
                    var paymentResponse = CardPayment(signup);
                    if (paymentResponse.success)
                    {
                        if (signup.BuyHairKit == true)
                        {
                            AddHairAnalysisStatus(user.Result.Id, signup.KitSerialNumber);
                        }
                        dbContextTransaction.Commit();
                        var emailRes = SendEmail(entity, Operation.ForgetPassword, "REGISTERUSER");
                        if (!emailRes.success)
                            _logger.LogError(emailRes.error);
                        return (user.Result.Id.ToString(), "");

                    }
                    return ("", paymentResponse.error);
                }
                catch (Exception Ex)
                {
                    _logger.LogError(Ex.Message, Ex);
                    return (null, "Something went wrong. Please try again later.");
                }
            }
        }
        public (bool success, string error) CardPayment(SignupAndPayment checkout)
        {
            try
            {
               // var Subscription = _context.SubscriptionsEntities.Where(s => s.StripePlanId.Trim() == checkout.SubscriptionId.Trim()).FirstOrDefault();
               // if (Subscription != null)
               // {
                    Guid uId = new Guid(checkout.userId);
                    UserEntity accountNo = _context.Users.Where(x => x.Id == uId).FirstOrDefault();
                    if (accountNo != null)
                    {
                       
                        PaymentResponse paymentResponse;

                        if (checkout.IsSubscriptionPayment == true)
                        {
                            var Subscription = _context.SubscriptionsEntities.Where(s => s.StripePlanId.Trim() == checkout.SubscriptionId.Trim()).FirstOrDefault();
                        if (Subscription != null)
                        {
                            var (subscription, error) = StripPayments(checkout, accountNo);
                            paymentResponse = new PaymentResponse { Subscription = subscription, Error = error };
                        }
                        else
                        {
                            return (false, "Invalid subscription Id");
                        }
                    }
                        else
                        {
                            var (charge, error) = StripOneTimePayments(checkout, accountNo);
                            paymentResponse = new PaymentResponse { Charge = charge, Error = error }; 
                        }
                        
                        if ((checkout.IsSubscriptionPayment ?? false) ? paymentResponse.Subscription != null : paymentResponse.Charge != null)
                        {
                            _context.PaymentEntities.Add(new PaymentEntity()
                            {
                                EmailAddress = accountNo.Email,
                                CCNumber = checkout.CardNumber,
                                CreatedDate = DateTime.UtcNow,
                                PaymentAmount = checkout.Amount.ToString(),
                                PaymentId = Guid.NewGuid(),
                                SubscriptionId = checkout.SubscriptionId,
                                ProviderId = (checkout.IsSubscriptionPayment ?? false) ? paymentResponse.Subscription.Id : paymentResponse.Charge.Id,
                                ProviderName = "STRIPE",
                                //Country=checkout.CountryId,
                                //State=checkout.StateId,
                                //City=checkout.City,
                                //Address=checkout.Address,
                                //ZipCode=checkout.Zipcode

                            });
                            accountNo.IsPaid = true;
                            accountNo.IsProCustomer = false;

                            CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                            customerTypeHistory.CustomerId = accountNo.Id;
                            customerTypeHistory.OldCustomerTypeId = (int)(accountNo.CustomerTypeId);
                            customerTypeHistory.CreatedOn = DateTime.Now;
                            customerTypeHistory.IsActive = true;
                            customerTypeHistory.UpdatedByUserId = null;



                            if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKit && accountNo.BuyHairKit == true)
                            {
                                accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                            }
                            else if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKitPlus)
                            {
                                accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                            }
                            else
                            {
                                accountNo.CustomerTypeId = (int)CustomerTypeEnum.DigitalAnalysis;
                            }
                            customerTypeHistory.NewCustomerTypeId = (int)(accountNo.CustomerTypeId);
                            customerTypeHistory.Comment = "Updated after payment completed.";
                            _context.UserEntity.Update(accountNo);
                            _context.CustomerTypeHistory.Add(customerTypeHistory);
                            _context.SaveChanges();
                            return (true, "");
                        }
                        return (false, "Error in processing payments." + "Error :- " + paymentResponse.Error);

                    }
                    return (false, "Invalid User");
                

            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }
        public (Subscription charge, string error) StripPayments(SignupAndPayment checkout, UserEntity userEntity)
        {
            try
            {
                //StripeConfiguration.ApiKey = (Convert.ToBoolean(_configuration.GetSection("Payment:IsLive").Value)) 
                //                                    ? _configuration.GetSection("Payment:KeyLive").Value : _configuration.GetSection("Payment:KeyTest").Value;
                StripeConfiguration.ApiKey = _configuration.GetSection("Payment:KeyTest").Value;

                //Create Card Object to create Token  
                CreditCardOptions card = new Stripe.CreditCardOptions();
                card.Name = checkout.CardOwnerFirstName + " " + checkout.CardOwnerLastName;
                card.Number = checkout.CardNumber;
                card.ExpYear = checkout.ExpirationYear;
                card.ExpMonth = checkout.ExpirationMonth;
                card.Cvc = checkout.CVV2;

                //Assign Card to Token Object and create Token  
                TokenCreateOptions token = new TokenCreateOptions
                {
                    Card = card
                };
                Stripe.TokenService serviceToken = new Stripe.TokenService();
                Token newToken = serviceToken.Create(token);

                //Create Customer Object and Register it on Stripe  
                CustomerCreateOptions myCustomer = new CustomerCreateOptions
                {
                    Email = userEntity.Email,
                    Source = newToken.Id,
                    Name = checkout.CardOwnerFirstName + " " + checkout.CardOwnerLastName,
                    Address = new AddressOptions() { State = checkout.State, Country = checkout.Country, PostalCode = checkout.Zipcode, Line1 = checkout.Address }

                };
                var customerService = new CustomerService();
                Stripe.Customer stripeCustomer = customerService.Create(myCustomer);

                var items = new List<SubscriptionItemOption> {
                                new SubscriptionItemOption {PlanId = checkout.SubscriptionId}
                            };
                var suboptions = new SubscriptionCreateOptions
                {
                    CustomerId = stripeCustomer.Id,
                    Items = items,
                    
                    //TrialPeriodDays = 7
                };

                var subservice = new Stripe.SubscriptionService();
                Subscription subscription = subservice.Create(suboptions);

                return (subscription, "");
                

            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (null, Ex.Message);
            }
        }
        
        public (Charge charge, string error) StripOneTimePayments(SignupAndPayment checkout, UserEntity userEntity)
        {
            try
            {
                //StripeConfiguration.ApiKey = (Convert.ToBoolean(_configuration.GetSection("Payment:IsLive").Value)) 
                //                                    ? _configuration.GetSection("Payment:KeyLive").Value : _configuration.GetSection("Payment:KeyTest").Value;
                StripeConfiguration.ApiKey = _configuration.GetSection("Payment:KeyTest").Value;

                //Create Card Object to create Token  
                CreditCardOptions card = new Stripe.CreditCardOptions();
                card.Name = checkout.CardOwnerFirstName + " " + checkout.CardOwnerLastName;
                card.Number = checkout.CardNumber;
                card.ExpYear = checkout.ExpirationYear;
                card.ExpMonth = checkout.ExpirationMonth;
                card.Cvc = checkout.CVV2;

                //Assign Card to Token Object and create Token  
                TokenCreateOptions token = new TokenCreateOptions
                {
                    Card = card
                };
                Stripe.TokenService serviceToken = new Stripe.TokenService();
                Token newToken = serviceToken.Create(token);
               


                //Create Customer Object and Register it on Stripe  
                CustomerCreateOptions myCustomer = new CustomerCreateOptions
                {
                    Email = userEntity.Email,
                    Source = newToken.Id,
                    Name = checkout.CardOwnerFirstName + " " + checkout.CardOwnerLastName,
                    Address=new AddressOptions() { State = checkout.State, Country = checkout.Country, PostalCode = checkout.Zipcode, Line1 = checkout.Address }

                };
                var customerService = new CustomerService();
                Stripe.Customer stripeCustomer = customerService.Create(myCustomer);

                
                //Create Charge Object with details of Charge
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(checkout.Amount) * 100,
                    Currency = "USD",
                    ReceiptEmail = userEntity.Email,
                    CustomerId = stripeCustomer.Id,
                    //Description = GetDescription(checkout), //Optional 

                };

                //and Create Method of this object is doing the payment execution.  
                var service = new ChargeService();
                Charge charge = service.Create(options); // This will do the Payment 
                return (charge, "");
               //// Create PaymentIntent to handle one - time payment
               //  var options = new PaymentIntentCreateOptions
               // {
               //     Amount = Convert.ToInt32(checkout.Amount) * 100, // amount in cents for $29.00
               //     Currency = "usd",
               //     Description = "One-time payment",
               // PaymentMethodId = newToken.Id, // This should be obtained from the Stripe.js client
               //     //PaymentMethod
               //     ConfirmationMethod = "automatic", // or "automatic" if you want to confirm immediately
               //     Confirm = true,
               //     ReceiptEmail=userEntity.Email,
               //     CustomerId=stripeCustomer.Id
               // };

               // var paymentIntentService = new PaymentIntentService();
               // PaymentIntent paymentIntent = paymentIntentService.Create(options);

               // // Check the status of the PaymentIntent
               // if (paymentIntent.Status == "requires_action" ||
               //     paymentIntent.Status == "requires_source_action")
               // {
               //     // PaymentIntent requires confirmation, handle the confirmation flow
               //     var confirmOptions = new PaymentIntentConfirmOptions
               //     {
               //         PaymentMethodId = newToken.Id
               //     };

               //     // Confirm the PaymentIntent, which may trigger 3D Secure authentication
               //     paymentIntent = paymentIntentService.Confirm(paymentIntent.Id, confirmOptions);
               // }

               // if (paymentIntent.Status == "succeeded")
               // {
               //     return (null, null);
               // }
               // else
               // {
               //     return (null, "");
               // }
                //var service = new PaymentIntentService();
                //PaymentIntent paymentIntent = service.Create(options);

                //// Confirm the PaymentIntent
                ////var confirmOptions = new PaymentIntentConfirmOptions {
                // var confirmOptions = new PaymentIntentConfirmOptions
                //{
                //     PaymentMethodId = newToken.Id,

                // };
                //paymentIntent =service.Confirm(paymentIntent.Id, confirmOptions);

                //if (paymentIntent.Status == "succeeded")
                //{
                //    // Payment succeeded
                //    var charge = new Charge
                //    {
                //        Id = paymentIntent.Charges.Data[0].Id,
                //        Amount = (long)paymentIntent.Amount,
                //        Currency = paymentIntent.Currency
                //    };

                //    return (charge, "");
                //}
                //else
                //    return (null, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (null, Ex.Message);
            }
        }
        public (JsonResult result, bool success, string error) GetAdminNonce(string email)
        {
            try
            {
                var user = _context.WebLogins.FirstOrDefault(x => x.UserEmail == email);
                if (user.IsActive == true)
                {
                    var existNonce = _context.AdminAuthentications.FirstOrDefault(x => x.UserId == user.UserId && x.IsActive == true);
                    if (existNonce != null)
                    {
                        existNonce.IsActive = false;
                        _context.AdminAuthentications.Update(existNonce);
                    }
                    var obj = new AdminAuthentication();
                    obj.token = Guid.NewGuid();
                    obj.UserId = user.UserId;
                    obj.CreatedOn = DateTime.UtcNow;
                    obj.ExpireOn = DateTime.UtcNow.AddHours(2);
                    obj.IsActive = true;
                    _context.AdminAuthentications.Add(obj);
                    _context.SaveChanges();

                    return (new JsonResult(obj.token), true, "");
                }
                _logger.LogError("Method: GetAdminNonce, Email:" + email + " , Error:Please activate your account.");
                return (new JsonResult(""), false, "Please activate your account.");

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetAdminNonce, Email:" + email + " , Error:" + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }

        public (WebLogin result, bool success, string error) AuthenticateAdminNonce(string nonce)
        {
            try
            {
                WebLogin webLogin = new WebLogin();
                var existNonce = _context.AdminAuthentications.FirstOrDefault(x => x.token.ToString() == nonce && x.IsActive == true && x.ExpireOn > DateTime.UtcNow);
                if (existNonce != null)
                {
                    var user = _context.WebLogins.FirstOrDefault(x => x.UserId == existNonce.UserId);
                    if (user.IsActive == true)
                    {
                        webLogin.UserId = user.UserId;
                        webLogin.UserEmail = user.UserEmail;
                        webLogin.Password = user.Password;
                        webLogin.IsActive = user.IsActive;
                        webLogin.UserType = user.UserType;
                        webLogin.UserTypeId = user.UserTypeId;
                        return (webLogin, true, "");
                    }
                    _logger.LogError("Method: AuthenticateAdminNonce, Error:Please activate your account.");
                    return (webLogin, false, "Please activate your account.");
                }
                _logger.LogError("Method: AuthenticateAdminNonce, Error:Your token is expired or used.");
                return (webLogin, false, "Your token is expired or used.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AuthenticateAdminNonce, Error:" + Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }

        public (bool success, string error) UpdateCustomer(UpdateUserModel updateUserModel)
        {
            try
            {
                var user = _context.Users.Where(s => s.Id == updateUserModel.UserId).FirstOrDefault();
                user.FirstName = updateUserModel.FirstName;
                user.LastName = updateUserModel.LastName;
                if (user.Email != updateUserModel.Email)
                {
                    var paymentExists = _context.PaymentEntities.Where(x => x.EmailAddress == user.Email).FirstOrDefault();
                    if (paymentExists != null)
                    {
                        return (false, "Payment exists.");
                    }
                    var HHCPexists = _context.HairProfiles.Where(x => x.UserId == user.Email).FirstOrDefault();
                    if (HHCPexists != null)
                    {
                        return (false, "Unable to update.");
                    }
                    var existUser = _context.UserEntity.FirstOrDefault(x => x.Email == updateUserModel.Email);
                    if (existUser != null)
                    {
                        return (false, "Email already exists.");
                    }
                    else
                    {
                            user.Email = updateUserModel.Email;
                            user.UserName = updateUserModel.Email;
                    }    
                }
                user.PhoneNumber = updateUserModel.PhoneNo;
                if (updateUserModel.IsInfluencer != null)
                {
                    user.IsInfluencer = updateUserModel.IsInfluencer;
                }
                _context.UserEntity.Update(user);
                _context.SaveChanges();
                return (true, "");
               
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateCustomer, Email:" + updateUserModel.Email + " , Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
        #endregion

        public List<Countries> GetCountriesList()
        {
            try
            {
                List<Countries> lstCountries = _context.Countries.OrderBy(c => c.Country).ToList();
                return lstCountries;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCountriesList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<States> GetStatesList(int CountryId)
        {
            try
            {
                List<States> lstStates= _context.States.Where(x=>x.CountryId==CountryId).OrderBy(c => c.State).ToList();
                return lstStates;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetStatesList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool IsEmailExists(string email)
        {
            try
            {
                var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: IsEmailExists, Email:" + email + " , Error:" + Ex.Message, Ex);
                return false;
            }
        }


        public async Task<(bool success, string error)> SetCustomerLoginPass(SetCustomerPassword setPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(setPassword.Email);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, setPassword.Password);
                user.LoginAlert = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return (true, "");
                }
                _logger.LogError("Method: SetCustomerLoginPass, Email:" + setPassword.Email + " , Error:Error in updating the password.");
                return (false, "Error in updating the password.");

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SetCustomerLoginPass, Email:" + setPassword.Email + " , Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
        public (bool success, string error) SaveInAppPayment(InAppPaymentModel inAppPaymentModel)
        {
            try
            {
                UserEntity accountNo = _context.Users.Where(x => x.Id == inAppPaymentModel.UserId).FirstOrDefault();
                if (accountNo != null)
                {
                    _context.PaymentEntities.Add(new PaymentEntity()
                    {
                        EmailAddress = accountNo.Email,
                        CCNumber = "",
                        CreatedDate = DateTime.Parse(inAppPaymentModel.transactionDate).ToUniversalTime(), //DateTime.UtcNow
                        PaymentAmount = inAppPaymentModel.paymentAmount,
                        PaymentId = Guid.NewGuid(),
                        SubscriptionId = inAppPaymentModel.transactionId,
                        ProviderId = inAppPaymentModel.productId,
                        ProviderName = inAppPaymentModel.providerName,
                        ExpirationDate = DateTime.Parse(inAppPaymentModel.transactionDate).AddMonths(1).AddDays(-1),
                        PurchaseToken=inAppPaymentModel.purchaseToken
                    });
                    _context.SaveChanges();
                    
                    accountNo.IsPaid = true;
                    accountNo.IsProCustomer = false;

                    CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                    customerTypeHistory.CustomerId = accountNo.Id;
                    customerTypeHistory.OldCustomerTypeId = (int)(accountNo.CustomerTypeId);
                    customerTypeHistory.CreatedOn = DateTime.Now;
                    customerTypeHistory.IsActive = true;
                    customerTypeHistory.UpdatedByUserId = null;



                    if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKit && accountNo.BuyHairKit == true)
                    {
                        accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                    }
                    else if (accountNo.CustomerTypeId == (int)CustomerTypeEnum.HairKitPlus)
                    {
                        accountNo.CustomerTypeId = (int)CustomerTypeEnum.HairKitPlus;
                    }
                    else
                    {
                        accountNo.CustomerTypeId = (int)CustomerTypeEnum.DigitalAnalysis;
                    }

                    customerTypeHistory.NewCustomerTypeId = (int)accountNo.CustomerTypeId;
                    customerTypeHistory.Comment = "Updated after in-app payment completed.";
                    _context.UserEntity.Update(accountNo);
                    _context.CustomerTypeHistory.Add(customerTypeHistory);
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Invalid User.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveInAppPayment, UserId:" + inAppPaymentModel.UserId + " , Error:" + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }

        public bool saveInAppPayload(string notification, string platform)
        {

            try
            {

                // string Payload = JsonConvert.SerializeObject(notification);
                if (notification != null)
                {
                    InAppPayload appmodel = new InAppPayload();
                    appmodel.Payload = notification;
                    appmodel.Platform = platform;
                    appmodel.CreatedOn = DateTime.Now;
                    _context.InAppPayload.Add(appmodel);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: savePayload,  Error:" + Ex.Message, Ex);
                return false;
            }
        }
        public string DecodeIOSSignedPayload(string signedPayload)
        {
            try
            {
                // Decode Base64 payload
                var splitParts = signedPayload.Split('.');
                var payload = splitParts[1];
                var decodedBytes = Base64UrlTextEncoder.Decode(payload);
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Base64 payload");
            }
        }
        public string DecodeSignedPayload(string signedPayload)
        {
            try
            {
                // Decode Base64 payload
                var decodedBytes = Base64UrlTextEncoder.Decode(signedPayload);
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Base64 payload");
            }
        }

        public async Task<bool> HandleSubscriptionRenewal(NotificationV2Data notification)
        {
            SignedRenewalInfo renewalInfo = null;
            SignedTransactionInfo transactionInfo = null;
            string decodedRenewalV2 = DecodeIOSSignedPayload(notification.SignedRenewalInfo);

            renewalInfo = JsonConvert.DeserializeObject<SignedRenewalInfo>(decodedRenewalV2);
            string decodedTransactionV2 = DecodeIOSSignedPayload(notification.SignedRenewalInfo);
            transactionInfo = JsonConvert.DeserializeObject<SignedTransactionInfo>(decodedTransactionV2);

            // Extract transaction ID and renewal status
            string transactionId = transactionInfo.TransactionId;
            string originalTransactionId = transactionInfo.OriginalTransactionId;
            int renewalStatus = renewalInfo.AutoRenewStatus;
            long newExpiryDate = renewalInfo.RenewalDate;

            // Check if the renewal was successful (assuming 1 indicates success)
            if (renewalStatus == 1)
            {
                try
                {
                    // Proceed to update your database
                    //ExpirationDate.Value.Date >= DateTime.Now.Date
                    var paymentExists = _context.PaymentEntities.Where(x => x.SubscriptionId == originalTransactionId && x.ProviderName == "InApp").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    if (paymentExists != null)
                    {
                        paymentExists.ExpirationDate = DateTimeOffset.FromUnixTimeMilliseconds(newExpiryDate).UtcDateTime.AddMonths(1).AddDays(-1);// Update expiry date
                        _context.SaveChanges();
                        return true;
                    }
                    {
                        _logger.LogError("Method: HandleSubscriptionRenewal, OriginalTransactionId:" + originalTransactionId + ", RenewalStatus:" + renewalStatus + "_ Subscritpion not found.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                // Handle the case where renewal was not successful
                _logger.LogError("Method: HandleSubscriptionRenewal,  OriginalTransactionId:" + originalTransactionId + ", RenewalStatus:" + renewalStatus + "_ Renewal Status received indicates renewal was not successful");
                return false;
            }
        }

        public async Task<bool> UpdateSubscriptionExpirationAndriod(string PurchaseToken, long PurchaseTime)
        {
            try
            {
                
                // Find the subscription record in the database using the SubscriptionId or PurchaseToken
                var userSubscription = _context.PaymentEntities.Where(x => x.PurchaseToken == PurchaseToken && x.ProviderName == "InApp").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (userSubscription != null)
                {
                    // Extend the expiration date and set it to new expiry date from payload
                    userSubscription.ExpirationDate = DateTimeOffset.FromUnixTimeMilliseconds(PurchaseTime).UtcDateTime.AddMonths(1).AddDays(-1);// Update expiry date
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    
                    _logger.LogError("Method: UpdateSubscriptionExpirationAndriod, SubscriptionId: " + PurchaseToken + " , Error: Subscription record not found.");
                    return false; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UpdateSubscriptionExpirationAndriod, SubscriptionId: " + PurchaseToken+ " , Error: " + ex.Message, ex);
                return false; 
            }
        }
        public async Task<(bool IsSuccess, string Message, int StatusCode, string ErrorMessage)> HandleAndroidNotificationService(GooglePlayBase64Payload notification)
        {
            try
            {
               
                // Serialize the notification and save it
                string jsonNotification = JsonConvert.SerializeObject(notification);
                

                // Decode the signed payload
                var decodedJson = DecodeSignedPayload(notification.Message.Data);
                var googlePlayNotification = JsonConvert.DeserializeObject<GooglePlayNotification>(decodedJson);

                // Check if it's a subscription renewal
                if (googlePlayNotification.SubscriptionNotification.NotificationType == 2)
                {
                    bool saveResult = saveInAppPayload(jsonNotification, "android");
                    if (!saveResult)
                    {
                        _logger.LogError("Failed to save Android in-app payload");
                        return (false, null, 500, "Failed to save Android in-app payload");
                    }
                    bool isRenewed = await UpdateSubscriptionExpirationAndriod(googlePlayNotification.SubscriptionNotification.PurchaseToken, googlePlayNotification.EventTimeMillis);

                    if (!isRenewed)
                    {
                        _logger.LogError("Failed to renew Android subscription");
                        return (false, null, 500, "Failed to renew Android subscription");
                    }

                    _logger.LogError("Android subscription renewed successfully");
                    return (true, "Android subscription renewed successfully", 200, null);
                }
                else
                {
                    _logger.LogError("Received notification is not a subscription renewal");
                    return (false, null, 400, "Received notification is not a subscription renewal");
                }
            }
            catch (JsonSerializationException jex)
            {
                _logger.LogError($"JSON serialization error: {jex.Message}");
                return (false, null, 400, "Failed to process the notification payload.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return (false, null, 500, "An error occurred while processing the notification.");
            }
        }
    } 
}

