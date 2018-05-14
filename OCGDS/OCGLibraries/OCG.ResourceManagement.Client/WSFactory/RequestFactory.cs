using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

using OCG.ResourceManagement.Client.WSMessage;
using OCG.ResourceManagement.Client.XMLHandler;

using OCG.ResourceManagement.ObjectModel;

namespace OCG.ResourceManagement.Client.WSFactory
{
    public class RequestFactory : RmFactory
    {
        private List<string> ProhibitedAttributes;

        public RequestFactory()
            : this(new XmlSchemaSet())
        {
        }

        public RequestFactory(XmlSchemaSet schemaSet)
            : base(schemaSet)
        {
            this.ProhibitedAttributes = new List<string>();

            initProhibitedAttributes();
        }

        public Message CreatePullRequest(RequestPull request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (request.EnumerationContext == null)
            {
                throw new InvalidOperationException("EnumerationContext must be set in order to call Pull");
            }

            Message pullRequest;
            lock (request)
            {
                pullRequest = Message.CreateMessage(MessageVersion.Soap12WSAddressing10, Constants.WsEnumeration.PullAction, request, new ClientSerializer(typeof(RequestPull)));
            }

            return pullRequest;
        }

        public Message CreateEnumerationRequest(RequestEnumeration request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Message enumerationRequest = null;
            lock (request)
            {
                enumerationRequest = Message.CreateMessage(MessageVersion.Default, Constants.WsEnumeration.EnumerateAction, request, new ClientSerializer(typeof(RequestEnumeration)));
            }

            return enumerationRequest;
        }

        public Message CreateDeleteRequest(RmReference objectId)
        {
            if (objectId == null)
            {
                throw new ArgumentNullException("objectId");
            }

            RequestDelete deleteRequest = new RequestDelete();
            deleteRequest.ResourceReferenceProperty = objectId.Value;

            Message msgRequest = null;
            lock (deleteRequest)
            {
                msgRequest = Message.CreateMessage(MessageVersion.Default, Constants.WsTransfer.DeleteAction, deleteRequest, new ClientSerializer(typeof(RequestDelete)));
                ClientHelper.AddRmHeaders(deleteRequest, msgRequest);
            }

            return msgRequest;
        }

        public Message CreatePutRequest(RmResourceChanges transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentException("transaction");
            }
            RmResource rmObject = transaction.RmObject;
            if (rmObject == null)
            {
                throw new InvalidOperationException("transaction does not have rmObject");
            }

            lock (rmObject)
            {
                RequestPut putRequest = new RequestPut();
                putRequest.ResourceReferenceProperty = rmObject.ObjectID.ToString();
                if (String.IsNullOrEmpty(rmObject.Locale) == false)
                {
                    putRequest.ResourceLocaleProperty = CultureInfo.GetCultureInfo(rmObject.Locale).ToString(); //System.Globalization.CultureInfo(rmObject.Locale)
                }

                putRequest.ModifyRequest = new ModifyRequest();

                IList<RmAttributeChange> changes = transaction.GetChanges();

                foreach (RmAttributeChange attributeChange in changes)
                {
                    if (!string.IsNullOrEmpty(ProhibitedAttributes.Find(p => p.Equals(attributeChange.Name.Name, StringComparison.OrdinalIgnoreCase))))
                        continue;

                    DirectoryAccessChange putReqChange = BuildDirectoryAccessChange(attributeChange);

                    if (base.IsMultiValued(attributeChange.Name))
                    {
                        putReqChange.Operation = attributeChange.Operation.ToString();
                    }
                    else
                    {
                        if (attributeChange.Operation == RmAttributeChangeOperation.Add)
                        {
                            putReqChange.Operation = RmAttributeChangeOperation.Replace.ToString();
                        }
                        else if (attributeChange.Operation == RmAttributeChangeOperation.Delete)
                        {
                            putReqChange.Operation = RmAttributeChangeOperation.Replace.ToString();
                            putReqChange.AttributeValue = null;
                        }
                    }
                    putRequest.ModifyRequest.Changes.Add(putReqChange);
                }

                Message msgRequest = null;
                lock (putRequest)
                {
                    msgRequest = Message.CreateMessage(MessageVersion.Default, Constants.WsTransfer.PutAction, putRequest.ModifyRequest, new ClientSerializer(typeof(ModifyRequest)));
                    ClientHelper.AddImdaHeaders(putRequest, msgRequest);
                    ClientHelper.AddRmHeaders(putRequest, msgRequest);
                }

                return msgRequest;
            }
        }

