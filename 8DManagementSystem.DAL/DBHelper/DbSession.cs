using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace _8DManagementSystem.DAL.DBHelper
{
    public class DbSession
    {
        /// <summary>
        /// 可写库连接
        /// </summary>
        private ISession _mSession = null;
        public ISession NhSession
        {
            get
            {
                if (_mSession == null)
                {
                    _mSession = NHibernateHelper.GetCurrentSession();
                }
                return _mSession;
            }
        }

        /// <summary>
        /// 只读库连接
        /// </summary>
        private ISession _readNhSession = null;

        public ISession ReadNhSession
        {
            get
            {
                if (_readNhSession == null)
                {
                    _readNhSession = NHibernateHelper.GetCurrentReadonlySession();
                }
                return _readNhSession;
            }
        }
    }
}
