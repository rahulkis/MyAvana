using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyAvanaQuestionaire.Factory;
using MyAvanaQuestionaire.Models;
using MyAvanaQuestionaire.Utility;
using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MyAvanaQuestionaire.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<AppSettingsModel> config;
        private Uri BaseEndpoint;
        public IConfiguration _configuration;
        public AuthController(IOptions<AppSettingsModel> config, IConfiguration configuration)
        {
            this.config = config;
            _httpClient = new HttpClient();
            BaseEndpoint = new Uri(config.Value.WebApiBaseUrl);
            ApplicationSettings.WebApiUrl = config.Value.WebApiBaseUrl;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login([FromQuery(Name = "token")] string token)
        {
            if (token != null)
            {
                _httpClient.BaseAddress = BaseEndpoint;

                MultipartFormDataContent multipartContent = new MultipartFormDataContent();

                multipartContent.Add(new StringContent(token), "Token");
                var result = _httpClient.PostAsync("Questionnaire/AuthenticateUser", multipartContent).Result;

                var data = await result.Content.ReadAsStringAsync();
                dynamic res = JObject.Parse(data);

                var value = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)res).Last).Value); //.ToString();

                //var resultUser = JsonConvert.DeserializeObject(value);
                //var valueUser = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)resultUser).Last).Value); //.ToString();
                try
                {
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(value);

                    if (user.Id != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(
                          claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync("CustomerCookies",
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("start", "Questionaire", new { id = user.Id });
                    }
                    return View();
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Authentication authentication)
        {
            if (authentication.UserName != null && authentication.Password != null)
            {
                _httpClient.BaseAddress = BaseEndpoint;
               //_httpClient.BaseAddress = new Uri("http://localhost:5002/api/");
                var response = _httpClient.PostAsync("Account/signin", CreateHttpContent<Authentication>(authentication)).Result;

                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(data);
                var value = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)result).Last).Value); //.ToString();
                try
                {
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(value);

                    if (user.Id != null && user.Id.ToString() != "")
                    {
                        HttpContext.Session.SetString("token", user.access_token);
                        HttpContext.Session.SetString("email", user.Email);
                        HttpContext.Session.SetString("id", user.Id.ToString());
                        if (user.CustomerTypeId == 1)
                        {
                            HttpContext.Session.SetString("isprocustomer", "false");
                        }
                        else
                        {
                            HttpContext.Session.SetString("isprocustomer", "true");
                        }

                        HttpContext.Session.SetString("ispaid", user.IsPaid.ToString());
                        HttpContext.Session.SetString("isOnTrial", user.IsOnTrial.ToString());
                        HttpContext.Session.SetString("CustomerTypeId", user.CustomerTypeId.ToString());
                        HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType.ToString());
                        HttpContext.Session.SetString("Name", user.Name.ToString());
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(
                          claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Configure cookie options based on "Remember Me"
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = authentication.RememberMe, // Set cookie persistence
                            ExpiresUtc = authentication.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : (DateTimeOffset?)null
                        };

                        //var authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync("CustomerCookies",
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        Thread.CurrentPrincipal = claimsPrincipal;
                        if (user.CustomerTypeId == 1 && !Convert.ToBoolean(user.IsPaid) && !Convert.ToBoolean(user.IsOnTrial))
                        {
                            return Content("NotPaid");
                        }
                        return Content(user.Id.ToString());
                    }
                }
                catch (Exception ex)
                {
                    return Content("0");
                }
                return Content("0");
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


        public IActionResult Signup()
        {
            return View();
        }
        public async Task<IActionResult> PackagePayments(CheckoutRequest checkoutRequest)
        {
            _httpClient.BaseAddress = BaseEndpoint;
            
            if (checkoutRequest.IsSubscriptionPayment == true)
            {
                checkoutRequest.SubscriptionId = _configuration.GetSection("SubscriptionId").Value;
            }
            else
            {
                //checkoutRequest.SubscriptionId =  _configuration.GetSection("OneTimePlan").Value;
            }

            checkoutRequest.userId = HttpContext.Session.GetString("id");
            string token = HttpContext.Session.GetString("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.PostAsync("Payments/CardPayment", CreateHttpContent<CheckoutRequest>(checkoutRequest)).Result;
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseModel>(data);
            if (result.StatusCode == "200")
            {
                HttpContext.Session.SetString("ispaid", "true");
                HttpContext.Session.SetString("isprocustomer", "false");
                HttpContext.Session.SetString("CustomerTypeId", "1");
                return Content("1");
            }
            return Content(result.Value);
        }
        public IActionResult PackagePayment(string id, string token)
        {

            if (!string.IsNullOrEmpty(id))
            {
                if (HttpContext.Session.GetString("ispaid") == "true")
                {
                    return RedirectToAction("CustomerHair", "HairProfile");
                }
                HttpContext.Session.SetString("id", id.ToString());
            }
            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.mobToken = token;
                var response = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/authenticateCustomerNonce?nonce=" + token).Result;
                //var response = _httpClient.GetAsync("http://localhost:5002/api/" + "Account/authenticateCustomerNonce?nonce=" + token).Result;
                var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                dynamic res = JObject.Parse(data);

                var value = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)res).Last).Value); //.ToString();
                UserModel user = JsonConvert.DeserializeObject<UserModel>(value);

                if (user.Id != null && user.Id.ToString() != "")
                {
                    HttpContext.Session.SetString("token", user.access_token);
                    HttpContext.Session.SetString("email", user.Email);
                    HttpContext.Session.SetString("id", user.Id.ToString());
                    if (user.CustomerTypeId == 1)
                    {
                        HttpContext.Session.SetString("isprocustomer", "false");
                    }
                    else
                    {
                        HttpContext.Session.SetString("isprocustomer", "true");
                    }
                    HttpContext.Session.SetString("ispaid", user.IsPaid.ToString());
                    HttpContext.Session.SetString("CustomerTypeId", user.CustomerTypeId.ToString());
                    HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType.ToString());
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                        };

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    HttpContext.SignInAsync("CustomerCookies",
                       new ClaimsPrincipal(claimsIdentity),
                       authProperties);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    Thread.CurrentPrincipal = claimsPrincipal;
                }
            }
            return View("PackagePayment");
        }

        [HttpPost]
        public async Task<IActionResult> Signup(Signup signup)
        {
            _httpClient.BaseAddress = BaseEndpoint;
            signup.IsProCustomer = false;

            var response = _httpClient.PostAsync("Account/WebSignup", CreateHttpContent<Signup>(signup)).Result;

            var data = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(data);
            Response res = null;
            try
            {
                res = JsonConvert.DeserializeObject<Response>(Convert.ToString(result));
                if (true)
                {
                    var userDetail = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/GetUserById?userId=" + res.value.item1).Result;
                    if (userDetail.IsSuccessStatusCode)
                    {
                        var dataUser = userDetail.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        UserModel user = JsonConvert.DeserializeObject<UserModel>(dataUser);
                        if (user != null)
                        {
                            HttpContext.Session.SetString("isOnTrial", user.IsOnTrial.ToString());
                            HttpContext.Session.SetString("CustomerTypeId", user.CustomerTypeId.ToString());
                            HttpContext.Session.SetString("isprocustomer", user.IsProCustomer.ToString());
                            HttpContext.Session.SetString("ispaid", user.IsPaid.ToString());
                            HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType.ToString());
                        }
                    }
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, res.value.item1),
                        };

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync("CustomerCookies",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    Thread.CurrentPrincipal = claimsPrincipal;
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

            if (res.value.item1 != null)
                return Content(res.value.item1);
            else
                return Content("-1");
        }

        [HttpGet]
        public async Task<bool> Logout()
        {
            Response.Cookies.Delete("CustomerCookies");
            return true;
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> CheckForgotPassword(string email)
        {
            try
            {
                var response = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/forgetpassword?email=" + email).Result;
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

        public IActionResult ResetPwd(string email)
        {
            SetPassword setPassword = new SetPassword();
            if (email != "")
            {
                setPassword.Email = email;
            }
            return View(setPassword);
        }

        public async Task<IActionResult> ResetPassword(SetPassword setPassword)
        {
            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(new StringContent(setPassword.Email.ToString()), "Email");
                multiContent.Add(new StringContent(setPassword.Code.ToString()), "Code");
                multiContent.Add(new StringContent(setPassword.Password), "Password");

                _httpClient.BaseAddress = BaseEndpoint;

                var response = _httpClient.PostAsync("Account/setpass", CreateHttpContent<SetPassword>(setPassword)).Result;
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

        public IActionResult AccountLinking(string client_id, string response_type, string scope, string state, string redirect_uri)
        {
            AlexaAccountLinkingModel alexaAccountLinkingModel = new AlexaAccountLinkingModel();
            alexaAccountLinkingModel.client_id = client_id;
            alexaAccountLinkingModel.response_type = response_type;
            alexaAccountLinkingModel.scope = scope;
            alexaAccountLinkingModel.state = state;
            alexaAccountLinkingModel.redirect_uri = redirect_uri;
            return View(alexaAccountLinkingModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AccountLinking(AlexaAccountLinkingModel alexaAccountLinkingModel)
        {
            if (alexaAccountLinkingModel.UserName != null && alexaAccountLinkingModel.Password != null)
            {
                _httpClient.BaseAddress = BaseEndpoint;
                //_httpClient.BaseAddress = new Uri("http://localhost:5002/api/");
                var response = _httpClient.PostAsync("Account/alexalinking", CreateHttpContent<AlexaAccountLinkingModel>(alexaAccountLinkingModel)).Result;

                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(data);
                var value = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)result).First).Value).ToString(); //.ToString();
                try
                {
                    char[] charsToTrim3 = { '"', };
                    // alexaAccountLinkingModel.redirect_uri = "https://alexa.amazon.co.jp/api/skill/link/M10VO440HDZFBN";
                    //alexaAccountLinkingModel.state = "somestate";
                    var rurl = alexaAccountLinkingModel.redirect_uri + "?state=" + alexaAccountLinkingModel.state + "&code=" + value.Trim(charsToTrim3);
                    return Redirect(rurl);
                }
                catch (Exception ex)
                {
                    return Content("0");
                }
                //return Content("0");
            }
            return Content("0");
        }

        [HttpGet]
        public async Task<IActionResult> Authorize()
        {
            var response = _httpClient.GetAsync("https://www.amazon.com/ap/oa?" + "client_id=amzn1.application-oa2-client.d88db04eb6b64342ab8f687cf5414ff1&scope=postal_code&response_type=code&redirect_uri=https://customer.myavana.com/Auth/token &state=someState").Result;
            var data = response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return Content("1");
            else
                return Content("0");
        }

        [HttpGet]
        public async Task<IActionResult> token(string code, string scope, string state)
        {
            var url = "https://api.amazon.com/auth/o2/token?grant_type=authorization_code&code=" + code + "&redirect_uri=https://customer.myavana.com/Auth/token&client_id=amzn1.application-oa2-client.d88db04eb6b64342ab8f687cf5414ff1&client_secret=c064baf5bca0a08ce9edfd2b7fbfed7d692e6024b0da3374dbcf36b4c976f82a";
            var response = _httpClient.PostAsync(url, null).Result;
            var data = response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return Content(data.Result);
            else
                return Content("");
        }

        public IActionResult CustomerQuestionnaire(string token)
        {

            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.mobToken = token;
                var response = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/authenticateCustomerNonce?nonce=" + token).Result;
                var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                dynamic res = JObject.Parse(data);

                var value = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)res).Last).Value); //.ToString();
                UserModel user = JsonConvert.DeserializeObject<UserModel>(value);

                if (user.Id != null && user.Id.ToString() != "")
                {
                    HttpContext.Session.SetString("token", user.access_token);
                    HttpContext.Session.SetString("email", user.Email);
                    HttpContext.Session.SetString("id", user.Id.ToString());
                    if (user.CustomerTypeId == 1)
                    {
                        HttpContext.Session.SetString("isprocustomer", "false");
                    }
                    else
                    {
                        HttpContext.Session.SetString("isprocustomer", "true");
                    }
                    HttpContext.Session.SetString("ispaid", user.IsPaid.ToString());
                    HttpContext.Session.SetString("isOnTrial", user.IsOnTrial.ToString());
                    HttpContext.Session.SetString("CustomerTypeId", user.CustomerTypeId.ToString());
                    HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType.ToString());
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                        };

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    HttpContext.SignInAsync("CustomerCookies",
                       new ClaimsPrincipal(claimsIdentity),
                       authProperties);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    Thread.CurrentPrincipal = claimsPrincipal;
                }
                return RedirectToAction("Questionnaire", "Questionaire");
            }
            return Content("0");
        }


        public IActionResult DigitalAssessmentMarket(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HttpContext.Session.SetString("id", id.ToString());
            }
            var digitalAssessmentMarketModel = new DigitalAssessmentMarketModel()
            {
                isTrialOn = CheckPromotionalPeriod(),
                userId = id
            };
            return View(digitalAssessmentMarketModel);
        }

        public bool CheckPromotionalPeriod()
        {
            var isExist = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/CheckPromotionalPeriod").Result;
            if (isExist.IsSuccessStatusCode)
                return true;
            else
                return false;

        }
        public async Task<IActionResult> IsEmailExists(string email)
        {
            
           
            var response = await _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/IsEmailExists?email="+email);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                bool emailExists = bool.Parse(apiResponse);
                if (emailExists)
                {
                    //return Ok(true);
                    return Content("1");
                }
                else
                {
                    return Content("0");
                }
                
            }
            else
                return Content("2");

        }
        public async Task<IActionResult> GetTrialPackageDigitalAssessment(DigitalAssessmentMarketModel digitalAssessmentMarketModel)
        {
            _httpClient.BaseAddress = BaseEndpoint;
            digitalAssessmentMarketModel.userId = HttpContext.Session.GetString("id");
            var response = _httpClient.PostAsync("Account/GetTrialDigitalAssessment", CreateHttpContent<DigitalAssessmentMarketModel>(digitalAssessmentMarketModel)).Result;
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("isOnTrial","true");
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }


        #region Customer
        public IActionResult HairAICustomerSignup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HairAIPackagePayments(SignupAndPayment signupAndPayment)
        {
            _httpClient.BaseAddress = BaseEndpoint;
          
            if (signupAndPayment.IsSubscriptionPayment==true)
            {
                signupAndPayment.SubscriptionId = _configuration.GetSection("SubscriptionId").Value;
            }
            else
            {
                //signupAndPayment.SubscriptionId =  _configuration.GetSection("OneTimePlan").Value;
            }
            string token = HttpContext.Session.GetString("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.PostAsync("Account/WebSignupWithPayment", CreateHttpContent<SignupAndPayment>(signupAndPayment)).Result;
            var data = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(data);
            Response res = null;
            StatusCode status = JsonConvert.DeserializeObject<StatusCode>(Convert.ToString(result));
            if (status.statusCode == 200)
            {
                res = JsonConvert.DeserializeObject<Response>(Convert.ToString(result));
                if (res.statusCode == 200)
                {
                    var userDetail = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/GetUserById?userId=" + res.value.item1).Result;
                    if (userDetail.IsSuccessStatusCode)
                    {
                        var dataUser = userDetail.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        UserModel user = JsonConvert.DeserializeObject<UserModel>(dataUser);
                        if (user != null)
                        {
                            HttpContext.Session.SetString("isOnTrial", user.IsOnTrial.ToString());
                            HttpContext.Session.SetString("CustomerTypeId", user.CustomerTypeId.ToString());
                            HttpContext.Session.SetString("isprocustomer", user.IsProCustomer.ToString());
                            HttpContext.Session.SetString("ispaid", user.IsPaid.ToString());
                            HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType.ToString());
                        }
                    }
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, res.value.item1),
                        };

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync("CustomerCookies",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    Thread.CurrentPrincipal = claimsPrincipal;
                }
            }

            if (status.statusCode == 200)
                return Content(res.value.item1);
            else
            {
                var errres = JsonConvert.DeserializeObject<ErrorResponse>(Convert.ToString(result));
                return Content("-1,"+ errres.value);
            }
               
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> GetStatesList(int CountryId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    //ApplicationSettings.WebApiUrl = "http://localhost:5002/api/";
                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "Account/GetStatesList?CountryId=" + CountryId).Result;
                    var data = await requestUrl.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(data);
                    IEnumerable<States> filterStates = JsonConvert.DeserializeObject<List<States>>(Convert.ToString(result.value));
                    IEnumerable<States> states = filterStates.Select(e => new States
                    {StateId=e.StateId,
                    State=e.State,
                    Type=e.Type,
                    CountryId=e.CountryId
                        
                    });
                    // return Json(blogs.ToDataTablesResponse(dataRequest, blogs.Count()));
                    return Ok(states);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while fetching states.");
                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetDiscountCodesList()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    
                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "PromoCode/GetDiscountCodes").Result;
                    var data = await requestUrl.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(data);
                    IEnumerable<DiscountCodeListModel> filterCodes = JsonConvert.DeserializeObject<List<DiscountCodeListModel>>(Convert.ToString(result.data));
                    IEnumerable<DiscountCodeListModel> codes = filterCodes.Select(e => new DiscountCodeListModel
                    {
                        DiscountPercent = e.DiscountPercent,
                        DiscountCode=e.DiscountCode,
                        CreatedDate=e.CreatedDate,
                        ExpireDate=e.ExpireDate
                        

                    });
                    // return Json(blogs.ToDataTablesResponse(dataRequest, blogs.Count()));
                    return Ok(codes);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while fetching codes.");
                }
            }
        }

        public async Task<IActionResult> ResetLoginPassword(SetCustomerPassword setPassword)
        {
            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                setPassword.Email = HttpContext.Session.GetString("email");
                multiContent.Add(new StringContent(setPassword.Password), "Password");

                //_httpClient.BaseAddress = new Uri("http://localhost:5002/api/");

                _httpClient.BaseAddress = BaseEndpoint;
                var response = _httpClient.PostAsync("Account/setcustomerloginpass", CreateHttpContent<SetCustomerPassword>(setPassword)).Result;
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

        public async Task<IActionResult> GetProfileImage(UserProfileImageModel userProfileModel)
        {
            try
            {

                userProfileModel.Id = HttpContext.Session.GetString("id");
                using (var client = new HttpClient())
                {
                    try
                    {

                        var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetProfileImageCustomer?userId=" + userProfileModel.Id).Result;
                        var data = await requestUrl.Content.ReadAsStringAsync();
                        dynamic result = JObject.Parse(data);
                        UserProfileImageModel userProfile = JsonConvert.DeserializeObject<UserProfileImageModel>(Convert.ToString(result.data));
                        return Ok(userProfile);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "An error occurred while fetching codes.");
                    }
                }
            }

            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}