using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

using OCG.ResourceManagement.Client.WSProxy;
using OCG.ResourceManagement.Client.WSMessage;

using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Client.WSClient
{
    public class WSResourceFactoryClient : System.ServiceModel.ClientBase<IResourceFactory>, IResourceFactory
    {
        public WSResourceFactoryClient()
            : base()
        {
        }

        public WSResourceFactoryClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public WSResourceFactoryClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WSResourceFactoryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WSResourceFactoryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public Message Create(Message request)
        {
            IResourceFactory channel = base.ChannelFactory.CreateChannel();
            using (channel as IDisposable)
            {
                return channel.Create(request);
            }
        }


        public void Approve(RmApproval approval, bool isApproved)
        {
            // the AuthZ endpoint does not use the MS-WSTIM extensions.
            // Rather than create a whole new object model or adapt the serializer, I construct the XML manually:
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNamespaceManager nsManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace(Constants.Rm.Prefix, Constants.Rm.Namespace);

            System.Xml.XmlElement approvalResponseElement = doc.CreateElement("rm:ApprovalResponse", Constants.Rm.Namespace);
            System.Xml.XmlElement approvalElement = doc.CreateElement("rm:Approval", Constants.Rm.Namespace);
            approvalElement.InnerText = approval.ObjectID.Value;
            approvalResponseElement.AppendChild(approvalElement);

            System.Xml.XmlElement approvalDecisionElement = doc.CreateElement("rm:Decision", Constants.Rm.Namespace);
            approvalDecisionElement.InnerText = isApproved ? @"Approved" : @"Rejected";
            approvalResponseElement.AppendChild(approvalDecisionElement);

            System.Xml.XmlElement objectTypenElement = doc.CreateElement("rm:ObjectType", Constants.Rm.Namespace);
            objectTypenElement.InnerText = "ApprovalResponse";
            approvalResponseElement.AppendChild(objectTypenElement);

            doc.AppendChild(approvalResponseElement);

            Message requestMessage = Message.CreateMessage(MessageVersion.Default, Constants.WsTransfer.CreateAction, approvalResponseElement);

            if (String.IsNullOrEmpty(approval.WorkflowInstance.Value))
            {
                throw new InvalidOperationException("The approval does not have an active workflow activity.");
            }

            ContextMessageHeader ctx = new ContextMessageHeader(approval.WorkflowInstance.Value);

            requestMessage.Headers.Add(ctx);

            // send the create request. If an error occurs, an exception is thrown.
            Message responseMessage = Create(requestMessage);
        }
    }
}
