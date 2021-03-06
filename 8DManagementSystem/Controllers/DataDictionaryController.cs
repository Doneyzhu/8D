﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Filter;
using _8DManagementSystem.Common;
using Newtonsoft.Json.Linq;
using System.Collections;
using _8DManagementSystem.Models;

namespace _8DManagementSystem.Controllers
{
    public class DataDictionaryController : BaseController
    {
        #region 查询
        //
        // GET: /DataDictionary/
        [LoginFilter()]
        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter()]
        public ActionResult DictionaryList()
        {

            #region 查询条件
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            int startCount = 0;

            string dicName = string.Empty;
            string dicType = string.Empty;
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

                if (jobj.Property("name").Value.ToString().Equals("DicName"))
                    dicName = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DicType"))
                    dicType = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("DateCreated"))
                    dateCreated = jobj.Property("value").Value.EToString();

            }
            #endregion

            #region 调用数据访问层
            //总条数
            int totalCount = 0;

            IList<Model.D_Dictionary_Model> list = new DAL.D_Dictionary_DAL().GetAllByPage(startCount, rowCount, null, dicName, dateCreated, out totalCount);

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
                                "<li><a href=\"" + Url.Content("~/DataDictionary/DictionaryEdit") + "/" + item.DicGuid + "\"><span class=\"glyphicon glyphicon-edit\"></span> Show Detail</a></li>" +
                                "<li><a href=\"#\"><span class=\"glyphicon glyphicon-cog\"></span> Configuration</a></li>" +
                                "<li><a href=\"#\"><span class=\"glyphicon glyphicon-export\"></span> Export to Excel</a></li>" +
                                "<li class=\"divider\"></li>" +
                                "<li><a href=\"#\" id=\"" + item.DicGuid + "\" onclick=\"del(this)\"><span class=\"glyphicon glyphicon-remove-circle\"></span> Remove</a></li>" +
                            "</ul></div>"),
                    ID = item.DicGuid,
                    DicName = item.DicName,
                    DicType =  EnumUtility.GetEnumDescription(item.DicType),
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
        public ActionResult DictionaryEdit(Guid? id)
        {

            DictionaryModel model = new DictionaryModel();
            model.LoadDicTypeSelectList();

            if (id.HasValue)
            {
                Model.D_Dictionary_Model dataModel = new DAL.D_Dictionary_DAL().GetModel(id.Value);
                if (dataModel != null)
                {
                    if (!dataModel.DataStatus)
                    {
                        model.DicGuid = dataModel.DicGuid;
                        model.DicName = dataModel.DicName;
                        model.DicType = Convert.ToInt32(dataModel.DicType).ToString();
                        model.Ord = dataModel.Ord;
                    }
                }
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
        public ActionResult DictionaryEdit(DictionaryModel model)
        {
            bool success = false;

            try
            {
                Model.D_Dictionary_Model dataModel = null;
                if (model.DicGuid == Guid.Empty)
                {
                    dataModel = new Model.D_Dictionary_Model();

                    dataModel.CreateDateTime = DateTime.Now;
                    dataModel.DataStatus = false;
                    dataModel.CreateUserGuid = UserView.UserGuid;
                    dataModel.CreateUserName = UserView.UserName;

                }
                else
                {
                    dataModel = new DAL.D_Dictionary_DAL().GetModel(model.DicGuid);
                }

                dataModel.DicName = model.DicName;
                dataModel.DicType = (Model.DicTypeEnum)model.DicType.EToInt();
                dataModel.Ord = model.Ord;
                dataModel.ModifyDateTime = DateTime.Now;
                dataModel.ModifyUserGuid = UserView.UserGuid;
                dataModel.ModifyUserName = UserView.UserName;


                success = new DAL.D_Dictionary_DAL().Save(dataModel);
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
        public ActionResult DictionaryDel(Guid? id)
        {
            bool success = false;
            string msg = string.Empty;
            if (id.HasValue)
            {
                try
                {

                    Model.D_Dictionary_Model model = new DAL.D_Dictionary_DAL().GetModel(id.Value);

                    model.ModifyDateTime = DateTime.Now;
                    model.DataStatus = true;

                    success = new DAL.D_Dictionary_DAL().Save(model);
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
    }
}
