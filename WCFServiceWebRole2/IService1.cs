using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCFServiceWebRole2.DB;
using System.Web.Http.Cors;

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
        ResponseObject<List<QuickEvent>> GetEvents();

        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "events/{id}")]
        ResponseObject<QuickEvent> GetEvent(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "events/{eventID}/{userID}")]
        ResponseObject<bool> JoinEvent(string eventID, string userID);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "events")]
        ResponseObject<QuickEvent> AddEvent(QuickEvent newEvent);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "login")]
        ResponseObject<int> Login(LoginDetails loginDetails);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "Register")]
        ResponseObject<int> Register(RegisterUser user);

    }

}
