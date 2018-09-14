using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class C2BRequest
    {
        public string CommandID { get; set; }
        public string Amount { get; set; }
        public string Msisdn { get; set; }
        public string BillRefNumber { get; set; }
        public string ShortCode { get; set; }
    }
}
