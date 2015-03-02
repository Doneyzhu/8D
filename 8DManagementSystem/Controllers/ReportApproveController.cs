using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Filter;
using _8DManagementSystem.Model;
using _8DManagementSystem.Models;

namespace _8DManagementSystem.Controllers
{
    public class ReportApproveController : BaseController
    {
        //
        // GET: /ReportApprove/

        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter()]
        public ActionResult Approve(Guid? Id)
        {
            Report_WorkFlow_Log_Model logModel = new Report_WorkFlow_Log_Model();
            if (Id.HasValue)
            {
                Model.D_Report_Model dataModel = NhSession.Get<Model.D_Report_Model>(Id.Value);
                logModel.ReportGuid = dataModel.ReportGuid;
                logModel.ReportStatus = dataModel.ReportStatus;
            }
            return View(logModel);
        }

        [HttpPost]
        [LoginFilter()]
        public ActionResult Approve(Report_WorkFlow_Log_Model model)
        {
            bool success = false;

            try
            {
                string type = Request.Form["type"] == null ? string.Empty : Request.Form["type"].ToString();
                Model.D_Report_Model dataModel = NhSession.Get<Model.D_Report_Model>(model.ReportGuid);
                if (dataModel != null)
                {
                    if (!string.IsNullOrEmpty(type))
                    {
                        Model.D_WorkFlowLog_Model logModel = new D_WorkFlowLog_Model();
                        switch (type)
                        {
                            case "1":
                                if (dataModel.ReportStatus == (int)ReportStatusEnum.TeamLeader ||
                                    dataModel.ReportStatus == 0)
                                    dataModel.ReportStatus = (int)ReportStatusEnum.Sponsor;
                                else if (dataModel.ReportStatus == (int)ReportStatusEnum.Sponsor)
                                    dataModel.ReportStatus = (int)ReportStatusEnum.Approve;
                                else if (dataModel.ReportStatus == (int)ReportStatusEnum.Approve)
                                    dataModel.ReportStatus = (int)ReportStatusEnum.Completed;

                                logModel.OperateType = "Approve";
                                break;
                            case "2":
                                dataModel.ReportStatus = (int)ReportStatusEnum.TeamLeader;
                                logModel.OperateType = "Review";
                                break;
                            case "3":
                                dataModel.ReportStatus = (int)ReportStatusEnum.Sponsor;
                                logModel.OperateType = "Submit Review To Sponsor";
                                break;
                            case "4":
                                dataModel.ReportStatus = (int)ReportStatusEnum.Approve;
                                logModel.OperateType = "Submit Review To AA";
                                break;
                            case "5":
                                dataModel.ReportStatus = (int)ReportStatusEnum.TeamLeader;
                                logModel.OperateType = "Reject";
                                break;

                            default:
                                break;
                        }

                        logModel.ReportGuid = dataModel;
                        logModel.Status = ((ReportStatusEnum)dataModel.ReportStatus).ToString();
                        logModel.CreateUser = UserView.UserLoginName;
                        logModel.CreateUserName = UserView.UserName;
                        logModel.Comments = model.Comment;
                        logModel.CreateDateTime = DateTime.Now;

                        dataModel.WorkFlowLog_Models.Add(logModel);

                        success = new DAL.D_Report_DAL().SaveWorkFlowLog(dataModel, logModel);
                    }


                }

                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }

        }

        [LoginFilter()]
        public ActionResult CancelApprove(Guid? Id)
        {
            Models.Report_WorkFlow_Log_Model logModel = new Models.Report_WorkFlow_Log_Model();
            if (Id.HasValue)
            {
                Model.D_Report_Model dataModel = NhSession.Get<Model.D_Report_Model>(Id.Value);
                logModel.ReportGuid = dataModel.ReportGuid;
                logModel.ReportStatus = dataModel.ReportStatus;
            }
            return View(logModel);
        }

        [HttpPost]
        [LoginFilter()]
        public ActionResult CancelApprove(Report_WorkFlow_Log_Model model)
        {
            bool success = false;

            try
            {
                string type = Request.Form["type"] == null ? string.Empty : Request.Form["type"].ToString();
                Model.D_Report_Model dataModel = NhSession.Get<Model.D_Report_Model>(model.ReportGuid);
                if (dataModel != null)
                {
                    if (!string.IsNullOrEmpty(type))
                    {
                        Model.D_WorkFlowLog_Model logModel = new D_WorkFlowLog_Model();
                        switch (type)
                        {
                            case "1":
                                if (dataModel.ReportCancelStatus == (int)ReportCancelStatusEnum.TeamLeader ||
                                    dataModel.ReportCancelStatus == 0)
                                    dataModel.ReportCancelStatus = (int)ReportCancelStatusEnum.QE;
                                else if (dataModel.ReportCancelStatus == (int)ReportCancelStatusEnum.QE)
                                    dataModel.ReportCancelStatus = (int)ReportCancelStatusEnum.Sponsor;
                                else if (dataModel.ReportCancelStatus == (int)ReportStatusEnum.Sponsor)
                                    dataModel.ReportCancelStatus = (int)ReportStatusEnum.Completed;

                                logModel.OperateType = "Approve";
                                break;
                            case "2":
                                dataModel.ReportCancelStatus = (int)ReportStatusEnum.TeamLeader;
                                logModel.OperateType = "Review";
                                break;
                            case "3":
                                dataModel.ReportCancelStatus = (int)ReportStatusEnum.Approve;
                                logModel.OperateType = "Submit Review To QE";
                                break;
                            case "4":
                                dataModel.ReportCancelStatus = (int)ReportStatusEnum.Sponsor;
                                logModel.OperateType = "Submit Review To Sponsor";
                                break;
                            case "5":
                                dataModel.ReportCancelStatus = (int)ReportStatusEnum.TeamLeader;
                                logModel.OperateType = "Reject";
                                break;

                            default:
                                break;
                        }

                        logModel.ReportGuid = dataModel;
                        logModel.Status = ((ReportCancelStatusEnum)dataModel.ReportCancelStatus).ToString();
                        logModel.CreateUser = UserView.UserLoginName;
                        logModel.CreateUserName = UserView.UserName;
                        logModel.Comments = model.Comment;
                        logModel.CreateDateTime = DateTime.Now;

                        success = new DAL.D_Report_DAL().SaveWorkFlowLog(dataModel, logModel);
                    }


                }

                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }

        }
    }
}
