using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iapi" in both code and config file together.
    //[ServiceContract(SessionMode = SessionMode.NotAllowed)]
    [ServiceContract]
    public interface Iapi
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "events/{id}")]
        Event GetEvent(string id);
    }
}
