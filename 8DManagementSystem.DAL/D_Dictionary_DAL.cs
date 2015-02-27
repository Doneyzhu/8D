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
    public class D_Dictionary_DAL : DbSession
    {
        /// <summary>
        /// 获取字典值通过数据字典枚举
        /// </summary>
        /// <returns></returns>
        public IList<D_Dictionary_Model> GetDictionaryByDicType(DicTypeEnum dicType)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Dictionary_Model));

            ic.Add(Restrictions.Eq("DicType", dicType));

            ic.Add(Restrictions.Eq("DataStatus", false));

            IList<D_Dictionary_Model> list = ic.List<D_Dictionary_Model>() ?? new List<D_Dictionary_Model>();
            return list;
        }

        /// <summary>
        /// 分页获取数据字典
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowCount"></param>
        /// <param name="roleName"></param>
        /// <param name="createDate"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<D_Dictionary_Model> GetAllByPage(int startCount, int rowCount, DicTypeEnum? dicType, string dicName, string createDate, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Dictionary_Model));
            if (!string.IsNullOrEmpty(dicName))
                ic.Add(Restrictions.Like("DicName", dicName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(createDate))
                ic.Add(Restrictions.Le("CreateDateTime", createDate));
            if (dicType.HasValue)
                ic.Add(Restrictions.Eq("DicType", dicType));

            ic.Add(Restrictions.Eq("DataStatus", false));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            ic.AddOrder(new Order("Ord", false));

            IList<D_Dictionary_Model> list = ic.SetFirstResult(startCount).SetMaxResults(rowCount).List<D_Dictionary_Model>();

            return list;
        }

        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public D_Dictionary_Model GetModel(Guid id)
        {
            return new DbSession().NhSession.Get<D_Dictionary_Model>(id);
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(D_Dictionary_Model model)
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
        /// 保存数据字典信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool Save(D_Dictionary_Model model)
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
    }
}
