using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poliment_UI.Models
{
    public class AdminDashboard
    {
        public int? TotalVoiceCall { get; set; }
        public int? TotalMessage { get; set; }
        public long? TotalVideoSpace { get; set; }
    }
}