﻿using DataBrain.Core.Extensions;
using DataBrain.Core.Logging;
using Newtonsoft.Json;
using NLog;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DataBrain.Ingestion.Api.Exceptions
{
    public class LoggingExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly Logger _log;
        public LoggingExceptionFilterAttribute()
        {
            _log = this.GetLogger();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            object requestId;
            context.Request.Properties.TryGetValue("requestId", out requestId);
            var errorId = _log.ErrorEvent(
                    "OnException",
                    context.Exception,
                    new Facet { Name = "RequestId", Value = requestId });

            var error = new { errorId = errorId };
            var content = JsonConvert.SerializeObject(error);

            context.Response =
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Server error.",
                        Content = new StringContent(content),
                        StatusCode = HttpStatusCode.InternalServerError,
                    };
        }
    }
}