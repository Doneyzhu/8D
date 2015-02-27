using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Common;
using _8DManagementSystem.Model;

namespace _8DManagementSystem.Models
{
    public class DictionaryModel
    {
        /// <summary>
        /// ID
        /// </summary>
        virtual public Guid DicGuid { get; set; }

        /// <summary>
        /// 数据字典类型
        /// </summary>
        virtual public string DicType { get; set; }

        /// <summary>
        /// 操作类型枚举
        /// </summary>
        public IEnumerable<SelectListItem> DicTypeSelectList { get; set; }

        public void LoadDicTypeSelectList()
        {
            List<SelectListItem> lists = new List<SelectListItem>();
            lists = EnumUtility.ConvertEnumToListItems(typeof(DicTypeEnum));
            lists.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });
            DicTypeSelectList = lists;
        }

        /// <summary>
        /// 数据字典名称
        /// </summary>
        virtual public string DicName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        virtual public int Ord { get; set; }
    }
}