using DataBrain.Ingestion.Api.Handlers;
using System.Web.Http;
using System.Web.Routing;

namespace DataBrain.Ingestion.Api.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "device-events",
                routeTemplate: "events",
                defaults: new { controller = "Events" },
                handler: new GZipToJsonHandler(GlobalConfiguration.Configuration),
                constraints: null);
        }
    }
}