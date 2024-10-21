using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.Auth.Api.Contract
{
    public interface IAccountService
    {
        (bool success, string error) SignUp(Signup signup);
        (string userId, string error) WebSignup(Signup signup);
        (JsonResult result, bool success, string error) SignIn(Authentication authentication);
        (JsonResult result, bool success, string error) ForgetPass(string email);
        (JsonResult result, bool success, string error) ActivateUser(CodeVerify codeVerify);
        Task<(bool success, string error)> SetPass(SetPassword setPassword);
        Task<(bool success, string error)> ChangePass(SetPassword setPassword);
        UserEntity GetAccountNo(ClaimsPrincipal user);

        string GetHailAnalysisIemage(Guid UserId);
        (JsonResult result, bool success, string error) ResendCode(string email);
        (UserEntity user, bool success, string error) GetAccountNo(string email);
        bool updateDeviceId(NewUserEntityModel userEntity, string deviceId);
        bool saveAIResult(UserEntity userEntity, string aiResult);
        Task<(bool success, string error)> SetCustomerPass(SetCustomerPassword setPassword);
        (string code, bool success, string error) AlexaLinking(AlexaLinkingModel alexaLinkingModel);
        (JsonResult token, bool success, string error) GetToken(string code);
     
        (bool success, string error) SignUpShopifySubscription(ShopifyOrderNew signup);
         (bool success, string error) SignUpShopifyOneTime(ShopifyOrder signup);
       

        (JsonResult result, bool success, string error) GetCustomerNonce(string email);
        (JsonResult result, bool success, string error) AuthenticateCustomerNonce(string nonce);
        (bool success, string error) DeleteCustomer(UserEntity userModel);

        (bool success, string error) ShopifySample(ShopifyOrder signup);
        (UserEntity user, bool success, string error) GetUserById(string userId);
        bool CheckPromotionalPeriod();
        int GetSalonIdByUserId(int UserId);
        (bool success, string error) GetTrialDigitalAssessment(DigitalAssessmentMarketModel digitalAssessmentMarketModel);

        (SalonLoginDetail result, bool success) GetSalonNameByUserId(int UserId);
        
        bool saveAIResultV2(UserEntity userEntity, string aiResult, string hairType);
        (string userId, string error) WebSignupWithPayment(SignupAndPayment signup);

        //(bool success, string error) SaveHairAnalysisStatus(StatusTracker tracker);
        (JsonResult result, bool success, string error) GetAdminNonce(string email);
        (WebLogin result, bool success, string error) AuthenticateAdminNonce(string nonce);

        (bool success, string error) UpdateCustomer(UpdateUserModel updateUserModel);
        List<Countries> GetCountriesList();
        List<States> GetStatesList(int CountryId);

        bool IsEmailExists(string email);
        Task<(bool success, string error)> SetCustomerLoginPass(SetCustomerPassword setPassword);
        (bool success, string error) SaveInAppPayment(InAppPaymentModel inAppPaymentModel);
        Task<bool> HandleSubscriptionRenewal(NotificationV2Data notification);
        bool saveInAppPayload(string notification, string platform);
        Task<bool> UpdateSubscriptionExpirationAndriod(string PurchaseToken, long PurchaseTime);
        Task<(bool IsSuccess, string Message, int StatusCode, string ErrorMessage)> HandleAndroidNotificationService(GooglePlayBase64Payload notification);
    }
}
