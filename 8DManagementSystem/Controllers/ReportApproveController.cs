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

                ReportAssignModel model = new ReportAssignModel();
                model.LoadBoardSelectList(UserView.Boards);
                model.LoadReportTypeSelectList();
                model.ReportHeader = new ReportHeaderModel();
                model.ReportD1 = new ReoprtD1Model();
                model.ReportD2 = new ReoprtD2Model();
                model.ReportD3 = new ReoprtD3Model();
                model.ReportD4 = new ReoprtD4Model();
                model.ReportD5 = new ReoprtD5Model();
                model.ReportD6 = new ReoprtD6Model();
                model.ReportD7 = new ReoprtD7Model();
                model.WorkFlow = new ReportWorkFlow();

                if (dataModel != null)
                {
                    #region MyRegion
                    #region 表头
                    model.ReportAssignGuid = dataModel.ReportGuid;
                    model.ReportTitle = dataModel.ReportTitle;
                    model.ReportType = dataModel.ReportTypeGuid.DicGuid.ToString();

                    model.BoardGuid = dataModel.ReportBoardGuid.BoardGuid.ToString();
                    model.ReportOwner = dataModel.ReportOwner;
                    model.ResponsibleQE = dataModel.ResponsibleQE;
                    model.ReportNo = dataModel.ReportNo;


                    model.ReportHeader.Bosch_Material_No = dataModel.Bosch_Material_No;
                    model.ReportHeader.Complaint_TypeMode = dataModel.Complaint_TypeMode;
                    model.ReportHeader.ComplaintDate = dataModel.ComplaintDate.HasValue ? dataModel.ComplaintDate.Value.ToString("dd/MM/yyyy") : "";
                    model.ReportHeader.Customer = dataModel.Customer;
                    model.ReportHeader.Customer_Material_No = dataModel.Customer_Material_No;
                    model.ReportHeader.Issuer = dataModel.Issuer;
                    model.ReportHeader.Manufacturing_Plant = dataModel.Manufacturing_Plant;
                    model.ReportHeader.Product = dataModel.Product;
                    model.ReportHeader.Serial_No = dataModel.Serial_No;
                    model.ReportHeader.Supplier_Name = dataModel.Supplier_Name;
                    model.ReportHeader.Supplier_No = dataModel.Supplier_No;
                    model.ReportHeader.Warranty_Descision = dataModel.Warranty_Descision;
                    #endregion

                    #region ReportD1
                    model.ReportD1.Team_Leader = dataModel.Team_Leader;
                    model.ReportD1.Sponaor = dataModel.Sponaor;
                    model.ReportD1.Coordinator = dataModel.Coordinator;
                    model.ReportD1.Team_Member = dataModel.Team_Member;
                    #endregion

                    #region ReportD2
                    if (dataModel.ReportD2 != null)
                    {
                        model.ReportD2.Manufacturing_Date = dataModel.ReportD2.Manufacturing_Date.HasValue ? dataModel.ReportD2.Manufacturing_Date.Value.ToString("dd/MM/yyyy") : "";

                        model.ReportD2.Bosch_Description = dataModel.ReportD2.Bosch_Description;
                        model.ReportD2.Customer_Complaint = dataModel.ReportD2.Customer_Complaint;
                        model.ReportD2.No_of_complaint_parts = dataModel.ReportD2.No_of_complaint_parts;

                        model.ReportD2.End_of_D2_Date = dataModel.ReportD2.End_of_D2_Date.HasValue ? dataModel.ReportD2.End_of_D2_Date.Value.ToString("dd/MM/yyyy") : "";

                    }
                    #endregion

                    #region ReportD3
                    //model.ReportD3Json = dataModel.ReportD3Json;
                    if (!string.IsNullOrEmpty(dataModel.ReportD3Json))
                    {
                        Models.ReoprtD3Model d3Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD3Model>(dataModel.ReportD3Json);
                        model.ReportD3 = d3Model;
                    }
                    #endregion

                    #region ReportD4
                    if (!string.IsNullOrEmpty(dataModel.ReportD4Json))
                    {
                        Models.ReoprtD4Model d4Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD4Model>(dataModel.ReportD4Json);
                        model.ReportD4 = d4Model;
                    }
                    #endregion

                    #region ReportD5
                    if (!string.IsNullOrEmpty(dataModel.ReportD5Json))
                    {
                        Models.ReoprtD5Model d5Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD5Model>(dataModel.ReportD5Json);
                        model.ReportD5 = d5Model;
                    }
                    #endregion

                    #region ReportD6
                    if (!string.IsNullOrEmpty(dataModel.ReportD6Json))
                    {
                        Models.ReoprtD6Model d6Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD6Model>(dataModel.ReportD6Json);
                        model.ReportD6 = d6Model;
                    }
                    #endregion

                    #region ReportD7
                    if (!string.IsNullOrEmpty(dataModel.ReportD7Json))
                    {
                        Models.ReoprtD7Model d7Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD7Model>(dataModel.ReportD7Json);
                        model.ReportD7 = d7Model;
                    }
                    #endregion

                    #region ReportD8
                    if (!string.IsNullOrEmpty(dataModel.ReportD8Json))
                    {
                        Models.ReoprtD8Model d8Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD8Model>(dataModel.ReportD8Json);
                        model.ReportD8 = d8Model;
                    }

                    foreach (var item in dataModel.ReportD8DataModels)
                    {
                        Models.ReoprtD8Data sign = new ReoprtD8Data();
                        sign.Number = item.Number;
                        sign.ReportD8Guid = item.ReportD8Guid.ToString();
                        sign.Signature = item.Signature;
                        sign.SignatureDate = item.SignatureDate.HasValue ? item.SignatureDate.Value.ToString("dd/MM/yyyy") : "";
                        sign.SponsorName = item.SponsorName;
                        model.ReportD8Sign.Add(sign);
                    }
                    #endregion
                    #endregion

                    if (dataModel.WorkFlow_Models.Count > 0)
                    {
                        model.WorkFlow.EightD_WorkFlowGuid = dataModel.WorkFlow_Models[0].EightD_WorkFlowGuid;
                        model.WorkFlow.Additional_Approver = dataModel.WorkFlow_Models[0].Additional_Approver;
                        model.WorkFlow.Comments = dataModel.WorkFlow_Models[0].Comments;
                        model.WorkFlow.Sponsor = dataModel.WorkFlow_Models[0].Sponsor;
                        model.WorkFlow.Team_Leader = dataModel.WorkFlow_Models[0].Team_Leader;
                    }
                }

                logModel.ReportModel = model;
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

                ReportAssignModel model = new ReportAssignModel();
                model.LoadBoardSelectList(UserView.Boards);
                model.LoadReportTypeSelectList();
                model.ReportHeader = new ReportHeaderModel();
                model.ReportD1 = new ReoprtD1Model();
                model.ReportD2 = new ReoprtD2Model();
                model.ReportD3 = new ReoprtD3Model();
                model.ReportD4 = new ReoprtD4Model();
                model.ReportD5 = new ReoprtD5Model();
                model.ReportD6 = new ReoprtD6Model();
                model.ReportD7 = new ReoprtD7Model();
                model.WorkFlow = new ReportWorkFlow();

                if (dataModel != null)
                {
                    #region MyRegion
                    #region 表头
                    model.ReportAssignGuid = dataModel.ReportGuid;
                    model.ReportTitle = dataModel.ReportTitle;
                    model.ReportType = dataModel.ReportTypeGuid.DicGuid.ToString();

                    model.BoardGuid = dataModel.ReportBoardGuid.BoardGuid.ToString();
                    model.ReportOwner = dataModel.ReportOwner;
                    model.ResponsibleQE = dataModel.ResponsibleQE;
                    model.ReportNo = dataModel.ReportNo;


                    model.ReportHeader.Bosch_Material_No = dataModel.Bosch_Material_No;
                    model.ReportHeader.Complaint_TypeMode = dataModel.Complaint_TypeMode;
                    model.ReportHeader.ComplaintDate = dataModel.ComplaintDate.HasValue ? dataModel.ComplaintDate.Value.ToString("dd/MM/yyyy") : "";
                    model.ReportHeader.Customer = dataModel.Customer;
                    model.ReportHeader.Customer_Material_No = dataModel.Customer_Material_No;
                    model.ReportHeader.Issuer = dataModel.Issuer;
                    model.ReportHeader.Manufacturing_Plant = dataModel.Manufacturing_Plant;
                    model.ReportHeader.Product = dataModel.Product;
                    model.ReportHeader.Serial_No = dataModel.Serial_No;
                    model.ReportHeader.Supplier_Name = dataModel.Supplier_Name;
                    model.ReportHeader.Supplier_No = dataModel.Supplier_No;
                    model.ReportHeader.Warranty_Descision = dataModel.Warranty_Descision;
                    #endregion

                    #region ReportD1
                    model.ReportD1.Team_Leader = dataModel.Team_Leader;
                    model.ReportD1.Sponaor = dataModel.Sponaor;
                    model.ReportD1.Coordinator = dataModel.Coordinator;
                    model.ReportD1.Team_Member = dataModel.Team_Member;
                    #endregion

                    #region ReportD2
                    if (dataModel.ReportD2 != null)
                    {
                        model.ReportD2.Manufacturing_Date = dataModel.ReportD2.Manufacturing_Date.HasValue ? dataModel.ReportD2.Manufacturing_Date.Value.ToString("dd/MM/yyyy") : "";

                        model.ReportD2.Bosch_Description = dataModel.ReportD2.Bosch_Description;
                        model.ReportD2.Customer_Complaint = dataModel.ReportD2.Customer_Complaint;
                        model.ReportD2.No_of_complaint_parts = dataModel.ReportD2.No_of_complaint_parts;

                        model.ReportD2.End_of_D2_Date = dataModel.ReportD2.End_of_D2_Date.HasValue ? dataModel.ReportD2.End_of_D2_Date.Value.ToString("dd/MM/yyyy") : "";

                    }
                    #endregion

                    #region ReportD3
                    //model.ReportD3Json = dataModel.ReportD3Json;
                    if (!string.IsNullOrEmpty(dataModel.ReportD3Json))
                    {
                        Models.ReoprtD3Model d3Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD3Model>(dataModel.ReportD3Json);
                        model.ReportD3 = d3Model;
                    }
                    #endregion

                    #region ReportD4
                    if (!string.IsNullOrEmpty(dataModel.ReportD4Json))
                    {
                        Models.ReoprtD4Model d4Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD4Model>(dataModel.ReportD4Json);
                        model.ReportD4 = d4Model;
                    }
                    #endregion

                    #region ReportD5
                    if (!string.IsNullOrEmpty(dataModel.ReportD5Json))
                    {
                        Models.ReoprtD5Model d5Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD5Model>(dataModel.ReportD5Json);
                        model.ReportD5 = d5Model;
                    }
                    #endregion

                    #region ReportD6
                    if (!string.IsNullOrEmpty(dataModel.ReportD6Json))
                    {
                        Models.ReoprtD6Model d6Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD6Model>(dataModel.ReportD6Json);
                        model.ReportD6 = d6Model;
                    }
                    #endregion

                    #region ReportD7
                    if (!string.IsNullOrEmpty(dataModel.ReportD7Json))
                    {
                        Models.ReoprtD7Model d7Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD7Model>(dataModel.ReportD7Json);
                        model.ReportD7 = d7Model;
                    }
                    #endregion

                    #region ReportD8
                    if (!string.IsNullOrEmpty(dataModel.ReportD8Json))
                    {
                        Models.ReoprtD8Model d8Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ReoprtD8Model>(dataModel.ReportD8Json);
                        model.ReportD8 = d8Model;
                    }

                    foreach (var item in dataModel.ReportD8DataModels)
                    {
                        Models.ReoprtD8Data sign = new ReoprtD8Data();
                        sign.Number = item.Number;
                        sign.ReportD8Guid = item.ReportD8Guid.ToString();
                        sign.Signature = item.Signature;
                        sign.SignatureDate = item.SignatureDate.HasValue ? item.SignatureDate.Value.ToString("dd/MM/yyyy") : "";
                        sign.SponsorName = item.SponsorName;
                        model.ReportD8Sign.Add(sign);
                    }
                    #endregion
                    #endregion

                    if (dataModel.WorkFlow_Models.Count > 0)
                    {
                        model.WorkFlow.EightD_WorkFlowGuid = dataModel.WorkFlow_Models[0].EightD_WorkFlowGuid;
                        model.WorkFlow.Additional_Approver = dataModel.WorkFlow_Models[0].Additional_Approver;
                        model.WorkFlow.Comments = dataModel.WorkFlow_Models[0].Comments;
                        model.WorkFlow.Sponsor = dataModel.WorkFlow_Models[0].Sponsor;
                        model.WorkFlow.Team_Leader = dataModel.WorkFlow_Models[0].Team_Leader;
                    }
                }

                logModel.ReportModel = model;
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
