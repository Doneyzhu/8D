using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using _8DManagementSystem.Common;
using System.Collections;
using _8DManagementSystem.Filter;
using _8DManagementSystem.Models;

namespace _8DManagementSystem.Controllers
{
    public class BoardController : BaseController
    {
        //
        // GET: /Board/

        #region 查询
        [LoginFilter()]
        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter()]
        public ActionResult BoardList()
        {

            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            int startCount = 0;
            string boardName = string.Empty;
            string dateCreated = string.Empty;

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

                if (jobj.Property("name").Value.ToString().Equals("BoardName"))
                    boardName = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DateCreated"))
                    dateCreated = jobj.Property("value").Value.EToString();

            }
            #endregion

            #region 调用数据访问层
            //总条数
            int totalCount = 0;

            IList<Model.D_Board_Model> list = new DAL.D_Board_DAL().GetAllByPage(startCount, rowCount, boardName, dateCreated, out totalCount);

            int totalPage = totalCount % rowCount == 0 ? totalCount / rowCount : totalCount / rowCount + 1;

            #endregion

            #region 查询的结果集赋值
            ArrayList rows = new ArrayList();
            foreach (var item in list)
            {
                rows.Add(new
                {
                    Operation = string.Format("<div class=\"btn-group\">" +
                            "<button type=\"button\" class=\"btn btn-sm btn-primary dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">" +
                                "Action <span class=\"caret\"></span></button>" +
                            "<ul class=\"dropdown-menu\" role=\"menu\">" +
                                "<li><a href=\"" + Url.Content("~/Board/BoardEdit") + "/" + item.BoardGuid + "\"><span class=\"glyphicon glyphicon-edit\"></span> Show Detail</a></li>" +
                                "<li><a href=\"" + Url.Content("~/Board/Board_User_Role_Edit") + "/" + item.BoardGuid + "\"><span class=\"glyphicon glyphicon-cog\"></span> Configuration</a></li>" +
                                "<li><a href=\"#\"><span class=\"glyphicon glyphicon-export\"></span> Export to Excel</a></li>" +
                                "<li class=\"divider\"></li>" +
                                "<li><a href=\"#\" id=\"" + item.BoardGuid + "\" onclick=\"del(this)\"><span class=\"glyphicon glyphicon-remove-circle\"></span> Remove</a></li>" +
                            "</ul></div>"),
                    ID = item.BoardGuid,
                    BoardName = item.BoardName,
                    ModifyUserName = item.ModifyUserName,
                    ModifyDate = item.ModifyDateTime.HasValue ? item.ModifyDateTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                    CreateUserName = item.CreateUserName,
                    CreateDate = item.CreateDateTime.HasValue ? item.CreateDateTime.Value.ToString("yyyy-MM-dd") : string.Empty
                });
            }
            #endregion

