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
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="id">[Required] ObjectID</param>
        /// <param name="attributesToGet">[Optional(Default:["DisplayName"])] Attributes to Get</param>
        /// <param name="includePermission">[Optional(Default:False)] Inculde Permission</param>
        /// <param name="cultureKey">[Optional(Default:127)] Culture Key</param>
        /// <param name="resolveID">[Optional(Default:False)] Resolve Reference Attributes</param>
        /// <param name="deepResolve">[Optional(Default:False)] Resolve nested Reference Attributes</param>
        /// <param name="attributesToResolve">[Optional(Default:["DisplayName"])] Attributes to Resolve)]</param>
        /// <returns>DSResource</returns>
        [HttpGet]
        [Route("get/id")]
        public IHttpActionResult GetResourceByID(
            string connectionInfo, string id, [FromUri] string[] attributesToGet = null, 
            bool includePermission = false, int cultureKey = 127, bool resolveID = false, bool deepResolve = false, 
            [FromUri] string[] attributesToResolve = null)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(connectionInfo);
                    ResourceOption ro = new ResourceOption(ci, cultureKey, resolveID, deepResolve, attributesToResolve);

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

        /// <summary>
        /// Get Resource by XPath Query
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="query">[Required] XPath Query</param>
        /// <param name="attributesToGet">[Optional(Default:["DisplayName"])] Attributes to Get</param>
        /// <param name="pageSize">[Optional(Default:0)] Page Size, if set to 0, no Paging will be made</param>
        /// <param name="index">[Optional(Default:0)] Start Index of Paging</param>
        /// <param name="cultureKey">[Optional(Default:127)] Culture Key</param>
        /// <param name="resolveID">[Optional(Default:False)] Resolve Reference Attributes</param>
        /// <param name="deepResolve">[Optional(Default:False)] Resolve nested Reference Attributes</param>
        /// <param name="attributesToResolve">[Optional(Default:["DisplayName"])] Attributes to Resolve)]</param>
        /// <param name="attributesToSort">[Optional(Default:null)] Attributes to sort)]</param>
        /// <returns>DSResourceSet</returns>
        [HttpGet]
        [Route("get/query")]
        public IHttpActionResult GetResourceByQuery(
            string connectionInfo, string query, [FromUri] string[] attributesToGet = null, 
            int pageSize = 0, int index = 0, int cultureKey = 127, bool resolveID = false, bool deepResolve = false, 
            [FromUri] string[] attributesToResolve = null, [FromUri] string[] attributesToSort = null)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(connectionInfo);
                    ResourceOption ro = new ResourceOption(
                        ci, cultureKey, resolveID, deepResolve, attributesToResolve, attributesToSort);

                    DSResourceSet rss = repo.Value.GetResourceByQuery(
                        query, 
                        (attributesToGet == null || attributesToGet.Length == 0) ? new string[] { "DisplayName" } : attributesToGet, 
                        pageSize,
                        index,
                        ro);

                    return Ok(rss);
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

        /// <summary>
        /// Get Resource Count
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="query">[Required] XPath Query to filter Resources</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/count")]
        public IHttpActionResult GetResourceCount(string connectionInfo, string query)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    int count = repo.Value.GetResourceCount(query);

                    return Ok(count);
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

        /// <summary>
        /// Delete Resource with the given Object ID
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="id">[Required] ObjectID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult DeleteResource(string connectionInfo, string id)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    repo.Value.DeleteResource(id);

                    return Ok();
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

        /// <summary>
        /// Create Resource
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="resource">[Required] Resource to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateResource(string connectionInfo, DSResource resource)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    string id = repo.Value.CreateResource(resource);

                    return Ok(id);
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

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="resource">[Required] Resource to create</param>
        /// <param name="isDelta">[Optional] Only update Attributes with Dirty-Flag</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateResource(string connectionInfo, DSResource resource, bool isDelta = false)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    string id = repo.Value.UpdateResource(resource, isDelta);

                    if (id.Equals("AuthorizationRequired"))
                    {
                        return Content(HttpStatusCode.PartialContent, id);
                    }

                    return Ok(id);
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

        /// <summary>
        /// Add Values to a multivalued Attribute
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="objectID">[Required] ObjectID of the Resource to add Values</param>
        /// <param name="attributeName">[Required] Attribute to add Values</param>
        /// <param name="valuesToAdd">[Required] Values to add</param>
        /// <returns></returns>
        [HttpPost]
        [Route("values/add")]
        public IHttpActionResult AddValuesToResource(string connectionInfo, string objectID, string attributeName, [FromUri] string[] valuesToAdd)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    string id = repo.Value.AddValuesToResource(objectID, attributeName, valuesToAdd);

                    if (id.Equals("AuthorizationRequired"))
                    {
                        return Content(HttpStatusCode.PartialContent, id);
                    }

                    return Ok(id);
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

        /// <summary>
        /// Remove Values from a multivalued Attribute
        /// </summary>
        /// <param name="connectionInfo">[Required] Connection Info ex. baseaddress://localhost:5725;domain:contoso;username:mimadmin; password:E5AkXRT0VoCo3JSc0oc81A==</param>
        /// <param name="objectID">[Required] ObjectID of the Resource to remove Values from</param>
        /// <param name="attributeName">[Required] Attribute to remove Values from</param>
        /// <param name="valuesToRemove">[Required] Values to remove</param>
        /// <returns></returns>
        [HttpPost]
        [Route("values/remove")]
        public IHttpActionResult RemoveValuesFromResource(string connectionInfo, string objectID, string attributeName, [FromUri] string[] valuesToRemove)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    string id = repo.Value.RemoveValuesFromResource(objectID, attributeName, valuesToRemove);

                    if (id.Equals("AuthorizationRequired"))
                    {
                        return Content(HttpStatusCode.PartialContent, id);
                    }

                    return Ok(id);
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
