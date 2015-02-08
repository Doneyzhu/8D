using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.DAL.DBHelper;
using _8DManagementSystem.Common;

namespace _8DManagementSystem
{
    public class BaseController : Controller
    {
        //登录用户
        public Model.D_User_Model UserView { get; set; }

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

            try
            {
                #region 验证用户是否登陆过
                if (Request.Cookies["LoginCookie"] != null)
                {
                    if (Request.Cookies["LoginCookie"]["membername"] != null &&
                        Request.Cookies["LoginCookie"]["memberpassword"] != null &&
                        Request.Cookies["LoginCookie"]["keeping"] != null)
                    {
                        string memberName = Request.Cookies["LoginCookie"]["membername"];
                        memberName = DESEncrypt.Decrypt(memberName, "membername");
                        memberName = HttpUtility.UrlDecode(memberName);
                        string memberPassword = Request.Cookies["LoginCookie"]["memberpassword"];
                        memberPassword = DESEncrypt.Decrypt(memberPassword, "memberpassword");
                        memberPassword = HttpUtility.UrlDecode(memberPassword);
                        string keeping = Request.Cookies["LoginCookie"]["keeping"];
                        if (memberName != "" && memberPassword != "" && keeping != "")
                        {
                            //MemberLogin(memberName, memberPassword, keeping == "1");

                            //bool result = UserLogin(memberName, memberPassword, keeping == "1");
                            #region 用户登录
                            Model.D_User_Model model = new DAL.D_User_DAL().GetUserByUserLoginName(memberName);
                            if (model != null)
                            {
                                if (new DAL.D_User_DAL().PassWordMD5(memberPassword).Equals(model.PassWord.ToLower()))
                                {
                                    UserView = model;
                                    ViewBag.ManageView = model;

                                    if (Request.Cookies["LoginCookie"] == null)
                                    {
                                        SetLoginCookies(memberName, memberPassword, keeping == "1");
                                    }

                                }
                            }
                            #endregion
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                HttpContextWrite(filterContext, ex.Message);
            }
            HttpContext.Items["User"] = UserView;
        }


        public void HttpContextWrite(ActionExecutingContext filterContext, string message)
        {
            HttpContextBase context = filterContext.HttpContext;
            context.Response.Write(message);
            filterContext.Result = new HttpUnauthorizedResult();
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // NHibernateHelper.UnbindSession();
            //if (NHibernate.Context.CurrentSessionContext.HasBind(MvcApplication.NhsessionFactory))
            //{
            //    NHibernate.ISession sess = NHibernate.Context.CurrentSessionContext.Unbind(MvcApplication.NhsessionFactory);
            //    sess.Close();
            //}

            NHibernateHelper.UnbindSession();

        }


        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            NHibernateHelper.UnbindSession();
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="longinName">登录名</param>
        /// <param name="loginPwd">密码</param>
        /// <param name="keeping">是否保存密码</param>
        public bool UserLogin(string longinName, string loginPwd, bool keeping)
        {

            DAL.D_User_DAL userDAL = new DAL.D_User_DAL();
            Model.D_User_Model model = userDAL.GetUserByUserLoginName(longinName);
            if (model != null)
            {
                if (userDAL.PassWordMD5(loginPwd).Equals(model.PassWord.ToLower()))
                {
                    UserView = model;
                    ViewBag.ManageView = UserView;

                    if (Request.Cookies["LoginCookie"] == null)
                    {
                        SetLoginCookies(longinName, loginPwd, keeping);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="keeping">是否保持登录</param>
        public void SetLoginCookies(string userName, string passWord, bool keeping)
        {
            HttpCookie ck = new HttpCookie("LoginCookie");
            ck.Values.Add("membername", DESEncrypt.Encrypt(HttpUtility.UrlEncode(userName), "membername"));
            ck.Values.Add("memberpassword", DESEncrypt.Encrypt(HttpUtility.UrlEncode(passWord), "memberpassword"));
            ck.Values.Add("keeping", keeping ? "1" : "0");
            ck.Expires = DateTime.Now.AddDays(keeping ? 7 : 1);
            //ck.Domain = Request.ApplicationPath;
            Response.Cookies.Add(ck); //添加,使Cookies生效
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void ClearCookie()
        {
            if (Request.Cookies["LoginCookie"] != null)
            {
                HttpCookie ck = Request.Cookies["LoginCookie"];
                ck.Values.Add("membername", string.Empty);
                ck.Values.Add("memberpassword", string.Empty);
                ck.Values.Add("keeping", string.Empty);
                ck.Expires = DateTime.Now.AddDays(-1);
                //ck.Domain = Request.ApplicationPath;
                Response.Cookies.Set(ck);
            }
        }


    }
}