        public Message CreateCreateRequest(RmGeneric newResource)
        {
            if (newResource == null)
            {
                throw new ArgumentNullException("newResource");
            }

            lock (newResource)
            {
                CreateRequest createRequest = new CreateRequest();

                createRequest.AddRequest = new AddRequest();
                createRequest.AddRequest.AttributeTypeAndValues = new List<DirectoryAccessChange>();
                foreach (KeyValuePair<RmAttributeName, RmAttributeValue> attribute in newResource.Attributes)
                {
                    if (!string.IsNullOrEmpty(ProhibitedAttributes.Find(p=>p.Equals(attribute.Key.Name, StringComparison.OrdinalIgnoreCase))))
                        continue;

                    foreach (IComparable value in attribute.Value.Values)
                    {
                        DirectoryAccessChange createReqChange = BuildDirectoryAccessChange(attribute.Key, value);
                        // cannot specify the operation on create
                        createReqChange.Operation = null;
                        createRequest.AddRequest.AttributeTypeAndValues.Add(createReqChange);
                    }
                }

                Message msgCreateRequest = null;
                lock (createRequest)
                {
                    msgCreateRequest = Message.CreateMessage(MessageVersion.Default, Constants.WsTransfer.CreateAction, createRequest.AddRequest, new ClientSerializer(typeof(AddRequest)));
                    ClientHelper.AddImdaHeaders(createRequest, msgCreateRequest);

                    return msgCreateRequest;
                }
            }
        }

        public Message CreateGetRequest(RmReference objectId, CultureInfo culture, string[] attributes)
        {
            if (objectId == null || string.IsNullOrEmpty(objectId.Value))
            {
                throw new ArgumentNullException("objectId");
            }

            Message msgGetRequest = null;
            bool isAttributeSearchRequest = false;

            RequestGet requestGet = new RequestGet() { ResourceReferenceProperty = objectId.Value };
            if (culture != null)
            {
                requestGet.ResourceLocaleProperty = culture.Name;
            }
            if (attributes != null && attributes.Length != 0)
            {
                isAttributeSearchRequest = true;
                List<string> attributeList = new List<string>(attributes);
                if (string.IsNullOrEmpty(attributeList.Find(a=>a.ToLower()=="objecttype")))
                {
                    attributeList.Add("ObjectType");
                }
                requestGet.RequestAttributeSearch = new RequestAttributeSearch(attributeList.ToArray());
            }

            if (!isAttributeSearchRequest)
            {
                msgGetRequest = Message.CreateMessage(MessageVersion.Default, Constants.WsTransfer.GetAction);
            }
            else
            {
                msgGetRequest = Message.CreateMessage(MessageVersion.Default, Constants.WsTransfer.GetAction, 
                    requestGet.RequestAttributeSearch, new ClientSerializer(typeof(RequestAttributeSearch)));
                ClientHelper.AddImdaHeaders(requestGet, msgGetRequest);
            }

            ClientHelper.AddRmHeaders(requestGet, msgGetRequest);

            return msgGetRequest;
        }

        private void initProhibitedAttributes()
        {
            this.ProhibitedAttributes.Add(@"ObjectID");
            this.ProhibitedAttributes.Add(@"Creator");
            this.ProhibitedAttributes.Add(@"CreatedTime");
            this.ProhibitedAttributes.Add(@"ExpectedRulesList");
            this.ProhibitedAttributes.Add(@"DetectedRulesList");
            this.ProhibitedAttributes.Add(@"DeletedTime");
            this.ProhibitedAttributes.Add(@"ResourceTime");
            this.ProhibitedAttributes.Add(@"ComputedMember");
            this.ProhibitedAttributes.Add(@"ComputedActor");
        }

        private DirectoryAccessChange BuildDirectoryAccessChange(RmAttributeName name, IComparable value)
        {
            DirectoryAccessChange retReqChange = new DirectoryAccessChange();
            retReqChange.AttributeType = name.Name;
            XmlElement attributeValueElem = base.RmDoc.CreateElement(retReqChange.AttributeType, RmNamespace);
            attributeValueElem.InnerText = value.ToString();
            retReqChange.AttributeValue.Values.Add(attributeValueElem);
            return retReqChange;
        }

        DirectoryAccessChange BuildDirectoryAccessChange(RmAttributeChange attribute)
        {
            DirectoryAccessChange retReqChange = new DirectoryAccessChange();
            retReqChange.AttributeType = attribute.Name.Name;
            XmlElement attributeValueElem = base.RmDoc.CreateElement(retReqChange.AttributeType, RmNamespace);
            if (attribute.Value != null)
            {
                attributeValueElem.InnerText = attribute.Value.ToString();
            }
            retReqChange.AttributeValue.Values.Add(attributeValueElem);
            return retReqChange;
        }
    }
}
