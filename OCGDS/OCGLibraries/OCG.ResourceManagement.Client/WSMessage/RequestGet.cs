using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    public class RequestGet : BaseTransferRequest
    {
        [XmlElement(Namespace = Constants.Imda.Namespace)]
        public RequestAttributeSearch RequestAttributeSearch;
    }
}
