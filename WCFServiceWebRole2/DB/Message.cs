using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFServiceWebRole2.DB
{
    public class Message : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public string Body { get; set; }

        public Message() { }

        public Message(string body, string partitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks -
                DateTime.Now.Ticks, Guid.NewGuid());
            Body = body;
        }
    }
}