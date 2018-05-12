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
    [RoutePrefix("api/cosmos")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class CosmosController : ApiController
    {
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            return Ok("Cosmos DB Operations");
        }
    }
}
