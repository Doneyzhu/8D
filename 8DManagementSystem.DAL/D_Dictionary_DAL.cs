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
        public IList<D_Dictionary_Model> GetAllByPage(int page, int rowCount, DicTypeEnum dicType, string dicName, string createDate, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Dictionary_Model));
            if (!string.IsNullOrEmpty(dicName))
                ic.Add(Restrictions.Like("DicName", dicName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(createDate))
                ic.Add(Restrictions.Le("CreateDateTime", createDate));

            ic.Add(Restrictions.Eq("DicType", dicType));

            ic.Add(Restrictions.Eq("DataStatus", false));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            IList<D_Dictionary_Model> list = ic.SetFirstResult(page * rowCount).SetMaxResults(rowCount).List<D_Dictionary_Model>();

            return list;
        }
    }
}
