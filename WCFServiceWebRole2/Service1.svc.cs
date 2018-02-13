using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WCFServiceWebRole2.DB;
using System.Web.Http.Cors;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        
        public ResponseObject<QuickEvent> AddEvent(QuickEvent newEvent)
        {
            dynamic response = new ResponseObject<QuickEvent>();
            try
            {
                DateTime start = Convert.ToDateTime(newEvent.StartTime);
                DateTime end = Convert.ToDateTime(newEvent.EndTime);
                double latitudePoint = Convert.ToDouble(newEvent.Latitude);
                double longitudePoint = Convert.ToDouble(newEvent.Longtitude);
                eventfinderEntitiesModel ent = new eventfinderEntitiesModel();
                User userEntity = ent.Users.First(u => u.ID == newEvent.UserID);
                Event eventEntity = new Event()
                {
                    Name = newEvent.Name,
                    StartTime = start,
                    EndTime = end,
                    UserID = newEvent.UserID,
                    Latitude = latitudePoint,
                    Longitude = longitudePoint,
                    Description = newEvent.Description,
                    User = userEntity,
                    Type = newEvent.Type
                };
                ent.Events.Add(eventEntity);
                ent.SaveChanges();
                int eventID = eventEntity.ID;
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));// retrieve a reference to the messages queue
                var queueEvent = storageAccount.CreateCloudQueueClient();
                var queue = queueEvent.GetQueueReference("neweventqueue");
                queue.CreateIfNotExists(null);
                var msg = new CloudQueueMessage(eventID.ToString());
                queue.AddMessage(msg);
                //SaveImageInBlob(newEvent.ID, new System.IO.MemoryStream());
                response.data = newEvent;
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on AddEvent event name={0}", newEvent.Name);
            }
            return response;
        }

        public ResponseObject<QuickEvent> GetEvent(string id)
        {
            dynamic response = new ResponseObject<QuickEvent>();
            try
            {
                int eventID = Int32.Parse(id);
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                Event eventEntity = model.Events.First(e => e.ID == eventID);

                QuickEvent quickEvent = new QuickEvent()
                {
                    ID = eventEntity.ID,
                    UserID = eventEntity.UserID,
                    Name = eventEntity.Name,
                    Description = eventEntity.Description,
                    StartTime = eventEntity.StartTime.ToString(),
                    EndTime = eventEntity.EndTime.ToString(),
                    Latitude = eventEntity.Latitude,
                    Longtitude = eventEntity.Longitude,
                    Type = eventEntity.Type
                };

                response.data = quickEvent;
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on GetEvent eventID={0}", id);
            }
            return response;
        }

        public ResponseObject<List<QuickEvent>> GetEvents()
        {
            dynamic response = new ResponseObject<List<QuickEvent>>();
            try
            {
                List<QuickEvent> events = new List<QuickEvent>();
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                ICollection<Event> eventEntity = model.Events.ToList();
                foreach (var e in eventEntity)
                {
                    events.Add(new QuickEvent()
                    {
                        ID = e.ID,
                        UserID = e.UserID,
                        Name = e.Name,
                        Description = e.Description,
                        StartTime = e.StartTime.ToString(),
                        EndTime = e.EndTime.ToString(),
                        Latitude = e.Latitude,
                        Longtitude = e.Longitude,
                        Type = e.Type
                    });
                }
                response.data = events;
                response.success = true;

            }
            catch (Exception)
            {
                response.success = false;
                response.message = "error on GetEvents";
            }
            return response;
        }

        public ResponseObject<bool> JoinEvent(string eventID, string userID)
        {
            dynamic response = new ResponseObject<bool>();
            try
            {
                int user = Int32.Parse(userID);
                int eventId = Int32.Parse(eventID);
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                Event eventEntity = model.Events.First(e => e.ID == eventId);
                User userEntity = model.Users.First(u => u.ID == user);
                eventEntity.Users.Add(userEntity);
                model.SaveChanges();
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                var queueEvent = storageAccount.CreateCloudQueueClient();
                var queue = queueEvent.GetQueueReference("joineventqueue");
                queue.CreateIfNotExists(null);
                string message = eventEntity.ID + ',' + userID;
                var msg = new CloudQueueMessage(message);
                queue.AddMessage(msg);
                response.success = true;
                response.data = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message =string.Format("error on JoinEvent eventID={0} userID={1}", eventID,userID);
            }
            return response;
        }

        public ResponseObject<List<QuickEvent>> GetUserEvents(string userID)
        {
            dynamic response = new ResponseObject<List<QuickEvent>>();
            try
            {
                List<QuickEvent> events = new List<QuickEvent>();
                int user = Int32.Parse(userID);
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                User userEntity = model.Users.First(u => u.ID == user);
                ICollection<Event> eventEntity = userEntity.Events.ToList();
                foreach (var e in eventEntity)
                {
                    events.Add(new QuickEvent()
                    {
                        ID = e.ID,
                        UserID = e.UserID,
                        Name = e.Name,
                        Description = e.Description,
                        StartTime = e.StartTime.ToString(),
                        EndTime = e.EndTime.ToString(),
                        Latitude = e.Latitude,
                        Longtitude = e.Longitude,
                        Type = e.Type
                    });
                }
                response.data = events;
                response.success = true;

            }
            catch (Exception)
            {
                response.success = false;
                response.message = "error on GetEvents";
            }
            return response;
        }

        public ResponseObject<List<QuickEvent>> GetUserEvents1(string userID)
        {
            dynamic response = new ResponseObject<List<QuickEvent>>();
            try
            {
                List<QuickEvent> events = new List<QuickEvent>();
                int user = Int32.Parse(userID);
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                User userEntity = model.Users.First(u => u.ID == user);
                ICollection<Event> eventEntity = userEntity.Events1.ToList();
                foreach (var e in eventEntity)
                {
                    events.Add(new QuickEvent()
                    {
                        ID = e.ID,
                        UserID = e.UserID,
                        Name = e.Name,
                        Description = e.Description,
                        StartTime = e.StartTime.ToString(),
                        EndTime = e.EndTime.ToString(),
                        Latitude = e.Latitude,
                        Longtitude = e.Longitude,
                        Type = e.Type
                    });
                }
                response.data = events;
                response.success = true;

            }
            catch (Exception)
            {
                response.success = false;
                response.message = "error on GetEvents";
            }
            return response;
        }

        public ResponseObject<List<QuickEvent>> GetAllUserEvents(string userID)
        {
            dynamic response = new ResponseObject<List<QuickEvent>>();
            try
            {
                List<QuickEvent> events = new List<QuickEvent>();
                int user = Int32.Parse(userID);
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                User userEntity = model.Users.First(u => u.ID == user);
                ICollection<Event> eventEntity = userEntity.Events1.ToList();
                foreach (var e in eventEntity)
                {
                    events.Add(new QuickEvent()
                    {
                        ID = e.ID,
                        UserID = e.UserID,
                        Name = e.Name,
                        Description = e.Description,
                        StartTime = e.StartTime.ToString(),
                        EndTime = e.EndTime.ToString(),
                        Latitude = e.Latitude,
                        Longtitude = e.Longitude,
                        Type = e.Type
                    });
                }
                
                eventEntity = userEntity.Events.ToList();
                foreach (var e in eventEntity)
                {
                    events.Add(new QuickEvent()
                    {
                        ID = e.ID,
                        UserID = e.UserID,
                        Name = e.Name,
                        Description = e.Description,
                        StartTime = e.StartTime.ToString(),
                        EndTime = e.EndTime.ToString(),
                        Latitude = e.Latitude,
                        Longtitude = e.Longitude,
                        Type = e.Type
                    });
                }
                response.data = events;
                response.success = true;

            }
            catch (Exception)
            {
                response.success = false;
                response.message = "error on GetEvents";
            }
            return response;
        }

        public ResponseObject<List<Message>> GetMessages(string eventID)
        {
            dynamic response = new ResponseObject<List<Message>>();
            // Add a new message to the Azure table
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Create the table if it doesn't exist.
                CloudTable table = tableClient.GetTableReference("messages");
                // Construct the query operation for all customer entities where PartitionKey="MESSAGE".
                TableQuery<Message> query = new TableQuery<Message>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, eventID));
                var temp = table.ExecuteQuery(query);
                response.data = table.ExecuteQuery(query).ToList();
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on GetMessages eventID={0}", eventID);
            }

            return response;
        }

        public ResponseObject<int> Login(LoginDetails loginDetails)
        {
            dynamic response = new ResponseObject<int>();
            try
            {
                eventfinderEntitiesModel model = new eventfinderEntitiesModel();
                User userEntity = model.Users.First(e => e.Email == loginDetails.Email && e.Password == loginDetails.Password);
                response.data = userEntity.ID;
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on Login email={0} password={1}", loginDetails.Email, loginDetails.Password);
            }
            return response;
        }

        public ResponseObject<int> Register(RegisterUser user)
        {
            dynamic response = new ResponseObject<int>();
            try
            {
                eventfinderEntitiesModel ent = new eventfinderEntitiesModel();
                User userEntity = new User()
                {
                    Email = user.Email,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };
                ent.Users.Add(userEntity);
                ent.SaveChanges();
                SaveUserPhoneID(user.PhoneID, userEntity.ID.ToString());
                response.data = userEntity.ID;
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on Register email={0}", user.Email);
            }
            
            return response;
        }

        private void SaveUserPhoneID(string phoneID,string userID)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // Create the table users.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the CloudTable object that represents the "users" table.
            CloudTable table = tableClient.GetTableReference("users");
            table.CreateIfNotExists();
            // Create a new customer entity.
            UserEntity userEntity = new UserEntity(phoneID, userID);
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(userEntity);
            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public ResponseObject<List<Message>> SendMessage(Message message)
        {
            dynamic response = new ResponseObject<List<Message>>();
            // Add a new message to the Azure table
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Create the table if it doesn't exist.
                CloudTable table = tableClient.GetTableReference("messages");
                table.CreateIfNotExists();
                Message newMessage = new Message(message.Body, message.PartitionKey);
                // object to place into table
                // Build insert operation.
                TableOperation insertOperation = TableOperation.Insert(newMessage);
                // Execute the insert operation.
                table.Execute(insertOperation);
                // Construct the query operation for all customer entities where PartitionKey="MESSAGE".
                TableQuery<Message> query = new TableQuery<Message>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, newMessage.PartitionKey));
                var temp = table.ExecuteQuery(query);
                response.data = table.ExecuteQuery(query).ToList();
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on SendMessage message={0}", message.Body);
            }

            return response;
        }

        public void SaveImageInBlob(int eventID, Stream fileStream)
        {
            dynamic response = new ResponseObject<int>();
            try
            {
                EnsureContainerExists();
                SaveImage(eventID, fileStream);
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = string.Format("error on SaveImageInBlob");
            }
            
        }
        private void EnsureContainerExists()
        {
            var container = GetContainer();
            container.CreateIfNotExists();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);
        }
        private CloudBlobContainer GetContainer()
        {
            // Get a handle on account, create a blob service client and get container proxy
            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference("photos");
        }
        private void SaveImage(int eventid, Stream fileStream)
        {
            // Create a blob in container and upload image bytes to it
            var blob = this.GetContainer().GetBlockBlobReference(eventid.ToString());
            blob.Properties.ContentType = "image/jpg";
            blob.Metadata.Add("Id", Guid.NewGuid().ToString());
            blob.Metadata.Add("ImageName", eventid.ToString());
            blob.UploadFromStream(fileStream);
            blob.SetMetadata();
        }
        //private void DeleteImage(string blobUri)
        //{
        //    var blob = this.GetContainer().GetBlockBlobReference(blobUri);
        //    bool result = blob.DeleteIfExists();
        //}
    }
}
