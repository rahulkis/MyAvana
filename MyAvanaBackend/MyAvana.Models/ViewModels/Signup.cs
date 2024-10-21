using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class Signup
    {
        [EmailAddress]
        public string Email { get; set; }
        //[Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
		public int CountryCode { get; set; }
        public bool? CustomerType { get; set; }
        public bool? IsProCustomer { get; set; }
        public bool? IsPaid { get; set; }
        public int CustomerTypeId { get; set; }
        public bool? BuyHairKit { get; set; }
        public int? SalonId { get; set; }

        public int? CreatedByUserId { get; set; }
        public string KitSerialNumber { get; set; }
        public bool? IsInfluencer { get; set; }
    }

    public class SignupAndPayment
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public int CountryCode { get; set; }
        public bool? CustomerType { get; set; }
        public bool? IsProCustomer { get; set; }
        public bool? IsPaid { get; set; }
        public int CustomerTypeId { get; set; }
        public bool? BuyHairKit { get; set; }
        public int? SalonId { get; set; }

        public int? CreatedByUserId { get; set; }
        
        public string SubscriptionId { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string CardOwnerFirstName { get; set; }
        [Required]
        public string CardOwnerLastName { get; set; }
        [Required]
        public long? ExpirationYear { get; set; }
        // [CreditCard]
        public string CardNumber { get; set; }
        [Required]
        public long? ExpirationMonth { get; set; }
        [Required]
        public string CVV2 { get; set; }
        public int  StateId { get; set; }
        public int CountryId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string userId { get; set; }
        public string KitSerialNumber { get; set; }
        public bool? IsSubscriptionPayment { get; set; }
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

    public class HairKitUserSignupModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public CustomerHairkit customer { get; set; }
    }
    public class CustomerHairkit
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class ChangeUserTypeModel
    {
        public string UserId { get; set; }
        public int? CustomerTypeId { get; set; }
    }

    #region Shopify
    public class ShopifyOrder
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public long? app_id { get; set; }
        public string browser_ip { get; set; }
        public bool buyer_accepts_marketing { get; set; }
        public string cancel_reason { get; set; }
        public DateTime? cancelled_at { get; set; }
        public string cart_token { get; set; }
        public long? checkout_id { get; set; }
        public object checkout_token { get; set; }
        public object client_details { get; set; }
        public DateTime? closed_at { get; set; }
        public string confirmation_number { get; set; }
        public bool confirmed { get; set; }
        public string contact_email { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public string current_subtotal_price { get; set; }
        public PriceSet current_subtotal_price_set { get; set; }
        public object current_total_additional_fees_set { get; set; }
        public string current_total_discounts { get; set; }
        public PriceSet current_total_discounts_set { get; set; }
        public object current_total_duties_set { get; set; }
        public string current_total_price { get; set; }
        public PriceSet current_total_price_set { get; set; }
        public string current_total_tax { get; set; }
        public PriceSet current_total_tax_set { get; set; }
        public string customer_locale { get; set; }
        public string device_id { get; set; }
        public List<object> discount_codes { get; set; }
        public string email { get; set; }
        public bool estimated_taxes { get; set; }
        public string financial_status { get; set; }
        public string fulfillment_status { get; set; }
        public object landing_site { get; set; }
        public object landing_site_ref { get; set; }
        public object location_id { get; set; }
        public object merchant_of_record_app_id { get; set; }
        
        public string name { get; set; }
        public object note { get; set; }
        public List<object> note_attributes { get; set; }
        public int number { get; set; }
        public int order_number { get; set; }

        //public string total_price { get; set; }
        public string order_status_url { get; set; }
        public object original_total_additional_fees_set { get; set; }
        public object original_total_duties_set { get; set; }
        public List<string> payment_gateway_names { get; set; }
        public object phone { get; set; }
        public object po_number { get; set; }
        public string presentment_currency { get; set; }
        public DateTime? processed_at { get; set; }
        public object reference { get; set; }
        public object referring_site { get; set; }
        public object source_identifier { get; set; }
        public string source_name { get; set; }
        public object source_url { get; set; }
        public string subtotal_price { get; set; }
        public PriceSet subtotal_price_set { get; set; }
        public string tags { get; set; }
        public bool tax_exempt { get; set; }
        public List<object> tax_lines { get; set; }
        public bool taxes_included { get; set; }
        public bool test { get; set; }
        public string token { get; set; }
        public string total_discounts { get; set; }
        public PriceSet total_discounts_set { get; set; }
        public string total_line_items_price { get; set; }
        public PriceSet total_line_items_price_set { get; set; }
        public string total_outstanding { get; set; }
        public string total_price { get; set; }
        public PriceSet total_price_set { get; set; }
        public PriceSet total_shipping_price_set { get; set; }
        public string total_tax { get; set; }
        public PriceSet total_tax_set { get; set; }
        public string total_tip_received { get; set; }
        public int total_weight { get; set; }
        public DateTime? updated_at { get; set; }
       
        public string user_id { get; set; }
        public BillingAddress billing_address { get; set; }
        public Customer customer { get; set; }
        public List<object> discount_applications { get; set; }
        public List<object> fulfillments { get; set; }
        public List<LineItem> line_items { get; set; }
        public object payment_terms { get; set; }
        public List<object> refunds { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public List<ShippingLine> shipping_lines { get; set; }
    }

    public class ShopifyOrderSubscription
    {
        public long id { get; set; }
        public long Shopifyid { get; set; }
        public string cancellationReason { get; set; }
        public DateTime? cancelledat { get; set; }
        public object client_details { get; set; }
        public object closed_at { get; set; }
        public bool confirmed { get; set; }
        public string contact_email { get; set; }
        public DateTime createdat { get; set; }
        public DateTime nextOrderDate { get; set; }
        public string deliveryPrice { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public int number { get; set; }
        public int order_number { get; set; }
        public string order_status_url { get; set; }
        public object phone { get; set; }
        public bool test { get; set; }
        public string user_id { get; set; }
        public BillingAddress billing_address { get; set; }
        public CustomerSubscriber customer { get; set; }
        public List<LineItem> line_items { get; set; }
    }

    public class BillingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public object address2 { get; set; }
        public string company { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class Customer
    {
        public long id { get; set; }
        public string email { get; set; }
        public object created_at { get; set; }
        public object updated_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string state { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public string phone { get; set; }
        public EmailMarketingConsent email_marketing_consent { get; set; }
        public object sms_marketing_consent { get; set; }
        public string tags { get; set; }
        public string currency { get; set; }
        public string admin_graphql_api_id { get; set; }
        public DefaultAddress default_address { get; set; }
    }
    public class EmailMarketingConsent
    {
        public string state { get; set; }
        public object opt_in_level { get; set; }
        public DateTime? consent_updated_at { get; set; }
    }

    public class CustomerSubscriber
    {
        public long id { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string state { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public string phone { get; set; }
        public DefaultAddress default_address { get; set; }
    }

    public class DefaultAddress
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
        public object company { get; set; }
        public string address1 { get; set; }
        public object address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool @default { get; set; }
    }

    public class LineItem
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public List<object> attributed_staffs { get; set; }
        public int current_quantity { get; set; }
        public int fulfillable_quantity { get; set; }
        public string fulfillment_service { get; set; }
        public object fulfillment_status { get; set; }
        public bool gift_card { get; set; }
        public int grams { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        //public PriceSet price_set { get; set; }
        public bool product_exists { get; set; }
        public long product_id { get; set; }
        public List<object> properties { get; set; }
        public int quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string sku { get; set; }
        public bool taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public object variant_id { get; set; }
        public object variant_inventory_management { get; set; }
        public object variant_title { get; set; }
        public object vendor { get; set; }
        public List<object> tax_lines { get; set; }
        public List<object> duties { get; set; }
        public List<object> discount_allocations { get; set; }
    }
    public class ShippingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public object address2 { get; set; }
        public string company { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class ShippingLine
    {
        public long id { get; set; }
        public object carrier_identifier { get; set; }
        public object code { get; set; }
        public string discounted_price { get; set; }
        //public PriceSet discounted_price_set { get; set; }
        public object phone { get; set; }
        public string price { get; set; }
        //public PriceSet price_set { get; set; }
        public object requested_fulfillment_service_id { get; set; }
        public string source { get; set; }
        public string title { get; set; }
        public List<object> tax_lines { get; set; }
        public List<object> discount_allocations { get; set; }
    }
    public class PresentmentMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class PriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class ShopMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }
    #endregion
    public class ShopifyOrderNew
    {
        public long id { get; set; }
        public DateTime order_placed { get; set; }
        public string delivery_interval { get; set; }
        public string billing_interval { get; set; }
        public long order_id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string s_first_name { get; set; }
        public string s_last_name { get; set; }
        public string s_address1 { get; set; }
        public string s_address2 { get; set; }
        public string s_phone { get; set; }
        public string s_city { get; set; }
        public string s_zip { get; set; }
        public string s_province { get; set; }
        public string s_country { get; set; }
        public string s_company { get; set; }
        public string s_country_code { get; set; }
        public string s_province_code { get; set; }
        public string b_first_name { get; set; }
        public string b_last_name { get; set; }
        public string b_address1 { get; set; }
        public string b_city { get; set; }
        public string b_zip { get; set; }
        public string b_province { get; set; }
        public string b_country { get; set; }
        public string b_country_code { get; set; }
        public string b_province_code { get; set; }
        public string total_value { get; set; }
        public string status { get; set; }

        public DateTime? cancelled_on { get; set; }
        public DateTime? paused_on { get; set; }
        public string card_brand { get; set; }
        public string card_expiry_month { get; set; }
        public string card_expiry_year { get; set; }
        public string card_last_digits { get; set; }
        public List<Item> items { get; set; }
        public List<BillingAttempt> billing_attempts { get; set; }
        public List<LogEntry> log { get; set; }
    }
    public class Item
    {
        public int id { get; set; }
        public string product_id { get; set; }
        public string variant_id { get; set; }
        public string title { get; set; }
        public string variant_sku { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
        public string total_discount { get; set; }
        public string discount_per_item { get; set; }
        public int taxable { get; set; }
        public int requires_shipping { get; set; }
        public string original_price { get; set; }
        public double original_amount { get; set; }
        public double discount_value { get; set; }
        public double discount_amount { get; set; }
        public string final_price { get; set; }
        public double final_amount { get; set; }
        public List<object> properties { get; set; }
        public int is_one_time_item { get; set; }
        public string selling_plan_id { get; set; }
        public List<object> cycle_discounts { get; set; }
        public List<object> discount_codes { get; set; }
    }
    public class BillingAttempt
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }
        public string triggered_manually { get; set; }
        public string customer_authentication_challenge_url { get; set; }
    }

    public class LogEntry
    {
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
    public class DigitalAssessmentMarketModel
    {
        public string userId { get; set; }
        public bool? isTrialOn { get; set; }
    }

    public class UpdateUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public Guid UserId { get; set; }
        public int CreatedByUserId { get; set; }
        public bool? IsInfluencer { get; set; }
    }
    public class PaymentResponse
    {
        public Subscription Subscription { get; set; }
        public Charge Charge { get; set; }
        public string Error { get; set; }
    }
    public class InAppPaymentModel
    {
        public Guid UserId { get; set; }
        public string paymentAmount { get; set; }
        public string transactionId { get; set; }
        public string transactionDate { get; set; }
        public string productId { get; set; }
        public string providerName { get; set; }
        public string purchaseToken { get; set; }
    }
    public class AppleStoreNotification
    {
        [JsonProperty("signedPayload")]
        public string signedPayload { get; set; }

    }
    public class NotificationV2
    {
        [JsonProperty("notificationType")]
        public string NotificationType { get; set; }
        
        [JsonProperty("notificationUUID")]
        public string NotificationUUID { get; set; }
        [JsonProperty("data")]
        public NotificationV2Data Data { get; set; }
    }

    public class NotificationV2Data
    {
        [JsonProperty("appAppleId")]
        public long AppAppleId { get; set; }
        [JsonProperty("bundleId")]
        public string BundleId { get; set; }
        [JsonProperty("bundleVersion")]
        public string BundleVersion { get; set; }
        [JsonProperty("environment")]
        public string Environment { get; set; }
        [JsonProperty("signedTransactionInfo")]
        public string SignedTransactionInfo { get; set; }
        [JsonProperty("signedRenewalInfo")]
        public string SignedRenewalInfo { get; set; }
    }
    public class SignedRenewalInfo
    {
        [JsonProperty("originalTransactionId")]
        public string OriginalTransactionId { get; set; }

        [JsonProperty("autoRenewProductId")]
        public string AutoRenewProductId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("autoRenewStatus")]
        public int AutoRenewStatus { get; set; }

        [JsonProperty("renewalPrice")]
        public long RenewalPrice { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("signedDate")]
        public long SignedDate { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("recentSubscriptionStartDate")]
        public long RecentSubscriptionStartDate { get; set; }

        [JsonProperty("renewalDate")]
        public long RenewalDate { get; set; }
    }
    public class SignedTransactionInfo
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("originalTransactionId")]
        public string OriginalTransactionId { get; set; }

        [JsonProperty("webOrderLineItemId")]
        public string WebOrderLineItemId { get; set; }

        [JsonProperty("bundleId")]
        public string BundleId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("subscriptionGroupIdentifier")]
        public string SubscriptionGroupIdentifier { get; set; }

        [JsonProperty("purchaseDate")]
        public long PurchaseDate { get; set; }

        [JsonProperty("originalPurchaseDate")]
        public long OriginalPurchaseDate { get; set; }

        [JsonProperty("expiresDate")]
        public long ExpiresDate { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("inAppOwnershipType")]
        public string InAppOwnershipType { get; set; }

        [JsonProperty("signedDate")]
        public long SignedDate { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("transactionReason")]
        public string TransactionReason { get; set; }

        [JsonProperty("storefront")]
        public string Storefront { get; set; }

        [JsonProperty("storefrontId")]
        public string StorefrontId { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
    public class SubscriptionNotification
    {
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("notificationType")]
        public int NotificationType { get; set; }
        [JsonProperty("purchaseToken")]
        public string PurchaseToken { get; set; }
        [JsonProperty("subscriptionId")]
        public string SubscriptionId { get; set; }
    }
    public class PurchaseValidationRequest
    {
        public string PurchaseToken { get; set; }
        public string Platform { get; set; } // "ios" or "android"
        public string ProductId { get; set; } // Sent from the mobile app
        public string UserId { get; set; }
        public bool IsSubscription { get; set; } // True for subscription, false for one-time purchase
        public InAppPaymentModel Inappmodel { get; set; }
        public string TransactionId  { get; set; }
    }
    public class PurchaseValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
    public class AndroidPurchaseResponse
    {
        public string ExpiryTimeMillis { get; set; }
        public int PurchaseState { get; set; }
    }
    public class IOSPurchaseResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("receipt")]
        public IOSReceiptData Receipt { get; set; }  // This is where the "receipt" data from Apple's response is stored

        [JsonProperty("latest_receipt_info")]
        public List<IOSReceipt> LatestReceiptInfo { get; set; }  // This is for subscription receipts
    }
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
    
    public class AppleReceiptRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("receipt-data")]
        public string ReceiptData { get; set; }

        [JsonProperty("exclude-old-transactions")]
        public bool ExcludeOldTransactions { get; set; }
    }
    public class IOSReceiptData
    {
        [JsonProperty("in_app")]
        public List<IOSReceipt> InApp { get; set; }  // This will store in-app purchase data
    }

    public class IOSReceipt
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("expires_date_ms")]
        public string ExpiresDateMs { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }
    }
    public class GooglePlayNotification
    {
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("packageName")]
        public string PackageName { get; set; }
        [JsonProperty("eventTimeMillis")]
        public long EventTimeMillis { get; set; }
        [JsonProperty("subscriptionNotification")]
        public SubscriptionNotification SubscriptionNotification { get; set; }
    }
    public class GooglePlayBase64Payload
    {
        [JsonProperty("message")]
        public PubSubMessage Message { get; set; }
    }

    public class PubSubMessage
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
