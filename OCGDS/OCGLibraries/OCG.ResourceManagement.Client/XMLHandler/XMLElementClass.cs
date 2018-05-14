using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace OCG.ResourceManagement.Client.XMLHandler
{
    public enum EnumerationDirection
    {
        Forwards,
        Backwards
    }

    [XmlRoot(Namespace = Constants.WsTransfer.Namespace)]
    public class ResourceCreated
    {
        [XmlElement(Namespace = Constants.Addressing.Namespace)]
        public EndpointReference EndpointReference;
    }

    [XmlRoot(Namespace = Constants.Addressing.Namespace)]
    public class EndpointReference
    {
        [XmlElement(Namespace = Constants.Addressing.Namespace)]
        public String Address;

        //[XmlElement(Namespace = Constants.Addressing.Namespace)]
        [XmlIgnore()]
        public ReferenceProperties ReferenceProperties;
    }

    public class ReferenceProperties
    {
        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public ResourceReferenceProperty ResourceReferenceProperty;
    }

    public class ResourceReferenceProperty
    {
        public ResourceReferenceProperty()
            : this(null)
        {
        }

        public ResourceReferenceProperty(String value)
        {
            this.value = value;
        }
        private String value;
        [XmlText(Type = typeof(String))]
        public String Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.Imda.Namespace)]
    public class ModifyRequest
    {
        private String dialect;
        private List<DirectoryAccessChange> changes;

        public ModifyRequest()
        {
            this.changes = new List<DirectoryAccessChange>();
            this.dialect = Constants.Dialect.IdmAttributeType;
        }

        [XmlAttribute(AttributeName = Constants.Imda.Dialect)]
        public String Dialect
        {
            get
            {
                return this.dialect;
            }
            set
            {
                this.dialect = value;
            }
        }

        [XmlElement(ElementName = Constants.Imda.Change)]
        public List<DirectoryAccessChange> Changes
        {
            get
            {
                return this.changes;
            }
            set
            {
                this.changes = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.Imda.Namespace)]
    public class AddRequest
    {
        private String dialect;
        private List<DirectoryAccessChange> attributeTypes;

        public AddRequest()
        {
            this.attributeTypes = new List<DirectoryAccessChange>();
            this.dialect = Constants.Dialect.IdmAttributeType;
        }

        [XmlAttribute(AttributeName = Constants.Imda.Dialect)]
        public String Dialect
        {
            get
            {
                return this.dialect;
            }
            set
            {
                this.dialect = value;
            }
        }

        // List so the serializer can add elements
        [XmlElement(ElementName = Constants.Imda.AttributeTypeAndValue)]
        public List<DirectoryAccessChange> AttributeTypeAndValues
        {
            get
            {
                return this.attributeTypes;
            }
            set
            {
                this.attributeTypes = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.Imda.Namespace)]
    public class PartialAttributeType
    {
        [XmlAnyElement(Namespace = Constants.Rm.Namespace)]
        public List<XmlNode> Values;

        public PartialAttributeType()
        {
            this.Values = new List<XmlNode>();
        }
    }

    [XmlRoot(Namespace = Constants.Imda.Namespace, ElementName = Constants.Imda.BaseObjectSearchResponse)]
    public class BaseSearchResponse
    {
        private List<PartialAttributeType> partialAttributes;

        [XmlElement(ElementName = Constants.Imda.PartialAttribute)]
        public List<PartialAttributeType> PartialAttributes
        {
            get
            {
                return this.partialAttributes;
            }
            set
            {
                this.partialAttributes = value;
            }
        }

        public BaseSearchResponse()
        {
            this.partialAttributes = new List<PartialAttributeType>();
        }
    }

    public class BaseTransferRequest
    {
        [XmlIgnore()]
        public string ResourceReferenceProperty;

        [XmlIgnore()]
        public string ResourceLocaleProperty;

        [XmlIgnore()]
        public string ResourceTimeProperty;
    }

    [XmlRoot(Namespace = Constants.Imda.Namespace)]
    public class DirectoryAccessChange
    {
        private String operation;
        private string attributeType;
        private PartialAttributeType attributeValue;
        public DirectoryAccessChange()
        {
            this.attributeType = String.Empty;
            this.attributeValue = new PartialAttributeType();
        }


        [XmlAttribute(AttributeName = Constants.Imda.Operation)]
        public String Operation
        {
            get
            {
                return this.operation;
            }
            set
            {
                this.operation = value;
            }
        }

        [XmlElement()]
        public String AttributeType
        {
            get
            {
                return this.attributeType;
            }
            set
            {
                this.attributeType = value;
            }
        }

        [XmlElement()]
        public PartialAttributeType AttributeValue
        {
            get
            {
                return this.attributeValue;
            }
            set
            {
                this.attributeValue = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.Imda.Namespace, ElementName = Constants.Imda.BaseObjectSearchRequest)]
    public class RequestAttributeSearch
    {
        public RequestAttributeSearch()
            : this(new String[0] { })
        {

        }

        public RequestAttributeSearch(String[] attributeNames)
        {
            if (attributeNames == null)
                throw new ArgumentNullException("attributeNames");
            this.AttributeTypes = new List<string>();
            this.AttributeTypes.AddRange(attributeNames);
            this.Dialect = Constants.Dialect.IdmAttributeType;
        }

        [XmlAttribute(AttributeName = Constants.Imda.Dialect)]
        public String Dialect;

        [XmlElement(ElementName = Constants.Imda.AttributeType)]
        public List<String> AttributeTypes;
    }

    [XmlRoot(Namespace = Constants.Rm.Namespace)]
    public class LocalePreference
    {
        [XmlElement()]
        public String Locale;
        [XmlElement()]
        public int PreferenceValue;
    }

    [XmlRoot(Namespace = Constants.Rm.Namespace)]
    public class LocalePreferences
    {
        public LocalePreferences()
        {
            this.localePreference = new List<LocalePreference>();
        }
        List<LocalePreference> localePreference;
        public List<LocalePreference> LocalePreference
        {
            get
            {
                if (this.localePreference == null || this.localePreference.Count == 0)
                    return null;
                else
                    return this.localePreference;
            }
            set
            {
                this.localePreference = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.Rm.Namespace)]
    public class SortingAttribute
    {
        public SortingAttribute()
        {
            this.Ascending = true;
        }

        [XmlAttribute()]
        public bool Ascending;

        [XmlText()]
        public String Value;
    }

    [XmlRoot(Namespace = Constants.Rm.Namespace)]
    public class Sorting
    {

        List<SortingAttribute> sorting;

        public Sorting()
        {
            this.Dialect = Constants.Rm.Namespace;
            this.SortingAttribute = new List<SortingAttribute>();
        }
        [XmlAttribute()]
        public String Dialect;

        [XmlElement()]
        public List<SortingAttribute> SortingAttribute
        {
            get
            {
                if (sorting.Count == 0)
                    return null;
                else
                    return sorting;
            }
            set
            {
                this.sorting = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.Rm.Namespace)]
    public class Selection
    {
        public Selection()
        {
            this.@string = new List<string>();
        }
        private List<String> stringList;
        [XmlElement()]
        public List<String> @string
        {
            get
            {
                return stringList;
            }
            set
            {
                stringList = value;
            }
        }
    }

    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace, ElementName = Constants.WsEnumeration.Filter)]
    public class EnumerationFilter
    {
        public EnumerationFilter()
            : this(String.Empty)
        {

        }
        public EnumerationFilter(String filter)
        {
            if (String.IsNullOrEmpty(filter))
                throw new ArgumentNullException("filter");
            this.Filter = filter;
            this.Dialect = Constants.Dialect.IdmXpathFilter;
        }
        [XmlText()]
        public String Filter;

        [XmlAttribute()]
        public String Dialect;
    }

    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace)]
    public class PullItem
    {
        public PullItem()
        {
            this.Values = new List<XmlNode>();
        }
        [XmlAnyElement(Namespace = Constants.Rm.Namespace)]
        public List<XmlNode> Values;

    }

    [XmlRoot(Namespace = Constants.WsEnumeration.Namespace)]
    public class PullAdjustment
    {
        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public long StartingIndex;
        [XmlElement(Namespace = Constants.Rm.Namespace)]
        public EnumerationDirection EnumerationDirection;
    }
}
