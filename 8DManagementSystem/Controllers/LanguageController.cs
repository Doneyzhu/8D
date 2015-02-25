using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Filter;
using _8DManagementSystem.Models;

namespace _8DManagementSystem.Controllers
{
    public class LanguageController : BaseController
    {
        //
        // GET: /Language/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [LoginFilter()]
        public ActionResult SetLanguage(UserLanguageModel model)
        {
            bool success = false;

            try
            {
                Model.D_User_Model dataModel = new DAL.D_User_DAL().GetModel(model.UserGuid);
                dataModel.Languages = model.Language;
                success = new DAL.D_User_DAL().Save(dataModel);
                return Json(new { success = success, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }

        }

    }
}
