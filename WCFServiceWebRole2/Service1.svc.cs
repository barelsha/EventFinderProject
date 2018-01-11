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

        public ResponseObject<int> AddEvent(QuickEvent newEvent)
        {
            dynamic response = new ResponseObject<QuickEvent>();
            try
            {
                DateTime start = Convert.ToDateTime(newEvent.StartTime);
                DateTime end = Convert.ToDateTime(newEvent.EndTime);
                double latitudePoint = Convert.ToDouble(newEvent.Latitude);
                double longitudePoint = Convert.ToDouble(newEvent.Longtitude);
                eventfinderModel ent = new eventfinderModel();
                Events eventEntity = new Events()
                {
                    Name = newEvent.Name,
                    StartTime = start,
                    EndTime = end,
                    UserID = newEvent.UserID,
                    Latitude = latitudePoint,
                    Longitude = longitudePoint,
                    Description = newEvent.Description,
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

                response.data = eventID;
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

        public ResponseObject<int> Login(LoginDetails loginDetails)
        {
            dynamic response = new ResponseObject<int>();
            try
            {
                eventfinderModel model = new eventfinderModel();
                Users userEntity = model.Users.First(e => e.Email == loginDetails.Email && e.Password == loginDetails.Password);
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
                eventfinderModel ent = new eventfinderModel();
                Users userEntity = new Users()
                {
                    Email = user.Email,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };
                ent.Users.Add(userEntity);
                ent.SaveChanges();
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
    }
}
