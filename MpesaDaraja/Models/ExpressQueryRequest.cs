using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class ExpressQueryRequest
    {
        public string BusinessShortCode { get; set; }
        public string Password { get; set; }
        public string TimeStamp { get; set; }
        public string CheckoutRequestID { get; set; }
    }
}
