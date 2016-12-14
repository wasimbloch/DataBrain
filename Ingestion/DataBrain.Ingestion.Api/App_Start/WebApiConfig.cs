using DataBrain.Ingestion.Api.Exceptions;
using DataBrain.Ingestion.Api.Handlers;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace DataBrain.Ingestion.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());

            config.Filters.Add(new LoggingExceptionFilterAttribute());
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new StandardResponseHeadersHandler());
        }
    }
}
