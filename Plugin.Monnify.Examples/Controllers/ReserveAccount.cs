using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Plugin.Monnify.ReserveAccountApi;

using System.Threading.Tasks;

namespace Plugin.Monnify.Examples.Controllers
{
    public class ReserveAccount : Controller
    {
        public IMonnifyClient _monnifyClient;
        public ReserveAccount(IMonnifyClient monnifyClient)
        {
            _monnifyClient = monnifyClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateReserveAccount()
        {
            var response = await _monnifyClient.ReserveAccount(new CreateReserveAccountRequest
            {
                AccountName = "Joseph Ajileye",
                CustomerName = "Joseph Ajileye",
                CustomerEmail = "johndoe@gmail.com", // this is a unique field
                AccountReference = $"MNFREF002", // this is a unique field
                ContractCode = _monnifyClient.ContractCode,
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
