using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class RegisterUrlRequest
    {
        public string ValidationURL { get; set; }
        public string ConfirmationURL { get; set; }
        public string ResponseType { get; set; }
        public string ShortCode { get; set; }
    }
}
