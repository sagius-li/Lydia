using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.WSEnumeration;
using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace, ElementName = Constants.WsEnumeration.Pull)]
    public class RequestPull
    {
        public RequestPull()
        {
            this.MaxCharacters = Constants.WsEnumeration.DefaultMaxCharacters;
            this.MaxElements = Constants.WsEnumeration.DefaultMaxElements;
        }

        [XmlElement()]
        public EnumerationContext EnumerationContext;

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public Int32 MaxElements;

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public Int32 MaxCharacters;

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public PullAdjustment PullAdjustment;
    }
}
