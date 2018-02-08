using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.NotificationHubs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRole6
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private string connectionStringNotification = "Endpoint=sb://notificationhubfnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=r3YQI3oM7rqlnptfnLO/kfuCpCv7v30GTjpzYr0AdAk=";
        private string hubName = "notificationhubfirebase";
        public override void Run()
        {
            // initialize the account information
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // retrieve a reference to the messages queue
            var queueClient = storageAccount.CreateCloudQueueClient();
            var JoinEventQueue = queueClient.GetQueueReference("joineventqueue");
            JoinEventQueue.CreateIfNotExists();
            // retrieve messages and write them to the development fabric log
            while (true)
            {
                Thread.Sleep(10000);

                if (JoinEventQueue.Exists())
                {
                    Trace.TraceInformation(string.Format("queue size'{0}' .", JoinEventQueue.ApproximateMessageCount));
                    var msg = JoinEventQueue.GetMessage();
                    if (msg != null)
                    {
                        Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));
                        //NotifyManager(msg.ToString(), storageAccount);
                        Console.Write(msg);
                        JoinEventQueue.DeleteMessage(msg);
                    }
                }
            }
        }

        private void NotifyManager(string msg, CloudStorageAccount storageAccount)
        {
            string[] message = msg.Split(',');
            string eventID = message[0];
            string userID = message[1];
            eventfinderEntities model = new eventfinderEntities();
            Event eventEntity = model.Events.First(e => e.ID == Int32.Parse(eventID));
            string managerID = eventEntity.User.ID.ToString();
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionStringNotification, hubName);
            WnsHeaderCollection wnsHeaderCollection = new WnsHeaderCollection();
            wnsHeaderCollection.Add("X-WNS-Type", @"wns/raw");
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("users");
            table.CreateIfNotExists();
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,"Users" )).Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, managerID));
            UserEntity manager = table.ExecuteQuery(query).First();
            var notif = "{ \"data\" : {\"message\":\"" + "EventId " + eventID + " UserID "+ userID+"\"}}";
            hub.SendGcmNativeNotificationAsync(notif, manager.PhoneID);
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole3 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole3 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole3 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
