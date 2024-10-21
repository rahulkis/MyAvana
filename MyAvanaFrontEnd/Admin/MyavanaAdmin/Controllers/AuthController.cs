using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Routing;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]

    public class AuthController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private readonly IOptions<AppSettingsModel> config;
        private readonly HttpClient _httpClient;
        public AuthController(IOptions<AppSettingsModel> config)
        {
            this.config = config; 
            ApplicationSettings.WebApiUrl = config.Value.WebApiBaseUrl;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(WebLogin webLogin)
        {
            if (webLogin.UserEmail != null && webLogin.Password != null)
            {
                var result = await MyavanaAdminApiClientFactory.Instance.Login(webLogin);
                if (result.Data != null)
                { 
                    if (result.Data.UserId != 0 && result.Data.IsActive == true)
                    {

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, result.Data.UserEmail),
                            new Claim(ClaimTypes.Role, result.Data.UserTypeId.ToString())
                        };

                        // Configure cookie options based on "Remember Me"
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = webLogin.RememberMe, // Set cookie persistence
                            ExpiresUtc = webLogin.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : (DateTimeOffset?)null
                        };

                        CookieOptions option = new CookieOptions();

                        Response.Cookies.Append("UserTypeId", result.Data.UserTypeId.ToString(), option);
                        Response.Cookies.Append("UserId", result.Data.UserId.ToString(), option);
                        _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                        if (result.Data.UserTypeId== (int)UserTypeEnum.B2B) 
                        {
                            //_httpClient.BaseAddress = new Uri("https://apistaging.myavana.com/");
                            var SalonResult = _httpClient.GetAsync("Account/GetSalonNameByUserId?UserId=" + Convert.ToInt32(result.Data.UserId)).Result;
                            if (SalonResult.IsSuccessStatusCode)
                            {
                                var data = SalonResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                dynamic res = JObject.Parse(data);
                                SalonLoginDetail Salon = JsonConvert.DeserializeObject<SalonLoginDetail>(Convert.ToString(res));
                                Response.Cookies.Append("SalonName", Salon.SalonName);
                                if (!string.IsNullOrEmpty(Salon.SalonLogo))
                                {
                                    Response.Cookies.Append("SalonLogo", Salon.SalonLogo);
                                }
                            }
                        }
                        var claimsIdentity = new ClaimsIdentity(
                          claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        //var authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync("AdminCookies",
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        if (result.Data.UserTypeId == (int)UserTypeEnum.Admin)
                        {
                            return Content("1");
                        }
                        else if (result.Data.UserTypeId ==(int) UserTypeEnum.B2B)
                        {
                            return Content("2");
                        }
                        else
                        {
                            return Content("3");
                        }
                    }
                }
                return Content("0");
            }
            return Content("0");
        }

        [HttpGet]
        public async Task<bool> Logout()
        {
            Response.Cookies.Delete("AdminCookies");
            Response.Cookies.Delete("UserTypeId");
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("SalonName");
            Response.Cookies.Delete("SalonLogo");
            return true;
        }

        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            IEnumerable<WebLogin> filterUsers = await MyavanaAdminApiClientFactory.Instance.GetUsers(loginUserId.ToString());
            //if (Request.Cookies["UserType"].ToString() == "false" )
            //{
            //    filterUsers = filterUsers.Where(x => x.UserType.ToString() == Request.Cookies["UserType"].ToString());
            //}
            //if (Request.Cookies["UserTypeId"] != null && int.TryParse(Request.Cookies["UserTypeId"], out int userTypeId) && userTypeId == (int)UserTypeEnum.B2B)
            //{
            //    filterUsers = filterUsers.Where(x => x.UserTypeId.ToString() == Request.Cookies["UserTypeId"].ToString());
            //}
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<WebLogin, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.UserEmail);
                            filterUsers =
                                sortDirection == "asc"
                                    ? filterUsers.OrderBy(orderingFunctionString)
                                    : filterUsers.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.Password);
                            filterUsers =
                                sortDirection == "asc"
                                    ? filterUsers.OrderBy(orderingFunctionString)
                                    : filterUsers.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<WebLogin> codes = filterUsers.Select(e => new WebLogin
                {
                    UserId = e.UserId,
                    UserEmail = e.UserEmail,
                    Password = e.Password,
                    CreatedOn = e.CreatedOn,
                    CreatedBy = e.CreatedBy,
                    IsActive = e.IsActive,
                    UserType = e.UserType,
                    UserTypeId=e.UserTypeId,
                    SalonOwner = e.SalonOwner
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            IEnumerable<WebLogin> filterUser = await MyavanaAdminApiClientFactory.Instance.GetUsers(loginUserId.ToString());
            return Json(filterUser);
        }

        public async Task<IActionResult> CreateNewUser(string id)
        {
            if (id != null)
            {
                WebLoginModel webLogin = new WebLoginModel();
                webLogin.UserId = Convert.ToInt32(id);
               
                webLogin.UserTypeId =Convert.ToInt32( Request.Cookies["UserTypeId"]);
                var response = await MyavanaAdminApiClientFactory.Instance.GetUserById(webLogin);
                webLogin = response.Data;
                return View(webLogin);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(WebLoginModel webLogin)
        {
            webLogin.LoggedInUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            var response = await MyavanaAdminApiClientFactory.Instance.CreateNewUser(webLogin);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(WebLogin webLogin)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteUser(webLogin);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel ResetPassword)
        {
            
            try
            {
                var response = await MyavanaAdminApiClientFactory.Instance.ResetPassword(ResetPassword);
                if (response.message == "Success")
                    return Content("1");
                else
                    return Content("0");
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        
        [AllowAnonymous]
        public async Task<IActionResult> ForgotAdminPassword(string email)
        {
            try
            {
                var response = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "WebLogin/ForgotAdminPassword?email=" + email).Result;
                var data = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(data);
                ForgotPassword res = null;
                res = JsonConvert.DeserializeObject<ForgotPassword>(Convert.ToString(result));
                if (res.statusCode.ToString() == "200")
                    return Content("1");
                else
                    return Content("0");
            }

            catch (Exception ex)
            {
                //return Content("1");
            }
            return Content("0");
        }
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
        
        [AllowAnonymous]
        public IActionResult ResetPwd(string email)
        {
            SetPasswordAdmin setPassword = new SetPasswordAdmin();
            if (email != "")
            {
                setPassword.Email = email;
            }
            return View(setPassword);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetUserPassword(SetPasswordAdmin setPassword)
        {
            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(new StringContent(setPassword.Email.ToString()), "Email");
                multiContent.Add(new StringContent(setPassword.Code.ToString()), "Code");
                multiContent.Add(new StringContent(setPassword.Password), "Password");

                _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);

                var response = _httpClient.PostAsync("WebLogin/setAdminpass", CreateHttpContent<SetPasswordAdmin>(setPassword)).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return Content("1");
                else
                    return Content("0");
            }

            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [AllowAnonymous]
        public IActionResult CustomerQuestionnaire(string token, string customerId)
        {

            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.mobToken = token;
                var response = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/authenticateAdminNonce?nonce=" + token).Result;
                var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                dynamic res = JObject.Parse(data);

                WebLogin user = JsonConvert.DeserializeObject<WebLogin>(Convert.ToString(res));
               
                if (user.UserId != 0 && user.IsActive == true)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserEmail),
                            new Claim(ClaimTypes.Role, user.UserTypeId.ToString())
                        };

                    CookieOptions option = new CookieOptions();


                   
                    Response.Cookies.Append("UserTypeId", user.UserTypeId.ToString(), option);
                    Response.Cookies.Append("UserId", user.UserId.ToString(), option);
                    if (user.UserTypeId ==(int) UserTypeEnum.B2B) 
                    {
                        var SalonResult = _httpClient.GetAsync(ApplicationSettings.WebApiUrl  + "Account/GetSalonNameByUserId?UserId=" + Convert.ToInt32(user.UserId)).Result;
                        if (SalonResult.IsSuccessStatusCode)
                        {
                            var Salondata = SalonResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            dynamic res1 = JObject.Parse(Salondata);
                            SalonLoginDetail Salon = JsonConvert.DeserializeObject<SalonLoginDetail>(Convert.ToString(res1));
                            Response.Cookies.Append("SalonName", Salon.SalonName); 
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                     HttpContext.SignInAsync("AdminCookies",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    
                    
                }

                var routeValues = new RouteValueDictionary
                {
                    { "userId", customerId },
                    { "controller", "Questionnaire" }
                };

                return RedirectToAction("QuestionaireSurvey", routeValues);
           
            }
            return Content("0");
        }

        [HttpGet]
        public int GetHairStrandNotificationCount()
        {

            try
            {
                _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                var count = 0;
                var notificationCount = _httpClient.GetAsync("WebLogin/GetHairStrandNotificationCount").Result;
                if (notificationCount.IsSuccessStatusCode)
                {
                    var res = notificationCount.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!string.IsNullOrEmpty(res))
                    {
                        count = Convert.ToInt32(res);
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        [HttpGet]
        public string GetSalonLogo(int UserId)
        {

            try
            {
                _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                string logoUrl ="";
                var logo = _httpClient.GetAsync("Account/GetSalonLogoByUserId?UserId=" + Convert.ToInt32(UserId)).Result;

                if (logo.IsSuccessStatusCode)
                {
                    var res = logo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!string.IsNullOrEmpty(res))
                    {
                        logoUrl = res.ToString();
                    }
                }
                return logoUrl;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [HttpGet]
        public int GetHairDiaryNotificationCount()
        {

            try
            {
                _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                var count = 0;
                var notificationCount = _httpClient.GetAsync("WebLogin/GetHairDiaryNotificationCount").Result;
                if (notificationCount.IsSuccessStatusCode)
                {
                    var res = notificationCount.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!string.IsNullOrEmpty(res))
                    {
                        count = Convert.ToInt32(res);
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
