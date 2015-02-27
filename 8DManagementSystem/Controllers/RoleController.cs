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
using NHibernate;
using NHibernate.Criterion;
//using _8DManagementSystem.DAL.DBHelper;

namespace _8DManagementSystem.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Role/

        #region 查询
        [LoginFilter()]
        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter()]
        public ActionResult RoleList()
        {

            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            int startCount = 0;
            string roleName = string.Empty;

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

                if (jobj.Property("name").Value.ToString().Equals("RoleName"))
                    roleName = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DateCreated"))
                    dateCreated = jobj.Property("value").Value.EToString();

            }
            #endregion

            #region 调用数据访问层
            //总条数
            int totalCount = 0;

            IList<Model.D_Role_Model> list = new DAL.D_Role_DAL().GetAllByPage(startCount, rowCount, roleName, dateCreated, out totalCount);

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
                                "<li><a href=\"" + Url.Content("~/Role/RoleEdit") + "/" + item.RoleGuid + "\"><span class=\"glyphicon glyphicon-edit\"></span> Show Detail</a></li>" +
                                "<li><a href=\"" + Url.Content("~/Role/Role_Permission_Edit") + "/" + item.RoleGuid + "\"><span class=\"glyphicon glyphicon-cog\"></span> Configuration</a></li>" +
                                "<li><a href=\"#\"><span class=\"glyphicon glyphicon-export\"></span> Export to Excel</a></li>" +
                                "<li class=\"divider\"></li>" +
                                "<li><a href=\"#\" id=\"" + item.RoleGuid + "\" onclick=\"del(this)\"><span class=\"glyphicon glyphicon-remove-circle\"></span> Remove</a></li>" +
                            "</ul></div>"),
                    ID = item.RoleGuid,
                    RoleName = item.RoleName,
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
        [LoginFilter()]
        [OutputCache(Duration = 0)]
        public ActionResult RoleEdit(Guid? id)
        {
            Model.D_Role_Model model = new Model.D_Role_Model();

            if (id.HasValue)
            {
                model = new DAL.D_Role_DAL().GetModel(id.Value);
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
        public ActionResult RoleEdit(Model.D_Role_Model model)
        {
            bool success = false;

            try
            {
                Model.D_Role_Model dataModel = null;
                if (model.RoleGuid == Guid.Empty)
                {
                    dataModel = new Model.D_Role_Model();

                    dataModel.CreateDateTime = DateTime.Now;
                    dataModel.CreateUserGuid = UserView.UserGuid;
                    dataModel.CreateUserName = UserView.UserName;

                    dataModel.DataStatus = false;
                }
                else
                {
                    dataModel = new DAL.D_Role_DAL().GetModel(model.RoleGuid);
                }

                dataModel.RoleName = model.RoleName;
                dataModel.ModifyDateTime = DateTime.Now;
                dataModel.ModifyUserGuid = UserView.UserGuid;
                dataModel.ModifyUserName = UserView.UserName;


                success = new DAL.D_Role_DAL().Save(dataModel);
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
        public ActionResult RoleDel(Guid? id)
        {
            bool success = false;
            string msg = string.Empty;
            if (id.HasValue)
            {
                try
                {

                    Model.D_Role_Model model = new DAL.D_Role_DAL().GetModel(id.Value);

                    model.ModifyDateTime = DateTime.Now;
                    model.DataStatus = true;

                    success = new DAL.D_Role_DAL().Save(model);
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

        #region 角色权限编辑
        [LoginFilter()]
        public ActionResult Role_Permission_Edit(Guid? id)
        {
            RolePermissionModel model = new RolePermissionModel();
            model.LoadPermission();
            if (id.HasValue)
            {
                Model.D_Role_Model modelData = new DAL.D_Role_DAL().GetModel(id.Value);
                model.RoleGuid = modelData.RoleGuid;
                model.RoleName = modelData.RoleName;

                IList<Model.D_Role_Permissions_Model> permissionList = new DAL.D_Role_Permissions_DAL().GetRole_Permissions_ByRole(modelData);
                List<string> permissions = new List<string>();
                foreach (var item in permissionList)
                {
                    permissions.Add(item.Permissions);
                }

                model.SelectedPermissions = permissions;
            }

            return View(model);
        }

        [LoginFilter()]
        [HttpPost]
        public ActionResult Role_Permission_Edit()
        {
            bool success = false;
            string roleGuid = Request.Form["RoleGuid"];
            string permissions = Request.Form[2].ToString();
            string[] permission = permissions.Split(',');
            using (NHibernate.ITransaction tran = NhSession.BeginTransaction())
            {
                try
                {
                    Guid RoleGuid = new Guid(roleGuid);
                    Model.D_Role_Model role = NhSession.Get<Model.D_Role_Model>(RoleGuid);
                    ICriteria ic = NhSession.CreateCriteria(typeof(Model.D_Role_Permissions_Model));
                    ic.Add(Restrictions.Eq("RoleGuid", role));
                    IList<Model.D_Role_Permissions_Model> rolePermissionList = ic.List<Model.D_Role_Permissions_Model>();

                    foreach (var item in rolePermissionList)
                    {
                        item.DataStatus = true;
                        item.ModifyDateTime = DateTime.Now;
                        item.ModifyUserGuid = UserView.UserGuid;
                        item.ModifyUserName = UserView.UserName;
                        NhSession.Update(item);
                    }

                    foreach (var item in permission)
                    {
                        Model.D_Role_Permissions_Model rolePermission = rolePermissionList.SingleOrDefault(m => m.Permissions == item);
                        if (rolePermission == null)
                        {
                            rolePermission = new Model.D_Role_Permissions_Model();
                            rolePermission.CreateDateTime = DateTime.Now;
                            rolePermission.CreateUserGuid = UserView.UserGuid;
                            rolePermission.CreateUserName = UserView.UserName;
                            rolePermission.RoleGuid = role;
                            rolePermission.Permissions = item;
                        }

                        rolePermission.DataStatus = false;
                        rolePermission.ModifyDateTime = DateTime.Now;
                        rolePermission.ModifyUserGuid = UserView.UserGuid;
                        rolePermission.ModifyUserName = UserView.UserName;

                        NhSession.SaveOrUpdate(rolePermission);
                    }
                    tran.Commit();
                    return Json(new { success = success, message = "成功" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = success, message = ex.Message });
                }
                finally
                {
                    if (!tran.WasCommitted && !tran.WasRolledBack)
                        tran.Rollback();
                }
            }


        }
        #endregion
    }
}
