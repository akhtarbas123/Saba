using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Poliment_DL.Model;

namespace Poliment_UI.Models
{
    public class LayoutVM
    {
        public AdminML AdminML { get; set; }
        public List<BlockML> BlockML { get; set; }
    }
}