﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSEnumeration
{
    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace)]
    public class EnumerationContext
    {
        public EnumerationContext()
        {
        }
        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public long Count;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public long CurrentIndex;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public EnumerationDirection EnumerationDirection;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public String Filter;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public DateTime Expires;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public LocalePreferences LocalePreferences;

        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public Sorting Sorting;

        Selection selection;
        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public Selection Selection
        {
            get
            {
                if (this.selection == null)// || this.selection.@string == null)
                    return null;
                else
                    return this.selection;
            }
            set
            {
                this.selection = value;
            }
        }

        [XmlElement(Namespace = Constants.WsEnumeration.Namespace)]
        public String Time;
    }
}
