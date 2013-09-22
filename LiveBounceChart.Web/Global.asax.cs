using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LiveBounceChart.Web.DAL;
using LiveBounceChart.Web.Infra;
using Unity.Mvc4;
using SignalR = Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;

namespace LiveBounceChart.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var container = CreateContainer();

            System.Web.Mvc.DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            SignalR.GlobalHost.DependencyResolver = new SignalRUnityDependencyResolver(container);

            RouteTable.Routes.MapHubs();
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private static IUnityContainer CreateContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IBounceDB, BounceDBContext>();
            container.RegisterType<LobbyHub, LobbyHub>();


            return container;
        }
    }
}