    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _8DManagementSystem.Filter
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            bool accept = false;

            HttpContextBase context = filterContext.HttpContext;

            Models.UserModel user = context.Items["User"] as Models.UserModel;
            if (user != null)
            {
                accept = true;
            }

            if (!accept)
            {
                filterContext.Result = new RedirectResult("~/Login/Index?" + Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(HttpContext.Current.Request.Url.ToString())));
            }
        }
    }
}