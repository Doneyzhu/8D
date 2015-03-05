using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8DManagementSystem.Model
{
    class Enum
    {
    }

    public enum DicTypeEnum
    {
        [Description("Language")]
        Language = 1,

        [Description("ReportType")]
        ReportType = 2,
    }

    public enum PermissionEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Description("New")]
        New = 1,
        /// <summary>
        /// 编辑
        /// </summary>
        [Description("Edit")]
        Edit = 2,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("Approve")]
        Approve = 3,
        /// <summary>
        /// 回顾
        /// </summary>
        [Description("Review")]
        Review = 4,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("Completed")]
        Completed = 5,
        /// <summary>
        /// SubmitReview
        /// </summary>
        [Description("SubmitReview")]
        SubmitReview = 6,
        /// <summary>
        /// Reject
        /// </summary>
        [Description("Reject")]
        Reject = 7,
    }

    public enum ReportStatusEnum
    {

        /// <summary>
        /// New
        /// </summary>
        [Description("New")]
        New = 0,
        /// <summary>
        /// TeamLeader
        /// </summary>
        [Description("TeamLeader")]
        TeamLeader = 1,
        /// <summary>
        /// Sponsor
        /// </summary>
        [Description("Sponsor")]
        Sponsor = 2,
        /// <summary>
        /// Approve
        /// </summary>
        [Description("Approve")]
        Approve = 3,
        /// <summary>
        /// Completed
        /// </summary>
        [Description("Completed")]
        Completed = 4,
    }

    public enum ReportCancelStatusEnum
    {
        /// <summary>
        /// New
        /// </summary>
        [Description("New")]
        New = 0,
        /// <summary>
        /// TeamLeader
        /// </summary>
        [Description("TeamLeader")]
        TeamLeader = 1,
        /// <summary>
        /// QE
        /// </summary>
        [Description("QE")]
        QE = 2,
        /// <summary>
        /// Sponsor
        /// </summary>
        [Description("Sponsor")]
        Sponsor = 3,
        /// <summary>
        /// Completed
        /// </summary>
        [Description("Completed")]
        Completed = 4,
    }
}
