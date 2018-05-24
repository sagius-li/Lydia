using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
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
        // Get MEF assemblies
        [ImportMany("ResourceManagement", typeof(IOCGDSRepository))]
        //[ImportMany(typeof(IOCGDSRepository))]
        IEnumerable<Lazy<IOCGDSRepository, Dictionary<string, object>>> repos { get; set; }

        /// <summary>
        /// Get version number
        /// </summary>
        /// <returns>Current version number</returns>
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            // Get version from git
            string gitVersion = String.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("OCGDS." + "version.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                gitVersion = reader.ReadLine();
            }

            return Ok(gitVersion);
        }

        /// <summary>
        /// Get information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("info")]
        public IHttpActionResult GetInfo(string name)
        {
            Lazy<IOCGDSRepository> repo = RepositoryManager.GetRepository(repos, name);

            if (repo != null)
            {
                return Ok($"Using Repository: {repo.Value.GetType()}");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
