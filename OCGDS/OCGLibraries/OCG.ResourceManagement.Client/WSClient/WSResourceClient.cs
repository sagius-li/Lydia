using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

using OCG.ResourceManagement.Client.WSProxy;

namespace OCG.ResourceManagement.Client.WSClient
{
    public class WSResourceClient : ClientBase<IResource>, IResource
    {
        #region Constructor

        public WSResourceClient()
            : base()
        {
        }

        public WSResourceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public WSResourceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WSResourceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WSResourceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        #endregion

        #region IResource Implementation

        public Message Get(Message request)
        {
            IResource channel = base.ChannelFactory.CreateChannel();
            using (channel as IDisposable)
            {
                return channel.Get(request);
            }
        }

        public Message Put(Message request)
        {
            IResource channel = base.ChannelFactory.CreateChannel();
            using (channel as IDisposable)
            {
                return channel.Put(request);
            }
        }

        public Message Delete(Message request)
        {
            IResource channel = base.ChannelFactory.CreateChannel();
            using (channel as IDisposable)
            {
                return channel.Delete(request);
            }
        }

        #endregion
    }
}
