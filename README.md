# Monnify API for .Net Demo
Monnify API for .Net
This library makes it easy to consume the Payment API from .Net projects.

# How to install
From nuget
```
Install-Package Plugin.Monnify

```

Clone or fork this project to see all the examples, this project is built with .net core 3.1

# Usage
Store your keys in the appsetting.json file like this
```
"MonnifyUrl": {
    "BaseUrl": "https://sandbox.monnify.com/",
    "ApiKey": "",
    "SecretKey": "",
    "ContractCode": ""
  }
 ```
 
 Register this service in the Startup.cs file
 ``` 
  services.AddSingleton<IMonnifyClient>(x => new MonnifyClient(
    new MonnifySetting
    {
        BaseUrl = Configuration.GetValue<string>("MonnifyUrl:BaseUrl"),
        ApiKey = Configuration.GetValue<string>("MonnifyUrl:ApiKey"),
        SecretKey = Configuration.GetValue<string>("MonnifyUrl:SecretKey"),
        ContractCode = Configuration.GetValue<string>("MonnifyUrl:ContractCode")
    }));
  
 ```
 OR
 ```
     services.AddScoped<IMonnifyClient>(x => 
            new MonnifyClient(baseUrl:Configuration.GetValue<string>("MonnifyUrl:BaseUrl"),
            apiKey: Configuration.GetValue<string>("MonnifyUrl:ApiKey"), 
            secretKey: Configuration.GetValue<string>("MonnifyUrl:SecretKey"),
            contractCode:Configuration.GetValue<string>("MonnifyUrl:ContractCode")));
 ```
 
 # One Time Payment
 To consume the One Time API, inject the IMonnifyClient that is registered in the startup.cs file
 You can use the following snippet
 ```
 
            var oneTimePaymentApiResponse = await _monnifyClient.InitiateOneTimeTransaction(new OneTimeTransactionRequest
            {
                Amount = 350,
                CustomerName = "John Doe",
                CustomerEmail = "johndoe@gmail.com", // my real email... Feel free to contact me
                CustomerPhoneNumber = "07036000000", // my real phone number... Feel free to contact me
                PaymentReference = Guid.NewGuid().ToString(),
                PaymentDescription = "Testig one time payment",
                CurrencyCode = "NGN",
                ContractCode = _monnifyClient.ContractCode,
                RedirectUrl = Url.Action("ConfirmPayment", "Payments"),
                IncomeSplitConfig = new List<Incomesplitconfig>()
            });
            // Retun or persit the response            
 ```
 The Webhook notification 
 ```
        // Get the transaction details by payment reference
        var paymentDetail = await _monnifyClient.VerifyTransactionByPaymentReference(
                                        new TransactionStatusRequest { PaymentReference = paymentReference });

        //Verify Payment Status by calling back verifying the transaction by Transaction Number
        var response = await _monnifyClient.VerifyTransactionByTransactionReference(paymentDetail.ResponseBody.TransactionReference);
 ```
 # Create/Generate Invoice
 
 To consume the One Time API, inject the IMonnifyClient that is registered in the startup.cs file
 You can use the following snippet
 
 ```
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
 ```
 
 Use thesame webhook notification as describe above.
