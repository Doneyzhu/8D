using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8DManagementSystem.DAL.DBHelper;
using _8DManagementSystem.Model;
using NHibernate;

namespace _8DManagementSystem.DAL
{
    public class D_User_DAL : DbSession
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static D_User_Model GetD_User_Model(Guid id)
        {
            return new DbSession().NhSession.Get<D_User_Model>(id);
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public IList<D_User_Model> GetAll()
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_User_Model));
            IList<D_User_Model> list = ic.List<D_User_Model>() ?? new List<D_User_Model>();
            return list;
        }


        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool Save(D_User_Model model)
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
