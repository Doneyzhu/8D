using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8DManagementSystem.DAL.DBHelper;
using _8DManagementSystem.Model;
using NHibernate;
using NHibernate.Criterion;
using System.Security.Cryptography;

namespace _8DManagementSystem.DAL
{
    public class D_User_DAL : DbSession
    {

        public string PassWordMD5(string pwd)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(pwd));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();

        }

        public D_User_Model GetUserByUserLoginName(string userLoginName)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_User_Model));
            ic.Add(Restrictions.Eq("UserLoginName", userLoginName));
            ic.Add(Restrictions.Eq("DataStatus", false));
            D_User_Model model = ic.UniqueResult<D_User_Model>();
            return model;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(D_User_Model model)
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


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public D_User_Model GetModel(Guid id)
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

        public IList<D_User_Model> GetAllByPage(int page, int rowCount, string userName, string userCode, string departmentName,
            string userLoginName, string createDate, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_User_Model));
            if (!string.IsNullOrEmpty(userName))
                ic.Add(Restrictions.Like("UserName", userName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(userCode))
                ic.Add(Restrictions.Like("UserCode", userCode, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(userLoginName))
                ic.Add(Restrictions.Like("UserLoginName", userLoginName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(departmentName))
                ic.Add(Restrictions.Like("DepartmentName", departmentName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(createDate))
                ic.Add(Restrictions.Le("CreateDateTime", createDate));

            ic.Add(Restrictions.Eq("DataStatus", false));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            IList<D_User_Model> list = ic.SetFirstResult(page * rowCount).SetMaxResults(rowCount).List<D_User_Model>();

            return list;
        }



    }
}
