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
    [Class(Table = "EightD_ReportD8")]
    public class D_ReportD8_Model
    { /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "ReportD8Guid")]
        [Generator(1, Class = "guid")]
        virtual public Guid ReportD8Guid { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [Property()]
        virtual public int Number { get; set; }

        /// <summary>
        /// SponsorName
        /// </summary>
        [Property()]
        virtual public string SponsorName { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Property()]
        virtual public DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Signature
        /// </summary>
        [Property()]
        virtual public string Signature { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property()]
        virtual public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 报表主表
        /// </summary>
        [ManyToOne(Column = "ReportGuid", NotNull = false)]
        virtual public D_Report_Model ReportGuid { get; set; }

    }
}
