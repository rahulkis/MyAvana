using HubSpot.NET.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Logger.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using Stripe;
using System.Collections.Generic;

namespace MyAvana.Payments.Api.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IHostingEnvironment _env;
        public readonly AvanaContext _context;
        private readonly IStripeServices _stripe;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly HubSpotApi _hubSpotApi;
        public PaymentServices(IHostingEnvironment hostingEnvironment, AvanaContext avanaContext,
                               IConfiguration configuration, IStripeServices stripe, ITokenService tokenService,
                               ILogger logger)
        {
            _context = avanaContext;
            _configuration = configuration;
            _stripe = stripe;
            _env = hostingEnvironment;
            _tokenService = tokenService;
            _logger = logger;
            _hubSpotApi = new HubSpotApi(_configuration.GetSection("Hubspot:Key").Value);
        }
        public (bool success, string error) Checkout(CheckoutRequest checkout, ClaimsPrincipal user)
        {
            try
            {
                var subscription = _context.SubscriptionsEntities.Where(s => s.StripePlanId.Trim() == checkout.SubscriptionId.Trim()).FirstOrDefault();
                if (subscription != null)
                {
                    var accountNo = _tokenService.GetAccountNo(user);
                    if (accountNo != null)
                    {
                        checkout.Amount = subscription.Amount;
                        var paymentResponse = _stripe.StripPayments(checkout, accountNo);
                        if (paymentResponse.charge != null)
                        {
                            _context.PaymentEntities.Add(new PaymentEntity()
                            {
                                EmailAddress = accountNo.Email,
                                CCNumber = checkout.CardNumber,
                                CreatedDate = DateTime.UtcNow,
                                PaymentAmount = subscription.Amount.ToString(),
                                PaymentId = Guid.NewGuid(),
                                SubscriptionId = checkout.SubscriptionId.ToString(),
                                ProviderId = paymentResponse.charge.Id,
                                ProviderName = "STRIPE"
                            });
                            accountNo.StripeCustomerId = paymentResponse.charge.CustomerId;
                            accountNo.Country = checkout.Country;
                            accountNo.State = checkout.State;
                            accountNo.ZipCode = checkout.Zipcode;
                            accountNo.Address = checkout.Address;
                            _context.SaveChanges();
                            return (true, "");
                        }
                        return (false, "Error in processing payments." + "Error :- " + paymentResponse.error);

                    }
                    return (false, "Invalid User");
                }
                return (false, "Invalid subscription Id");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }
        public (bool success, string error) SaveAppleResponse(AppleRequest appleRequest, ClaimsPrincipal user)
        {
            try
            {
                var accountNo = _tokenService.GetAccountNo(user);
                if (accountNo != null)
                {
                    _context.PaymentEntities.Add(new PaymentEntity()
                    {
                        EmailAddress = accountNo.Email,
                        CCNumber = "",
                        CreatedDate = DateTime.UtcNow,
                        PaymentAmount = "",
                        PaymentId = Guid.NewGuid(),
                        SubscriptionId = appleRequest.SubscriptionId,
                        ProviderId = appleRequest.TransactionID,
                        ProviderName = "APPLE"
                    });
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Invalid User.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }
        }

        public (bool success, string error) SavePromoCodeSubscription(PromoCodeSubscription codeSubscription, ClaimsPrincipal user)
        {
            try
            {
                var accountNo = _tokenService.GetAccountNo(user);

                if (accountNo != null)
                {
                    var response = _context.PromoCodes.Where(x => x.Code == codeSubscription.PromoCode).FirstOrDefault();
                    if (response.ExpireDate >= DateTime.UtcNow)
                    {
                        var subscribe = _context.PaymentEntities.Where(x => x.SubscriptionId == codeSubscription.PromoCode && x.EmailAddress == accountNo.Email).FirstOrDefault();
                        if (subscribe == null)
                        {
                            _context.PaymentEntities.Add(new PaymentEntity()
                            {
                                EmailAddress = accountNo.Email,
                                CCNumber = "",
                                CreatedDate = DateTime.UtcNow,
                                PaymentAmount = "",
                                PaymentId = Guid.NewGuid(),
                                SubscriptionId = response.StripePlanId,
                                ProviderId = null,
                                ProviderName = "PROMOCODE"
                            });
                            _context.SaveChanges();
                            return (true, "");
                        }
                        return (false, "Code already applied.");
                    }
                    return (false, "Invalid PromoCode.");
                }
                return (false, "Invalid User.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }
        }

        public (bool success, string error) GetPaymentStatus(ClaimsPrincipal user)
        {
            try
            {
                var accountNo = _tokenService.GetAccountNo(user);
                if (accountNo != null)
                {
                    // if (!_context.PaymentEntities.Where(s => s.EmailAddress.ToLower() == accountNo.Email.ToLower()).Any())
                    //   return (true, "");
                    return (false, "");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }

        public (bool success, string error) CancelStripeSubscription(ClaimsPrincipal user)
        {
            try
            {
                var userDetail = _tokenService.GetAccountNo(user);
                if (userDetail != null)
                {
                    var payments = _context.PaymentEntities.Where(x => x.EmailAddress == userDetail.Email).FirstOrDefault();
                    var paymentResponse = _stripe.CancelStripeSubscription(payments.ProviderId);
                    if (paymentResponse.success)
                    {
                        var planName = _context.SubscriptionsEntities.Where(x => x.StripePlanId == payments.ProviderId).Select(x => x.PlanName).FirstOrDefault();
                        return (true, "You are successfully unsubscribed from : " + planName);
                    }
                    else
                    {
                        return (false, paymentResponse.error);
                    }
                }
                return (false, "Something went wrong!");
            }
            catch (Exception ex) { return (false, "Something went wrong!"); }
        }

        public (bool success, string error) CardPayment(CheckoutRequest checkout)
        {
            try
            {
                //var subscription = _context.SubscriptionsEntities.Where(s => s.StripePlanId.Trim() == checkout.SubscriptionId.Trim()).FirstOrDefault();
                //if (subscription != null)
                //{
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
                    // var paymentResponse = _stripe.StripPayments(checkout, accountNo);
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
                                ProviderName = (checkout.IsSubscriptionPayment ?? false) ? "STRIPE" : "OneTime",
                                ExpirationDate = DateTime.UtcNow.AddMonths(1).AddDays(-1)
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
                        
                            customerTypeHistory.NewCustomerTypeId =(int)accountNo.CustomerTypeId;
                            customerTypeHistory.Comment = "Updated after stripe payment completed.";
                            _context.UserEntity.Update(accountNo);
                            _context.CustomerTypeHistory.Add(customerTypeHistory);
                            _context.SaveChanges();
                            return (true, "");
                        }
                        return (false, "Error in processing payments." + "Error :- " + paymentResponse.Error);

                    }
                    return (false, "Invalid User");
                //}
               // return (false, "Invalid subscription Id");

            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }
        public (Subscription charge, string error) StripPayments(CheckoutRequest checkout, UserEntity userEntity)
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

        public (Charge charge, string error) StripOneTimePayments(CheckoutRequest checkout, UserEntity userEntity)
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


                //Create Charge Object with details of Charge
                var options = new ChargeCreateOptions
                {   //amount should be in cents so multiplying amount with 100
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
               
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (null, Ex.Message);
            }
        }

        public (bool success, string Error) UpdateCustomerSubscription(string subscriptionId, bool isactive)
        {
            try
            {
                var payments = _context.PaymentEntities.Where(x => x.ProviderId == subscriptionId).FirstOrDefault();
                if (payments != null)
                {
                    payments.IsActive = isactive;
                    _context.PaymentEntities.Update(payments);
                    _context.SaveChanges();
                    return (true, "updated successfully!");
                }
                return (false, "Something went wrong!");
            }
            catch (Exception ex) { return (false, "Something went wrong!"); }
        }
    }
}
