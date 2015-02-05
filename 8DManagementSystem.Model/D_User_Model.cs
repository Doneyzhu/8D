using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace _8DManagementSystem.Model
{
    /// <summary>
    /// 8D Report 报表用户表
    /// </summary>
    [Serializable]
    [Class(Table = "EightD_User")]
    public class D_User_Model
    {
        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "UserGuid")]
        [Generator(1, Class = "guid")]
        virtual public Guid UserGuid { get; set; }


        /// <summary>
        /// 用户编码
        /// </summary>
        [Property()]
        virtual public string UserCode { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        [Property()]
        virtual public string UserLoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Property()]
        virtual public string UserName { get; set; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        [Property()]
        virtual public string PassWord { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Property()]
        virtual public string DepartmentName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        [Property()]
        virtual public bool IsAdmin { get; set; }

        /// <summary>
        /// 创建人GUID
        /// </summary>
        [Property()]
        virtual public Guid CreateUserGuid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property()]
        virtual public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 创建人名字
        /// </summary>
        [Property()]
        virtual public string CreateUserName { get; set; }

        /// <summary>
        /// 修改人用户GUID
        /// </summary>
        [Property()]
        virtual public Guid ModifyUserGuid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Property()]
        virtual public DateTime? ModifyDateTime { get; set; }

        /// <summary>
        /// 修改人用户名字
        /// </summary>
        [Property()]
        virtual public string ModifyUserName { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        [Property()]
        virtual public bool DataStatus { get; set; }
    }
}
