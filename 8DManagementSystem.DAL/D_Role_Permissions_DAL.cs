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
    public class D_Role_Permissions_DAL : DbSession
    {
        /// <summary>
        /// 获取所有用户角色权限
        /// </summary>
        /// <returns></returns>
        public IList<D_Role_Permissions_Model> GetRole_Permissions_ByRole(D_Role_Model role)
        {
            ICriteria ic = NhSession.CreateCriteria(typeof(D_Role_Permissions_Model));
            ic.Add(Restrictions.Eq("RoleGuid", role));
            ic.Add(Restrictions.Eq("DataStatus", false));
            IList<D_Role_Permissions_Model> list = ic.List<D_Role_Permissions_Model>() ?? new List<D_Role_Permissions_Model>();
            return list;
        }
    }
}
