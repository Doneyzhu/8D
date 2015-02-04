using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.DAL.DBHelper;

namespace _8DManagementSystem
{
    public class BaseController : Controller
    {
        public NHibernate.ISession NhSession
        {
            get
            {
                return new DbSession().NhSession;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NHibernateHelper.BindSession();

            if (Request.Cookies["Id"] != null &&
                    Request.Cookies["UserPass"] != null &&
                    !string.IsNullOrEmpty(Request.Cookies["Id"].Value) &&
                    !string.IsNullOrEmpty(Request.Cookies["UserPass"].Value))
            {
                //MUser = NhSession.Get<Manage>(new Guid(Request.Cookies["Id"].Value));
                //if (MUser != null && MUser.UserPass != Request.Cookies["UserPass"].Value && !MUser.IfValid)
                //    MUser = null;
            }

            //ViewBag.MUser = MUser;
            //HttpContext.Items["CurrentManage"] = MUser;
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // NHibernateHelper.UnbindSession();
            //if (NHibernate.Context.CurrentSessionContext.HasBind(MvcApplication.NhsessionFactory))
            //{
            //    NHibernate.ISession sess = NHibernate.Context.CurrentSessionContext.Unbind(MvcApplication.NhsessionFactory);
            //    sess.Close();
            //}
        }


        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            NHibernateHelper.UnbindSession();
        }

    }
}
