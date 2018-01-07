using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        public string Event(string newEvent)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));// retrieve a reference to the messages queue
            var queueEvent = storageAccount.CreateCloudQueueClient();
            var queue = queueEvent.GetQueueReference("neweventqueue");
            queue.CreateIfNotExists(null);
            var msg = new CloudQueueMessage(newEvent);
            queue.AddMessage(msg);

            //// initialize the account information
            //var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            //// retrieve a reference to the messages queue
            //var queueEvent = storageAccount.CreateCloudQueueClient();
            //var queue = queueEvent.GetQueueReference("joineventqueue");

            //queue.CreateIfNotExists(null);

            //var msg = new CloudQueueMessage(TextBox2.Text);
            //queue.AddMessage(msg);

            //// retrieve number of messages queue
            //var numOfMessagesInQueue = queue.ApproximateMessageCount;


            return newEvent;
        }


        public Event GetEvent(string id)
        {
            //eventfinderEntities ent = new eventfinderEntities();
            //ent.Events.Include
            Event even = new Event()
            {
                ID = 1,
                Name = "shahar",
                StartTime = new DateTime(),
                EndTime = new DateTime(),
                UserID = 1,
                Description = "shahar",
                User = new User()
            };

            return even;
        }

        public bool GetEvents()
        {
            return true;
        }
    }
}
