using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;

namespace OCG.ResourceManagement.Client.WSProxy
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute(Constants.ServiceModel.AssemblyName, Constants.ServiceModel.AssemblyVersion)]
    [System.ServiceModel.ServiceContractAttribute(Namespace = Constants.WsTransfer.Namespace, ConfigurationName = Constants.WsTransfer.IMEX)]
    public interface IMEX
    {
        [System.ServiceModel.OperationContractAttribute(ProtectionLevel = System.Net.Security.ProtectionLevel.None, Action = Constants.WsTransfer.GetAction, ReplyAction = Constants.WsTransfer.GetResponseAction)]
        Message Get(Message request);
    }
}
