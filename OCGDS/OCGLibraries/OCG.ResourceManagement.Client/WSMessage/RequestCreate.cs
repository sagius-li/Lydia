using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    [XmlRoot(Namespace = Constants.WsTransfer.Namespace)]
    public class CreateRequest : BaseTransferRequest
    {
        [XmlElement(Namespace = Constants.Imda.Namespace)]
        public AddRequest AddRequest;
    }
}
