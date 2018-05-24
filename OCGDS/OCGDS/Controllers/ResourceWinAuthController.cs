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
        /// <param name="attributes"></param>
        /// <param name="getPermission"></param>
        /// <param name="getResolved"></param>
        /// <returns>DSResource</returns>
        [Authorize]
        [HttpGet]
        [Route("get/id")]
        public IHttpActionResult GetResourceByID(string id, 
            [FromUri] string[] attributes = null, bool getPermission = false, bool getResolved = false)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    DSResource rs = repo.Value.GetResourceByID(null, id, 
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
