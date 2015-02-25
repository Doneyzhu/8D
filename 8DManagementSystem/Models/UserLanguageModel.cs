using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _8DManagementSystem.Models
{
    public class UserLanguageModel
    {
        public Guid UserGuid { get; set; }

        public string Language { get; set; }

        /// <summary>
        /// 语言字典集合
        /// </summary>
        public IEnumerable<SelectListItem> LanguageList { get; set; }

        /// <summary>
        /// 加载语言选择
        /// </summary>
        public void LoadLanguageList()
        {
            LanguageList = new List<SelectListItem>();

            IList<Model.D_Dictionary_Model> dics = new DAL.D_Dictionary_DAL().GetDictionaryByDicType(Model.DicTypeEnum.Language);

            List<SelectListItem> lists = new List<SelectListItem>();
            foreach (var item in dics)
            {
                SelectListItem child = new SelectListItem() { Text = item.DicName, Value = item.DicName };
                lists.Add(child);
            }
            lists.Insert(0, new SelectListItem() { Text = "请选择", Value = string.Empty });
            LanguageList = lists;
        }
    }
}