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

        public D_Report_Model()
        {
            ReportD2 = new D_ReportD2_Model();
            ReportD8DataModels = new List<D_ReportD8_Model>();
            WorkFlow_Models = new List<D_WorkFlow_Model>();
            WorkFlowLog_Models = new List<D_WorkFlowLog_Model>();
        }

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



        #region HeaderData
        /// <summary>
        /// 提交日期
        /// </summary>
        [Property()]
        virtual public DateTime? ComplaintDate { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [Property()]
        virtual public string Product { get; set; }
        /// <summary>
        /// 博世物料号
        /// </summary>
        [Property()]
        virtual public string Bosch_Material_No { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        [Property()]
        virtual public string Warranty_Descision { get; set; }
        /// <summary>
        /// 维护计划
        /// </summary>
        [Property()]
        virtual public string Manufacturing_Plant { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        [Property()]
        virtual public string Customer { get; set; }
        /// <summary>
        /// 客户物料号
        /// </summary>
        [Property()]
        virtual public string Customer_Material_No { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        [Property()]
        virtual public string Complaint_TypeMode { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        [Property()]
        virtual public string Serial_No { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        [Property()]
        virtual public string Supplier_No { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Property()]
        virtual public string Supplier_Name { get; set; }
        /// <summary>
        /// Issuer
        /// </summary>
        [Property()]
        virtual public string Issuer { get; set; }
        #endregion

        #region ReportD1Model
        /// <summary>
        /// Team_Leader
        /// </summary>
        [Property()]
        virtual public string Team_Leader { get; set; }
        /// <summary>
        /// Sponaor
        /// </summary>
        [Property()]
        virtual public string Sponaor { get; set; }
        /// <summary>
        /// Coordinator
        /// </summary>
        [Property()]
        virtual public string Coordinator { get; set; }
        /// <summary>
        /// Team_Member
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string Team_Member { get; set; }
        #endregion

        /// <summary>
        /// ReportD2
        /// </summary>
        [ManyToOne(Column = "ReportD2Guid", NotNull = false)]
        virtual public D_ReportD2_Model ReportD2 { get; set; }

        /// <summary>
        /// ReportD3Json
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string ReportD3Json { get; set; }
        /// <summary>
        /// ReportD4Json
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string ReportD4Json { get; set; }
        /// <summary>
        /// ReportD5Json
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string ReportD5Json { get; set; }
        /// <summary>
        /// ReportD6Json
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string ReportD6Json { get; set; }
        /// <summary>
        /// ReportD7Json
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string ReportD7Json { get; set; }
        /// <summary>
        /// ReportD8Json
        /// </summary>
        [Property(Type = "StringClob")]
        virtual public string ReportD8Json { get; set; }


        /// <summary>
        /// 相关团队
        /// </summary>
        [Bag(0, Table = "EightD_ReportD8", OrderBy = "CreateDateTime ASC")]
        [Key(1, Column = "ReportGuid")]
        [OneToMany(2, ClassType = typeof(D_ReportD8_Model))]
        virtual public IList<D_ReportD8_Model> ReportD8DataModels { get; set; }

        /// <summary>
        /// 相关工作流程
        /// </summary>
        [Bag(0, Table = "EightD_WorkFlow", OrderBy = "CreateDateTime ASC")]
        [Key(1, Column = "ReportGuid")]
        [OneToMany(2, ClassType = typeof(D_WorkFlow_Model))]
        virtual public IList<D_WorkFlow_Model> WorkFlow_Models { get; set; }

        /// <summary>
        /// 相关工作流程日志
        /// </summary>
        [Bag(0, Table = "EightD_WorkFlowLog", OrderBy = "CreateDateTime ASC")]
        [Key(1, Column = "ReportGuid")]
        [OneToMany(2, ClassType = typeof(D_WorkFlowLog_Model))]
        virtual public IList<D_WorkFlowLog_Model> WorkFlowLog_Models { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        [Property()]
        virtual public bool DataStatus { get; set; }

        /// <summary>
        /// ReportStatus
        /// </summary>
        [Property()]
        virtual public int ReportStatus { get; set; }

        /// <summary>
        /// ReportCancelStatus
        /// </summary>
        [Property()]
        virtual public int ReportCancelStatus { get; set; }


    }
}
