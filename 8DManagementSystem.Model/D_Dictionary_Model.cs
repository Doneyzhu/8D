using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace _8DManagementSystem.Model
{
    /// <summary>
    /// 8D Report 报表数据字典
    /// </summary>
    [Serializable]
    [Class(Table = "EightD_Dictionary")]
    public class D_Dictionary_Model
    {
        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "DicGuid")]
        [Generator(1, Class = "guid")]
        virtual public Guid DicGuid { get; set; }

        /// <summary>
        /// 数据字典类型
        /// </summary>
        [Property()]
        virtual public DicTypeEnum DicType { get; set; }

        /// <summary>
        /// 数据字典名称
        /// </summary>
        [Property()]
        virtual public string DicName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property()]
        virtual public int Ord { get; set; }

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
