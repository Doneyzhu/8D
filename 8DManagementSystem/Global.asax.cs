using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using _8DManagementSystem.DAL.DBHelper;

namespace _8DManagementSystem
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            string cfgFile = Server.MapPath(@"~\App_Data\DataAccess\MSSQL.cfg.xml");
            NHibernateHelper.CreateSessionFactory(cfgFile);

            string useReadonlyDatabase = ConfigurationManager.AppSettings["UseReadonlyDatabase"];
            if (useReadonlyDatabase == "true")
            {
                string cfgReadonlyFile = Server.MapPath(@"~\App_Data\DataAccess\MSSQL_Read.cfg.xml");
                NHibernateHelper.CreateReadonlySessionFactory(cfgReadonlyFile);
            }


            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}