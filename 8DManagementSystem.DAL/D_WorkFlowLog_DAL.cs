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
    public class D_WorkFlowLog_DAL : DbSession
    {
        public IList<D_WorkFlowLog_Model> GetAllByPage(int startCount, int rowCount, D_Report_Model reportModel, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_WorkFlowLog_Model));

            ic.Add(Restrictions.Eq("ReportGuid", reportModel));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            IList<D_WorkFlowLog_Model> list = ic.SetFirstResult(startCount).SetMaxResults(rowCount).List<D_WorkFlowLog_Model>();

            return list;
        }
    }
}
