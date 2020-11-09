using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Plugin.Monnify.ReserveAccountApi;

using System.Threading.Tasks;

namespace Plugin.Monnify.Demo.Controllers
{
    public class ReserveAccount : Controller
    {
        public IMonnifyClient _monnifyClient;
        IConfiguration _configuration;
        public ReserveAccount(IMonnifyClient monnifyClient, IConfiguration configuration)
        {
            _monnifyClient = monnifyClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateReserveAccount()
        {
            var monnifyConfig = _configuration.GetSection("MonnifyUrl");
            var authResponse = await _monnifyClient.GetBearerAccessToken(monnifyConfig["ApiKey"], monnifyConfig["SecretKey"]);
            var response = await _monnifyClient.ReserveAccount(new CreateReserveAccountRequest
            {
                AccountName = "Joseph Ajileye",
                CustomerName = "Joseph Ajileye",
                CustomerEmail = "Kunlesymls@gmail.com",
                AccountReference = $"MNFREF001",
                ContractCode = monnifyConfig["ContractCode"],
            });
            if (response.RequestSuccessful)
            {
                //customerWallet.HasMonnifyAccount = true;
                //customerWallet.MonnifyAccountName = response.ResponseBody.AccountName;
                //customerWallet.MonnifyAccountNumber = response.ResponseBody.AccountNumber;
                //customerWallet.MonnifyAccountReference = response.ResponseBody.AccountReference;
                //customerWallet.MonnifyBankCode = response.ResponseBody.BankCode;
                //customerWallet.MonnifyBankName = response.ResponseBody.BankName;

                //_db.Entry(customerWallet).State = EntityState.Modified;
                //await _db.SaveChangesAsync();
                //return true;
                return View(response);
            }
            return View(response);
        }
    }
}
