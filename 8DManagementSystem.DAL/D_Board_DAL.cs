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
    public class D_Board_DAL : DbSession
    {
        /// <summary>
        /// 删除板块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(D_Board_Model model)
        {
            NHibernate.ITransaction tran = NhSession.BeginTransaction();
            try
            {
                NhSession.Delete(model);
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
        /// 保存板块信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool Save(D_Board_Model model)
        {
            NHibernate.ITransaction tran = NhSession.BeginTransaction();
            try
            {
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


        /// <summary>
        /// 获取板块信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public D_Board_Model GetModel(Guid id)
        {
            return new DbSession().NhSession.Get<D_Board_Model>(id);
        }

        /// <summary>
        /// 获取所有板块信息
        /// </summary>
        /// <returns></returns>
        public IList<D_Board_Model> GetAll()
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Board_Model));
            IList<D_Board_Model> list = ic.List<D_Board_Model>() ?? new List<D_Board_Model>();
            return list;
        }

        /// <summary>
        /// 分页获取板块
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowCount"></param>
        /// <param name="roleName"></param>
        /// <param name="createDate"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<D_Board_Model> GetAllByPage(int page, int rowCount, string boardName, string createDate, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Board_Model));
            if (!string.IsNullOrEmpty(boardName))
                ic.Add(Restrictions.Like("BoardName", boardName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(createDate))
                ic.Add(Restrictions.Le("CreateDateTime", createDate));

            ic.Add(Restrictions.Eq("DataStatus", false));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            IList<D_Board_Model> list = ic.SetFirstResult(page * rowCount).SetMaxResults(rowCount).List<D_Board_Model>();

            return list;
        }

    }
}
