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
    /// Controller to work with resources using portal admin authentication
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/resource/admin")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class ResourceAdminAuthController : ApiController
    {
        // Get MEF assemblies
        [ImportMany("ResourceManagement", typeof(IOCGDSRepository))]
        IEnumerable<Lazy<IOCGDSRepository, Dictionary<string, object>>> repos { get; set; }

        private string encryptionKey = string.Empty;
        private string adminConnection = string.Empty;

        /// <summary>
        /// Constructure
        /// </summary>
        public ResourceAdminAuthController()
        {
            adminConnection = $"baseaddress:{ConfigManager.GetAppSetting("FIMServiceUrl", "//localhost:5725")};domain:{ConfigManager.GetAppSetting("FIMPortalAdminAccountDomain", string.Empty)};username:{ConfigManager.GetAppSetting("FIMPortalAdminAccountName", string.Empty)};password:{ConfigManager.GetAppSetting("FIMPortalAdminAccountPWD", string.Empty)}";
            encryptionKey = ConfigManager.GetAppSetting("EncryptionKey", string.Empty);
        }

        /// <summary>
        /// Get Resource by ObjectID
        /// </summary>
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
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
            string encryptionKey, string id, [FromUri] string[] attributesToGet = null,
            bool includePermission = false, int cultureKey = 127, bool resolveID = false, bool deepResolve = false,
            [FromUri] string[] attributesToResolve = null)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
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
            string encryptionKey, string query, [FromUri] string[] attributesToGet = null,
            int pageSize = 0, int index = 0, int cultureKey = 127, bool resolveID = false, bool deepResolve = false,
            [FromUri] string[] attributesToResolve = null, [FromUri] string[] attributesToSort = null)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
        /// <param name="query">[Required] XPath Query to filter Resources</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/count")]
        public IHttpActionResult GetResourceCount(string encryptionKey, string query)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
                    ResourceOption ro = new ResourceOption(ci);

                    int count = repo.Value.GetResourceCount(query, ro);

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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
        /// <param name="id">[Required] ObjectID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult DeleteResource(string encryptionKey, string id)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
                    ResourceOption ro = new ResourceOption(ci);

                    repo.Value.DeleteResource(id, ro);

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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
        /// <param name="resource">[Required] Resource to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateResource(string encryptionKey, DSResource resource)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
                    ResourceOption ro = new ResourceOption(ci);

                    string id = repo.Value.CreateResource(resource, ro);

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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
        /// <param name="resource">[Required] Resource to create</param>
        /// <param name="isDelta">[Optional] Only update Attributes with Dirty-Flag</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateResource(string encryptionKey, DSResource resource, bool isDelta = false)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
                    ResourceOption ro = new ResourceOption(ci);

                    string id = repo.Value.UpdateResource(resource, isDelta, ro);

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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
        /// <param name="objectID">[Required] ObjectID of the Resource to add Values</param>
        /// <param name="attributeName">[Required] Attribute to add Values</param>
        /// <param name="valuesToAdd">[Required] Values to add</param>
        /// <returns></returns>
        [HttpPost]
        [Route("values/add")]
        public IHttpActionResult AddValuesToResource(string encryptionKey, string objectID, string attributeName, [FromUri] string[] valuesToAdd)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
                    ResourceOption ro = new ResourceOption(ci);

                    string id = repo.Value.AddValuesToResource(objectID, attributeName, valuesToAdd, ro);

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
        /// <param name="encryptionKey">[Required] Required to use admin access</param>
        /// <param name="objectID">[Required] ObjectID of the Resource to remove Values from</param>
        /// <param name="attributeName">[Required] Attribute to remove Values from</param>
        /// <param name="valuesToRemove">[Required] Values to remove</param>
        /// <returns></returns>
        [HttpPost]
        [Route("values/remove")]
        public IHttpActionResult RemoveValuesFromResource(string encryptionKey, string objectID, string attributeName, [FromUri] string[] valuesToRemove)
        {
            try
            {
                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (!encryptionKey.Equals(this.encryptionKey))
                {
                    return InternalServerError(new Exception("Invalid Encryption Key"));
                }

                if (repo != null)
                {
                    ConnectionInfo ci = ConnectionInfo.BuildConnectionInfo(this.adminConnection);
                    ResourceOption ro = new ResourceOption(ci);

                    string id = repo.Value.RemoveValuesFromResource(objectID, attributeName, valuesToRemove, ro);

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