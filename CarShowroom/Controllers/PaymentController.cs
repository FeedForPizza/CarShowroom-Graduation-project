using System.Web.Http;
using CarShowroom.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe.Checkout;

namespace CarShowroom.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("create-payment-intent")]
    [ApiController]
    public class PaymentIntentApiController : Controller
    { 

        // Endpoint for creating a payment
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public ActionResult Create(PaymentIntentCreateRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var amount = ViewBag.TotalSum;
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
            //RedirectToAction("ProcessPayment",email)
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("amount")]
            public int Amount { get; set; }
        }


    }
}

