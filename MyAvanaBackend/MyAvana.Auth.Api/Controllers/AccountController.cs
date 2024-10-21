using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.Auth.Api.Attributes;
using MyAvana.Auth.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyAvana.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
namespace MyAvana.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly HttpClient _httpClient;
        private readonly AvanaContext _avanacontext;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        public AccountController(IAccountService account, AvanaContext avanacontext, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _account = account;
            _avanacontext = avanacontext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            _configuration = configuration;
            _environment = hostingEnvironment;
        }
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] Signup signup)
        {
            var result = _account.SignUp(signup);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] Authentication authentication)
        {
            var result = _account.SignIn(authentication);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("activateuser")]
        public IActionResult ActivateUser([FromBody] CodeVerify codeVerify)
        {
            var result = _account.ActivateUser(codeVerify);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("resendcode")]
        public IActionResult ResendCode(string email)
        {
            var result = _account.ResendCode(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("forgetpassword")]
        public IActionResult ForgetPass(string email)
        {
            var result = _account.ForgetPass(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("setpass")]
        public async Task<IActionResult> SetPassAsync([FromBody] SetPassword setPassword)
        {
            var result = await _account.SetPass(setPassword);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] SetPassword setPassword)
        {
            var result = await _account.ChangePass(setPassword);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetAccountNo")]
        public IActionResult GetAccountNo(string email)
        {
            var result = _account.GetAccountNo(email);
            if (result.success) return Ok(result.user);
            return BadRequest(new JsonResult(result.error));
        }

        [HttpPost("WebSignup")]
        public IActionResult WebSignup([FromBody] Signup signup)
        {
            var result = _account.WebSignup(signup);
            if (result.userId != null) return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("WebSignupWithPayment")]
        public IActionResult WebSignupWithPayment([FromBody] SignupAndPayment signup)
        {
            var result = _account.WebSignupWithPayment(signup);
            if (result.userId != null && result.userId != "") return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("UserDetails")]
        public IActionResult UserDetails(string deviceId)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                NewUserEntityModel entity = JsonConvert.DeserializeObject<NewUserEntityModel>(content);
                if (entity != null)
                {
                    bool result = _account.updateDeviceId(entity, deviceId);
                    if (result)
                        entity.DeviceId = deviceId;
                    string HairAnalysisImage = _account.GetHailAnalysisIemage(entity.Id);
                    entity.HairAnalysisImage = HairAnalysisImage;
                    return Ok(new JsonResult(entity) { StatusCode = (int)HttpStatusCode.OK });
                }

                return BadRequest(new JsonResult("User not found") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception Ex)
            {
                return BadRequest(new JsonResult("Authorization token is not valid") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        [HttpPost("saveAIResult")]
        public IActionResult saveAIResult(UserEntity userDetails)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                    bool result = _account.saveAIResult(entity, userDetails.AIResult);
                    if (result)
                        entity.AIResult = userDetails.AIResult;
                    return Ok(new JsonResult(entity) { StatusCode = (int)HttpStatusCode.OK });
                }

                return BadRequest(new JsonResult("User not found") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception Ex)
            {
                return BadRequest(new JsonResult("Authorization token is not valid") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        [HttpPost("setcustomerpass")]
        public async Task<IActionResult> SetCustomerPassAsync([FromBody] SetCustomerPassword setPassword)
        {
            var result = await _account.SetCustomerPass(setPassword);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("alexalinking")]
        public IActionResult AlexaLinking([FromBody] AlexaLinkingModel alexaLinkingModel)
        {
            var result = _account.AlexaLinking(alexaLinkingModel);
            if (result.success) return Ok(result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        //[ApiKey]
        
        [HttpPost("SignUpShopifyOneTime")]
        public IActionResult SignUpShopifyOneTime([FromBody] ShopifyOrder signup)
        {
            var result = _account.SignUpShopifyOneTime(signup);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

      
        [HttpPost("SignUpShopifySubscription")]
        public IActionResult SignUpShopifySubscription([FromBody] ShopifyOrderNew signup)
        {
            var result = _account.SignUpShopifySubscription(signup);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("token")]
        public IActionResult GetToken(
            [FromForm] string grant_type,
            [FromForm] string code,
            [FromForm] string client_id,
            [FromForm] string client_secret,
            [FromForm] string redirect_uri,
            [FromForm] string refresh_token)
        {
            var result = _account.GetToken(code);
            if (result.success) return Ok(result.token.Value);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("getCustomerNonce")]
        public IActionResult GetCustomerNonce()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var result = _account.GetCustomerNonce(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("authenticateCustomerNonce")]
        public IActionResult AuthenticateCustomerNonce(string nonce)
        {
            var result = _account.AuthenticateCustomerNonce(nonce);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("DeleteCustomer")]
        public IActionResult DeleteCustomer(UserEntity userModel)
        {
            var result = _account.DeleteCustomer(userModel);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetUserById")]
        public IActionResult GetUserById(string userId)
        {
            var result = _account.GetUserById(userId);
            if (result.success) return Ok(result.user);
            return BadRequest(new JsonResult(result.error));
        }

        [HttpGet("CheckPromotionalPeriod")]
        public IActionResult CheckPromotionalPeriod()
        {
            var result = _account.CheckPromotionalPeriod();
            if (result) return Ok(result);
            return BadRequest(new JsonResult(false));
        }

        [HttpGet("GetSalonIdByUserID")]
        public IActionResult GetSalonIdByUserId(int UserId)
        {
            int result = _account.GetSalonIdByUserId(UserId);
            if (result != 0) return Ok(result);
            return BadRequest(new JsonResult(0));
        }
        [HttpPost("GetTrialDigitalAssessment")]
        public IActionResult GetTrialDigitalAssessment(DigitalAssessmentMarketModel digitalAssessmentMarketModel)
        {
            var result = _account.GetTrialDigitalAssessment(digitalAssessmentMarketModel);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetSalonNameByUserId")]
        public IActionResult GetSalonNameByUserId(int UserId)
        {
            var result = _account.GetSalonNameByUserId(UserId);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(""));
            
        }
        
        [HttpPost("saveAIResultv2")]
        public IActionResult saveAIResultv2(UserEntity userDetails)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                    bool result = _account.saveAIResultV2(entity, userDetails.AIResult, userDetails.HairType);
                    if (result)
                        entity.AIResult = userDetails.AIResult;
                    return Ok(new JsonResult(entity) { StatusCode = (int)HttpStatusCode.OK });
                }

                return BadRequest(new JsonResult("User not found") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception Ex)
            {
                return BadRequest(new JsonResult("Authorization token is not valid") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        //[HttpPost("SaveHairAnalysisStatus")]
        //public IActionResult SaveHairAnalysisStatus(StatusTracker tracker)
        //{
        //    var result = _account.SaveHairAnalysisStatus(tracker);
        //    if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
        //    return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });

        //}

        [HttpGet("getAdminNonce")]
        public IActionResult GetAdminNonce(string email)
        {
            var result = _account.GetAdminNonce(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("authenticateAdminNonce")]
        public IActionResult AuthenticateAdminNonce(string nonce)
        {
            var result = _account.AuthenticateAdminNonce(nonce);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("updateCustomer")]
        public IActionResult UpdateCustomer([FromBody] UpdateUserModel updateUserModel)
        {
            var result =  _account.UpdateCustomer(updateUserModel);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetCountriesList")]
        public IActionResult GetCountriesList()
        {
            List<Countries> result = _account.GetCountriesList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("GetStatesList")]
        public IActionResult GetStatesList(int CountryId)
        {
            List<States> result = _account.GetStatesList(CountryId);
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("IsEmailExists")]
        public IActionResult IsEmailExists(string email)
        {
            var result = _account.IsEmailExists(email);
           
            if (result) return Ok(true);
            return Ok(false);
        }

        [HttpPost("setcustomerloginPass")]
        public async Task<IActionResult> SetCustomerLoginPassAsync([FromBody] SetCustomerPassword setPassword)
        {
            var result = await _account.SetCustomerLoginPass(setPassword);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("SaveInAppPayment")]
        public IActionResult SaveInAppPayment([FromBody] InAppPaymentModel inAppPaymentModel)
        {
            var result = _account.SaveInAppPayment(inAppPaymentModel);
            if (result.success) return Ok(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("ValidatePurchase")]
        public async Task<IActionResult> ValidatePurchase([FromBody] PurchaseValidationRequest request)
        {
            if (string.IsNullOrEmpty(request.PurchaseToken) || string.IsNullOrEmpty(request.Platform) || string.IsNullOrEmpty(request.ProductId))
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            PurchaseValidationResult validationResult;

            // Check if the request is for a subscription or one-time purchase
            bool isSubscription = request.IsSubscription;

            if (request.Platform.ToLower() == "android")
            {
                validationResult = await ValidateAndroidPurchase(request.PurchaseToken, request.ProductId, isSubscription);
            }
            else if (request.Platform.ToLower() == "ios")
            {
                validationResult = await ValidateIOSPurchase(request.PurchaseToken, request.ProductId,request.TransactionId);
            }
            else
            {
                return BadRequest(new { success = false, message = "Unsupported platform" });
            }

            return Ok(new { success = validationResult.IsValid, message = validationResult.Message });
        }

        private async Task<PurchaseValidationResult> ValidateAndroidPurchase(string purchaseToken, string productId, bool isSubscription)
        {
            var packageName = "com.myavanaai";
            var accessToken = await GetGoogleAccessToken();
            var url = isSubscription
                ? $"https://androidpublisher.googleapis.com/androidpublisher/v3/applications/{packageName}/purchases/subscriptions/{productId}/tokens/{purchaseToken}"
                : $"https://androidpublisher.googleapis.com/androidpublisher/v3/applications/{packageName}/purchases/products/{productId}/tokens/{purchaseToken}";

            using (var httpClient = new HttpClient())  // Use HttpClient directly
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AndroidPurchaseResponse>(responseString);

                    if (isSubscription)
                    {
                        var expiryTimeMillis = jsonResponse?.ExpiryTimeMillis;
                        if (expiryTimeMillis != null)
                        {
                            var expiryTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(expiryTimeMillis)).UtcDateTime;
                                
                            if (expiryTime > DateTime.UtcNow)
                            {
                                return new PurchaseValidationResult
                                {
                                    IsValid = true,
                                    Message = $"Android subscription is valid and expires on {expiryTime}"
                                };
                            }
                            return new PurchaseValidationResult
                            {
                                IsValid = false,
                                Message = "Android subscription has expired."
                            };
                        }
                        else
                        {
                            return new PurchaseValidationResult
                            {
                                IsValid = false,
                                Message = "Expiry time not found in the response."
                            };
                        }
                    }
                    else
                    {
                        var purchaseState = jsonResponse?.PurchaseState;
                        if (purchaseState == 0) // 0 means purchase is completed
                        {
                            return new PurchaseValidationResult
                            {
                                IsValid = true,
                                Message = "Android one-time purchase validated successfully."
                            };
                        }
                        return new PurchaseValidationResult
                        {
                            IsValid = false,
                            Message = "Android one-time purchase not valid."
                        };
                    }
                }

                return new PurchaseValidationResult
                {
                    IsValid = false,
                    Message = $"Android validation failed: {responseString}"
                };
            }
        }

        private async Task<PurchaseValidationResult> ValidateIOSPurchase(string receiptData, string productId, string TransactionId)
        {
            var sharedSecret = "a78ae40ec8164b6bb0d275afbc80366f";
            //var productionUrl = "https://buy.itunes.apple.com/verifyReceipt";
            //var sandboxUrl = "https://sandbox.itunes.apple.com/verifyReceipt";
            var requestUrl = _configuration.GetSection("IOSInAppPurchase:InAPPApiUrl").Value; ; // Default to sandbox URL, switch to production as needed

            var payload = new AppleReceiptRequest
            {
                Password = sharedSecret,
                ReceiptData = receiptData,
                ExcludeOldTransactions = false
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(requestUrl, payload);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<IOSPurchaseResponse>(responseString);
                    var status = jsonResponse?.Status;

                    // Check for success status (0 = valid receipt)
                    if (status == 0)
                    {
                        var receiptInfo = jsonResponse?.LatestReceiptInfo;
                        var inAppReceipts = jsonResponse?.Receipt?.InApp; // For non-consumables

                        // Search for the product in LatestReceiptInfo (for auto-renewable subscriptions)
                        if (receiptInfo != null)
                        {
                            foreach (var receipt in receiptInfo)
                            {
                                if (receipt.ProductId == productId && receipt.TransactionId == TransactionId)
                                {
                                    var expirationDateMs = receipt.ExpiresDateMs;
                                    if (expirationDateMs != null)
                                    {
                                        var expirationDate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(expirationDateMs)).UtcDateTime;

                                        if (expirationDate > DateTime.UtcNow)
                                        {
                                            return new PurchaseValidationResult
                                            {
                                                IsValid = true,
                                                Message = $"iOS subscription is valid for {productId} and expires on {expirationDate}"
                                            };
                                        }

                                        return new PurchaseValidationResult
                                        {
                                            IsValid = false,
                                            Message = $"iOS subscription for {productId} has expired."
                                        };
                                    }
                                    else
                                    {
                                        return new PurchaseValidationResult
                                        {
                                            IsValid = false,
                                            Message = "Expiration date not found in receipt."
                                        };
                                    }
                                }
                            }
                        }

                        // Search for the product in InApp receipts (for non-consumable or consumable products)
                        if (inAppReceipts != null)
                        {
                            foreach (var receipt in inAppReceipts)
                            {
                                if (receipt.ProductId == productId && receipt.TransactionId==TransactionId)
                                {
                                    return new PurchaseValidationResult
                                    {
                                        IsValid = true,
                                        Message = $"iOS product {productId} found in in-app purchase receipt."


                                    };
                                }
                            }
                        }

                        // Return if the product is not found in the receipt
                        return new PurchaseValidationResult
                        {
                            IsValid = false,
                            Message = "iOS product not found in receipt."
                        };
                    }
                    else
                    {
                        // Handle different status codes (e.g., 21002, 21007)
                        return new PurchaseValidationResult
                        {
                            IsValid = false,
                            Message = $"iOS receipt validation failed. Status code: {status}"
                        };
                    }
                }

                // Handle cases where the request fails
                return new PurchaseValidationResult
                {
                    IsValid = false,
                    Message = $"iOS validation failed: {responseString}"
                };
            }
        }

        private async Task<string> GetGoogleAccessToken()
        {

            string jsonPath = Path.Combine(_environment.ContentRootPath, "myavana-a1d85284a51a.json");
            //string jsonPath = "D:\\Work2022\\MYAVANA 16feb2024\\MyAvana_Backend_ASP\\MyAvana.Auth.Api\\myavana-a1d85284a51a.json";

            // Read the JSON content from the file.
            string json = ReadJsonFromFile(jsonPath);

            // Parse the JSON content.
            var jsonData = JObject.Parse(json);

            // Extract client_email, private_key, and token_uri.
            var clientEmail = jsonData["client_email"].ToString();
            var privateKey = jsonData["private_key"].ToString();
            var tokenUri = jsonData["token_uri"].ToString();

            // Load the service account credentials from the JSON file.
            GoogleCredential credential;

            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/androidpublisher");
            }

            // Request an access token from the credentials.
            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync(tokenUri);

            return accessToken;
        }

        private string ReadJsonFromFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }


        [HttpPost("ReceiveAppleNotification")]
        public async Task<IActionResult> ReceiveAppleNotification([FromBody] AppleStoreNotification notification)
        {

            if (notification == null || string.IsNullOrEmpty(notification.signedPayload))
            {
                return BadRequest("Invalid or missing notification payload");
            }
            try
            {
                // Every encoded payload will have three parts: JWS header, payload, and signature representations.
                var splitParts = notification.signedPayload.Split('.');

                if (splitParts.Length != 3)
                {
                    return BadRequest("Malformed signed payload");
                }

                var payload = splitParts[1];

                // Decode the signed payload
                string decodedPayload = DecodeSignedPayload(payload);
                if (string.IsNullOrEmpty(decodedPayload))
                {
                    return BadRequest("Failed to decode payload");
                }

                var notificationV2 = JsonConvert.DeserializeObject<NotificationV2>(decodedPayload);
                if (notificationV2 == null)
                {
                    return BadRequest("Invalid decoded payload structure");
                }

                // Save the notification payload to the database (or perform any other business logic)
                string serializedPayload = JsonConvert.SerializeObject(notification);
               

                // Handle only the "DID_RENEW" notification type
                if (notificationV2.NotificationType == "DID_RENEW")
                {
                    var saveResult = _account.saveInAppPayload(payload, "ios");

                    if (!saveResult)
                    {
                        return StatusCode(500, "Failed to save payload in the database");
                    }
                    var renewalResult = await _account.HandleSubscriptionRenewal(notificationV2.Data);
                    if (!renewalResult)
                    {
                        return StatusCode(500, "Failed to handle subscription renewal");
                    }
                }
                else
                {
                    return BadRequest($"Notification type not supported: {notificationV2.NotificationType}");
                }

                // Acknowledge receipt of the notification
                return Ok("DID_RENEW notification processed successfully");
            }
            catch (JsonException jsonEx)
            {
                // Handle JSON serialization/deserialization issues
                return BadRequest($"JSON error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                // Log the error and return a generic message
                // You can log the error using a logger or another logging mechanism here
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

        //[HttpPost("HandleAndroidNotification")]
        //public async Task<IActionResult> HandleAndroidNotification([FromBody] GooglePlayBase64Payload notification)
        //{
        //    if (notification == null || notification.Message == null || string.IsNullOrEmpty(notification.Message.Data))
        //    {
        //        return BadRequest("Invalid notification payload");
        //    }
        //    try
        //    {
        //        string jsonNotification = JsonConvert.SerializeObject(notification);
        //        var res = _account.saveInAppPayload(jsonNotification, "andriod");
        //        // Decode the signed payload
        //        var decodedJson = DecodeSignedPayload(notification.Message.Data);
        //        var googlePlayNotification = JsonConvert.DeserializeObject<GooglePlayNotification>(decodedJson);

        //        // Check if the notification is for renewal (2 corresponds to SUBSCRIPTION_RENEWED)
        //        if (googlePlayNotification.SubscriptionNotification.NotificationType == 2) 
        //        {
        //            bool isRenewed = await _account.UpdateSubscriptionExpirationAndriod(googlePlayNotification.SubscriptionNotification.PurchaseToken,googlePlayNotification.EventTimeMillis);
        //            if (!isRenewed)
        //            {
        //                return StatusCode(500, "Failed to renew subscription");
        //            }

        //            return Ok("Android subscription renewed successfully");
        //        }
        //        else
        //        {
        //            return BadRequest("Received notification is not a subscription renewal");
        //        }
        //    }
        //    catch (JsonSerializationException jex)
        //    {
                
        //        return BadRequest("Failed to process the notification payload.");
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return StatusCode(500, "An error occurred while processing the notification.");
        //    }

        //}


        [HttpPost("HandleAndroidNotification")]
        public async Task<IActionResult> HandleAndroidNotification([FromBody] GooglePlayBase64Payload notification)
        {
            if (notification == null || notification.Message == null || string.IsNullOrEmpty(notification.Message.Data))
            {
                return BadRequest("Invalid notification payload");
            }

            try
            {
                // Move the core logic to _account service
                var result = await _account.HandleAndroidNotificationService(notification);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode, result.ErrorMessage);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the notification.");
            }
        }
    }
}