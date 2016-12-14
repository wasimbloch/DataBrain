using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataBrain.Core.Logging
{
    [Target("AzureAppendBlob")]
    public sealed class AzureAppendBlobTarget : TargetWithLayout
    {
        private Dictionary<Guid, string> _buffer = new Dictionary<Guid, string>();
        private static object _SyncLock = new object();

        [RequiredParameter]
        public string ConnectionString { get; set; }

        [RequiredParameter]
        public Layout Container { get; set; }

        [RequiredParameter]
        public Layout BlobName { get; set; }

        private CloudBlobClient _client;
        private CloudBlobContainer _container;
        private CloudAppendBlob _blob;

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            _client = CloudStorageAccount.Parse(ConnectionString)
                                         .CreateCloudBlobClient();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (_client == null)
            {
                return;
            }

            string containerName = Container.Render(logEvent);
            string blobName = BlobName.Render(logEvent);
            
            if (_container == null || _container.Name != containerName)
            {
                _container = _client.GetContainerReference(containerName);
                _blob = null;
            }

            if (_blob == null || _blob.Name != blobName)
            {
                _blob = _container.GetAppendBlobReference(blobName);

                if (!_blob.Exists())
                {
                    try
                    {
                        _blob.Properties.ContentType = "text/plain";
                        _blob.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition(), null, null);
                    }
                    catch (StorageException)
                    { }
                }
            }
            var logMessage = this.Layout.Render(logEvent);
            _blob.AppendText(logMessage + "\r\n", Encoding.UTF8);
            
        }
    }
}
