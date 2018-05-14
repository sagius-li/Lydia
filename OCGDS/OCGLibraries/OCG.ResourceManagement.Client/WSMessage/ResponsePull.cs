using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.WSEnumeration;
using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace, ElementName = Constants.WsEnumeration.PullResponse)]
    public class ResponsePull
    {
        [XmlElement()]
        public EnumerationContext EnumerationContext;

        [XmlElement()]
        public PullItem Items;

        [XmlElement()]
        public String EndOfSequence;

        [XmlIgnore()]
        public bool IsEndOfSequence
        {
            get
            {
                return EndOfSequence != null;
            }
        }
    }
}
