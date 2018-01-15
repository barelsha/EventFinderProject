using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFServiceWebRole2.DB
{
    class UserEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public string PhoneID { get; set; }

        public UserEntity() { }

        public UserEntity(string phoneID, string userID)
        {
            PartitionKey = "Users";
            RowKey = userID;
            PhoneID = phoneID;
        }
    }
}