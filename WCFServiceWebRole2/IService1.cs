using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCFServiceWebRole2.DB;

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
        List<QuickEvent> GetEvents();

        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "events/{id}")]
        QuickEvent GetEvent(string id);

        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //  ResponseFormat = WebMessageFormat.Json,
        //  UriTemplate = "events/{eventID}/{userID}")]
        //string JoinEvent(string eventID, string userID);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "events")]
        string AddEvent(QuickEvent eve);

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //        ResponseFormat = WebMessageFormat.Json)]
        //int Login(LoginDetails loginDetails);

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //        ResponseFormat = WebMessageFormat.Json,
        //        BodyStyle = WebMessageBodyStyle.Bare)]
        //int Register(User user);

    }

}
