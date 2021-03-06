﻿using DataBrain.Core.Extensions;
using DataBrain.Core.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog;
using System;
using System.Timers;

namespace DataBrain.Core.BlobStorage
{
    public class RenewingBlobLease : IDisposable
    {
        private readonly Logger _log;
        private readonly CloudBlockBlob _blob;
        private readonly System.Timers.Timer _timer;

        public string Id { get; private set; }

        public RenewingBlobLease(CloudBlockBlob blob)
        {
            _log = this.GetLogger();
            _blob = blob;
            Id = _blob.AcquireLease(TimeSpan.FromSeconds(40), null);
            _log.DebugEvent("AcquiredLease", new Facet("blobUri", _blob.Uri), new Facet("leaseId", Id));

            _timer = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
            _timer.Elapsed += RenewLease;
            _timer.Start();
        }

        private void RenewLease(object sender, ElapsedEventArgs e)
        {
            _blob.ReleaseLease(AccessCondition.GenerateLeaseCondition(Id));
            _log.DebugEvent("RenewedLease", new Facet("blobUri", _blob.Uri), new Facet("leaseId", Id));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }

                try
                {
                    _blob.ReleaseLease(AccessCondition.GenerateLeaseCondition(Id));
                    _log.DebugEvent("ReleaseLease", new Facet("blobUri", _blob.Uri), new Facet("leaseId", Id));
                }
                catch (Exception ex)
                {
                    _log.ErrorEvent("Disposed", ex, new Facet("blobUri", _blob.Uri), new Facet("leaseId", Id));
                }
            }
        }
    }
}
