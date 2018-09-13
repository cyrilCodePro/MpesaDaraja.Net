using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class ReversalRequest
    {
        public string CommandID { get; set; }
        public string ReceiverParty { get; set; }
        public string ReceiverIdentifierType { get; set; }
        public string Remarks { get; set; }
        public string Initiator { get; set; }
        public string SecurityCredential { get; set; }
        public string QueueTimeOutURL { get; set; }
        public string ResultURL { get; set; }
        public string TransactionID { get; set; }
        public string Occasion { get; set; }
    }
}
