using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poliment_UI.Models
{
    public class VoiceCallDeSerialize
    {
        public string status { get; set; }
        public string mobile { get; set; }
        public string invalidMobile { get; set; }
        public string transactionId { get; set; }
        public string statusCode { get; set; }
        public string reason { get; set; }
    }

    // Class to serialize bulk sms

    public class Sm
    {
        public string message { get; set; }
        public List<string> to { get; set; }
    }

    public class RootObject
    {
        public string sender { get; set; }
        public string route { get; set; }
        public string country { get; set; }
        public string unicode { get; set; }
        public List<Sm> sms { get; set; }
    }

    public class DeSerializeMessageResult
    {
        public string message { get; set; }
        public string type { get; set; }
    }

}