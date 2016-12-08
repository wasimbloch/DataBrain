﻿using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Text;

namespace DataBrain.Ingestion.Api.Processor.EventHubs
{
    public static class EventDataTransform
    {
        public static EventData ToEventData(dynamic eventObject, out int payloadSize)
        {
            var json = eventObject.ToString(Formatting.None);
            payloadSize = Encoding.UTF8.GetByteCount(json);
            var payload = Encoding.UTF8.GetBytes(json);
            var eventData = new EventData(payload)
            {
                PartitionKey = (string)eventObject.deviceId
            };
            eventData.SetEventName((string)eventObject.eventName);
            eventData.SetReceivedAt((long)eventObject.receivedAt);
            return eventData;
        }
    }
}