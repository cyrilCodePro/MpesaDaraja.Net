using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MpesaDaraja;
using MpesaDaraja.Models;

namespace Sample.Controllers
{
    public class MpesaController : Controller
    {
        private readonly MpesaKeys _options;

        public MpesaController(IOptions<MpesaKeys> options)
        {
            _options = options.Value;
        }
        public IActionResult Index()
        {
            string[] hello = new string[] { "Mpesa Api Dubbed Daraja", "Send and Receive money" };
            return Json(hello);
        }

        public async Task<IActionResult> GetAuthToken()
        {
            Mpesa mpesa = new Mpesa();
            var response = await mpesa.GetAccessTokenAsync(_options.ConsumerKey, _options.ConsumerSecret);
            return Json(response);
        }

        public async Task<IActionResult> GetAccountBalance()
        {
            Mpesa mpesa = new Mpesa();
            AccountBalanceRequest accountBalanceRequest = new AccountBalanceRequest
            {
                Initiator = "testapi",
                Password = "Safaricom214!",
                PartyA = "600214",
                Remarks = "Test",
                QueueTimeOutURL = "https://testurl.co.ke",
                ResultURL = "https://testurl.co.ke"
            };

            var response = await mpesa.AccountBalanceAsync(_options.ConsumerKey, _options.ConsumerSecret, accountBalanceRequest);
            return Json(response);
        }

        public async Task<IActionResult> SendReversalRequest()
        {
            Mpesa mpesa = new Mpesa();

            ReversalRequest reversalRequest = new ReversalRequest
            {
                ReceiverParty = "600214",
                Remarks = "Test",
                Initiator = "testapi",
                Password = "Safaricom214!",
                QueueTimeOutURL = "https://testurl.co.ke",
                ResultURL = "https://testurl.co.ke",
                TransactionID = "MIC8LOY6P8",
                Occasion = "Test"
            };

            var response = await mpesa.ReversalAsync(_options.ConsumerKey, _options.ConsumerSecret, reversalRequest);
            return Json(response);
        }

        public async Task<IActionResult> GetTransactionStatus()
        {
            TransactionStatusRequest transactionStatus = new TransactionStatusRequest
            {
                PartyA = "600214",
                Remarks = "Test",
                Initiator = "testapi",
                Password = "Safaricom214!",
                QueueTimeOutURL = "https://testurl.co.ke",
                ResultURL = "https://testurl.co.ke",
                TransactionID = "MIC8LOY6P8",
                Occasion = "Test"
            };

            Mpesa mpesa = new Mpesa();
            var response = await mpesa.TransactionStatusAsync(_options.ConsumerKey, _options.ConsumerSecret, transactionStatus);
            return Json(response);
        }

        public async Task<IActionResult> SimulateC2B()
        {
            C2BRequest c2BRequest = new C2BRequest
            {
                Amount = "10",
                Msisdn = "254712345678",
                BillRefNumber = "UDF9FDKJ98",
                ShortCode = "600214"
            };

            Mpesa mpesa = new Mpesa();
            var response = await mpesa.C2BAsync(_options.ConsumerKey, _options.ConsumerSecret, c2BRequest);
            return Json(response);
        }

        public async Task<IActionResult> B2BpaymentRequest()
        {
            B2BRequest b2BRequest = new B2BRequest
            {
                Amount = "10",
                PartyA = "600214",
                SenderIdentifier = "4",
                PartyB = "600214",
                RecieverIdentifierType = "4",
                Remarks = "Test",
                Initiator = "testapi",
                Password = "Safaricom214!",
                QueueTimeOutURL = "https://testurl.co.ke",
                ResultURL = "https://testurl.co.ke",
                AccountReference = "YU9783D98D"
            };

            Mpesa mpesa = new Mpesa();
            var response = await mpesa.B2BAsync(_options.ConsumerKey, _options.ConsumerSecret, b2BRequest);
            return Json(response);
        }

        public async Task<IActionResult> B2CPaymentRequest()
        {
            B2CRequest b2CRequest = new B2CRequest
            {
                InitiatorName = "testapi",
                Password = "Safaricom214!",
                Amount = "10",
                PartyA = "600214",
                PartyB = "254712345678",
                Remarks = "Test",
                QueueTimeOutURL = "https://testurl.co.ke",
                ResultURL = "https://testurl.co.ke",
                Occassion = "Test"
            };

            Mpesa mpesa = new Mpesa();
            var response = await mpesa.B2CAsync(_options.ConsumerKey, _options.ConsumerSecret, b2CRequest);
            return Json(response);
        }

        public async Task<IActionResult> StkPushQuery()
        {
            ExpressQueryRequest queryRequest = new ExpressQueryRequest
            {
                BusinessShortCode = _options.ShortCode,
                Password = _options.PassKey,
                CheckoutRequestID = "ws_CO_DMZ_76690535_14092018152240763"
            };

            Mpesa mpesa = new Mpesa();
            var response = await mpesa.StkPushQueryAsync(_options.ConsumerKey, _options.ConsumerSecret, queryRequest);
            return Json(response);
        }

        public async Task<IActionResult> StkPushInitiate()
        {
            ExpressInitiateRequest expressInitiateRequest = new ExpressInitiateRequest
            {
                BusinessShortCode = _options.ShortCode,
                Password = _options.PassKey,
                Amount = "10",
                PartyA = "254712345678",
                PartyB = _options.ShortCode,
                PhoneNumber = "254712345678",
                CallBackURL = "https://testurl.co.ke",
                AccountReference="YR9HS9SD2",
                TransactionDesc = "Test",
            };

            Mpesa mpesa = new Mpesa();
            var response = await mpesa.StkPushInitiateAsync(_options.ConsumerKey, _options.ConsumerSecret, expressInitiateRequest);
            return Json(response);
        }
    }
}