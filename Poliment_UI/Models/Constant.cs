using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Poliment_UI.Models
{
    public static class Constant
    {
        public static readonly string AbsalutePath = ConfigurationManager.AppSettings["AbsalutePath"];
    }
}