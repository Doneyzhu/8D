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
    [Class(Table = "EightD_Report")]
    public class D_Report_Model
    {
        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "ReportGuid")]
        [Generator(1, Class = "guid")]
        virtual public Guid ReportGuid { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        [Property()]
        virtual public string ReportTitle { get; set; }

        /// <summary>
        /// 报表类型
        /// </summary>
        [ManyToOne(Column = "ReportTypeGuid", NotNull = false)]
        virtual public D_Dictionary_Model ReportTypeGuid { get; set; }


        /// <summary>
        /// 报表板块
        /// </summary>
        [ManyToOne(Column = "ReportBoardGuid", NotNull = false)]
        virtual public D_Board_Model ReportBoardGuid { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        [Property()]
        virtual public string ReportOwner { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        [Property()]
        virtual public string ResponsibleQE { get; set; }

        /// <summary>
        /// 报表编号
        /// </summary>
        [Property()]
        virtual public string ReportNo { get; set; }
    }
}
