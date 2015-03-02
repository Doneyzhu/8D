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
    [Class(Table = "EightD_Board")]
    public class D_Board_Model
    {
        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "BoardGuid")]
        [Generator(1, Class = "guid")]
        virtual public Guid BoardGuid { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        [Property()]
        virtual public string BoardName { get; set; }

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

        /// <summary>
        /// 相关用户角色
        /// </summary>
        [Bag(0, Table = "EightD_Board_User_Role", OrderBy = "CreateDateTime ASC")]
        [Key(1, Column = "BoardGuid")]
        [OneToMany(2, ClassType = typeof(D_Board_User_Role_Model))]
        virtual public IList<D_Board_User_Role_Model> Board_User_Role_Models { get; set; }
    }
}
