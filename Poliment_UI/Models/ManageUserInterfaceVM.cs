using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Poliment_UI.Models;
using Poliment_DL.Model;

namespace Poliment_UI.Models
{
    public class ManageUserInterfaceVM
    {
        public List<FrontEndImageML> FrontEndImageML { get; set; }
        public List<FrontEndVideoML> FrontEndVideoML { get; set; }
        public List<AdminNewsML> AdminNewsML { get; set; }
        public List<VoiceCallDetailsML> VoiceCallDetailsML { get; set; }
        public List<SentSmsDetailML> SentSmsDetailML { get; set; }
        public List<AdminDevelopmentWorkML> AdminDevelopmentWorkML { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int? totalVoiceCall { get; set; }
        public int? totalMessageSent { get; set; }

    }
}