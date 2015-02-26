using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace _8DManagementSystem.Model
{
    /// <summary>
    /// 8D Report 用户权限
    /// </summary>
    [Serializable]
    [Class(Table = "EightD_Role_Permissions")]
    public class D_Role_Permissions_Model
    {
        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "Role_Permissions_Guid")]
        [Generator(1, Class = "guid")]
        virtual public Guid Role_Permissions_Guid { get; set; }

        /// <summary>
        /// 角色GUID
        /// </summary>
        [ManyToOne(Column = "RoleGuid", NotNull = false)]
        virtual public D_Role_Model RoleGuid { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [Property()]
        virtual public string Permissions { get; set; }

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
