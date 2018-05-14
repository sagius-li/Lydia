using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

using OCG.ResourceManagement.Client.WSProxy;

namespace OCG.ResourceManagement.Client.WSClient
{
    public class WSEnumerationClient : System.ServiceModel.ClientBase<IEnumerate>, IEnumerate
    {
        public WSEnumerationClient()
            : base()
        {
        }

        public WSEnumerationClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public WSEnumerationClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WSEnumerationClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WSEnumerationClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public Message Enumerate(Message request)
        {
            IEnumerate channel = base.ChannelFactory.CreateChannel();
            using (channel as IDisposable)
            {
                return channel.Enumerate(request);
            }
        }

        public Message Pull(Message request)
        {
            IEnumerate channel = base.ChannelFactory.CreateChannel();
            using (channel as IDisposable)
            {
                return channel.Pull(request);
            }
        }
    }
}
