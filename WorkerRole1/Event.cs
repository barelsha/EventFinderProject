using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkerRole1
{
    public class EventEntity: Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public string Name { get; set; }

        public EventEntity() { }

        public EventEntity(string eventName)
        {
            PartitionKey = "EVENT";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks -
                DateTime.Now.Ticks, Guid.NewGuid());
            Name = eventName;
        }
    }
}