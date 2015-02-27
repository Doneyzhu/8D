using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Filter;
using _8DManagementSystem.Models;

namespace _8DManagementSystem.Controllers
{
    public class EightDReportController : BaseController
    {
        //
        // GET: /EightDReport/
        [LoginFilter()]
        public ActionResult Index()
        {
            return View();
        }

        #region 新建
        [LoginFilter()]
        public ActionResult Assign8DReport(Guid? Id)
        {
            ReportAssignModel model = new ReportAssignModel();
            model.LoadBoardSelectList();
            model.LoadReportTypeSelectList();
            if (Id.HasValue)
            {
                Model.D_Report_Model dataModel = NhSession.Get<Model.D_Report_Model>(Id.Value);
                if (dataModel != null)
                {
                    model.ReportAssignGuid = dataModel.ReportGuid;
                    model.ReportTitle = dataModel.ReportTitle;
                    model.ReportType = dataModel.ReportTypeGuid.DicGuid.ToString();

                    model.BoardGuid = dataModel.ReportBoardGuid.BoardGuid.ToString();
                    model.ReportOwner = dataModel.ReportOwner;
                    model.ResponsibleQE = dataModel.ResponsibleQE;
                    model.ReportNo = dataModel.ReportNo;
                }

            }

            return View(model);
        }

        [HttpPost]
        [LoginFilter()]
        public ActionResult Assign8DReport(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                Model.D_Board_Model boardModel = new DAL.D_Board_DAL().GetModel(new Guid(model.BoardGuid));
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    dataModel = new Model.D_Report_Model();

                    //dataModel.CreateDateTime = DateTime.Now;
                    //dataModel.DataStatus = false;
                    //dataModel.CreateUserGuid = UserView.UserGuid;
                    //dataModel.CreateUserName = UserView.UserName;
                    dataModel.ReportNo = "AE" + boardModel.BoardName + "-" + DateTime.Now.ToString("yyyyMMddHHssmmfff");
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);
                }

                //dataModel.BoardName = model.BoardName;
                //dataModel.ModifyDateTime = DateTime.Now;
                //dataModel.ModifyUserGuid = UserView.UserGuid;
                //dataModel.ModifyUserName = UserView.UserName;

                dataModel.ReportTitle = model.ReportTitle;
                dataModel.ReportTypeGuid = new DAL.D_Dictionary_DAL().GetModel(new Guid(model.ReportType));
                dataModel.ReportBoardGuid = boardModel;
                dataModel.ReportOwner = model.ReportOwner;
                dataModel.ResponsibleQE = model.ResponsibleQE;


                success = new DAL.D_Report_DAL().Save(dataModel);
                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }

            return View(model);
        }

        #endregion

    }
}
