using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using WCFServiceWebRole2.DB;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        public string AddEvent(QuickEvent eve)
        {
        //    DateTime start = Convert.ToDateTime(startTime);
        //    DateTime end = Convert.ToDateTime(endTime);
        //    int user = Int32.Parse(userID);
        //    double latitudePoint = Convert.ToDouble(latitude);
        //    double longitudePoint = Convert.ToDouble(longitude);
        //    eventfinderEntities1 ent = new eventfinderEntities1();
        //    Event eventEntity = new Event()
        //    {
        //        Name = eventName,
        //        StartTime = start,
        //        EndTime = end,
        //        UserID = user,
        //        Description = description,
        //    };
        //    ent.Events.Add(eventEntity);
        //    ent.SaveChanges();
        //    int eventID = eventEntity.ID;

        //    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));// retrieve a reference to the messages queue
        //    var queueEvent = storageAccount.CreateCloudQueueClient();
        //    var queue = queueEvent.GetQueueReference("neweventqueue");
        //    queue.CreateIfNotExists(null);
        //    var msg = new CloudQueueMessage(eventID.ToString());
        //    queue.AddMessage(msg);
            return "shahar";
        }


        public QuickEvent GetEvent(string id)
        {
            int eventID = Int32.Parse(id);
            eventfinderModel model = new eventfinderModel();
            Events eventEntity = model.Events.First(e => e.ID == eventID);
            QuickEvent quickEvent = new QuickEvent()
            {
                ID = eventEntity.ID,
                Name = eventEntity.Name,
                Description = eventEntity.Description,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                Latitude = eventEntity.Latitude,
                Longtitude = eventEntity.Longitude
            };
            return quickEvent;
        }

        public List<QuickEvent> GetEvents()
        {
            List<QuickEvent> events = new List<QuickEvent>();
            eventfinderModel model = new eventfinderModel();
            ICollection<Events> eventEntity = model.Events.ToList();
            foreach (var e in eventEntity)
            {
                events.Add(new QuickEvent()
                {
                    ID = e.ID,
                    Name = e.Name,
                    Description = e.Description,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Latitude = e.Latitude,
                    Longtitude = e.Longitude
                });
            }
            
            return events;
        }

        public bool JoinEvent(string eventID, string userID)
        {
            int user = Int32.Parse(userID);
            int eventId = Int32.Parse(eventID);
            eventfinderModel model = new eventfinderModel();
            Events eventEntity = model.Events.First(e => e.ID == eventId);
            Users userEntity = model.Users.First(u => u.ID == user);
            eventEntity.Users1.Add(userEntity);
            model.SaveChanges();
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var queueEvent = storageAccount.CreateCloudQueueClient();
            var queue = queueEvent.GetQueueReference("joineventqueue");
            queue.CreateIfNotExists(null);
            var msg = new CloudQueueMessage(eventEntity.ID.ToString());
            queue.AddMessage(msg);
            return true;
        }

        //public int Login(LoginDetails loginDetails)
        //{
        //    eventfinderEntities1 ent = new eventfinderEntities1();
        //    User userEntity = ent.Users.First(e => e.Email == email);
        //    return userEntity.ID;
        //}

        //public int Register(User user)
        //{
        //    eventfinderEntities1 ent = new eventfinderEntities1();
        //    //User userEntity = new User()
        //    //{
        //    //    Email = user.email,
        //    //    Password = user.password,
        //    //    FirstName = user.firstName,
        //    //    LastName = user.lastName,
        //    //    PhoneNumber = user.phoneNumber
        //    //};
        //    ent.Users.Add(user);
        //    ent.SaveChanges();
        //    int userID = user.ID;
        //    return userID;
        //}
    }
}
