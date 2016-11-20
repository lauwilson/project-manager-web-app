using System;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using Hemlock.Models.Enum;
using Hemlock.Models;
using System.Web.Routing;

namespace Hemlock.Controllers.ActionFilters
{
    public class PermissionsAttribute : ActionFilterAttribute
    {
        private readonly PermissionsEnum requiredPermissions;

        public PermissionsAttribute(PermissionsEnum requiredPermissions)
        {
            this.requiredPermissions = requiredPermissions;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = (Employee)HttpContext.Current.Session["User"];
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Error" },
                                                                                            { "action", "LoginRequired" } });
            }
            else
            {
                if (!((requiredPermissions & (PermissionsEnum)user.Permissions) == requiredPermissions))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Error" },
                                                                                                { "action", "PermissionDenied" } });
                }
            }
        }
    }
}
