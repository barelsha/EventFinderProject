using System;
using System.Collections;
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

namespace WorkerRole4
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
            var NewEventQueue = queueClient.GetQueueReference("neweventqueue");
            NewEventQueue.CreateIfNotExists();
            // retrieve messages and write them to the development fabric log
            while (true)
            {
                Thread.Sleep(10000);

                if (NewEventQueue.Exists())
                {
                    Trace.TraceInformation(string.Format("queue size'{0}' .", NewEventQueue.ApproximateMessageCount));

                    var msg = NewEventQueue.GetMessage();
                    if (msg != null)
                    {
                        Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));
                        NotifyUsers(msg.AsString, storageAccount);
                        Console.Write(msg.AsString);
                        NewEventQueue.DeleteMessage(msg);
                    }
                }
            }
        }

        private void NotifyUsers(string eventID, CloudStorageAccount storageAccount)
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionStringNotification, hubName);
            WnsHeaderCollection wnsHeaderCollection = new WnsHeaderCollection();
            wnsHeaderCollection.Add("X-WNS-Type", @"wns/raw");

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("users");
            table.CreateIfNotExists();
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Users"));

            List<string> usersTag = new List<string>();
            foreach (UserEntity entity in table.ExecuteQuery(query))
            {
                usersTag.Add(entity.PhoneID);
            }
            var notif = "{ \"data\" : {\"message\":\"" + "EventId " + eventID + "\"}}";
            hub.SendGcmNativeNotificationAsync(notif, usersTag);

            //var notif = "{ \"data\" : {\"message\":\"" + "Hola" + "\"}}";
            //hub.SendGcmNativeNotificationAsync(notif);
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
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
