using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace _8DManagementSystem.Model
{
    /// <summary>
    /// 8D Report 审批流程用户
    /// </summary>
    [Serializable]
    [Class(Table = "EightD_WorkFlow")]
    public class D_WorkFlow_Model
    {

        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "EightD_WorkFlowGuid")]
        [Generator(1, Class = "guid")]
        virtual public Guid EightD_WorkFlowGuid { get; set; }


        /// <summary>
        /// 报表主表
        /// </summary>
        [ManyToOne(Column = "ReportGuid", NotNull = false)]
        virtual public D_Report_Model ReportGuid { get; set; }

        /// <summary>
        /// Team_Leader
        /// </summary>
        [Property()]
        virtual public string Team_Leader { get; set; }

        /// <summary>
        /// Sponsor
        /// </summary>
        [Property()]
        virtual public string Sponsor { get; set; }

        /// <summary>
        /// Additional_Approver
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string Additional_Approver { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string Comments { get; set; }
    }
}
