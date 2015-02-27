using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8DManagementSystem.DAL.DBHelper;
using _8DManagementSystem.Model;

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
