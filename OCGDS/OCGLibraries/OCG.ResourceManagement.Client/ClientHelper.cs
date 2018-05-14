using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

using OCG.ResourceManagement.Client.WSMessage;
using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client
{
    public class ClientHelper
    {
        public static void HandleFault(Message message)
        {
            MessageFault fault = MessageFault.CreateFault(message, Int32.MaxValue);
            throw System.ServiceModel.FaultException.CreateFault(fault,
                typeof(Faults.PermissionDeniedFault),
                typeof(Faults.AuthenticationRequiredFault),
                typeof(Faults.AuthorizationRequiredFault),
                typeof(Faults.EndpointUnavailable),
                typeof(Faults.FragmentDialectNotSupported),
                typeof(Faults.InvalidRepresentationFault),
                typeof(Faults.UnwillingToPerformFault),
                typeof(Faults.CannotProcessFilter),
                typeof(Faults.FilterDialectRequestedUnavailable),
                typeof(Faults.UnsupportedExpiration)
            );
        }

        public static void AddRmHeaders(BaseTransferRequest transferRequest, Message message)
        {
            if (transferRequest == null)
                return;

            if (transferRequest.ResourceLocaleProperty != null && String.IsNullOrEmpty(transferRequest.ResourceLocaleProperty) == false)
            {
                if (message.Headers.FindHeader(Constants.Rm.ResourceLocaleProperty, Constants.Rm.Namespace) < 0)
                {
                    MessageHeader newHeader = MessageHeader.CreateHeader(Constants.Rm.ResourceLocaleProperty, Constants.Rm.Namespace, transferRequest.ResourceLocaleProperty);
                    message.Headers.Add(newHeader);
                }
            }

            if (transferRequest.ResourceReferenceProperty != null && String.IsNullOrEmpty(transferRequest.ResourceReferenceProperty) == false)
            {
                if (message.Headers.FindHeader(Constants.Rm.ResourceReferenceProperty, Constants.Rm.Namespace) < 0)
                {
                    MessageHeader newHeader = MessageHeader.CreateHeader(Constants.Rm.ResourceReferenceProperty, Constants.Rm.Namespace, transferRequest.ResourceReferenceProperty);
                    message.Headers.Add(newHeader);
                }
            }

            if (transferRequest.ResourceTimeProperty != null && String.IsNullOrEmpty(transferRequest.ResourceTimeProperty) == false)
            {
                if (message.Headers.FindHeader(Constants.Rm.ResourceTimeProperty, Constants.Rm.Namespace) < 0)
                {
                    MessageHeader newHeader = MessageHeader.CreateHeader(Constants.Rm.ResourceTimeProperty, Constants.Rm.Namespace, transferRequest.ResourceTimeProperty);
                    message.Headers.Add(newHeader);
                }
            }
        }

        public static void AddImdaHeaders(BaseTransferRequest imdaRequest, Message message)
        {
            if (imdaRequest == null)
                return;
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (message.Headers.FindHeader(Constants.Imda.ExtensionHeaderName, Constants.Imda.Namespace) < 0)
            {
                MessageHeader newHeader = MessageHeader.CreateHeader(Constants.Imda.ExtensionHeaderName, Constants.Imda.Namespace, String.Empty, true);
                message.Headers.Add(newHeader);
            }
        }
    }
}
