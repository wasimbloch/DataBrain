using DataBrain.Core.Extensions;
using DataBrain.Core.Logging;
using Newtonsoft.Json.Linq;
using NLog;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataBrain.Ingestion.Api.Processor
{
    public class IngestionApiEventSender : IEventSender
    {
        private Logger _log;
        private readonly string _requestUrl;

        public IngestionApiEventSender(string requestUrl)
        {
            _log = this.GetLogger();
            _requestUrl = requestUrl;
        }

        public async Task SendEventsAsync(JArray events, string deviceId)
        {
            _log.TraceEvent("SendEvents",
                new Facet("eventCount", events.Count));

            if (events.Count > 0)
            {
                using (var client = new HttpClient())
                {
                    var eventJson = events.ToString();
                    var requestContent = new StringContent(eventJson);
                    requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    await client.PostAsync(_requestUrl, requestContent);
                }
            }
        }
    }
}