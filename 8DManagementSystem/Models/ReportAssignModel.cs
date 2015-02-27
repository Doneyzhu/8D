using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _8DManagementSystem.Models
{
    public class ReportAssignModel
    {
        public Guid ReportAssignGuid { get; set; }

        public string ReportTitle { get; set; }


        #region 报表类型
        public string ReportType { get; set; }

        public IEnumerable<SelectListItem> ReportTypeSelectList { get; set; }

        public void LoadReportTypeSelectList()
        {
            ReportTypeSelectList = new List<SelectListItem>();

            IList<Model.D_Dictionary_Model> dics = new DAL.D_Dictionary_DAL().GetDictionaryByDicType(Model.DicTypeEnum.ReportType);

            List<SelectListItem> lists = new List<SelectListItem>();
            foreach (var item in dics)
            {
                SelectListItem child = new SelectListItem() { Text = item.DicName, Value = item.DicGuid.ToString() };
                lists.Add(child);
            }
            lists.Insert(0, new SelectListItem() { Text = "请选择", Value = string.Empty });
            ReportTypeSelectList = lists;
        }
        #endregion

        #region 8DBoard
        public string BoardGuid { get; set; }

        public IEnumerable<SelectListItem> BoardSelectList { get; set; }

        public void LoadBoardSelectList()
        {
            BoardSelectList = new List<SelectListItem>();

            IList<Model.D_Board_Model> boards = new DAL.D_Board_DAL().GetAll();

            List<SelectListItem> lists = new List<SelectListItem>();
            foreach (var item in boards)
            {
                SelectListItem child = new SelectListItem() { Text = item.BoardName, Value = item.BoardGuid.ToString() };
                lists.Add(child);
            }
            lists.Insert(0, new SelectListItem() { Text = "请选择", Value = string.Empty });
            BoardSelectList = lists;
        }
        #endregion

        public string ReportOwner { get; set; }

        public string ResponsibleQE { get; set; }

        public string ReportNo { get; set; }

    }
}