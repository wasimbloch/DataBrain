﻿using DataBrain.Core;
using DataBrain.Core.Extensions;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataBrain.Ingestion.Api.Processor.EventHubs
{
    public class EventBatchIterator : IEnumerator<IEnumerable<EventData>>, IEnumerable<IEnumerable<EventData>>
    {
        public static int MaxBatchSizeBytes { get; private set; }
        public static int EventDataOverheadBytes { get; private set; }

        private readonly JArray _allEvents;
        private int _lastBatchedEventIndex;
        private IEnumerable<EventData> _currentBatch;

        static EventBatchIterator()
        {
            MaxBatchSizeBytes = Config.Parse<int>("DataBrain.EventHubs.MaxMessageSizeBytes");
            EventDataOverheadBytes = Config.Parse<int>("DataBrain.EventHubs.EventDataOverheadBytes");
        }

        public EventBatchIterator(JArray events)
        {
            _allEvents = events;
        }

        public IEnumerable<EventData> Current
        {
            get { return _currentBatch; }
        }

        public bool MoveNext()
        {
            var batch = new List<EventData>(_allEvents.Count);
            var batchSize = 0;
            for (int i = _lastBatchedEventIndex; i < _allEvents.Count; i++)
            {
                dynamic evt = _allEvents[i];
                evt.receivedAt = DateTime.UtcNow.ToUnixMillseconds();
                int payloadSize = 0;
                var eventData = EventDataTransform.ToEventData(evt, out payloadSize);
                var eventSize = payloadSize + EventDataOverheadBytes;
                if (batchSize + eventSize > MaxBatchSizeBytes)
                {
                    break;
                }

                batch.Add(eventData);
                batchSize += eventSize;
            }
            _lastBatchedEventIndex += batch.Count();
            _currentBatch = batch;
            return _currentBatch.Count() > 0;
        }

        public void Reset()
        {
            _lastBatchedEventIndex = 0;
        }

        #region Boilerplate IEnumerable & IEnumerator implementation

        public void Dispose()
        {
            //nothing to do 
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        IEnumerator<IEnumerable<EventData>> IEnumerable<IEnumerable<EventData>>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion
    }
}