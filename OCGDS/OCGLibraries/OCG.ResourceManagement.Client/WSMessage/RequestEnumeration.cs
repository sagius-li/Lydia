using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace, ElementName = Constants.WsEnumeration.Enumerate)]
    public class RequestEnumeration
    {
        public RequestEnumeration()
            : this(null)
        {

        }

        public RequestEnumeration(String filter)
        {
            this.MaxCharacters = Constants.WsEnumeration.DefaultMaxCharacters;
            this.MaxElements = Constants.WsEnumeration.DefaultMaxElements;
            if (String.IsNullOrEmpty(filter) == false)
            {
                this.Filter = new EnumerationFilter(filter);
            }
        }

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public EnumerationFilter Filter;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public LocalePreferences LocalePreferences;

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public Int32 MaxElements;

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public Int32 MaxCharacters;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public Sorting Sorting;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public List<String> Selection;

        // Time should be in the ResourceManagement namespace
        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public String Time;
    }
}
