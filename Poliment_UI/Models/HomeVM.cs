using Poliment_DL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poliment_UI.Models
{
    public class HomeVM
    {
        public List<FrontEndImageML> FrontEndImageML { get; set; }
        public List<FrontEndVideoML> FrontEndVideoML { get; set; }
        public string AbsolutePath { get; set; }
        public ManageHomeScreenML ManageHomeScreenML { get; set; }
        public List<AdminNewsML> AdminNewsML { get; set; }
        public List<AdminDevelopmentWorkML> AdminDevelopmentWorkML { get; set; }
        public List<BlockML> BlockML { get; set; }
        public List<AreaML> AreaML { get; set; }
       


    }
}