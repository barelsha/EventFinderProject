using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole4
{
    class UserEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public string DeviceID { get; set; }

        public UserEntity() { }

        public UserEntity(string phoneID)
        {
            PartitionKey = "registeredDevices";
            RowKey = phoneID;
            DeviceID = phoneID;
        }
    }
}