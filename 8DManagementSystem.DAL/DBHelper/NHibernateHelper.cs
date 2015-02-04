using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace _8DManagementSystem.DAL.DBHelper
{
    public sealed class NHibernateHelper
    {
        #region ReadWriteDatabase

        /// <summary>
        /// 写入库 SessionFactory
        /// </summary>
        private static ISessionFactory m_SessionFactory;

        /// <summary>
        /// SessionFactory
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get { return m_SessionFactory; }
        }

        /// <summary>
        /// 创建 SessionFactory
        /// </summary>
        public static void CreateSessionFactory(string cfgFile)
        {
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure(cfgFile);

            // where the xml will be written
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                // Enable validation (optional)
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Validate = true;
                // Here, we serialize all decorated classes (but you can also do it class by class)
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Serialize(stream, Assembly.GetAssembly(typeof(Model.D_User_Model)));
                // Rewind
                stream.Position = 0;

                try
                {
                    System.IO.File.WriteAllBytes(cfgFile.Substring(0, cfgFile.LastIndexOf("\\") + 1) + "Mapping.cfg.xml", stream.GetBuffer());
                }
                catch (System.UnauthorizedAccessException) { }
                finally
                {
                    cfg.AddInputStream(stream); // Use the stream here
                }
            }

            m_SessionFactory = cfg.BuildSessionFactory();

            //m_SessionFactory = new NHibernate.Cfg.Configuration().Configure().BuildSessionFactory();
        }

        /// <summary>
        /// 打开数据库连接，返回 ISession 对象
        /// </summary>
        /// <returns></returns>
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// 获取当前 Session
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public static ITransaction BeginTransaction()
        {
            return GetCurrentSession().BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public static void CommitTransaction()
        {
            ITransaction tran = GetCurrentSession().Transaction;
            if (tran != null && !tran.WasCommitted)
            {
                tran.Commit();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public static void RollbackTransaction()
        {
            ITransaction tran = GetCurrentSession().Transaction;
            if (tran != null && !tran.WasCommitted && !tran.WasRolledBack)
            {
                tran.Rollback();
            }
        }

        /// <summary>
        /// 打开数据库连接并将ISession对象绑定到当前会话
        /// </summary>
        public static void BindSession()
        {
            if (!NHibernate.Context.CurrentSessionContext.HasBind(SessionFactory))
            {
                ISession sess = OpenSession();
                NHibernate.Context.CurrentSessionContext.Bind(sess);
            }
        }

        /// <summary>
        /// 解除ISession对象绑定并且关闭数据库连接
        /// </summary>
        public static void UnbindSession()
        {
            if (NHibernate.Context.CurrentSessionContext.HasBind(SessionFactory))
            {
                ISession sess = NHibernate.Context.CurrentSessionContext.Unbind(SessionFactory);
                sess.Close();
            }
        }

        #endregion

        #region ReadonlyDatabase

        /// <summary>
        /// 只读库 SessionFactory
        /// </summary>
        private static ISessionFactory m_ReadonlySessionFactory;

        /// <summary>
        /// ReadOnlySessionFactory
        /// </summary>
        public static ISessionFactory ReadonlySessionFactory
        {
            get { return m_ReadonlySessionFactory ?? m_SessionFactory; }
        }

        /// <summary>
        /// 创建 SessionFactory
        /// </summary>
        public static void CreateReadonlySessionFactory(string cfgFile)
        {
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure(cfgFile);

            // where the xml will be written
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                // Enable validation (optional)
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Validate = true;
                // Here, we serialize all decorated classes (but you can also do it class by class)
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Serialize(stream, Assembly.GetAssembly(typeof(Model.D_User_Model)));
                // Rewind
                stream.Position = 0;
                // Use the stream here
                cfg.AddInputStream(stream);
            }

            m_ReadonlySessionFactory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// 打开只读数据库连接，返回 ISession 对象
        /// </summary>
        /// <returns></returns>
        public static ISession OpenReadonlySession()
        {
            return ReadonlySessionFactory.OpenSession();
        }

        /// <summary>
        /// 获取当前只读 Session
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentReadonlySession()
        {
            return ReadonlySessionFactory.GetCurrentSession();
        }

        /// <summary>
        /// 打开只读数据库连接并将ISession对象绑定到当前会话
        /// </summary>
        public static void BindReadonlySession()
        {
            if (m_ReadonlySessionFactory != null && !NHibernate.Context.CurrentSessionContext.HasBind(ReadonlySessionFactory))
            {
                NHibernate.ISession sess = OpenReadonlySession();
                NHibernate.Context.CurrentSessionContext.Bind(sess);
            }
        }

        /// <summary>
        /// 解除ISession对象绑定并且关闭只读数据库连接
        /// </summary>
        public static void UnbindReadonlySession()
        {
            if (m_ReadonlySessionFactory != null && NHibernate.Context.CurrentSessionContext.HasBind(ReadonlySessionFactory))
            {
                NHibernate.ISession sess = NHibernate.Context.CurrentSessionContext.Unbind(ReadonlySessionFactory);
                sess.Close();
            }
        }

        #endregion
    }

    /// <summary>
    /// 当前Session上下文模式
    /// 特定环境下，当前Session保存在特定的上下文中
    /// </summary>
    public enum CurrentSessionContextClass
    {
        managed_web,
        call,
        thread_static,
        web
    }
}
