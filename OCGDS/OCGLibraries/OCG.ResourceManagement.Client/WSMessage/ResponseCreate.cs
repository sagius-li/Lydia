using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    [XmlRoot(Namespace = Constants.Rm.Namespace)]
    public class ResponseCreate
    {
        [XmlElement(Namespace = Constants.WsTransfer.Namespace)]
        public ResourceCreated ResourceCreated;
    }
}
