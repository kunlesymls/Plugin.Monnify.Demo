
using Microsoft.AspNetCore.Mvc;

using Plugin.Monnify.Helpers;
using Plugin.Monnify.InvoiceApi;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Monnify.Demo.Controllers
{

    public class Payments : Controller
    {
        public IMonnifyClient _monnifyClient;
        public Payments(IMonnifyClient monnifyClient)
        {
            _monnifyClient = monnifyClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OneTimePayment()
        {
            var oneTimePaymentApiResponse = await _monnifyClient.InitiateOneTimeTransaction(new OneTimeTransactionRequest
            {
                Amount = 350,
                CustomerName = "Joseph Ajileye",
                CustomerEmail = "kunlesymls@gmail.com", // my real email... Feel free to contact me
                CustomerPhoneNumber = "07036927669", // my real phone number... Feel free to contact me
                PaymentReference = Guid.NewGuid().ToString(),
                paymentDescription = "Testig one time payment",
                CurrencyCode = "NGN",
                ContractCode = _monnifyClient.ContractCode,
                RedirectUrl = "https://localhost:44389/Payments/ConfirmPayment",
                IncomeSplitConfig = new List<Incomesplitconfig>()
            });
            return View(oneTimePaymentApiResponse);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmPayment(string paymentReference)
        {

            //Verify Payment Status by calling back to Monnify
            var response = await _monnifyClient.VerifyTransactionByPaymentReference(new TransactionStatusRequest { PaymentReference = paymentReference });
            if (response.ResponseBody.PaymentStatus.Trim().ToLower() == "paid")
            {
                //persist and log successful response 
                return View(response);
            }

            return View(response);
        }
    }
}
