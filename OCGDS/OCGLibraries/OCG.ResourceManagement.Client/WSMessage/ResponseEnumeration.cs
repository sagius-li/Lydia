using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace OCG.ResourceManagement.Client.WSMessage
{
    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace, ElementName = Constants.WsEnumeration.EnumerationResponse)]
    public class ResponseEnumeration : ResponsePull
    {
        // this is identical to a PullResponse
    }
}
