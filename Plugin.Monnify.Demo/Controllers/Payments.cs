
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
                PaymentDescription = "Testing one time payment",
                CurrencyCode = "NGN",
                ContractCode = _monnifyClient.ContractCode,
                //RedirectUrl = "https://localhost:44389/Payments/ConfirmPayment",
                RedirectUrl = $"https://{Request.Host.Value}{Url.Action("ConfirmPayment", "Payments")}",
                IncomeSplitConfig = new List<Incomesplitconfig>()
            });
            return View(oneTimePaymentApiResponse);
        }


        public async Task<IActionResult> CreateInvoice()
        {
            var invoiceResponse = await _monnifyClient.CreateInvoice(new InvoiceRequest
            {
                Amount = 500,
                ContractCode = _monnifyClient.ContractCode,
                CurrencyCode = "NGN",
                ExpiryDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd HH:mm:ss"),
                CustomerEmail = "kunlesymls@gmail.com",
                CustomerName = "Joseph Ajileye",
                Description = $"Wallet Top-up for Joseph Ajileye",
                InvoiceReference = Guid.NewGuid().ToString(),
                RedirectUrl = $"https://{Request.Host.Value}{Url.Action("ConfirmPayment", "Payments")}",
                //AccountReference = customer.CustomerWallet.MonnifyAccountReference
            });
            return View(invoiceResponse);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmPayment(string paymentReference)
        {
            var paymentDetail = await _monnifyClient.VerifyTransactionByPaymentReference(
                                        new TransactionStatusRequest { PaymentReference = paymentReference });

            ////Verify Payment Status by calling back to Monnify
            //var response = await _monnifyClient.VerifyTransactionByTransactionReference(new TransactionStatusRequest
            //                                        { TransactionReference = paymentDetail.ResponseBody.TransactionReference });

            if (paymentDetail.ResponseBody.PaymentStatus.Trim().ToLower() == "paid")
            {
                //persist and log successful response 
                return View(paymentDetail);
            }

            return View(paymentDetail);
        }
    }
}
