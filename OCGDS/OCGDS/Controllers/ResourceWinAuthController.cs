using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCGDS.Controllers
{
    /// <summary>
    /// Controller to work with resources using windows authentication
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/resource/win")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class ResourceWinAuthController : ApiController
    {
        // Get MEF assemblies
        [ImportMany("ResourceManagement", typeof(IOCGDSRepository))]
        IEnumerable<Lazy<IOCGDSRepository, Dictionary<string, object>>> repos { get; set; }
        
        /// <summary>
        /// Get Resource by ObjectID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributesToGet"></param>
        /// <param name="includePermission"></param>
        /// <param name="cultureKey"></param>
        /// <param name="resolveID"></param>
        /// <param name="attributesToResolve"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get/id")]
        public IHttpActionResult GetResourceByID(
            string id, [FromUri] string[] attributesToGet = null, bool includePermission = false, 
            int cultureKey = 127, bool resolveID = false, [FromUri] string[] attributesToResolve = null)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ResourceOption ro = new ResourceOption(new ConnectionInfo(), cultureKey, resolveID, attributesToResolve);

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }


    }
}