            return Json(new { sEcho = sEcho, iTotalRecords = totalCount, iTotalDisplayRecords = totalCount, aaData = rows }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑板块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        [LoginFilter()]
        public ActionResult BoardEdit(Guid? id)
        {
            Model.D_Board_Model model = new Model.D_Board_Model();

            if (id.HasValue)
            {
                model = new DAL.D_Board_DAL().GetModel(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 编辑板块名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginFilter()]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BoardEdit(Model.D_Board_Model model)
        {
            bool success = false;

            try
            {
                Model.D_Board_Model dataModel = null;
                if (model.BoardGuid == Guid.Empty)
                {
                    dataModel = new Model.D_Board_Model();

                    dataModel.CreateDateTime = DateTime.Now;
                    dataModel.DataStatus = false;
                    dataModel.CreateUserGuid = UserView.UserGuid;
                    dataModel.CreateUserName = UserView.UserName;

                }
                else
                {
                    dataModel = new DAL.D_Board_DAL().GetModel(model.BoardGuid);
                }

                dataModel.BoardName = model.BoardName;
                dataModel.ModifyDateTime = DateTime.Now;
                dataModel.ModifyUserGuid = UserView.UserGuid;
                dataModel.ModifyUserName = UserView.UserName;


                success = new DAL.D_Board_DAL().Save(dataModel);
                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }
        #endregion

        #region 删除
        [LoginFilter()]
        public ActionResult BoardDel(Guid? id)
        {
            bool success = false;
            string msg = string.Empty;
            if (id.HasValue)
            {
                try
                {

                    Model.D_Board_Model model = new DAL.D_Board_DAL().GetModel(id.Value);

                    model.ModifyDateTime = DateTime.Now;
                    model.DataStatus = true;

                    success = new DAL.D_Board_DAL().Save(model);
                    return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = success, message = ex.Message });
                }
            }

            return Json(new { success = success, msg = "数据已不存在" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 板块用户角色
        [LoginFilter()]
        public ActionResult Board_User_Role_Edit(Guid id)
        {
            Board_User_Role_Model model = new Board_User_Role_Model();

            Model.D_Board_Model dataModel = new DAL.D_Board_DAL().GetModel(id);
            model.Board_Guid = dataModel.BoardGuid;
            model.BoardName = dataModel.BoardName;
            model.LoadRoleSelectList();
            model.LoadUserSelectList();

            return View(model);
        }

        [HttpPost]
        [LoginFilter()]
        public ActionResult Board_User_Role_Edit(Board_User_Role_Model model)
        {
            bool success = false;

            try
            {
                string users = Request.Form["user"] == null ? string.Empty : Request.Form["user"].ToString();
                string roles = Request.Form["role"] == null ? string.Empty : Request.Form["role"].ToString();

                if (string.IsNullOrEmpty(users) || string.IsNullOrEmpty(roles))
                    return Json(new { success = success, message = "请选择用户和角色" }, JsonRequestBehavior.AllowGet);


                Model.D_Board_Model boardModel = new DAL.D_Board_DAL().GetModel(model.Board_Guid);
                if (boardModel == null)
                    return Json(new { success = success, message = "板块已经不存在" }, JsonRequestBehavior.AllowGet);

                string[] Users = users.Split(',');
                string[] Roles = roles.Split(',');

                List<Model.D_Board_User_Role_Model> list = new List<Model.D_Board_User_Role_Model>();
                foreach (var item in Users)
                {
                    Model.D_User_Model userModel = new DAL.D_User_DAL().GetModel(new Guid(item));
                    foreach (var role in Roles)
                    {
                        Model.D_Role_Model roleModel = new DAL.D_Role_DAL().GetModel(new Guid(role));

                        Model.D_Board_User_Role_Model dataModel = new DAL.D_Board_User_Role_DAL().GetOne(boardModel, userModel, roleModel);

                        if (dataModel == null)
                        {
                            dataModel = new Model.D_Board_User_Role_Model();
                            dataModel.BoardGuid = boardModel;
                            dataModel.UserGuid = userModel;
                            dataModel.RoleGuid = roleModel;
                            dataModel.CreateDateTime = DateTime.Now;
                            list.Add(dataModel);
                        }

                    }
                }

                success = new DAL.D_Board_User_Role_DAL().SaveList(list);

                return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = success, message = ex.Message });
            }
        }


        [LoginFilter()]
        public ActionResult Board_User_Role_EditDel(Guid? id)
        {
            bool success = false;
            string msg = string.Empty;
            if (id.HasValue)
            {
                try
                {

                    Model.D_Board_User_Role_Model model = new DAL.D_Board_User_Role_DAL().GetModel(id.Value);

                    if (model != null)
                    {
                        success = new DAL.D_Board_User_Role_DAL().Delete(model);
                        return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = success, message = "失败，请刷新页面！" }, JsonRequestBehavior.AllowGet);
                    }


                }
                catch (Exception ex)
                {
                    return Json(new { success = success, message = ex.Message });
                }
            }

            return Json(new { success = success, msg = "数据已不存在" }, JsonRequestBehavior.AllowGet);
        }


        [LoginFilter()]
        public ActionResult Board_User_Role_List()
        {

            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            int startCount = 0;
            string boardGuid = string.Empty;

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

                if (jobj.Property("name").Value.ToString().Equals("BoardGuid"))
                    boardGuid = jobj.Property("value").Value.EToString();

            }
            #endregion

            #region 调用数据访问层
            //总条数
            int totalCount = 0;
            Model.D_Board_Model boardModel = new DAL.D_Board_DAL().GetModel(new Guid(boardGuid));

            IList<Model.D_Board_User_Role_Model> list = new DAL.D_Board_User_Role_DAL().GetAllByPage(startCount, rowCount, boardModel, out totalCount);

            int totalPage = totalCount % rowCount == 0 ? totalCount / rowCount : totalCount / rowCount + 1;

            #endregion

            #region 查询的结果集赋值
            ArrayList rows = new ArrayList();
            foreach (var item in list)
            {
                rows.Add(new
                {
                    Operation = string.Format("<div class=\"btn-group\">" +
                            "<button type=\"button\" class=\"btn btn-sm btn-primary dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">" +
                                "Action <span class=\"caret\"></span></button>" +
                            "<ul class=\"dropdown-menu\" role=\"menu\">" +
                                "<li><a href=\"#\" id=\"" + item.Board_User_Role_Guid + "\" onclick=\"del(this)\"><span class=\"glyphicon glyphicon-remove-circle\"></span> Remove</a></li>" +
                            "</ul></div>"),
                    ID = item.Board_User_Role_Guid,
                    UserName = item.UserGuid.UserName,
                    RoleName = item.RoleGuid.RoleName,
                    CreateDate = item.CreateDateTime.HasValue ? item.CreateDateTime.Value.ToString("yyyy-MM-dd") : string.Empty
                });
            }
            #endregion

            return Json(new { sEcho = sEcho, iTotalRecords = totalCount, iTotalDisplayRecords = totalCount, aaData = rows }, JsonRequestBehavior.AllowGet);

        }


        #endregion
    }
}
