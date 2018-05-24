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
    /// Controller to work with resources using basic authentication
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/resource/basic")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class ResourceBasicAuthController : ApiController
    {
        // Get MEF assemblies
        [ImportMany("ResourceManagement", typeof(IOCGDSRepository))]
        IEnumerable<Lazy<IOCGDSRepository, Dictionary<string, object>>> repos { get; set; }
        
        /// <summary>
        /// Get Resource by ObjectID
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="getPermission"></param>
        /// <param name="getResolved"></param>
        /// <returns>DSResource</returns>
        [HttpGet]
        [Route("get/id")]
        public IHttpActionResult GetResourceByID(string connectionInfo, string id,
            [FromUri] string[] attributes = null, bool getPermission = false, bool getResolved = false)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(connectionInfo);

                    DSResource rs = repo.Value.GetResourceByID(ci, id,
                        (attributes == null || attributes.Length == 0) ?
                            new string[] { "DisplayName" } : attributes, false, false);
                    return Ok(rs);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception exp)
            {
                return InternalServerError(exp);
            }
        }
    }
}
