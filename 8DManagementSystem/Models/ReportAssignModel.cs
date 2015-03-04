using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _8DManagementSystem.Models
{
    public class ReportAssignModel
    {
        public ReportAssignModel()
        {
            ReportD8Sign = new List<ReoprtD8Data>();
        }

        public Guid ReportAssignGuid { get; set; }

        public string ReportTitle { get; set; }


        #region 报表类型
        public string ReportType { get; set; }

        public IEnumerable<SelectListItem> ReportTypeSelectList { get; set; }

        public void LoadReportTypeSelectList()
        {
            ReportTypeSelectList = new List<SelectListItem>();

            IList<Model.D_Dictionary_Model> dics = new DAL.D_Dictionary_DAL().GetDictionaryByDicType(Model.DicTypeEnum.ReportType);

            List<SelectListItem> lists = new List<SelectListItem>();
            foreach (var item in dics)
            {
                SelectListItem child = new SelectListItem() { Text = item.DicName, Value = item.DicGuid.ToString() };
                lists.Add(child);
            }
            lists.Insert(0, new SelectListItem() { Text = "请选择", Value = string.Empty });
            ReportTypeSelectList = lists;
        }
        #endregion

        #region 8DBoard
        public string BoardGuid { get; set; }

        public IEnumerable<SelectListItem> BoardSelectList { get; set; }

        public void LoadBoardSelectList(List<Models.BoardModel> boards)
        {
            BoardSelectList = new List<SelectListItem>();

            //IList<Model.D_Board_Model> boards = new DAL.D_Board_DAL().GetAll();

            List<SelectListItem> lists = new List<SelectListItem>();
            foreach (var item in boards)
            {
                SelectListItem child = new SelectListItem() { Text = item.BoardName, Value = item.BoardGuid.ToString() };
                lists.Add(child);
            }
            lists.Insert(0, new SelectListItem() { Text = "请选择", Value = string.Empty });
            BoardSelectList = lists;
        }
        #endregion

        public string ReportOwner { get; set; }

        public string ResponsibleQE { get; set; }

        public string ReportNo { get; set; }

        public ReportHeaderModel ReportHeader { get; set; }

        public ReoprtD1Model ReportD1 { get; set; }

        public ReoprtD2Model ReportD2 { get; set; }

        public ReoprtD3Model ReportD3 { get; set; }

        public ReoprtD4Model ReportD4 { get; set; }

        public ReoprtD5Model ReportD5 { get; set; }

        public ReoprtD6Model ReportD6 { get; set; }

        public ReoprtD7Model ReportD7 { get; set; }

        public ReoprtD8Model ReportD8 { get; set; }

        public List<ReoprtD8Data> ReportD8Sign { get; set; }

        public ReportWorkFlow WorkFlow { get; set; }

        public int ReportStatus { get; set; }
        public int ReportCancelStatus { get; set; }
       
        virtual public Models.D_User CurrentStepUser { get; set; }
        virtual public Models.D_User PreStepUser { get; set; }
        virtual public string PreStepType { get; set; }
    }

    public class ReportHeaderModel
    {
        public string ComplaintDate { get; set; }

        public string Product { get; set; }

        public string Bosch_Material_No { get; set; }

        public string Warranty_Descision { get; set; }

        public string Manufacturing_Plant { get; set; }

        public string Customer { get; set; }

        public string Customer_Material_No { get; set; }

        public string Complaint_TypeMode { get; set; }

        public string Serial_No { get; set; }

        public string Supplier_No { get; set; }

        public string Supplier_Name { get; set; }

        public string Issuer { get; set; }

    }

    public class ReoprtD1Model
    {

        public string Team_Leader { get; set; }

        public string Sponaor { get; set; }

        public string Coordinator { get; set; }

        #region Team Member
        public string Team_Member { get; set; }

        //public IEnumerable<SelectListItem> TeamMemberSelectList { get; set; }

        //public void LoadTeamMemberSelectList()
        //{
        //    TeamMemberSelectList = new List<SelectListItem>();

        //    IList<Model.D_User_Model> boards = new DAL.D_User_DAL().GetAll();

        //    List<SelectListItem> lists = new List<SelectListItem>();
        //    foreach (var item in boards)
        //    {
        //        SelectListItem child = new SelectListItem() { Text = item.UserName, Value = item.UserGuid.ToString() };
        //        lists.Add(child);
        //    }
        //    lists.Insert(0, new SelectListItem() { Text = "请选择", Value = string.Empty });
        //    TeamMemberSelectList = lists;
        //}
        #endregion

    }

    public class ReoprtD2Model
    {

        public string Manufacturing_Date { get; set; }

        public string Customer_Complaint { get; set; }

        public string Bosch_Description { get; set; }

        public string No_of_complaint_parts { get; set; }

        public string End_of_D2_Date { get; set; }
    }

    #region ReoprtD3Model
    public class ReoprtD3Model
    {
        public ReoprtD3Model()
        {
            D3DataList = new List<ReoprtD3Data>();
        }
        public List<ReoprtD3Data> D3DataList { get; set; }
        public string EndDate { get; set; }
        public string CompleteOn { get; set; }
        public string StartDate { get; set; }
        public string MeasureDate { get; set; }
        public string ResponsibleMain { get; set; }
    }
    public class ReoprtD3Data
    {
        public int Number { get; set; }
        public string Remark { get; set; }
        public string Responsible { get; set; }
        public string Introduced { get; set; }
        public string Effectinve { get; set; }
    }
    #endregion

    #region ReoprtD4Model
    public class ReoprtD4Model
    {

        public ReoprtD4Model()
        {
            D4Data1List = new List<ReoprtD4Data1>();

            D4Data2List = new List<ReoprtD4Data2>();
        }

        public string Title { get; set; }

        public List<ReoprtD4Data1> D4Data1List { get; set; }

        public List<ReoprtD4Data2> D4Data2List { get; set; }

        public string Causing_Process { get; set; }

        public string Risk_Assessment { get; set; }

        public string Period_Affected_FromStart { get; set; }

        public string Period_Affected_FromEnd { get; set; }

        public string Responsible { get; set; }

        public string Expected_Number { get; set; }

        public string Completed_On { get; set; }

        public string Qty { get; set; }

        public string Internal_Km { get; set; }

        public string Field { get; set; }
    }

    public class ReoprtD4Data1
    {
        public int Number { get; set; }

        public string Non_Conformity_Occur { get; set; }

        public string Responsible { get; set; }

        public string Completed_On { get; set; }
    }

    public class ReoprtD4Data2
    {
        public int Number { get; set; }

        public string Non_Conformity_Detected { get; set; }

        public string Responsible { get; set; }

        public string Completed_On { get; set; }
    }
    #endregion

    #region ReoprtD5Model
    public class ReoprtD5Model
    {
        public ReoprtD5Model()
        {
            D5DataList = new List<ReoprtD5Data>();
        }
        public List<ReoprtD5Data> D5DataList { get; set; }

    }
    public class ReoprtD5Data
    {
        public int Number { get; set; }
        public string Corrective_Effectiveness { get; set; }
        public string Responsible { get; set; }
        public string Completed_On { get; set; }
    }
    #endregion

    #region ReoprtD6Model
    public class ReoprtD6Model
    {
        public ReoprtD6Model()
        {
            D6Data1List = new List<ReoprtD6Data1>();
            D6Data2List = new List<ReoprtD6Data2>();
            D6Data3List = new List<ReoprtD6Data3>();
        }
        public List<ReoprtD6Data1> D6Data1List { get; set; }

        public List<ReoprtD6Data2> D6Data2List { get; set; }

        public List<ReoprtD6Data3> D6Data3List { get; set; }

        public string CustomerAgreementAt { get; set; }

        public string CustomerAgreementBy { get; set; }
    }
    public class ReoprtD6Data1
    {
        public int Number { get; set; }
        public string DescriptionOfAction { get; set; }
        public string Responsible { get; set; }
        public string Planned { get; set; }
        public string Introduce_On { get; set; }
        public string Effective_From { get; set; }
    }

    public class ReoprtD6Data2
    {
        public int Number { get; set; }
        public string ValidationOfAction { get; set; }
        public string Responsible { get; set; }
        public string Completed_On { get; set; }
    }

    public class ReoprtD6Data3
    {
        public int Number { get; set; }
        public string RemovalOfAction { get; set; }
        public string Responsible { get; set; }
        public string Removed_At { get; set; }
    }

    #endregion

    #region ReoprtD7Model
    public class ReoprtD7Model
    {
        public ReoprtD7Model()
        {
            D7DataList = new List<ReoprtD7Data>();
        }
        public List<ReoprtD7Data> D7DataList { get; set; }

        public bool IsRootCause { get; set; }

        public string IfYes { get; set; }

        public string IfNo { get; set; }
    }
    public class ReoprtD7Data
    {
        public int Number { get; set; }
        public string ImprovementOfQM { get; set; }
        public string Responsible { get; set; }
        public string Planed { get; set; }
        public string Completed_On { get; set; }
    }
    #endregion

    #region ReoprtD8Model
    public class ReoprtD8Model
    {
        public ReoprtD8Model()
        {
            D8DataList = new List<ReoprtD8Data>();
        }
        public List<ReoprtD8Data> D8DataList { get; set; }

        public string Participants { get; set; }

        public string AccomplishedAt { get; set; }

        public string Results { get; set; }

    }
    public class ReoprtD8Data
    {
        public int Number { get; set; }
        public string ReportD8Guid { get; set; }
        public string SponsorName { get; set; }
        public string SignatureDate { get; set; }
        public string Signature { get; set; }
    }
    #endregion

    #region ReportWorkFlow
    public class ReportWorkFlow
    {
        /// <summary>
        /// ID
        /// </summary>
        virtual public Guid EightD_WorkFlowGuid { get; set; }

        /// <summary>
        /// Team_Leader
        /// </summary>
        virtual public string Team_Leader { get; set; }

        /// <summary>
        /// Sponsor
        /// </summary>
        virtual public string Sponsor { get; set; }

        /// <summary>
        /// Additional_Approver
        /// </summary>
        virtual public string Additional_Approver { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        virtual public string Comments { get; set; }
    }
    #endregion
}