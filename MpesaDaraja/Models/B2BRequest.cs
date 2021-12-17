using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class B2BRequest
    {
        public string CommandID { get; set; }
        public string Amount { get; set; }
        public string PartyA { get; set; }
        public string SenderIdentifier { get; set; }
        public string PartyB { get; set; }
        public string RecieverIdentifierType { get; set; }
        public string Remarks { get; set; }
        public string Initiator { get; set; }
        public string Password { get; set; }
        public string SecurityCredential { get; set; }
        public string QueueTimeOutURL { get; set; }
        public string ResultURL { get; set; }
        public string AccountReference { get; set; }
    }
}
