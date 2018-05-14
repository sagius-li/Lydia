using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.Client.XMLHandler;

namespace OCG.ResourceManagement.Client.WSMessage
{
    public class ResponseGet : BaseSearchResponse
    {
        public void ConvertFromBase(BaseSearchResponse bsr)
        {
            this.PartialAttributes = bsr.PartialAttributes;
        }
    }
}
