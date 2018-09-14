using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class ExpressResponse
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ResultDesc { get; set; }
        public string ResponseDescription { get; set; }
        public string ResultCode { get; set; }
        public string CustomerMessage { get; set; }
    }
}
