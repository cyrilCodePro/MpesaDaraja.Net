using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MpesaDaraja
{
    public class RootObj
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string ConversationID { get; set; }
        public string OriginatorConversationID { get; set; }
        public string OriginatorCoversationID { get; set; }
        public string OriginatorConverstionID { get; set; }
        public string OriginatorConversationId { get; set; }
        public string ResponseDescription { get; set; }
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ResultDesc { get; set; }
        public string ResultCode { get; set; }
        public string CustomerMessage { get; set; }
    }
    public class ProductionC2B
    {

        public string TransactionType { get; set; }
        public string TransID { get; set; }
        public string TransTime { get; set; }
        public decimal TransAmount { get; set; }
        public string BusinessShortCode { get; set; }
        public string BillRefNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrgAccountBalance { get; set; }
        public string ThirdPartyTransID { get; set; }
        public string MSISDN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

    }
    public class Mpesa
    {
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
        public static string Authenticate(string consumerKey, string consumersecret, bool isSandbox)
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
            RootObj test = JsonConvert.DeserializeObject<RootObj>(response.Content);
            return test.access_token;
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
            string callbackUrl, string accountRef,  string transactionDescription)
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
        public const string certKey = @"MIIGkzCCBXugAwIBAgIKXfBp5gAAAD+hNjANBgkqhkiG9w0BAQsFADBbMRMwEQYK
CZImiZPyLGQBGRYDbmV0MRkwFwYKCZImiZPyLGQBGRYJc2FmYXJpY29tMSkwJwYD
VQQDEyBTYWZhcmljb20gSW50ZXJuYWwgSXNzdWluZyBDQSAwMjAeFw0xNzA0MjUx
NjA3MjRaFw0xODAzMjExMzIwMTNaMIGNMQswCQYDVQQGEwJLRTEQMA4GA1UECBMH
TmFpcm9iaTEQMA4GA1UEBxMHTmFpcm9iaTEaMBgGA1UEChMRU2FmYXJpY29tIExp
bWl0ZWQxEzARBgNVBAsTClRlY2hub2xvZ3kxKTAnBgNVBAMTIGFwaWdlZS5hcGlj
YWxsZXIuc2FmYXJpY29tLmNvLmtlMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIB
CgKCAQEAoknIb5Tm1hxOVdFsOejAs6veAai32Zv442BLuOGkFKUeCUM2s0K8XEsU
t6BP25rQGNlTCTEqfdtRrym6bt5k0fTDscf0yMCoYzaxTh1mejg8rPO6bD8MJB0c
FWRUeLEyWjMeEPsYVSJFv7T58IdAn7/RhkrpBl1dT7SmIZfNVkIlD35+Cxgab+u7
+c7dHh6mWguEEoE3NbV7Xjl60zbD/Buvmu6i9EYz+27jNVPI6pRXHvp+ajIzTSsi
eD8Ztz1eoC9mphErasAGpMbR1sba9bM6hjw4tyTWnJDz7RdQQmnsW1NfFdYdK0qD
RKUX7SG6rQkBqVhndFve4SDFRq6wvQIDAQABo4IDJDCCAyAwHQYDVR0OBBYEFG2w
ycrgEBPFzPUZVjh8KoJ3EpuyMB8GA1UdIwQYMBaAFOsy1E9+YJo6mCBjug1evuh5
TtUkMIIBOwYDVR0fBIIBMjCCAS4wggEqoIIBJqCCASKGgdZsZGFwOi8vL0NOPVNh
ZmFyaWNvbSUyMEludGVybmFsJTIwSXNzdWluZyUyMENBJTIwMDIsQ049U1ZEVDNJ
U1NDQTAxLENOPUNEUCxDTj1QdWJsaWMlMjBLZXklMjBTZXJ2aWNlcyxDTj1TZXJ2
aWNlcyxDTj1Db25maWd1cmF0aW9uLERDPXNhZmFyaWNvbSxEQz1uZXQ/Y2VydGlm
aWNhdGVSZXZvY2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3RyaWJ1
dGlvblBvaW50hkdodHRwOi8vY3JsLnNhZmFyaWNvbS5jby5rZS9TYWZhcmljb20l
MjBJbnRlcm5hbCUyMElzc3VpbmclMjBDQSUyMDAyLmNybDCCAQkGCCsGAQUFBwEB
BIH8MIH5MIHJBggrBgEFBQcwAoaBvGxkYXA6Ly8vQ049U2FmYXJpY29tJTIwSW50
ZXJuYWwlMjBJc3N1aW5nJTIwQ0ElMjAwMixDTj1BSUEsQ049UHVibGljJTIwS2V5
JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1zYWZh
cmljb20sREM9bmV0P2NBQ2VydGlmaWNhdGU/YmFzZT9vYmplY3RDbGFzcz1jZXJ0
aWZpY2F0aW9uQXV0aG9yaXR5MCsGCCsGAQUFBzABhh9odHRwOi8vY3JsLnNhZmFy
aWNvbS5jby5rZS9vY3NwMAsGA1UdDwQEAwIFoDA9BgkrBgEEAYI3FQcEMDAuBiYr
BgEEAYI3FQiHz4xWhMLEA4XphTaE3tENhqCICGeGwcdsg7m5awIBZAIBDDAdBgNV
HSUEFjAUBggrBgEFBQcDAgYIKwYBBQUHAwEwJwYJKwYBBAGCNxUKBBowGDAKBggr
BgEFBQcDAjAKBggrBgEFBQcDATANBgkqhkiG9w0BAQsFAAOCAQEAC/hWx7KTwSYr
x2SOyyHNLTRmCnCJmqxA/Q+IzpW1mGtw4Sb/8jdsoWrDiYLxoKGkgkvmQmB2J3zU
ngzJIM2EeU921vbjLqX9sLWStZbNC2Udk5HEecdpe1AN/ltIoE09ntglUNINyCmf
zChs2maF0Rd/y5hGnMM9bX9ub0sqrkzL3ihfmv4vkXNxYR8k246ZZ8tjQEVsKehE
dqAmj8WYkYdWIHQlkKFP9ba0RJv7aBKb8/KP+qZ5hJip0I5Ey6JJ3wlEWRWUYUKh
gYoPHrJ92ToadnFCCpOlLKWc0xVxANofy6fqreOVboPO0qTAYpoXakmgeRNLUiar
0ah6M/q/KA==";

    }
}