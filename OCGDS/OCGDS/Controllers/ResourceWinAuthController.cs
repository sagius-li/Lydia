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
        /// <param name="id">[Required] ObjectID</param>
        /// <param name="attributesToGet">[Optional(Default:["DisplayName"])] Attributes to Get</param>
        /// <param name="includePermission">[Optional(Default:False)] Inculde Permission</param>
        /// <param name="cultureKey">[Optional(Default:127)] Culture Key</param>
        /// <param name="resolveID">[Optional(Default:False)] Resolve Reference Attributes</param>
        /// <param name="deepResolve">[Optional(Default:False)] Resolve nested Reference Attributes</param>
        /// <param name="attributesToResolve">[Optional(Default:["DisplayName"])] Attributes to Resolve)]</param>
        /// <returns>DSResource</returns>
        [Authorize]
        [HttpGet]
        [Route("get/id")]
        public IHttpActionResult GetResourceByID(
            string id, [FromUri] string[] attributesToGet = null, bool includePermission = false, 
            int cultureKey = 127, bool resolveID = false, bool deepResolve = false, 
            [FromUri] string[] attributesToResolve = null)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ResourceOption ro = new ResourceOption(new ConnectionInfo(), cultureKey, resolveID, deepResolve, attributesToResolve);

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

        /// <summary>
        /// Get Resource by XPath Query
        /// </summary>
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
        [Authorize]
        [HttpGet]
        [Route("get/query")]
        public IHttpActionResult GetResourceByQuery(
            string query, [FromUri] string[] attributesToGet = null, int pageSize = 0, int index = 0, 
            int cultureKey = 127, bool resolveID = false, bool deepResolve = false, 
            [FromUri] string[] attributesToResolve = null, [FromUri] string[] attributesToSort = null)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

                Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, "MIMResource");

                if (repo != null)
                {
                    ResourceOption ro = new ResourceOption(
                        new ConnectionInfo(), cultureKey, resolveID, deepResolve, attributesToResolve, attributesToSort);

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary>
        /// Get Resource Count
        /// </summary>
        /// <param name="query">[Required] XPath Query to filter Resources</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get/count")]
        public IHttpActionResult GetResourceCount(string query)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary>
        /// Delete Resource with the given Object ID
        /// </summary>
        /// <param name="id">[Required] ObjectID</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult DeleteResource(string id)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary>
        /// Create Resource
        /// </summary>
        /// <param name="resource">[Required] Resource to create</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateResource(DSResource resource)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="resource">[Required] Resource to create</param>
        /// <param name="isDelta">[Optional] Only update Attributes with Dirty-Flag</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateResource(DSResource resource, bool isDelta = false)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary>
        /// Add Values to a multivalued Attribute
        /// </summary>
        /// <param name="objectID">[Required] ObjectID of the Resource to add Values</param>
        /// <param name="attributeName">[Required] Attribute to add Values</param>
        /// <param name="valuesToAdd">[Required] Values to add</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("values/add")]
        public IHttpActionResult AddValuesToResource(string objectID, string attributeName, [FromUri] string[] valuesToAdd)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

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
            finally
            {
                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary>
        /// Remove Values from a multivalued Attribute
        /// </summary>
        /// <param name="objectID">[Required] ObjectID of the Resource to remove Values from</param>
        /// <param name="attributeName">[Required] Attribute to remove Values from</param>
        /// <param name="valuesToRemove">[Required] Values to remove</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("values/remove")]
        public IHttpActionResult RemoveValuesFromResource(string objectID, string attributeName, [FromUri] string[] valuesToRemove)
        {
            WindowsImpersonationContext wic = null;
            try
            {
                wic = ((WindowsIdentity)User.Identity).Impersonate();

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
