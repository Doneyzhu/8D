using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace _8DManagementSystem.Model
{
    /// <summary>
    /// 8D Report 审批流程日志
    /// </summary>
    [Serializable]
    [Class(Table = "EightD_WorkFlowLog")]
    public class D_WorkFlowLog_Model
    {
        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "EightD_WorkFlowLogGuid")]
        [Generator(1, Class = "guid")]
        virtual public Guid EightD_WorkFlowLogGuid { get; set; }


        /// <summary>
        /// 报表主表
        /// </summary>
        [ManyToOne(Column = "ReportGuid", NotNull = false)]
        virtual public D_Report_Model ReportGuid { get; set; }

        /// <summary>
        /// Team_Leader
        /// </summary>
        [Property()]
        virtual public string Status { get; set; }

        /// <summary>
        /// Sponsor
        /// </summary>
        [Property()]
        virtual public string CreateUser { get; set; }

        /// <summary>
        /// Additional_Approver
        /// </summary>
        [Property()]
        virtual public string CreateUserName { get; set; }


        /// <summary>
        /// Additional_Approver
        /// </summary>
        [Property()]
        virtual public string OperateType { get; set; }


        /// <summary>
        /// Additional_Approver
        /// </summary>
        [Property()]
        virtual public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string Comments { get; set; }
    }
}
