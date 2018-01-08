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

        //public string Event(string eventName, string startTime,string endTime,string userID, string description, string latitude, string longitude)
        //{
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
        //    return "";
        //}


        public string GetEvent(string id)
        {
            return "gfgg";
            //int eventID = Int32.Parse(id);
            //eventfinderEntities1 ent = new eventfinderEntities1();
            //Event eventEntity = ent.Events.First(e => e.ID == eventID);
            //return eventEntity;
        }

        public bool GetEvents()
        {
            eventfinderEntities1 ent = new eventfinderEntities1();
            //ent.Events
            return true;
        }

        //public string JoinEvent(string eventID, string userID)
        //{
        //    int user = Int32.Parse(userID);
        //    int eventId = Int32.Parse(eventID);
        //    eventfinderEntities1 ent = new eventfinderEntities1();
        //    Event eventEntity = ent.Events.First(e => e.ID == eventId);
        //    User userEntity = ent.Users.First(u => u.ID == user);
        //    eventEntity.Users.Add(userEntity);
        //    ent.SaveChanges();
        //    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        //    var queueEvent = storageAccount.CreateCloudQueueClient();
        //    var queue = queueEvent.GetQueueReference("joineventqueue");
        //    queue.CreateIfNotExists(null);
        //    //var msg = new CloudQueueMessage();
        //    //queue.AddMessage(msg);
        //    return "";
        //}

        //public int Login(string email, string password)
        //{
        //    eventfinderEntities1 ent = new eventfinderEntities1();
        //    User userEntity = ent.Users.First(e => e.Email == email);
        //    return userEntity.ID;
        //}

        //public string Register(string email, string password, string firstName, string lastName, string phoneNumber)
        //{
        //    return "moran";
        //    //eventfinderEntities1 ent = new eventfinderEntities1();
        //    //User userEntity = new User()
        //    //{
        //    //    Email = email,
        //    //    Password = password,
        //    //    FirstName = firstName,
        //    //    LastName = lastName,
        //    //    PhoneNumber = phoneNumber
        //    //};
        //    //ent.Users.Add(userEntity);
        //    //ent.SaveChanges();
        //    //int userID = userEntity.ID;
        //    //return userID;
        //}
    }
}
