using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8DManagementSystem.DAL.DBHelper;
using _8DManagementSystem.Model;
using NHibernate;
using NHibernate.Criterion;

namespace _8DManagementSystem.DAL
{
    public class D_Report_DAL : DbSession
    {
        /// <summary>
        /// 获取报表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public D_Report_Model GetModel(Guid id)
        {
            return new DbSession().NhSession.Get<D_Report_Model>(id);
        }

        /// <summary>
        /// 保存报表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool Save(D_Report_Model model)
        {
            NHibernate.ITransaction tran = NhSession.BeginTransaction();
            try
            {
                NhSession.SaveOrUpdate(model.ReportD2);
                NhSession.SaveOrUpdate(model);
                if (model.ReportD8DataModels.Count > 0)
                {
                    foreach (var item in model.ReportD8DataModels)
                    {
                        NhSession.SaveOrUpdate(item);
                    }
                }
                if (model.WorkFlow_Models.Count > 0)
                {
                    foreach (var item in model.WorkFlow_Models)
                    {
                        NhSession.SaveOrUpdate(item);
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!tran.WasCommitted && !tran.WasRolledBack)
                    tran.Rollback();
            }
        }

        /// <summary>
        /// 分页获取报表列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowCount"></param>
        /// <param name="roleName"></param>
        /// <param name="createDate"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<D_Report_Model> GetAllByPage(int startCount, int rowCount, List<Model.D_Board_Model> boardModel, string reportNo, string createDate, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Report_Model));
            if (boardModel != null)
            {
                if (boardModel.Count > 0)
                {
                    ic.Add(Restrictions.In("ReportBoardGuid", boardModel));
                }
                else
                {
                    ic.Add(Restrictions.IsNull("ReportBoardGuid"));
                }
            }
            else
            {
                ic.Add(Restrictions.IsNull("ReportBoardGuid"));
            }

            if (!string.IsNullOrEmpty(reportNo))
                ic.Add(Restrictions.Like("ReportNo", reportNo, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(createDate))
                ic.Add(Restrictions.Le("CreateDateTime", createDate));

            ic.Add(Restrictions.Eq("DataStatus", false));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            ic.AddOrder(Order.Desc("ModifyDateTime"));
            IList<D_Report_Model> list = ic.SetFirstResult(startCount).SetMaxResults(rowCount).List<D_Report_Model>();

            return list;
        }



        public bool SaveWorkFlowLog(D_Report_Model model, D_WorkFlowLog_Model logModel)
        {
            NHibernate.ITransaction tran = NhSession.BeginTransaction();
            try
            {

                logModel.ReportGuid = model;

                NhSession.SaveOrUpdate(model.ReportD2);
                NhSession.SaveOrUpdate(model);
                if (model.ReportD8DataModels.Count > 0)
                {
                    foreach (var item in model.ReportD8DataModels)
                    {
                        NhSession.SaveOrUpdate(item);
                    }
                }
                if (model.WorkFlow_Models.Count > 0)
                {
                    foreach (var item in model.WorkFlow_Models)
                    {
                        NhSession.SaveOrUpdate(item);
                    }
                }
                if (model.WorkFlowLog_Models.Count > 0)
                {
                    foreach (var item in model.WorkFlowLog_Models)
                    {
                        NhSession.SaveOrUpdate(item);
                    }
                }

                NhSession.SaveOrUpdate(model);

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!tran.WasCommitted && !tran.WasRolledBack)
                    tran.Rollback();
            }
        }
    }
}
