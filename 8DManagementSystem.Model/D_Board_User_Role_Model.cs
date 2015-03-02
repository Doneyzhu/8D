using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace _8DManagementSystem.Model
{
    /// <summary>
    /// 8D Report 板块用户角色关系表
    /// </summary>
    [Serializable]
    [Class(Table = "EightD_Board_User_Role")]
    public class D_Board_User_Role_Model
    {

        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "Board_User_Role_Guid")]
        [Generator(1, Class = "guid")]
        virtual public Guid Board_User_Role_Guid { get; set; }

        /// <summary>
        /// 板块GUID
        /// </summary>
        [ManyToOne(Column = "BoardGuid", NotNull = false)]
        virtual public D_Board_Model BoardGuid { get; set; }

        /// <summary>
        /// 用户GUID
        /// </summary>
        [ManyToOne(Column = "UserGuid", NotNull = false)]
        virtual public D_User_Model UserGuid { get; set; }

        /// <summary>
        /// 角色GUID
        /// </summary>
        [ManyToOne(Column = "RoleGuid", NotNull = false)]
        virtual public D_Role_Model RoleGuid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property()]
        virtual public DateTime? CreateDateTime { get; set; }

    }
}
