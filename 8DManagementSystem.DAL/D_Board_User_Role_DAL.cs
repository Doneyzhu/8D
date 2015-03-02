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
    public class D_Board_User_Role_DAL : DbSession
    {

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public D_Board_User_Role_Model GetModel(Guid id)
        {
            return new DbSession().NhSession.Get<D_Board_User_Role_Model>(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(D_Board_User_Role_Model model)
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
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool SaveList(List<D_Board_User_Role_Model> models)
        {
            NHibernate.ITransaction tran = NhSession.BeginTransaction();
            try
            {
                foreach (var item in models)
                {
                    NhSession.SaveOrUpdate(item);
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


        public D_Board_User_Role_Model GetOne(D_Board_Model boardModel, D_User_Model userModel, D_Role_Model roleModel)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Board_User_Role_Model));
            ic.Add(Restrictions.Eq("BoardGuid", boardModel));
            ic.Add(Restrictions.Eq("UserGuid", userModel));
            ic.Add(Restrictions.Eq("RoleGuid", roleModel));
            return ic.List<D_Board_User_Role_Model>().SingleOrDefault();
            //IList<D_Board_User_Role_Model> list = ic.List<D_Board_User_Role_Model>();
            //return list.FirstOrDefault<D_Board_User_Role_Model>();
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
        public IList<D_Board_User_Role_Model> GetAllByPage(int startCount, int rowCount, Model.D_Board_Model boardModel, out int totalCount)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Board_User_Role_Model));

            ic.Add(Restrictions.Eq("BoardGuid", boardModel));

            //if (!string.IsNullOrEmpty(createDate))
            //    ic.Add(Restrictions.Le("CreateDateTime", createDate));

            ICriteria pageCrit = CriteriaTransformer.Clone(ic);

            totalCount = Convert.ToInt32(pageCrit.SetProjection(Projections.RowCount()).UniqueResult());

            ic.AddOrder(Order.Desc("CreateDateTime"));

            IList<D_Board_User_Role_Model> list = ic.SetFirstResult(startCount).SetMaxResults(rowCount).List<D_Board_User_Role_Model>();

            return list;
        }
    }
}
