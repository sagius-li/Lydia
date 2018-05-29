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
        /// <param name="attributesToGet"></param>
        /// <param name="includePermission"></param>
        /// <param name="cultureKey"></param>
        /// <param name="resolveID"></param>
        /// <param name="attributesToResolve"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/id")]
        public IHttpActionResult GetResourceByID(
            string connectionInfo, string id, [FromUri] string[] attributesToGet = null, 
            bool includePermission = false, int cultureKey = 127, bool resolveID = false, [FromUri] string[] attributesToResolve = null)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(connectionInfo);
                    ResourceOption ro = new ResourceOption(ci, cultureKey, resolveID, attributesToResolve);

                    DSResource rs = repo.Value.GetResourceByID(id, (attributesToGet == null || attributesToGet.Length == 0) ? new string[] { "DisplayName" } : attributesToGet, includePermission, ro);

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
