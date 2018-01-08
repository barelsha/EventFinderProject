using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "api" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select api.svc or api.svc.cs at the Solution Explorer and start debugging.
    public class api : Iapi
    {
        public Event GetEvent(string id)
        {
            eventfinderEntities1 ent = new eventfinderEntities1();
            //ent.Events.Include
            Event even = new Event()
            {
                ID = 1,
                Name = "shahar",
                StartTime = new DateTime(),
                EndTime = new DateTime(),
                UserID = 1
            };
            return even;
        }
    }
}
