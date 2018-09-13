using MpesaDaraja.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja
{
    public class Mpesa
    {
        public static string Authenticate(string consumerKey, string consumersecret, bool isSandbox = true)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            string appkey = $"{consumerKey}:{consumersecret}";
            string auth = ToBase64String(appkey);
            var client = new RestClient(baseurl);
            var request = new RestRequest("oauth/v1/generate?grant_type=client_credentials", Method.GET);
            request.Parameters.Clear();
            request.AddHeader("authorization", "Basic " + auth);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content;
            }
            AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(response.Content);
            return accessToken.access_token;
        }

        public static ProductionC2B C2BData(JObject expectedJsonObject)
        {
            ProductionC2B test = JsonConvert.DeserializeObject<ProductionC2B>(expectedJsonObject.ToString());
            return test;
        }
        public static string[] AccountBalance(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,
            string password, string Business, string remarks,
            string queueTimeUrl, string resultUrl)
        {
            string security = Encryption(password.Trim());
            JObject simulate = new JObject
            {

                { "Initiator", initiatorName },
                { "SecurityCredential", security },
                 { "CommandID", "AccountBalance" },
                { "PartyA", Business },
                { "IdentifierType", "4" },
                { "Remarks", remarks },
                { "QueueTimeOutURL", queueTimeUrl },
                { "ResultURL", resultUrl }
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/accountbalance/v1/query", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }

            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
              "ConversationId:" + test.ConversationID,
              "OriginatorConversationID:"+  test.OriginatorConversationID,
              " ResponseDescription:"+ test.ResponseDescription,
            };
        }
        public static string[] Reversal(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,
            string password, string transactionId, decimal amount, string receiverparty, string remarks,
            string queueTimeUrl, string resultUrl, string occassion)
        {
            ServicePointManager.SecurityProtocol =
             SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string security = Encryption(password.Trim());
            JObject simulate = new JObject
            {
                { "Initiator", initiatorName },
                { "SecurityCredential", security },
                { "CommandID","TransactionReversal" },
                { "TransactionID", transactionId },
                { "Amount", amount },
                { "ReceiverParty", receiverparty },
                { "RecieverIdentifierType","11" },
                { "ResultURL", resultUrl },
                { "QueueTimeOutURL", queueTimeUrl },
                { "Remarks", remarks },
                { "Occasion", occassion }
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/reversal/v1/request", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }

            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
                "ConversationId:" +test.ConversationID,
                "OriginatorConversationID:"+test.OriginatorConversationID,
               "ResponseDescription:"+ test.ResponseDescription,
            };
        }
        public static string[] TransactionStatus(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,
            string password, string transactionId, string Business, string remarks, string queueTimeUrl, string resultUrl, string occassion)
        {

            ServicePointManager.SecurityProtocol =
             SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string security = Encryption(password.Trim());
            JObject simulate = new JObject
            {
                { "Initiator", initiatorName},
                { "SecurityCredential", security },
                { "CommandID", "TransactionStatusQuery" },
                { "TransactionID", transactionId },
                { "PartyA", Business },
                { "IdentifierType", "1" },
                { "ResultURL", resultUrl},
                {"QueueTimeOutURL", queueTimeUrl },
                { "Remarks", remarks },
                { "Occasion",  occassion}
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/transactionstatus/v1/query", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }
            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
                "OriginatorConverstionID:"+test.OriginatorConverstionID,
                "ConversationID:"+test.ConversationID,
                "ResponseDescription:"+test.ResponseDescription,
            };
        }
        
        public static string ToBase64String(string sInput)
        {
            return Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(sInput));
        }
        public static string[] C2B(string consumerKey, string consumersecret,
            bool isSandbox, string paybill, decimal amount, string phone, string reference)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            JObject simulate = new JObject
            {
                { "ShortCode", paybill },
                { "CommandID", "CustomerPayBillOnline" },
                { "Amount", amount },
                { "Msisdn", phone },
                { "BillRefNumber", reference }
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/c2b/v1/simulate", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }

            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
               "ConversationId:"+ test.ConversationID,
               "OriginatorConverstionID:" +test.OriginatorConverstionID,
                "ResponseDescription:"+test.ResponseDescription,
            };
        }
        public static string[] B2B(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,
            string password, decimal amount,
            string Business, string ReceivingBusiness, string remarks, string queueTimeUrl, string resultUrl,
         string accountRef, int SenderIdentifierType, int RecieverIdentifierType)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string security = Encryption(password.Trim());
            JObject simulate = new JObject
            {
                { "Initiator", initiatorName},
                { "SecurityCredential", security},
               { "CommandID", "BusinessToBusinessTransfer"},
                {"SenderIdentifierType",SenderIdentifierType },
                {"RecieverIdentifierType",RecieverIdentifierType},
                { "Amount", amount },
                { "PartyA", Business },
                { "PartyB", ReceivingBusiness},
                {"AccountReference", accountRef },
                { "Remarks", remarks},
                { "QueueTimeOutURL", queueTimeUrl} ,
                { "ResultURL", resultUrl}
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/b2b/v1/paymentrequest", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }

            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
               "ConversationID:"+ test.ConversationID,
               "OriginatorConversationId:"+ test.OriginatorConversationId,
                "ResponseDescription:"+test.ResponseDescription,
            };

        }
        public static string[] B2C(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,
            string password, decimal amount, string Business, string customerPhone, string remarks, string queueTimeUrl,
            string resultUrl, string occassion)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string security = Encryption(password.Trim());
            JObject simulate = new JObject
            {
                { "InitiatorName", initiatorName},
                { "SecurityCredential", security},
               { "CommandID", "BusinessPayment"},
                { "Amount", amount },
                { "PartyA", Business },
                { "PartyB", customerPhone},
                { "Remarks", remarks},
                { "QueueTimeOutURL", queueTimeUrl} ,
                { "ResultURL", resultUrl},
                { "Occassion",  occassion}
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/b2c/v1/paymentrequest", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }

            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
                "ConversationID:"+test.ConversationID,
                "OriginatorConversationId:"+test.OriginatorConversationId,
                "ResponseDescription:"+test.ResponseDescription,
            };
        }
        public static string[] LiLipaNaMpesaOnlineStatus(string consumerKey, string consumersecret, string password,
            bool isSandbox, DateTime TimeOfTransaction, string BusinessShortcode, string checkoutRequestID)
        {
            ServicePointManager.SecurityProtocol =
             SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            JObject simulate = new JObject
            {
                { "BusinessShortCode", BusinessShortcode } ,
                { "Password",ToBase64String(password.Trim()) },
                { "Timestamp", TimeOfTransaction},
                { "CheckoutRequestID",checkoutRequestID  }
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/stkpushquery/v1/query", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }

            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            {
               "CheckoutRequestID:"+ test.CheckoutRequestID,
                "MerchantRequestID:"+test.MerchantRequestID,
                "ResponseDescription:"+test.ResponseDescription,
                "CustomerMessage:"+test.CustomerMessage,
               "ResponseCode:" +test.ResponseCode
            };
        }
        public static string TimeStamp(DateTime TimeOfTransaction)
        {
            string month = TimeOfTransaction.Month < 10 ? "0" + TimeOfTransaction.Month : TimeOfTransaction.Month.ToString();
            string day = TimeOfTransaction.Day < 10 ? "0" + TimeOfTransaction.Day : TimeOfTransaction.Day.ToString();
            string hour = TimeOfTransaction.Hour < 10 ? "0" + TimeOfTransaction.Hour : TimeOfTransaction.Hour.ToString();
            string min = TimeOfTransaction.Minute < 10 ? "0" + TimeOfTransaction.Minute : TimeOfTransaction.Minute.ToString();
            string sec = TimeOfTransaction.Second < 10 ? "0" + TimeOfTransaction.Second : TimeOfTransaction.Second.ToString();
            return $"{TimeOfTransaction.Year}{month}{day}{hour}{min}{sec}";
        }

        public static string[] LipaNaMpesaOnlineInitiate(string consumerKey, string consumersecret, string password,
            bool isSandbox, DateTime TimeOfTransaction, decimal amount, string customerPhone, string BusinessPaybillOrTill,
            string callbackUrl, string accountRef, string transactionDescription)
        {
            ServicePointManager.SecurityProtocol =
               SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string key = BusinessPaybillOrTill + password + TimeStamp(TimeOfTransaction);
            JObject simulate = new JObject
               {

                { "BusinessShortCode",BusinessPaybillOrTill },
                { "Password", ToBase64String(key.Trim())},
                { "Timestamp", TimeStamp(TimeOfTransaction)},
                { "TransactionType", "CustomerPayBillOnline" },
                { "Amount", (int)amount},
                {"PartyA", customerPhone},
                { "PartyB", BusinessPaybillOrTill },
                { "PhoneNumber", customerPhone },
                { "CallBackURL", callbackUrl },
                { "AccountReference",accountRef },
                { "TransactionDesc", transactionDescription}

                };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);

            var request = new RestRequest("mpesa/stkpush/v1/processrequest", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", simulate, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.Content.Contains("error"))
            {
                return response.Content.Split(':');
            }
            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return new string[]
            { "CheckoutRequestID:"+ test.CheckoutRequestID,
                "MerchantRequestID:"+test.MerchantRequestID,
                "ResponseDescription:"+test.ResponseDescription,
                "CustomerMessage:"+test.CustomerMessage,
               "ResponseCode:" +test.ResponseCode
            };
        }
        public static string RegisterUrl(string consumerKey, string consumersecret, bool isSandbox, string paybill,
            string validationUrl, string confirmationUrl)
        {
            JObject register = new JObject
            {
                { "ShortCode", paybill },
                { "ResponseType", "String" },
                { "ConfirmationURL", confirmationUrl },
                { "ValidationURL", validationUrl }
            };
            string auth = Authenticate(consumerKey, consumersecret, isSandbox);
            string baseurl = "https://sandbox.safaricom.co.ke";
            if (isSandbox == false)
            {
                baseurl = "https://api.safaricom.co.ke";
            }
            var client = new RestClient(baseurl);
            var request = new RestRequest("mpesa/c2b/v1/registerurl", Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer " + auth);
            request.AddParameter("application/json", register, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;

        }
        public static string Encryption(string strText)
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = buildDir + @"\Security\certificate.txt";
            string certKey = File.ReadAllText(filePath);

            byte[] binaryCertData = Convert.FromBase64String(certKey);
            X509Certificate2 certFicate = new X509Certificate2(binaryCertData);
            string xmlKey = certFicate.PublicKey.Key.ToXmlString(false);
            var testData = Encoding.GetEncoding("iso-8859-1").GetBytes(strText);

            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(xmlKey);
                    var encryptedData = rsa.Encrypt(testData, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

    }
}