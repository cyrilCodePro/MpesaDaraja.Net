﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaDaraja.Models
{
    public class AccountBalanceRequest
    {
        public string CommandID { get; set; }
        public string PartyA { get; set; }
        public IdentifierType IdentifierType { get; set; }
        public string Remarks { get; set; }
        public string Initiator { get; set; }
        public string Password { get; set; }
        public string SecurityCredential { get; set; }
        public string QueueTimeOutURL { get; set; }
        public string ResultURL { get; set; }
    }
}
