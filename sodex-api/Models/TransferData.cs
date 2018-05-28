using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sodex_api.Models
{
    public class TransferData
    {
        public long Amount { get; set; }
        public String Sender { get; set; }
        public String Receiver { get; set; }
        public String SenderPrivateKey { get; set; }
    }
}