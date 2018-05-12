using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCGDS.ExtensionCosmos.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/graph")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class GraphController : ApiController
    {
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            return Ok("Graph API Operations");
        }
    }
}
