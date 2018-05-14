using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml;
using System.Text;

using OCG.ResourceManagement.ObjectModel;

namespace OCG.ResourceManagement.Client.WSFactory
{
    public class RmFactory
    {
        public XmlSchemaSet RmSchema;
        protected const String RmNamespace = "http://schemas.microsoft.com/2006/11/ResourceManagement";
        protected Dictionary<RmAttributeName, RmAttributeInfo> RmAttributeCache;
        protected Dictionary<String, Dictionary<RmAttributeName, RmAttributeInfo>> RmObjectCache;
        protected XmlDocument RmDoc;
        protected XmlNamespaceManager RmNsManager;

        public RmFactory(XmlSchemaSet rmSchema)
        {
            if (rmSchema == null)
            {
                throw new ArgumentNullException("rmSchema");
            }
            lock (rmSchema)
            {
                this.RmSchema = rmSchema;
                if (this.RmSchema.IsCompiled == false)
                {
                    this.RmSchema.Compile();
                }
                this.RmAttributeCache = new Dictionary<RmAttributeName, RmAttributeInfo>();
                this.RmObjectCache = new Dictionary<string, Dictionary<RmAttributeName, RmAttributeInfo>>();

                this.RmDoc = new XmlDocument();
                this.RmNsManager = new XmlNamespaceManager(this.RmDoc.NameTable);
                this.RmNsManager.AddNamespace("rm", RmNamespace);

                foreach (XmlSchemaObject schemaObj in this.RmSchema.GlobalTypes.Values)
                {
                    XmlSchemaComplexType schemaObjComplexType = schemaObj as XmlSchemaComplexType;
                    if (schemaObjComplexType != null)
                    {
                        if (schemaObjComplexType.Name == null || schemaObjComplexType.Particle == null)
                        {
                            continue;
                        }
                        RmObjectCache[schemaObjComplexType.Name] = new Dictionary<RmAttributeName, RmAttributeInfo>();
                        XmlSchemaSequence schemaObjSequence = schemaObjComplexType.Particle as XmlSchemaSequence;
                        if (schemaObjSequence != null)
                        {
                            foreach (XmlSchemaObject sequenceObj in schemaObjSequence.Items)
                            {
                                XmlSchemaElement sequenceElement = sequenceObj as XmlSchemaElement;
                                if (sequenceElement != null)
                                {
                                    RmAttributeInfo info = new RmAttributeInfo();

                                    if (sequenceElement.MaxOccurs > Decimal.One)
                                    {
                                        info.IsMultiValue = true;
                                    }

                                    if (sequenceElement.MinOccurs > Decimal.Zero)
                                    {
                                        info.IsRequired = true;
                                    }

                                    String attributeTypeName = sequenceElement.ElementSchemaType.QualifiedName.Name.ToUpperInvariant();
                                    if (attributeTypeName.Contains("COLLECTION"))
                                    {
                                        info.IsMultiValue = true;
                                    }

                                    if (attributeTypeName.Contains("REFERENCE"))
                                    {
                                        info.AttributeType = RmAttributeType.Reference;
                                    }
                                    else if (attributeTypeName.Contains("BOOLEAN"))
                                    {
                                        info.AttributeType = RmAttributeType.Boolean;
                                    }
                                    else if (attributeTypeName.Contains("INTEGER"))
                                    {
                                        info.AttributeType = RmAttributeType.Integer;
                                    }
                                    else if (attributeTypeName.Contains("DATETIME"))
                                    {
                                        info.AttributeType = RmAttributeType.DateTime;
                                    }
                                    else if (attributeTypeName.Contains("BINARY"))
                                    {
                                        info.AttributeType = RmAttributeType.Binary;
                                    }
                                    else
                                    {
                                        info.AttributeType = RmAttributeType.String;
                                    }
                                    RmAttributeName attributeName = new RmAttributeName(sequenceElement.Name);
                                    RmObjectCache[schemaObjComplexType.Name][attributeName] = info;
                                    RmAttributeCache[attributeName] = info;
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool IsMultiValued(RmAttributeName attributeName)
        {
            RmAttributeInfo retValue = null;
            RmAttributeCache.TryGetValue(attributeName, out retValue);
            if (retValue == null)
            {
                return false;
            }
            else
            {
                return retValue.IsMultiValue;
            }
        }
        
        public bool IsReference(RmAttributeName attributeName)
        {
            RmAttributeInfo retValue = null;
            RmAttributeCache.TryGetValue(attributeName, out retValue);
            if (retValue == null)
            {
                return false;
            }
            else
            {
                return retValue.AttributeType == RmAttributeType.Reference;
            }
        }
        
        public bool IsRequired(String objectType, RmAttributeName attributeName)
        {
            Dictionary<RmAttributeName, RmAttributeInfo> attributeValue = null;
            RmObjectCache.TryGetValue(objectType, out attributeValue);
            if (attributeValue == null)
            {
                return false;
            }
            else
            {
                RmAttributeInfo attributeInfo = null;
                attributeValue.TryGetValue(attributeName, out attributeInfo);
                if (attributeInfo == null)
                {
                    return false;
                }
                else
                {
                    return attributeInfo.IsRequired;
                }
            }
        }
        
        public List<RmAttributeName> RequiredAttributes(String objectType)
        {
            List<RmAttributeName> retList = new List<RmAttributeName>();
            Dictionary<RmAttributeName, RmAttributeInfo> attributeValue = null;
            RmObjectCache.TryGetValue(objectType, out attributeValue);
            if (attributeValue == null)
            {
                return retList;
            }
            else
            {
                foreach (KeyValuePair<RmAttributeName, RmAttributeInfo> pair in attributeValue)
                {
                    if (pair.Value.IsRequired)
                    {
                        retList.Add(pair.Key);
                    }
                }
                return retList;
            }
        }

        protected class RmAttributeInfo
        {
            public bool IsMultiValue;
            public bool IsRequired;
            public RmAttributeType AttributeType;
        }

        protected enum RmAttributeType
        {
            String,
            Reference,
            DateTime,
            Integer,
            Binary,
            Boolean
        }

    }
}
