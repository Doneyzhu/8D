using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8DManagementSystem.DAL.DBHelper;

namespace _8DManagementSystem.DAL
{
    public class D_WorkFlow_DAL : DbSession
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.D_WorkFlow_Model GetModel(Guid id)
        {
            return new DbSession().NhSession.Get<Model.D_WorkFlow_Model>(id);
        }
    }
}
