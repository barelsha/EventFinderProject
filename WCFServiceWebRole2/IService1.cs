using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    //[ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebInvoke(Method = "GET",
               ResponseFormat = WebMessageFormat.Json,
               UriTemplate = "events")]
        bool GetEvents();

        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "events/{id}")]
        Event GetEvent(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "events/{eventID}/{userID}")]
        string JoinEvent(string eventID, string userID);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json)]
        string Event(string eventName, string startTime, string endTime, string userID, string description, string latitude , string longitude);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json)]
        int Login(string email, string password);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json)]
        int Register(string email, string password, string firstName, string lastName, string phoneNumber);

    }

}
