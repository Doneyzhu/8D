using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _8DManagementSystem.Models
{
    public class Board_User_Role_Model
    {

        /// <summary>
        /// ID
        /// </summary>
        virtual public Guid Board_Guid { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        virtual public string BoardName { get; set; }

        /// <summary>
        ///用户
        /// </summary>
        public List<SelectListItem> UserSelectList { get; set; }

        public void LoadUserSelectList()
        {
            UserSelectList = new List<SelectListItem>();
            IList<Model.D_User_Model> dics = new DAL.D_User_DAL().GetAll();

            foreach (var item in dics)
            {
                SelectListItem child = new SelectListItem() { Text = item.UserName, Value = item.UserGuid.ToString() };
                UserSelectList.Add(child);
            }
        }

        /// <summary>
        /// 角色
        /// </summary>
        public List<SelectListItem> RoleSelectList { get; set; }

        public void LoadRoleSelectList()
        {
            RoleSelectList = new List<SelectListItem>();
            IList<Model.D_Role_Model> dics = new DAL.D_Role_DAL().GetAll();

            foreach (var item in dics)
            {
                SelectListItem child = new SelectListItem() { Text = item.RoleName, Value = item.RoleGuid.ToString() };
                RoleSelectList.Add(child);
            }
        }
    }
}