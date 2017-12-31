using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRole1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // initialize the account information
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // retrieve a reference to the messages queue
            var queueEvent = storageAccount.CreateCloudQueueClient();
            var queue = queueEvent.GetQueueReference("neweventqueue");

            queue.CreateIfNotExists(null);

            var msg = new CloudQueueMessage(TextBox1.Text);
            queue.AddMessage(msg);

            // retrieve number of messages queue
            var numOfMessagesInQueue = queue.ApproximateMessageCount;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // initialize the account information
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // retrieve a reference to the messages queue
            var queueEvent = storageAccount.CreateCloudQueueClient();
            var queue = queueEvent.GetQueueReference("joineventqueue");

            queue.CreateIfNotExists(null);

            var msg = new CloudQueueMessage(TextBox2.Text);
            queue.AddMessage(msg);

            // retrieve number of messages queue
            var numOfMessagesInQueue = queue.ApproximateMessageCount;
        }
    }
}