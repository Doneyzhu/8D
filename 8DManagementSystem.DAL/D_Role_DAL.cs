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
    public class D_Role_DAL : DbSession
    {
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(D_Role_Model model)
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
        /// 保存角色信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool Save(D_Role_Model model)
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
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public D_Role_Model GetModel(Guid id)
        {
            return new DbSession().NhSession.Get<D_Role_Model>(id);
        }

        /// <summary>
        /// 获取所有用户角色信息
        /// </summary>
        /// <returns></returns>
        public IList<D_Role_Model> GetAll()
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Role_Model));
            IList<D_Role_Model> list = ic.List<D_Role_Model>() ?? new List<D_Role_Model>();
            return list;
        }

        /// <summary>
        /// 分页获取角色
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowCount"></param>
        /// <param name="roleName"></param>
        /// <param name="createDate"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<D_Role_Model> GetAllByPage(int startCount, int rowCount, string roleName, string createDate, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Role_Model));
            if (!string.IsNullOrEmpty(roleName))
                ic.Add(Restrictions.Like("RoleName", roleName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(createDate))
                ic.Add(Restrictions.Le("CreateDateTime", createDate));

            ic.Add(Restrictions.Eq("DataStatus", false));
            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            ic.AddOrder(Order.Desc("ModifyDateTime"));
            IList<D_Role_Model> list = ic.SetFirstResult(startCount).SetMaxResults(rowCount).List<D_Role_Model>();

            return list;
        }


    }
}
