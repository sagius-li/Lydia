﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Text;

using OCG.ResourceManagement.Client.WSMessage;
using OCG.ResourceManagement.Client.XMLHandler;

using OCG.ResourceManagement.ObjectModel;

namespace OCG.ResourceManagement.Client.WSFactory
{
    public class RmResourceFactory : RmFactory
    {
        RmResourceTypeFactory resourceTypeFactory;

        const String ObjectType = @"ObjectType";
        const String ObjectID = @"ObjectID";

        public RmResourceFactory()
            : this(new XmlSchemaSet())
        { }

        public RmResourceFactory(XmlSchemaSet rmSchema)
            : this(rmSchema, new RmResourceTypeFactory())
        { }

        public RmResourceFactory(XmlSchemaSet rmSchema, RmResourceTypeFactory resourceTypeFactory)
            : base(rmSchema)
        {
            if (resourceTypeFactory == null)
            {
                throw new ArgumentNullException("resourceTypeFactory");
            }
            this.resourceTypeFactory = resourceTypeFactory;
        }

        public List<RmGeneric> CreateResource(ResponsePull pullOrEnumerateResponse, bool returnGenericType)
        {
            if (pullOrEnumerateResponse == null)
            {
                throw new ArgumentNullException("pullOrEnumerateResponse");
            }
            if (pullOrEnumerateResponse.Items == null || pullOrEnumerateResponse.Items.Values == null)
            {
                return new List<RmGeneric>();
            }
            lock (pullOrEnumerateResponse)
            {
                List<RmGeneric> retList = new List<RmGeneric>();

                foreach (XmlNode obj in pullOrEnumerateResponse.Items.Values)
                {
                    // look ahead for the type info;
                    string objectType = null;
                    foreach (XmlNode child in obj.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            if (child.LocalName.Equals(@"ObjectType"))
                            {
                                objectType = child.InnerText;
                                break;
                            }
                        }
                    }
                    if (objectType == null)
                    {
                        objectType = String.Empty;
                    }

                    RmGeneric rmResource = this.resourceTypeFactory.CreateResource(objectType, returnGenericType);

                    // now add the attributes to the resource object
                    foreach (XmlNode child in obj.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            RmAttributeName attributeName = new RmAttributeName(child.LocalName);
                            IComparable attributeValue = this.ConstructAttributeValue(attributeName, child.InnerText);
                            if (attributeValue == null)
                                continue;

                            RmAttributeValue newAttribute = null;
                            if (rmResource.TryGetValue(attributeName, out newAttribute) == false)
                            {
                                newAttribute = new RmAttributeValue();
                                rmResource[attributeName] = newAttribute;
                            }
                            if (base.IsMultiValued(attributeName) == false)
                            {
                                newAttribute.Values.Clear();
                            }
                            else
                            {
                                newAttribute.IsMultiValue = true;
                            }
                            if (attributeName.Name.Equals(ObjectType) || attributeName.Name.Equals(ObjectID))
                                newAttribute.Values.Clear();

                            newAttribute.IsReadOnly = child.IsReadOnly;
                            newAttribute.IsRequired = base.IsRequired(objectType, attributeName);

                            newAttribute.Values.Add(attributeValue);
                        }
                    }

                    retList.Add(rmResource);
                }

                return retList;
            }
        }

        public RmGeneric CreateResource(ResponseGet getResponse, bool returnGenericType)
        {
            if (getResponse == null)
            {
                throw new ArgumentNullException("getResponse");
            }

            lock (getResponse)
            {
                // look ahead for the type
                String objectType = null;
                foreach (PartialAttributeType partialAttribute in getResponse.PartialAttributes)
                {
                    if (partialAttribute.Values.Count > 0)
                    {
                        String localName = partialAttribute.Values[0].LocalName;
                        if (String.IsNullOrEmpty(localName))
                        {
                            continue;
                        }
                        if (localName.Equals(ObjectType))
                        {
                            objectType = partialAttribute.Values[0].InnerText;
                            break;
                        }

                    }
                }

                if (objectType == null)
                {
                    objectType = string.Empty;
                }

                RmGeneric rmResource = this.resourceTypeFactory.CreateResource(objectType, returnGenericType);

                // fill in the attribute values
                foreach (PartialAttributeType partialAttribute in getResponse.PartialAttributes)
                {
                    RmAttributeName attributeName = null;
                    RmAttributeValue newAttribute = null;
                    if (partialAttribute.Values.Count > 0)
                    {
                        String localName = partialAttribute.Values[0].LocalName;
                        if (String.IsNullOrEmpty(localName))
                        {
                            continue;
                        }
                        else
                        {
                            attributeName = new RmAttributeName(localName);
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if (rmResource.TryGetValue(attributeName, out newAttribute) == false)
                    {
                        newAttribute = new RmAttributeValue();
                        rmResource.Add(new KeyValuePair<RmAttributeName,RmAttributeValue>(attributeName, newAttribute));
                    }

                    // add values to the typed list
                    foreach (XmlNode value in partialAttribute.Values)
                    {
                        IComparable newValue = this.ConstructAttributeValue(attributeName, value.InnerText);
                        if (base.IsMultiValued(attributeName) == false)
                        {
                            newAttribute.Values.Clear();
                        }
                        else
                        {
                            newAttribute.IsMultiValue = true;
                        }
                        if (attributeName.Name.Equals(ObjectType) || attributeName.Name.Equals(ObjectID))
                            newAttribute.Values.Clear();

                        newAttribute.IsReadOnly = value.IsReadOnly;
                        newAttribute.IsRequired = base.IsRequired(objectType, attributeName);

                        newAttribute.Values.Add(newValue);
                    }
                }
                return rmResource;
            }
        }

        protected IComparable ConstructAttributeValue(RmAttributeName attributeName, String innerText)
        {
            if (innerText == null)
                return null;

            RmAttributeInfo info = null;
            if (base.RmAttributeCache.TryGetValue(attributeName, out info) == false)
            {
                if (attributeName.Name.Equals(ObjectID))
                {
                    return new RmReference(innerText);
                }
                else
                {
                    return innerText;
                }
            }

            try
            {
                switch (info.AttributeType)
                {
                    case RmAttributeType.String:
                        return innerText;
                    case RmAttributeType.DateTime:
                        return DateTime.Parse(innerText);
                    case RmAttributeType.Integer:
                        return Int32.Parse(innerText);
                    case RmAttributeType.Reference:
                        return new RmReference(innerText);
                    case RmAttributeType.Binary:
                        return new RmBinary(innerText);
                    case RmAttributeType.Boolean:
                        return Boolean.Parse(innerText);
                    default:
                        return innerText;
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(
                    String.Format(
                        "Failed to parse attribute {0} with value {1} into type {2}.  Please ensure the resource management schema is up to date.",
                        attributeName,
                        innerText,
                        info.AttributeType.ToString()),
                    ex);
            }
            catch (System.Text.EncoderFallbackException ex)
            {
                throw new ArgumentException(
                    String.Format(
                        "Failed to convert the string on binary attribute {0} into byte array.",
                        attributeName),
                    ex);
            }
        }
    }
}
