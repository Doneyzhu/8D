using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using _8DManagementSystem.Common;

namespace _8DManagementSystem.Controllers
{
    public class UserCenterController : Controller
    {
        //
        // GET: /UserCenter/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserList()
        {
            int page = !string.IsNullOrEmpty(Request.QueryString["iDisplayStart"]) ? int.Parse(Request.QueryString["iDisplayStart"]) : 0;
            int rowCount = !string.IsNullOrEmpty(Request.QueryString["iDisplayLength"]) ? int.Parse(Request.QueryString["iDisplayLength"]) : 2;
            String jsondata = HttpUtility.UrlDecode(Request.Params[0], System.Text.Encoding.UTF8);

            int sEcho = 0;
            string subCompanyId = string.Empty;
            string picName = string.Empty;
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

                if (jobj.Property("name").Value.ToString().Equals("SubCompany"))
                    subCompanyId = jobj.Property("value").Value.EToString();

                if (jobj.Property("name").Value.ToString().Equals("pictureName"))
                    picName = jobj.Property("value").Value.EToString();
            }

            //string subCompanyId = string.Empty;
            IList<Model.D_User_Model> list = new DAL.D_User_DAL().GetAll();
            int total = list.Count;
            int totalPage = total % rowCount == 0 ? total / rowCount : total / rowCount + 1;
            IList<object> objectList = new List<object>();
            int count = 1;
            foreach (var item in list.Skip(page).Take(rowCount))
            {
                //objectList.Add(new string[] { 
                //    item.Id.ToString(), count.ToString(), EnumUtility.GetEnumDescription(item.SubCompanyId), item.PicType, item.PicName, item.PicPath, item.PicDirectId,  
                //    item.IfValid == true ? "可用" : "不可用",item.Ord.ToString(),item.OpUserName,item.OpDate.HasValue ? item.OpDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" });
                //count++;
            }

            ArrayList rows = new ArrayList();
            foreach (var item in list.Skip(page).Take(rowCount))
            {
                rows.Add(new
                {

                    //id = item.Id,
//                    SubCompanyId = item.SubCompanyId,
//                    SubCompanyName = EnumUtility.GetEnumDescription(item.SubCompanyId),
//                    PicType = item.PicType,
//                    PicName = item.PicName,
//                    PicPath = string.Format(@" <img alt='{1}' src='{0}' style='width:90px;height:90px;' />", item.PicPath, item.PicName),
//                    PicDirectId = item.PicDirectId,
//                    IfValid = item.IfValid == true ? "可用" : "不可用",
//                    Ord = item.Ord,
//                    //OpUserName = item.OpUserName,
//                    OpDate = item.OpDate.HasValue ? item.OpDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
//                    Oper = string.Format(@"<div class='hidden-sm hidden-xs btn-group'><button class='btn btn-xs btn-info' id='{0}' onclick='edit(this.id)'><i class='ace-icon fa fa-pencil bigger-120'></i></button>
//                                            <button class='btn btn-xs btn-danger' id='{0}' onclick='del(this.id)'><i class='ace-icon fa fa-trash-o bigger-120'></i></button></div>", item.Id.ToString())


                });
            }

            //return Json(new { sEcho = rowCount, iTotalRecords = total, iTotalDisplayRecords = total, aaData = rows }, JsonRequestBehavior.AllowGet);

            return Json(new { sEcho = sEcho, iTotalRecords = total, iTotalDisplayRecords = total, aaData = rows }, JsonRequestBehavior.AllowGet);


        }

    }
}
