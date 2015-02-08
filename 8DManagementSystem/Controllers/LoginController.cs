﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _8DManagementSystem.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            bool success = false;
            bool keeping = false;
            if (Request.Cookies["LoginCookies"] != null)
            {
                success = true;
                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(Request.Form["loginName"]) && !string.IsNullOrEmpty(Request.Form["pwd"]))
            {
                string userName = Request.Form["loginName"];
                string pwd = Request.Form["pwd"];

                if (!string.IsNullOrEmpty(Request.Form["keeping"]))
                {
                    keeping = Request.Form["keeping"].Trim() == "on";
                }

                success = UserLogin(userName.Trim(), pwd, keeping);

                string requestUrl = Request.UrlReferrer.OriginalString;
                if (requestUrl.Contains("?"))
                {
                    requestUrl = requestUrl.Substring(requestUrl.IndexOf("?") + 1, requestUrl.Length - requestUrl.IndexOf("?") - 1);
                    requestUrl = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(requestUrl));
                }
                else
                {
                    requestUrl = Url.Action("Index", "Home");
                }
                if (success)
                    return Json(new { success = success, url = requestUrl, message = "成功！" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = success, message = "用户名或密码错误！" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }



    }
}
