using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Filter;
using _8DManagementSystem.Models;
using _8DManagementSystem.Common;
using Newtonsoft.Json.Linq;
using System.Collections;
using _8DManagementSystem.Model;

namespace _8DManagementSystem.Controllers
{
    public class EightDReportController : BaseController
    {
        //
        // GET: /EightDReport/
        [LoginFilter()]
        public ActionResult Index()
        {
            Models.ReportListQueryModel model = new ReportListQueryModel();
            string boardId = Request.QueryString.Count == 0 ? string.Empty : Request.QueryString[0].ToString();
            if (!string.IsNullOrEmpty(boardId))
            {
                if (UserView.Boards.Exists(m => m.BoardGuid.ToString() == boardId))
                {
                    model.BoardGuids += boardId;
                    model.BoardName = UserView.Boards.Find(m => m.BoardGuid.ToString() == boardId).BoardName;
                }
                else
                    model.BoardGuids = string.Empty;
            }

            return View(model);
        }

        [LoginFilter()]
        public ActionResult ReportList()
        {
            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            int startCount = 0;

            string reportNo = string.Empty;
            string dicType = string.Empty;
            string dateCreated = string.Empty;
            string boardGuids = string.Empty;

            JArray jsonarray = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(jsondata);

            for (int i = 0; i < jsonarray.Count; i++) //从传递参数里面选出待用的参数  
            {
                JObject jobj = (JObject)jsonarray[i];
                if (jobj.Property("name").Value.ToString().Equals("sEcho"))
                    sEcho = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("iDisplayStart"))
                    startCount = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("iDisplayLength"))
                    rowCount = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("ReportNo"))
                    reportNo = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("BoardGuids"))
                    boardGuids = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DateCreated"))
                    dateCreated = jobj.Property("value").Value.EToString();

            }
            #endregion

            List<Model.D_Board_Model> boardModels = new List<Model.D_Board_Model>();
            if (!string.IsNullOrEmpty(boardGuids))
            {
                string[] boards = boardGuids.Split(',');
                foreach (var item in boards)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Model.D_Board_Model board = new DAL.D_Board_DAL().GetModel(new Guid(item));
                        boardModels.Add(board);
                    }

                }
            }
            else
            {
                foreach (var item in UserView.Boards)
                {
                    Model.D_Board_Model board = new DAL.D_Board_DAL().GetModel(item.BoardGuid);
                    boardModels.Add(board);
                }

            }
            //总条数
            int totalCount = 0;

            IList<Model.D_Report_Model> list = new DAL.D_Report_DAL().GetAllByPage(startCount, rowCount, boardModels, reportNo, dateCreated, out totalCount);

            int totalPage = totalCount % rowCount == 0 ? totalCount / rowCount : totalCount / rowCount + 1;

            #region 查询的结果集赋值
            ArrayList rows = new ArrayList();
            foreach (var item in list)
            {
                string detail = "<li><a href=\"" + Url.Content("~/EightDReport/ReportEdit") + "/" + item.ReportGuid + "\"><span class=\"glyphicon glyphicon-edit\"></span> Show Detail</a></li>";
                string approve = string.Empty;
                string cancelApprove = string.Empty;
                string remove = "<li><a href=\"#\" id=\"" + item.ReportGuid + "\" onclick=\"del(this)\"><span class=\"glyphicon glyphicon-remove-circle\"></span> Remove</a></li>";
                if (item.ReportStatus != (int)Model.ReportStatusEnum.Completed &&
                    item.ReportStatus != 0)
                {
                    approve = "<li><a href=\"" + Url.Content("~/ReportApprove/Approve") + "/" + item.ReportGuid + "\"><span class=\"glyphicon glyphicon-cog\"></span> Approve</a></li>";
                    detail = string.Empty;
                    remove = string.Empty;
                }


                if (item.ReportCancelStatus != (int)Model.ReportCancelStatusEnum.Completed &&
                    item.ReportCancelStatus != 0)
                {
                    cancelApprove = "<li><a href=\"" + Url.Content("~/ReportApprove/CancelApprove") + "/" + item.ReportGuid + "\"><span class=\"glyphicon glyphicon-export\"></span> Cancel Approve</a></li>";
                    detail = string.Empty;
                    remove = string.Empty;
                }

                string owner = string.Empty;
                string qe = string.Empty;
                if (!string.IsNullOrEmpty(item.ReportOwner))
                    owner = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.D_User>(item.ReportOwner).UserLoginName;

                if (!string.IsNullOrEmpty(item.ResponsibleQE))
                    qe = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.D_User>(item.ResponsibleQE).UserLoginName;

                rows.Add(new
                {
                    Operation = string.Format("<div class=\"btn-group\">" +
                            "<button type=\"button\" class=\"btn btn-sm btn-primary dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">" +
                                "Action <span class=\"caret\"></span></button>" +
                            "<ul class=\"dropdown-menu\" role=\"menu\">" +
                            detail + approve + cancelApprove +
                                "<li class=\"divider\"></li>" +
                                 remove +
                            "</ul></div>"),
                    ID = item.ReportGuid,
                    ReportTitle = item.ReportTitle,
                    BoardName = item.ReportBoardGuid.BoardName,
                    ReportNo = item.ReportNo,
                    ReportTypeName = item.ReportTypeGuid.DicName,
                    ReportOwner = owner,
                    ResponsibleQE = qe,
                    Status = Common.EnumUtility.GetEnumDescription((Model.ReportStatusEnum)item.ReportStatus),
                    CancelStatus = Common.EnumUtility.GetEnumDescription((Model.ReportCancelStatusEnum)item.ReportCancelStatus),
                    //ModifyUserName = item.ModifyUserName,
                    //ModifyDate = item.ModifyDateTime.HasValue ? item.ModifyDateTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                    CreateUserName = item.CreateUserName,
                    CreateDate = item.CreateDateTime.HasValue ? item.CreateDateTime.Value.ToString("yyyy-MM-dd") : string.Empty
                });
            }
            #endregion

            return Json(new { sEcho = sEcho, iTotalRecords = totalCount, iTotalDisplayRecords = totalCount, aaData = rows }, JsonRequestBehavior.AllowGet);



        }

        #region 新建
        [LoginFilter()]
        public ActionResult Assign8DReport(Guid? Id)
        {
            ReportAssignModel model = new ReportAssignModel();
            model.LoadBoardSelectList(UserView.Boards);
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

                    dataModel.CreateDateTime = DateTime.Now;
                    dataModel.DataStatus = false;
                    dataModel.CreateUserGuid = UserView.UserGuid;
                    dataModel.CreateUserName = UserView.UserName;
                    dataModel.ReportNo = "AE" + boardModel.BoardName + "-" + DateTime.Now.ToString("yyyyMMddHHssmmfff");
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);
                }

                //dataModel.BoardName = model.BoardName;
                dataModel.ModifyDateTime = DateTime.Now;
                dataModel.ModifyUserGuid = UserView.UserGuid;
                dataModel.ModifyUserName = UserView.UserName;

                dataModel.ReportTitle = model.ReportTitle;
                dataModel.ReportTypeGuid = new DAL.D_Dictionary_DAL().GetModel(new Guid(model.ReportType));
                dataModel.ReportBoardGuid = boardModel;
                if (string.IsNullOrEmpty(model.ReportOwner))
                {
                    return Json(new { success = success, message = "Report Owner不能为空！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Model.D_User_Model userModel = new DAL.D_User_DAL().GetUserByUserLoginName(model.ReportOwner.TrimEnd(';'));
                    if (userModel == null) return Json(new { success = success, message = "用户不存在！" }, JsonRequestBehavior.AllowGet);

                    D_User user = new D_User();
                    user.Serial = 0;
                    user.UserGuid = userModel.UserGuid;
                    user.UserLoginName = userModel.UserLoginName;
                    user.UserName = userModel.UserName;
                    dataModel.ReportOwner = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                    //dataModel.ReportOwner = model.ReportOwner;
                }

                if (string.IsNullOrEmpty(model.ResponsibleQE))
                {
                    return Json(new { success = success, message = "Responsible QE不能为空！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Model.D_User_Model userModel = new DAL.D_User_DAL().GetUserByUserLoginName(model.ResponsibleQE.TrimEnd(';'));
                    if (userModel == null) return Json(new { success = success, message = "用户不存在！" }, JsonRequestBehavior.AllowGet);

                    D_User user = new D_User();
                    user.Serial = 0;
                    user.UserGuid = userModel.UserGuid;
                    user.UserLoginName = userModel.UserLoginName;
                    user.UserName = userModel.UserName;
                    dataModel.ResponsibleQE = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                    //dataModel.ResponsibleQE = model.ResponsibleQE;
                }

                success = new DAL.D_Report_DAL().Save(dataModel);
                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        #endregion

        #region 编辑
        [LoginFilter()]
        public ActionResult ReportEdit(Guid? Id)
        {

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

            if (Id.HasValue)
            {
                Model.D_Report_Model dataModel = NhSession.Get<Model.D_Report_Model>(Id.Value);
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
                    if (!string.IsNullOrEmpty(dataModel.Team_Leader))
                        model.ReportD1.Team_Leader = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.D_User>(dataModel.Team_Leader).UserLoginName;

                    if (!string.IsNullOrEmpty(dataModel.Sponaor))
                        model.ReportD1.Sponaor = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.D_User>(dataModel.Sponaor).UserLoginName;


                    model.ReportD1.Coordinator = dataModel.Coordinator;
                    if (!string.IsNullOrEmpty(dataModel.Team_Member))
                    {
                        List<Models.D_User> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.D_User>>(dataModel.Team_Member);
                        foreach (var item in users)
                        {
                            model.ReportD1.Team_Member += item.UserLoginName + ";";
                        }
                        model.ReportD1.Team_Member = model.ReportD1.Team_Member.TrimEnd(';');
                    }

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
                        if (!string.IsNullOrEmpty(dataModel.WorkFlow_Models[0].Additional_Approver))
                        {
                            List<Models.D_User> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.D_User>>(dataModel.WorkFlow_Models[0].Additional_Approver);
                            foreach (var item in users)
                            {
                                model.WorkFlow.Additional_Approver += item.UserLoginName + ";";
                            }
                            model.WorkFlow.Additional_Approver = model.WorkFlow.Additional_Approver.TrimEnd(';');

                        }

                        model.WorkFlow.Comments = dataModel.WorkFlow_Models[0].Comments;
                        model.WorkFlow.Sponsor = dataModel.WorkFlow_Models[0].Sponsor;
                        model.WorkFlow.Team_Leader = dataModel.WorkFlow_Models[0].Team_Leader;
                    }

                    model.ReportStatus = dataModel.ReportStatus;
                    model.ReportCancelStatus = dataModel.ReportCancelStatus;
                }

            }

            return View(model);
        }

        public ActionResult ReportHeaderSave(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        dataModel.Bosch_Material_No = model.ReportHeader.Bosch_Material_No;

                        dataModel.Complaint_TypeMode = model.ReportHeader.Complaint_TypeMode;
                        if (!string.IsNullOrEmpty(model.ReportHeader.ComplaintDate))
                            dataModel.ComplaintDate = Convert.ToDateTime(model.ReportHeader.ComplaintDate);
                        else
                            dataModel.ComplaintDate = null;

                        dataModel.Customer = model.ReportHeader.Customer;
                        dataModel.Customer_Material_No = model.ReportHeader.Customer_Material_No;
                        dataModel.Issuer = model.ReportHeader.Issuer;
                        dataModel.Manufacturing_Plant = model.ReportHeader.Manufacturing_Plant;
                        dataModel.Product = model.ReportHeader.Product;
                        dataModel.Serial_No = model.ReportHeader.Serial_No;
                        dataModel.Supplier_Name = model.ReportHeader.Supplier_Name;
                        dataModel.Supplier_No = model.ReportHeader.Supplier_No;
                        dataModel.Warranty_Descision = model.ReportHeader.Warranty_Descision;

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }


                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD1Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {

                        if (!string.IsNullOrEmpty(model.ReportD1.Team_Leader))
                        {
                            string[] leaderLoginName = model.ReportD1.Team_Leader.TrimEnd(';').Split(';');
                            if (leaderLoginName.Count() > 1)
                            {
                                return Json(new { success = success, message = "Team_Leader 只能是一个人！" });
                            }
                            Model.D_User_Model leader = new DAL.D_User_DAL().GetUserByUserLoginName(model.ReportD1.Team_Leader.TrimEnd(';'));

                            D_User user = new D_User();
                            user.Serial = 0;
                            user.UserGuid = leader.UserGuid;
                            user.UserLoginName = leader.UserLoginName;
                            user.UserName = leader.UserName;
                            dataModel.Team_Leader = Newtonsoft.Json.JsonConvert.SerializeObject(user);

                        }
                        else
                        {
                            dataModel.Team_Leader = model.ReportD1.Team_Leader;
                        }

                        if (!string.IsNullOrEmpty(model.ReportD1.Sponaor))
                        {
                            string[] loginName = model.ReportD1.Sponaor.TrimEnd(';').Split(';');
                            if (loginName.Count() > 1)
                            {
                                return Json(new { success = success, message = "Sponaor 只能是一个人！" });
                            }
                            Model.D_User_Model sponaor = new DAL.D_User_DAL().GetUserByUserLoginName(model.ReportD1.Sponaor.TrimEnd(';'));

                            D_User user = new D_User();
                            user.Serial = 0;
                            user.UserGuid = sponaor.UserGuid;
                            user.UserLoginName = sponaor.UserLoginName;
                            user.UserName = sponaor.UserName;
                            dataModel.Sponaor = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                        }
                        else
                        {
                            dataModel.Sponaor = model.ReportD1.Sponaor;
                        }

                        dataModel.Coordinator = model.ReportD1.Coordinator;

                        if (!string.IsNullOrEmpty(model.ReportD1.Team_Member))
                        {
                            string[] loginName = model.ReportD1.Team_Member.TrimEnd(';').Split(';');

                            List<D_User> userList = new List<D_User>();
                            for (int i = 0; i < loginName.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(loginName[i]))
                                {
                                    Model.D_User_Model member = new DAL.D_User_DAL().GetUserByUserLoginName(loginName[i]);
                                    D_User user = new D_User();
                                    user.Serial = i;
                                    user.UserGuid = member.UserGuid;
                                    user.UserLoginName = member.UserLoginName;
                                    user.UserName = member.UserName;
                                    userList.Add(user);
                                }
                            }

                            dataModel.Team_Member = Newtonsoft.Json.JsonConvert.SerializeObject(userList);

                        }
                        else
                        {
                            dataModel.Team_Member = model.ReportD1.Team_Member;
                        }


                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }


                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD2Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        if (dataModel.ReportD2 == null)
                        {
                            dataModel.ReportD2 = new Model.D_ReportD2_Model();

                        }
                        dataModel.ReportD2.Bosch_Description = model.ReportD2.Bosch_Description;
                        dataModel.ReportD2.Customer_Complaint = model.ReportD2.Customer_Complaint;
                        if (string.IsNullOrEmpty(model.ReportD2.Manufacturing_Date))
                            dataModel.ReportD2.Manufacturing_Date = null;
                        else
                            dataModel.ReportD2.Manufacturing_Date = Convert.ToDateTime(model.ReportD2.Manufacturing_Date);
                        dataModel.ReportD2.No_of_complaint_parts = model.ReportD2.No_of_complaint_parts;
                        if (string.IsNullOrEmpty(model.ReportD2.End_of_D2_Date))
                            dataModel.ReportD2.End_of_D2_Date = null;
                        else
                            dataModel.ReportD2.End_of_D2_Date = Convert.ToDateTime(model.ReportD2.End_of_D2_Date);


                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }


                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD3Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        string responsible = Request.Form["responsible"].ToString();
                        string introduced = Request.Form["introduced"].ToString();
                        string effectinve = Request.Form["effectinve"].ToString();
                        string remark = Request.Form["remark"].ToString();
                        string[] Responsible = responsible.Split(',');
                        string[] Introduced = introduced.Split(',');
                        string[] Effectinve = effectinve.Split(',');
                        string[] Remark = remark.Split(',');
                        Models.ReoprtD3Model resportD3Model = new ReoprtD3Model();
                        resportD3Model.ResponsibleMain = model.ReportD3.ResponsibleMain;
                        resportD3Model.MeasureDate = model.ReportD3.MeasureDate;

                        for (int i = 0; i < Responsible.Length; i++)
                        {
                            Models.ReoprtD3Data data = new ReoprtD3Data();
                            data.Number = i + 1;
                            data.Responsible = Responsible[i];
                            data.Introduced = Introduced[i];
                            data.Effectinve = Effectinve[i];
                            data.Remark = Remark[i];
                            resportD3Model.D3DataList.Add(data);
                        }

                        dataModel.ReportD3Json = Newtonsoft.Json.JsonConvert.SerializeObject(resportD3Model);

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD4Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        Models.ReoprtD4Model resportD4Model = new ReoprtD4Model();
                        resportD4Model.Title = model.ReportD4.Title;
                        resportD4Model.Causing_Process = model.ReportD4.Causing_Process;
                        resportD4Model.Completed_On = model.ReportD4.Completed_On;
                        resportD4Model.Expected_Number = model.ReportD4.Expected_Number;
                        resportD4Model.Field = model.ReportD4.Field;
                        resportD4Model.Internal_Km = model.ReportD4.Internal_Km;
                        resportD4Model.Period_Affected_FromEnd = model.ReportD4.Period_Affected_FromEnd;
                        resportD4Model.Period_Affected_FromStart = model.ReportD4.Period_Affected_FromStart;
                        resportD4Model.Qty = model.ReportD4.Qty;
                        resportD4Model.Responsible = model.ReportD4.Responsible;
                        resportD4Model.Risk_Assessment = model.ReportD4.Risk_Assessment;

                        #region D1
                        string non_conformity_occur = Request.Form["non_conformity_occur"].ToString();
                        string responsible = Request.Form["responsible"].ToString();
                        string completed = Request.Form["completed"].ToString();
                        string[] Responsible = responsible.Split(',');
                        string[] Occur = non_conformity_occur.Split(',');
                        string[] Completed = completed.Split(',');

                        for (int i = 0; i < Responsible.Length; i++)
                        {
                            Models.ReoprtD4Data1 data1 = new ReoprtD4Data1();
                            data1.Number = i + 1;
                            data1.Responsible = Responsible[i];
                            data1.Non_Conformity_Occur = Occur[i];
                            data1.Completed_On = Completed[i];
                            resportD4Model.D4Data1List.Add(data1);
                        }
                        #endregion

                        #region D2
                        string non_conformity_detected = Request.Form["non_conformity_detected"].ToString();
                        string nonresponsible = Request.Form["nonresponsible"].ToString();
                        string noncompleted = Request.Form["noncompleted"].ToString();
                        string[] Nonresponsible = nonresponsible.Split(',');
                        string[] NonDetected = non_conformity_detected.Split(',');
                        string[] Noncompleted = noncompleted.Split(',');

                        for (int i = 0; i < Nonresponsible.Length; i++)
                        {
                            Models.ReoprtD4Data2 data2 = new ReoprtD4Data2();
                            data2.Number = i + 1;
                            data2.Responsible = Nonresponsible[i];
                            data2.Non_Conformity_Detected = NonDetected[i];
                            data2.Completed_On = Noncompleted[i];
                            resportD4Model.D4Data2List.Add(data2);
                        }
                        #endregion

                        dataModel.ReportD4Json = Newtonsoft.Json.JsonConvert.SerializeObject(resportD4Model);


                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD5Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        string responsible = Request.Form["Responsible5"].ToString();
                        string corrective_Effectiveness5 = Request.Form["Corrective_Effectiveness5"].ToString();
                        string completed_On5 = Request.Form["Completed_On5"].ToString();
                        string[] Responsible = responsible.Split(',');
                        string[] Corrective_Effectiveness5 = corrective_Effectiveness5.Split(',');
                        string[] Completed_On5 = completed_On5.Split(',');
                        Models.ReoprtD5Model resportD5Model = new ReoprtD5Model();

                        for (int i = 0; i < Responsible.Length; i++)
                        {
                            Models.ReoprtD5Data data = new ReoprtD5Data();
                            data.Number = i + 1;
                            data.Responsible = Responsible[i];
                            data.Corrective_Effectiveness = Corrective_Effectiveness5[i];
                            data.Completed_On = Completed_On5[i];
                            resportD5Model.D5DataList.Add(data);
                        }

                        dataModel.ReportD5Json = Newtonsoft.Json.JsonConvert.SerializeObject(resportD5Model);

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD6Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        Models.ReoprtD6Model resportD6Model = new ReoprtD6Model();
                        resportD6Model.CustomerAgreementAt = model.ReportD6.CustomerAgreementAt;
                        resportD6Model.CustomerAgreementBy = model.ReportD6.CustomerAgreementBy;

                        #region D1
                        string descriptionOfAction1 = Request.Form["DescriptionOfAction1_6"].ToString();
                        string responsible1 = Request.Form["Responsible1_6"].ToString();
                        string planned1 = Request.Form["Planned1_6"].ToString();
                        string introduce_On1 = Request.Form["Introduce_On1_6"].ToString();
                        string effective_From1 = Request.Form["Effective_From1_6"].ToString();
                        string[] DescriptionOfAction1 = descriptionOfAction1.Split(',');
                        string[] Responsible1 = responsible1.Split(',');
                        string[] Planned1 = planned1.Split(',');
                        string[] Introduce_On1 = introduce_On1.Split(',');
                        string[] Effective_From1 = effective_From1.Split(',');


                        for (int i = 0; i < DescriptionOfAction1.Length; i++)
                        {
                            Models.ReoprtD6Data1 data1 = new ReoprtD6Data1();
                            data1.Number = i + 1;
                            data1.DescriptionOfAction = DescriptionOfAction1[i];
                            data1.Effective_From = Effective_From1[i];
                            data1.Responsible = Responsible1[i];
                            data1.Planned = Planned1[i];
                            data1.Introduce_On = Introduce_On1[i];
                            resportD6Model.D6Data1List.Add(data1);
                        }
                        #endregion

                        #region D2
                        string validationOfAction2 = Request.Form["ValidationOfAction2_6"].ToString();
                        string responsible2 = Request.Form["Responsible2_6"].ToString();
                        string completed_On2 = Request.Form["Completed_On2_6"].ToString();
                        string[] ValidationOfAction2 = validationOfAction2.Split(',');
                        string[] Responsible2 = responsible2.Split(',');
                        string[] Completed_On2 = completed_On2.Split(',');

                        for (int i = 0; i < ValidationOfAction2.Length; i++)
                        {
                            Models.ReoprtD6Data2 data2 = new ReoprtD6Data2();
                            data2.Number = i + 1;
                            data2.ValidationOfAction = ValidationOfAction2[i];
                            data2.Responsible = Responsible2[i];
                            data2.Completed_On = Completed_On2[i];
                            resportD6Model.D6Data2List.Add(data2);
                        }
                        #endregion

                        #region D3
                        string removalOfAction3 = Request.Form["RemovalOfAction3_6"].ToString();
                        string responsible3 = Request.Form["Responsible3_6"].ToString();
                        string removed_At3 = Request.Form["Removed_At3_6"].ToString();
                        string[] RemovalOfAction3 = removalOfAction3.Split(',');
                        string[] Responsible3 = responsible3.Split(',');
                        string[] Removed_At3 = removed_At3.Split(',');

                        for (int i = 0; i < RemovalOfAction3.Length; i++)
                        {
                            Models.ReoprtD6Data3 data3 = new ReoprtD6Data3();
                            data3.Number = i + 1;
                            data3.RemovalOfAction = RemovalOfAction3[i];
                            data3.Responsible = Responsible3[i];
                            data3.Removed_At = Removed_At3[i];
                            resportD6Model.D6Data3List.Add(data3);
                        }
                        #endregion


                        dataModel.ReportD6Json = Newtonsoft.Json.JsonConvert.SerializeObject(resportD6Model);

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD7Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        Models.ReoprtD7Model resportD7Model = new ReoprtD7Model();
                        resportD7Model.IsRootCause = model.ReportD7.IsRootCause;
                        resportD7Model.IfNo = model.ReportD7.IfNo;
                        resportD7Model.IfYes = model.ReportD7.IfYes;

                        string improvementOfQM7 = Request.Form["ImprovementOfQM7"].ToString();
                        string responsible7 = Request.Form["Responsible7"].ToString();
                        string planed7 = Request.Form["Planed7"].ToString();
                        string completed_On7 = Request.Form["Completed_On7"].ToString();
                        string[] ImprovementOfQM7 = improvementOfQM7.Split(',');
                        string[] Responsible7 = responsible7.Split(',');
                        string[] Planed7 = planed7.Split(',');
                        string[] Completed_On7 = completed_On7.Split(',');

                        for (int i = 0; i < ImprovementOfQM7.Length; i++)
                        {
                            Models.ReoprtD7Data data = new ReoprtD7Data();
                            data.Number = i + 1;
                            data.ImprovementOfQM = ImprovementOfQM7[i];
                            data.Responsible = Responsible7[i];
                            data.Planed = Planed7[i];
                            data.Completed_On = Completed_On7[i];
                            resportD7Model.D7DataList.Add(data);
                        }

                        dataModel.ReportD7Json = Newtonsoft.Json.JsonConvert.SerializeObject(resportD7Model);

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult ReportD8Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        string signatureGuid8 = Request.Form["SignatureGuid8"].ToString();
                        string sponsorName8 = Request.Form["SponsorName8"].ToString();
                        string signatureDate8 = Request.Form["SignatureDate8"].ToString();
                        string signature8 = Request.Form["Signature8"].ToString();
                        string[] SignatureGuid = signatureGuid8.Split(',');
                        string[] SponsorName = sponsorName8.Split(',');
                        string[] SignatureDate = signatureDate8.Split(',');
                        string[] Signature = signature8.Split(',');

                        Models.ReoprtD8Model resportD8Model = new ReoprtD8Model();
                        resportD8Model.AccomplishedAt = model.ReportD8.AccomplishedAt;
                        resportD8Model.Participants = model.ReportD8.Participants;
                        resportD8Model.Results = model.ReportD8.Results;

                        for (int i = 0; i < SponsorName.Length; i++)
                        {
                            Model.D_ReportD8_Model d8Model = dataModel.ReportD8DataModels.ToList().Find(m => m.ReportD8Guid.ToString() == SignatureGuid[i].ToString());
                            if (d8Model == null)
                            {
                                d8Model = new Model.D_ReportD8_Model();
                                d8Model.Number = i + 1;
                                d8Model.Signature = Signature[i];
                                if (!string.IsNullOrEmpty(SignatureDate[i].ToString()))
                                    d8Model.SignatureDate = Convert.ToDateTime(SignatureDate[i].ToString());
                                d8Model.SponsorName = SponsorName[i];
                                d8Model.ReportGuid = dataModel;
                                d8Model.CreateDateTime = DateTime.Now;
                                dataModel.ReportD8DataModels.Add(d8Model);
                            }


                            Models.ReoprtD8Data data = new ReoprtD8Data();
                            data.Number = i + 1;
                            data.Signature = Signature[i];
                            data.SignatureDate = SignatureDate[i];
                            data.SponsorName = SponsorName[i];
                            data.ReportD8Guid = SignatureGuid[i];
                            resportD8Model.D8DataList.Add(data);
                        }

                        dataModel.ReportD8Json = Newtonsoft.Json.JsonConvert.SerializeObject(resportD8Model);

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }
        #endregion

        #region ReportWF
        public ActionResult ReportWF_Save(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {

                        Model.D_WorkFlow_Model wfModel = new DAL.D_WorkFlow_DAL().GetModel(model.WorkFlow.EightD_WorkFlowGuid);
                        if (wfModel == null)
                            wfModel = new Model.D_WorkFlow_Model();

                        if (!string.IsNullOrEmpty(model.WorkFlow.Additional_Approver))
                        {
                            string[] loginName = model.WorkFlow.Additional_Approver.TrimEnd(';').Split(';');

                            List<D_User> userList = new List<D_User>();
                            for (int i = 0; i < loginName.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(loginName[i]))
                                {
                                    Model.D_User_Model approver = new DAL.D_User_DAL().GetUserByUserLoginName(loginName[i]);
                                    D_User user = new D_User();
                                    user.Serial = i;
                                    user.UserGuid = approver.UserGuid;
                                    user.UserLoginName = approver.UserLoginName;
                                    user.UserName = approver.UserName;
                                    userList.Add(user);
                                }
                            }

                            wfModel.Additional_Approver = Newtonsoft.Json.JsonConvert.SerializeObject(userList);
                        }
                        else
                        {
                            wfModel.Additional_Approver = model.WorkFlow.Additional_Approver;
                        }

                        wfModel.Comments = model.WorkFlow.Comments;
                        wfModel.Sponsor = model.WorkFlow.Sponsor;
                        wfModel.Team_Leader = model.WorkFlow.Team_Leader;

                        dataModel.WorkFlow_Models.Add(wfModel);

                        dataModel.ModifyDateTime = DateTime.Now;
                        dataModel.ModifyUserGuid = UserView.UserGuid;
                        dataModel.ModifyUserName = UserView.UserName;

                        success = new DAL.D_Report_DAL().Save(dataModel);

                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "报表已经不存在" });
                    }


                }
            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }
        #endregion


        [LoginFilter()]
        public ActionResult WFLog_List()
        {
            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            int startCount = 0;

            string reportGuid = string.Empty;

            JArray jsonarray = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(jsondata);

            for (int i = 0; i < jsonarray.Count; i++) //从传递参数里面选出待用的参数  
            {
                JObject jobj = (JObject)jsonarray[i];
                if (jobj.Property("name").Value.ToString().Equals("sEcho"))
                    sEcho = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("iDisplayStart"))
                    startCount = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("iDisplayLength"))
                    rowCount = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("ReportGuid"))
                    reportGuid = jobj.Property("value").Value.EToString();

            }
            #endregion

            //总条数
            int totalCount = 0;

            Model.D_Report_Model reportModel = new DAL.D_Report_DAL().GetModel(new Guid(reportGuid));

            IList<Model.D_WorkFlowLog_Model> list = new DAL.D_WorkFlowLog_DAL().GetAllByPage(startCount, rowCount, reportModel, out totalCount);

            int totalPage = totalCount % rowCount == 0 ? totalCount / rowCount : totalCount / rowCount + 1;

            #region 查询的结果集赋值
            ArrayList rows = new ArrayList();
            foreach (var item in list)
            {
                rows.Add(new
                {
                    ID = item.EightD_WorkFlowLogGuid,
                    Comments = item.Comments,
                    CreateUser = item.CreateUser,
                    OperateType = item.OperateType,
                    Status = item.Status,
                    //ModifyUserName = item.ModifyUserName,
                    //ModifyDate = item.ModifyDateTime.HasValue ? item.ModifyDateTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                    CreateUserName = item.CreateUserName,
                    CreateDate = item.CreateDateTime.HasValue ? item.CreateDateTime.Value.ToString("yyyy-MM-dd") : string.Empty
                });
            }
            #endregion

            return Json(new { sEcho = sEcho, iTotalRecords = totalCount, iTotalDisplayRecords = totalCount, aaData = rows }, JsonRequestBehavior.AllowGet);



        }


        #region 进入流程
        public ActionResult SubmitReportToApprove(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {
                        if (string.IsNullOrEmpty(dataModel.Sponaor))
                        {
                            return Json(new { success = success, message = "请设置Sponaor" });
                        }

                        if (dataModel.WorkFlow_Models.Count == 0)
                        {
                            return Json(new { success = success, message = "请设置Additional_Approver" });
                        }

                        if (string.IsNullOrEmpty(dataModel.WorkFlow_Models[0].Additional_Approver))
                        {
                            return Json(new { success = success, message = "请设置Additional_Approver" });
                        }

                        if (dataModel.ReportStatus == 0 && dataModel.ReportCancelStatus == 0)
                        {
                            dataModel.ReportStatus = (int)ReportStatusEnum.TeamLeader;
                            dataModel.CurrentStepUser = dataModel.Team_Leader;

                            success = new DAL.D_Report_DAL().Save(dataModel);
                            return Json(new { success = success, message = "成功" });
                        }
                        else
                        {
                            return Json(new { success = success, message = "本报表已在相关流程中！" });
                        }

                    }
                    else
                    {
                        return Json(new { success = success, message = "报表不存在！" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }

        public ActionResult SubmitReportToCancelApprove(ReportAssignModel model)
        {
            bool success = false;

            try
            {
                Model.D_Report_Model dataModel = null;
                if (model.ReportAssignGuid == Guid.Empty)
                {
                    return Json(new { success = success, message = "请先创建Report主体" });
                }
                else
                {
                    dataModel = new DAL.D_Report_DAL().GetModel(model.ReportAssignGuid);

                    if (dataModel != null)
                    {

                        if (string.IsNullOrEmpty(dataModel.ResponsibleQE))
                        {
                            return Json(new { success = success, message = "请设置QE" });
                        }

                        if (string.IsNullOrEmpty(dataModel.Sponaor))
                        {
                            return Json(new { success = success, message = "请设置Sponaor" });
                        }

                        if (dataModel.ReportStatus == 0 && dataModel.ReportCancelStatus == 0)
                        {
                            dataModel.ReportCancelStatus = (int)ReportStatusEnum.TeamLeader;
                            dataModel.CurrentStepUser = dataModel.Team_Leader;
                            success = new DAL.D_Report_DAL().Save(dataModel);
                            return Json(new { success = success, message = "成功" });
                        }
                        else
                        {
                            return Json(new { success = success, message = "本报表已在相关流程中！" });
                        }

                    }
                    else
                    {
                        return Json(new { success = success, message = "报表不存在！" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }
        #endregion


    }
}
