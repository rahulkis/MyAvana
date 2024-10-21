using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StripeWebhookController : ControllerBase
	{
        private readonly string _webhookSecret;
        private readonly IPaymentServices _paymentServices;
        public StripeWebhookController(IOptions<StripeOptions> options, IPaymentServices paymentServices)
        {
            _webhookSecret = options.Value.WebhookSigningKey;
			_webhookSecret = "whsec_tbvQTrvyupjq4N5oxf7ca6G9OMkREiFN";
			_paymentServices = paymentServices;
		}


		[HttpPost("stripe_webhooks")]
		public async Task<IActionResult> Index()
		{
			string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			try
			{
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], _webhookSecret,300,
	(long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
	false);
				var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
				switch (stripeEvent.Type)
				{
					case Events.PaymentIntentCreated:
						//once subscription added and payment need to be verifiy
						break;
					
					case Events.CustomerSubscriptionUpdated:
						// do something
						break;
					case Events.CustomerSubscriptionDeleted:
						// do something
						var sub = stripeEvent.Data.Object as Subscription;
						_paymentServices.UpdateCustomerSubscription(sub.Id, false);
						break;
					case Events.InvoicePaymentSucceeded:
						var subscription = stripeEvent.Data.Object as Invoice;
						// do something after payment succeeded
						_paymentServices.UpdateCustomerSubscription(subscription.SubscriptionId, true);
						break;
				}
				return Ok();
			}
			catch (StripeException e)
			{
				return BadRequest();
			}
		}
	}
}
