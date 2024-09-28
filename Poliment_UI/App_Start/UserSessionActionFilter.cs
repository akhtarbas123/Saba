using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Poliment_UI
{
    public class UserSessionActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContextORG)
        {
            if (HttpContext.Current.Session["UserId"] == null)
            {
                filterContextORG.Result = new RedirectToRouteResult(new
                                   RouteValueDictionary(new { controller = "User", action = "Index", area = "" }));
            }

        }
    }
}