using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookNet
{
    public enum Roles
    {
        Admin
    }

    public class AuthorizationAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Errors", action = "UnAuthorized" }));

            //base.HandleUnauthorizedRequest(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userAuth = httpContext.Session["userAuth"];

            return (userAuth != null && Roles.Split(',').Contains(userAuth.ToString()));
        }

        public static bool IsAdminLogedIn()
        {
            var userAuth = HttpContext.Current.Session["userAuth"];

            return (userAuth != null && userAuth.ToString() == BookNet.Roles.Admin.ToString());
        }
    }
}