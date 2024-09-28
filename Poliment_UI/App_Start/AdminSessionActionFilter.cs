using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Poliment_UI
{
    public class AdminSessionActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContextORG)
        {
            if (HttpContext.Current.Session["AdminId"] == null)
            {
                filterContextORG.Result = new RedirectToRouteResult(new
                   RouteValueDictionary(new { controller = "Admin", action = "Index", area = "" }));
            }

        }

    }
}