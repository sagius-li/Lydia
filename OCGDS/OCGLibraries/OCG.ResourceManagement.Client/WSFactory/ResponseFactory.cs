using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

using OCG.ResourceManagement.Client.WSMessage;
using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSFactory
{
    public class ResponseFactory
    {
        public ResponsePull CreatePullResponse(Message response)
        {
            return response.GetBody<ResponsePull>(new ClientSerializer(typeof(ResponsePull)));
        }

        public ResponseEnumeration CreateEnumerationResponse(Message response)
        {
            return response.GetBody<ResponseEnumeration>(new ClientSerializer(typeof(ResponseEnumeration)));
        }

        public ResponseCreate CreateCreateResponse(Message response)
        {
            ResponseCreate retVal = new ResponseCreate();

            // for a reason which is not clear, this isn't working
            //ResourceCreated resourceCreated = response.GetBody<ResourceCreated>(new ClientSerializer(typeof(ResourceCreated)));

            XmlNode body = response.GetBody<XmlNode>(new ClientSerializer(typeof(XmlNode)));

            retVal.ResourceCreated = new ResourceCreated();
            retVal.ResourceCreated.EndpointReference = new EndpointReference();
            try
            {
                retVal.ResourceCreated.EndpointReference.Address = body["EndpointReference"]["Address"].InnerText;
                retVal.ResourceCreated.EndpointReference.ReferenceProperties = new ReferenceProperties();
                retVal.ResourceCreated.EndpointReference.ReferenceProperties.ResourceReferenceProperty = new ResourceReferenceProperty();
                retVal.ResourceCreated.EndpointReference.ReferenceProperties.ResourceReferenceProperty.Value = body["EndpointReference"]["ReferenceProperties"]["ResourceReferenceProperty"].InnerText;
            }
            catch (NullReferenceException)
            {
            }

            return retVal;
        }

        public ResponseGet CreateGetResponse(Message response, bool fromSearchRequest)
        {
            ResponseGet retVal = new ResponseGet();

            if (fromSearchRequest)
            {
                retVal.ConvertFromBase(response.GetBody<BaseSearchResponse>(new ClientSerializer(typeof(BaseSearchResponse))));
            }
            else
            {
                XmlNode retObject = response.GetBody<XmlNode>(new ClientSerializer(typeof(XmlNode)));
                Dictionary<String, List<XmlNode>> seenAttributes = new Dictionary<string, List<XmlNode>>();
                foreach (XmlNode child in retObject.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        if (seenAttributes.ContainsKey(child.Name) == false)
                        {
                            seenAttributes[child.Name] = new List<XmlNode>();
                        }
                        seenAttributes[child.Name].Add(child);
                    }
                }

                foreach (KeyValuePair<String, List<XmlNode>> item in seenAttributes)
                {
                    PartialAttributeType partialAttribute = new PartialAttributeType();
                    partialAttribute.Values.AddRange(item.Value);
                    retVal.PartialAttributes.Add(partialAttribute);
                }
            }

            return retVal;
        }
    }
}
