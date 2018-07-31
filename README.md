# MpesaDaraja.Net

NugetPackage for .Net.

Supports .Net Framework 4.6.1 or higher.

Install the Nuget Package by going to Package Console Manager and type

                 Install-Package MPESADARAJA -Version 1.0.7

For C2B :
                 
                  include namespace
                  using MpesaDaraja;

              string []result=  Mpesa.C2B(string consumerKey, string consumersecret, bool isSandbox, string paybill, decimal amount, string phone, string reference);
      
     
1. for production registet Url then on your endpoint for validation url and confirmation url do this
         
                public JObject Confirm(JObject objects)
        {
            var data = Mpesa.C2BData(objects);
             return new JObject();
        }

FOR B2B:
 
                string []result=   Mpesa.B2B(string consumerKey, string consumersecret, bool isSandbox, string initiatorName, string password, decimal amount, string Business, string ReceivingBusiness, string remarks, string queueTimeUrl, string resultUrl, string accountRef, int SenderIdentifierType, int RecieverIdentifierType);
                
FOR B2C:

                string []result=Mpesa.B2C(string consumerKey, string consumersecret, bool isSandbox, string initiatorName, string password, decimal amount, string Business, string customerPhone, string remarks, string queueTimeUrl,string resultUrl, string occassion);
                
 FOR lipaNaMpesaOnline:
 
 
                 string []result=Mpesa.LipaNaMpesaOnlineInitiate(string consumerKey, string consumersecret, string password,  bool isSandbox, DateTime TimeOfTransaction, decimal amount, string customerPhone, string  BusinessPaybillOrTill,   string callbackUrl, string accountRef, [StringLength(20)] string transactionDescription); 
                 
                 
To check LipaNaMpesaStatus:
          
                 string[] result=Mpesa.LiLipaNaMpesaOnlineStatus(string consumerKey, string consumersecret, string password,  bool isSandbox, DateTime TimeOfTransaction, string BusinessShortcode, string checkoutRequestID);
                 
For Transaction Status:

                 string [] result=Mpesa.TransactionStatus(string consumerKey, string consumersecret, bool isSandbox, string    initiatorName,   string password,string transactionId,string BusinessShortcode, string remarks, string queueTimeUrl, string     resultUrl,string occassion);
                 
 For Reversal:
                
                     string [] result= Mpesa.Reversal(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,  string password,string transactionId,decimal amount,string receiverparty, string remarks,string queueTimeUrl, string resultUrl,string occassion);
                     
For AccountBalance:

                     string[] result=AccountBalance(string consumerKey, string consumersecret, bool isSandbox, string initiatorName,  string password,string Business, string remarks, string queueTimeUrl, string resultUrl);
                     
For Authentication token:

                     string token=Authenticate(string consumerKey, string consumersecret, bool isSandbox);
Please Note if Its SandBox Indicate IsSandBox equals true.
Make sure to get your credentials from
 https://developer.safaricom.co.ke
 
 NOTE PASSWORD FOR B2C ,C2B,REVERSAL Indicates Security Credentials
