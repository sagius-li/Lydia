using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    public class RequestPut : BaseTransferRequest
    {
        public ModifyRequest ModifyRequest;
    }
}
