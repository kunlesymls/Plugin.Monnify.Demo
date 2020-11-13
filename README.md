# Monnify API for .Net Demo
Monnify API for .Net
This library makes it easy to consume the Payment API from .Net projects.

# How to install
From nuget
```
Install-Package Plugin.Monnify

```

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
            return View(oneTimePaymentApiResponse);
 ```
 The Webhook notification 
 ```
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
 ```
 
