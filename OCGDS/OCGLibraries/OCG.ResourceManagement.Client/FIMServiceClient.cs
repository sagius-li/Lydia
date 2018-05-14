using System;
using System.Globalization;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Schema;
using System.Text;

using OCG.ResourceManagement.Client.WSClient;
using OCG.ResourceManagement.Client.WSFactory;
using OCG.ResourceManagement.Client.WSMessage;
using OCG.ResourceManagement.Client.WSEnumeration;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Client
{
    public class FIMServiceClient : IDisposable
    {
        #region Private Members

        private MEXClient mexClient;
        private WSResourceClient wsResourceClient;
        private WSResourceFactoryClient wsResourceFactoryClient;
        private WSEnumerationClient wsEnumerationClient;

        private RequestFactory requestFactory;
        private ResponseFactory responseFactory;
        private RmResourceFactory resourceFactory;

        private bool schemaCached;

        #endregion

        #region Constructor

        public FIMServiceClient()
        {
            Constants.Bindings bindings = new Constants.Bindings();
            Constants.EndpointAddresses endpointAddresses = new Constants.EndpointAddresses();

            mexClient = new MEXClient(bindings.MetadataExchangeHttpBinding_IMetadataExchange, endpointAddresses.MetadataExchangeHttpBinding_IMetadataExchange);
            wsResourceClient = new WSResourceClient(bindings.ServiceMultipleTokenBinding_Resource, endpointAddresses.ServiceMultipleTokenBinding_Resource);
            wsResourceFactoryClient = new WSResourceFactoryClient(bindings.ServiceMultipleTokenBinding_ResourceFactory, endpointAddresses.ServiceMultipleTokenBinding_ResourceFactory);
            wsEnumerationClient = new WSEnumerationClient(bindings.ServiceMultipleTokenBinding_Enumeration, endpointAddresses.ServiceMultipleTokenBinding_Enumeration);

            requestFactory = new RequestFactory();
            responseFactory = new ResponseFactory();
            resourceFactory = new RmResourceFactory();
        }

        public FIMServiceClient(EndpointAddress mexEndpoint, EndpointAddress resourceEndpoint, EndpointAddress resourceFactoryEndpoint, EndpointAddress enumerationEndpoint)
        {
            Constants.Bindings bindings = new Constants.Bindings();

            mexClient = new MEXClient(bindings.MetadataExchangeHttpBinding_IMetadataExchange, mexEndpoint);
            wsResourceClient = new WSResourceClient(bindings.ServiceMultipleTokenBinding_Resource, resourceEndpoint);
            wsResourceFactoryClient = new WSResourceFactoryClient(bindings.ServiceMultipleTokenBinding_ResourceFactory, resourceFactoryEndpoint);
            wsEnumerationClient = new WSEnumerationClient(bindings.ServiceMultipleTokenBinding_Enumeration, enumerationEndpoint);

            requestFactory = new RequestFactory();
            responseFactory = new ResponseFactory();
            resourceFactory = new RmResourceFactory();
        }

        #endregion

        #region Promoted Properties

        public bool SchemaCached
        {
            get
            {
                return this.schemaCached;
            }
        }

        public System.Net.NetworkCredential ClientCredential
        {
            get
            {
                return this.wsResourceClient.ClientCredentials.Windows.ClientCredential;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.wsResourceClient.ClientCredentials.Windows.ClientCredential = value;
                this.wsResourceFactoryClient.ClientCredentials.Windows.ClientCredential = value;
                this.wsEnumerationClient.ClientCredentials.Windows.ClientCredential = value;
                this.mexClient.ClientCredentials.Windows.ClientCredential = value;
            }
        }

        #endregion

        #region Schema

        public XmlSchemaSet RefreshSchema()
        {
            XmlSchemaSet metadata = this.mexClient.Get();

            lock (this.requestFactory)
            {
                this.requestFactory = new RequestFactory(metadata);
            }
            lock (this.resourceFactory)
            {
                this.resourceFactory = new RmResourceFactory(metadata);
            }

            this.schemaCached = true;

            return metadata;
        }

        #endregion

        #region Enumeration

        public IEnumerable<RmGeneric> GenericEnumerate(string filter)
        {
            return GenericEnumerate(filter, null);
        }

        public IEnumerable<RmGeneric> GenericEnumerate(string filter, string[] attributes)
        {
            if (String.IsNullOrEmpty(filter))
            {
                throw new ArgumentNullException("filter");
            }

            return new GenericResultEnumerator(wsEnumerationClient, requestFactory, responseFactory, resourceFactory, filter, attributes);
        }

        public IEnumerable<RmResource> Enumerate(string filter)
        {
            return Enumerate(filter, null);
        }

        public IEnumerable<RmResource> Enumerate(string filter, string[] attributes)
        {
            if (String.IsNullOrEmpty(filter))
            {
                throw new ArgumentNullException("filter");
            }

            return new ResultEnumerator(wsEnumerationClient, requestFactory, responseFactory, resourceFactory, filter, attributes);
        }

        #endregion

        #region WS-Transfer

        #region DELETE

        public bool Delete(RmReference objectId)
        {
            if (objectId == null)
            {
                throw new ArgumentNullException("objectId");
            }

            Message msgRequest = requestFactory.CreateDeleteRequest(objectId);
            Message msgResponse = wsResourceClient.Delete(msgRequest);
            if (msgResponse.IsFault)
            {
                ClientHelper.HandleFault(msgResponse);
            }

            return true;
        }

        #endregion

        #region PUT

        public bool Put(RmResourceChanges transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            Message msgRequest = requestFactory.CreatePutRequest(transaction);
            Message msgResponse = wsResourceClient.Put(msgRequest);
            if (msgResponse.IsFault)
            {
                ClientHelper.HandleFault(msgResponse);
            }

            return true;
        }

        #endregion

        #region CREATE

        public RmReference Create(RmGeneric newResource)
        {
            if (newResource == null)
                throw new ArgumentNullException("newResource");

            Message msgRequest = requestFactory.CreateCreateRequest(newResource);
            Message msgResponse = wsResourceFactoryClient.Create(msgRequest);
            if (msgResponse.IsFault)
            {
                ClientHelper.HandleFault(msgResponse);
            }

            ResponseCreate createResponse = responseFactory.CreateCreateResponse(msgResponse);

            try
            {
                RmReference reference = new RmReference(createResponse.ResourceCreated.EndpointReference.ReferenceProperties.ResourceReferenceProperty.Value);
                if (newResource.ContainsKey(new RmAttributeName(RmResource.AttributeNames.ObjectID.Name)))
                {
                    newResource[RmResource.AttributeNames.ObjectID.Name].Value = reference;
                }
                return reference;
            }
            catch (NullReferenceException)
            {
                return new RmReference();
            }
            catch (FormatException)
            {
                return new RmReference();
            }
        }

        #endregion

        #region GET

        public RmGeneric GenericGet(RmReference objectId)
        {
            return GenericGet(objectId, null, null);
        }

        public RmGeneric GenericGet(RmReference objectId, CultureInfo culture)
        {
            return GenericGet(objectId, culture, null);
        }

        public RmGeneric GenericGet(RmReference objectId, String[] attributes)
        {
            return GenericGet(objectId, null, attributes);
        }

        public RmGeneric GenericGet(RmReference objectId, CultureInfo culture, string[] attributes)
        {
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            return resourceFactory.CreateResource(prepareGetResponse(objectId, culture, attributes), true);
        }

        public RmResource Get(RmReference objectId)
        {
            return Get(objectId, null, null);
        }

        public RmResource Get(RmReference objectId, CultureInfo culture)
        {
            return Get(objectId, culture, null);
        }

        public RmResource Get(RmReference objectId, String[] attributes)
        {
            return Get(objectId, null, attributes);
        }

        public RmResource Get(RmReference objectId, CultureInfo culture, string[] attributes)
        {
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            return resourceFactory.CreateResource(prepareGetResponse(objectId, culture, attributes), false) as RmResource;
        }

        private ResponseGet prepareGetResponse(RmReference objectId, CultureInfo culture, string[] attributes)
        {
            bool haveSearchAttributes = true;
            if (attributes == null || attributes.Length == 0)
            {
                haveSearchAttributes = false;
            }

            Message msgRequest = requestFactory.CreateGetRequest(objectId, culture, attributes);
            Message msgResponse = wsResourceClient.Get(msgRequest);
            if (msgResponse.IsFault)
            {
                ClientHelper.HandleFault(msgResponse);
            }

            return responseFactory.CreateGetResponse(msgResponse, haveSearchAttributes);
        }

        #endregion

        #endregion

        #region Approvals - old version

        //public const string DefaultApprovalConfiguration = @"ServiceMultipleTokenBinding_ResourceFactory";

        //public void Approve(
        //    RmApproval approval,
        //    bool isApproved)
        //{
        //    Approve(approval, isApproved, DefaultApprovalConfiguration);
        //}

        //public void Approve(
        //    RmApproval approval,
        //    bool isApproved,
        //    string approvalConfiguration)
        //{
        //    WSResourceFactoryClient approvalClient = new WSResourceFactoryClient(approvalConfiguration, approval.ApprovalEndpointAddress);
        //    approvalClient.ClientCredentials.Windows.ClientCredential = this.ClientCredential;
        //    approvalClient.Approve(approval, isApproved);
        //}

        //public void Approve(
        //    RmApproval approval,
        //    bool isApproved,
        //    EndpointAddress address)
        //{
        //    Approve(approval, isApproved, address, DefaultApprovalConfiguration);
        //}

        //public void Approve(
        //    RmApproval approval,
        //    bool isApproved,
        //    EndpointAddress address,
        //    string approvalConfiguration)
        //{
        //    WSResourceFactoryClient approvalClient = new WSResourceFactoryClient(approvalConfiguration, address);
        //    approvalClient.ClientCredentials.Windows.ClientCredential = this.ClientCredential;
        //    approvalClient.Approve(approval, isApproved);
        //}

        #endregion

        #region Approvals - new version

        public void Approve(RmApproval approval, bool isApproved)
        {
            Constants.Bindings bindings = new Constants.Bindings();

            WSResourceFactoryClient approvalClient = new WSResourceFactoryClient(
                bindings.ServiceMultipleTokenBinding_ResourceFactory, 
                new EndpointAddress(approval.ApprovalEndpointAddress));
            approvalClient.ClientCredentials.Windows.ClientCredential = this.ClientCredential;
            approvalClient.Approve(approval, isApproved);
        }

        public void Approve(RmApproval approval, bool isApproved, EndpointAddress address)
        {
            Constants.Bindings bindings = new Constants.Bindings();

            WSResourceFactoryClient approvalClient = new WSResourceFactoryClient(bindings.ServiceMultipleTokenBinding_ResourceFactory, address);
            approvalClient.ClientCredentials.Windows.ClientCredential = this.ClientCredential;
            approvalClient.Approve(approval, isApproved);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.mexClient.Close();
            this.wsResourceClient.Close();
            this.wsResourceFactoryClient.Close();
            this.wsEnumerationClient.Close();

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
