using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCGDS.Controllers
{
    /// <summary>
    /// Controller to handle generic jobs
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/generic")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class GenericController : ApiController
    {
        [ImportMany("MIMResource", typeof(IOCGDSRepository))]
        IEnumerable<Lazy<IOCGDSRepository, Dictionary<string, object>>> repos { get; set; }

        /// <summary>
        /// Get version number
        /// </summary>
        /// <returns>Current version number</returns>
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            if (repos != null && repos.Count() > 0)
            {
                return Ok($"Using Repository: {repos.First().Value.GetType()}");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
