using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Poliment_DL.Model;

namespace Poliment_UI.Models
{
    public class AdminMessageVM
    {
        public List<UserMessageML> UserMessageML { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }

        // Send message by Admin Model -- below property
        public List<BlockML> BlockML { get; set; }
        public string BlockName { get; set; }

        public List<GuestQueryML> GuestQueryML { get; set; }

        public List<ContactMessageML> ContactMessageML { get; set; }
    }
}