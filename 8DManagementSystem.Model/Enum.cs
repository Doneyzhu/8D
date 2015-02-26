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
        Language = 1,

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
        /// 评论
        /// </summary>
        [Description("Comment")]
        Comment = 4,
        /// <summary>
        /// 回顾
        /// </summary>
        [Description("Review")]
        Review = 5,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("Completed")]
        Completed = 6,
    }
}
