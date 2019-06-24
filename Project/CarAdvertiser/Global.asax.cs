using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CarAdvertiser
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["CarAdvertiser"].ConnectionString;

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;//for security reasons

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlDependency.Start(_connString);
        }

        protected void Application_End()
        {
            SqlDependency.Stop(_connString);
        }
    }
}
