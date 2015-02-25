using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Filter;

namespace _8DManagementSystem.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [LoginFilter()]
        public ActionResult Index()
        {
            return View();
        }

    }
}
