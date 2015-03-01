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
    [Class(Table = "EightD_ReportD2")]
    public class D_ReportD2_Model
    {

        /// <summary>
        /// ID
        /// </summary>
        [Id(Name = "ReportD2Guid")]
        [Generator(1, Class = "guid")]
        virtual public Guid ReportD2Guid { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Property()]
        virtual public DateTime? Manufacturing_Date { get; set; }

        /// <summary>
        /// Team_Member
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string Bosch_Description { get; set; }

        /// <summary>
        /// Team_Member
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string Customer_Complaint { get; set; }

        /// <summary>
        /// Team_Member
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string No_of_complaint_parts { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Property()]
        virtual public DateTime? End_of_D2_Date { get; set; }

    }
}
