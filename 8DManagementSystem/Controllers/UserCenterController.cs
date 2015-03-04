using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using _8DManagementSystem.Common;
using _8DManagementSystem.Filter;

namespace _8DManagementSystem.Controllers
{
    public class UserCenterController : BaseController
    {
        //
        // GET: /UserCenter/

        #region 查询
        [LoginFilter()]
        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter()]
        public ActionResult UserList()
        {

            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            string userName = string.Empty;
            string userCode = string.Empty;
            string departmentName = string.Empty;
            string userLoginName = string.Empty;
            string dateCreated = string.Empty;

            JArray jsonarray = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(jsondata);

            for (int i = 0; i < jsonarray.Count; i++) //从传递参数里面选出待用的参数  
            {
                JObject jobj = (JObject)jsonarray[i];
                if (jobj.Property("name").Value.ToString().Equals("sEcho"))
                    sEcho = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("iDisplayStart"))
                    page = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("iDisplayLength"))
                    rowCount = jobj.Property("value").Value.EToInt();

                if (jobj.Property("name").Value.ToString().Equals("UserNo"))
                    userCode = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("UserName"))
                    userName = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("LoginName"))
                    userLoginName = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DepartmentName"))
                    departmentName = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DateCreated"))
                    dateCreated = jobj.Property("value").Value.EToString();

            }
            #endregion

            #region 调用数据访问层
            //总条数
            int totalCount = 0;

            IList<Model.D_User_Model> list = new DAL.D_User_DAL().GetAllByPage(page - 1, rowCount, userName, userCode, departmentName, userLoginName, dateCreated, out totalCount);

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
                                "<li><a href=\"" + Url.Content("~/UserCenter/UserEdit") + "/" + item.UserGuid + "\"><span class=\"glyphicon glyphicon-edit\"></span> Show Detail</a></li>" +
                                "<li><a href=\"#\"><span class=\"glyphicon glyphicon-cog\"></span> Configuration</a></li>" +
                                "<li><a href=\"#\"><span class=\"glyphicon glyphicon-export\"></span> Export to Excel</a></li>" +
                                "<li class=\"divider\"></li>" +
                                "<li><a href=\"#\" id=\"" + item.UserGuid + "\" onclick=\"del(this)\"><span class=\"glyphicon glyphicon-remove-circle\"></span> Remove</a></li>" +
                            "</ul></div>"),
                    ID = item.UserGuid,
                    UserCode = item.UserCode,
                    LoginName = item.UserLoginName,
                    UserName = item.UserName,
                    Password = "******",
                    DepartmentName = item.DepartmentName,
                    IsAdmin = item.IsAdmin ? "Yes" : "No",
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
        /// 编辑用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        [LoginFilter()]
        public ActionResult UserEdit(Guid? id)
        {
            Model.D_User_Model model = new Model.D_User_Model();

            if (id.HasValue)
            {
                model = new DAL.D_User_DAL().GetModel(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 编辑幻灯片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginFilter()]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserEdit(Model.D_User_Model model)
        {
            bool success = false;

            try
            {
                Model.D_User_Model dataModel = null;
                if (model.UserGuid == Guid.Empty)
                {
                    dataModel = new Model.D_User_Model();

                    dataModel.CreateDateTime = DateTime.Now;
                    dataModel.CreateUserGuid = UserView.UserGuid;
                    dataModel.CreateUserName = UserView.UserName;

                    dataModel.DataStatus = false;

                }
                else
                {
                    dataModel = new DAL.D_User_DAL().GetModel(model.UserGuid);
                }

                dataModel.UserLoginName = model.UserLoginName;
                dataModel.UserName = model.UserName;
                dataModel.PassWord = new DAL.D_User_DAL().PassWordMD5(model.PassWord);
                dataModel.IsAdmin = model.IsAdmin;
                dataModel.DepartmentName = model.DepartmentName;
                dataModel.ModifyDateTime = DateTime.Now;
                dataModel.ModifyUserGuid = UserView.UserGuid;
                dataModel.ModifyUserName = UserView.UserName;

                success = new DAL.D_User_DAL().Save(dataModel);
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
        public ActionResult UserDel(Guid? id)
        {
            bool success = false;
            string msg = string.Empty;
            if (id.HasValue)
            {
                try
                {

                    Model.D_User_Model model = new DAL.D_User_DAL().GetModel(id.Value);

                    model.ModifyDateTime = DateTime.Now;
                    model.DataStatus = true;

                    success = new DAL.D_User_DAL().Save(model);
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

        #region CheckUser
        public ActionResult CheckUserOne()
        {
            bool success = false;
            string msg = string.Empty;
            try
            {
                String UserLoginNames = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream)).ReadToEnd());
                if (!string.IsNullOrEmpty(UserLoginNames))
                {
                    UserLoginNameClass userLogin = Newtonsoft.Json.JsonConvert.DeserializeObject<UserLoginNameClass>(UserLoginNames);
                    if (!string.IsNullOrEmpty(userLogin.UserLoginNames))
                    {
                        string[] loginName = userLogin.UserLoginNames.TrimEnd(';').Split(';');
                        if (loginName.Count() > 1)
                        {
                            return Json(new { success = success, message = "只能有一个用户！" }, JsonRequestBehavior.AllowGet);
                        }

                        List<string> loginNameList = new List<string>();
                        foreach (var item in loginName)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                loginNameList.Add(item);
                            }
                        }

                        IList<Model.D_User_Model> users = new DAL.D_User_DAL().GetUserByUserLoginName(loginNameList);
                        foreach (var item in users)
                        {
                            msg += item.UserLoginName + ";";
                        }
                        msg.TrimEnd(';');
                    }
                    success = true;
                }


                return Json(new { success = success, message = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                msg = "参数错误";
                return Json(new { success = success, message = msg }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult CheckUser()
        {
            bool success = false;
            string msg = string.Empty;
            try
            {
                String UserLoginNames = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream)).ReadToEnd());
                if (!string.IsNullOrEmpty(UserLoginNames))
                {
                    UserLoginNameClass userLogin = Newtonsoft.Json.JsonConvert.DeserializeObject<UserLoginNameClass>(UserLoginNames);
                    if (!string.IsNullOrEmpty(userLogin.UserLoginNames))
                    {
                        string[] loginName = userLogin.UserLoginNames.Split(';');
                        List<string> loginNameList = new List<string>();
                        foreach (var item in loginName)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                loginNameList.Add(item);
                            }
                        }

                        IList<Model.D_User_Model> users = new DAL.D_User_DAL().GetUserByUserLoginName(loginNameList);
                        foreach (var item in users)
                        {
                            msg += item.UserLoginName + ";";
                        }
                        msg.TrimEnd(';');
                    }
                    success = true;
                }


                return Json(new { success = success, message = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                msg = "参数错误";
                return Json(new { success = success, message = msg }, JsonRequestBehavior.AllowGet);
            }

        }

        public class UserLoginNameClass
        {
            public string UserLoginNames { get; set; }
        }
        #endregion
    }
}

