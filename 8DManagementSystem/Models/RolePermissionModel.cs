using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _8DManagementSystem.Common;
using _8DManagementSystem.Model;

namespace _8DManagementSystem.Models
{
    public class RolePermissionModel
    {
        public Guid RoleGuid { get; set; }

        public string RoleName { get; set; }

        public List<string> SelectedPermissions { get; set; }
        /// <summary>
        /// 操作类型枚举
        /// </summary>
        public IEnumerable<SelectListItem> PermissionSelectList { get; set; }

        public void LoadPermission()
        {
            PermissionSelectList = EnumUtility.ConvertEnumToSelectListItems(typeof(PermissionEnum));
        }
    }
}