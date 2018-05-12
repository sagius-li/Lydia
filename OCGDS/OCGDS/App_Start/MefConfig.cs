using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace OCGDS.App_Start
{
    public class MefControllerFactory : DefaultControllerFactory
    {
        private readonly CompositionContainer container;

        public MefControllerFactory(CompositionContainer compositionContainer)
        {
            container = compositionContainer;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            var export = container.GetExports(controllerType, null, null).SingleOrDefault();

            IController result;

            if (null != export)
            {
                result = export.Value as IController;
            }
            else
            {
                result = base.GetControllerInstance(requestContext, controllerType);
                container.ComposeParts(result);
            }

            return result;
        }
    }

    public class MefDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly CompositionContainer container;

        public MefDependencyResolver(CompositionContainer c)
        {
            container = c;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            var export = container.GetExports(serviceType, null, null).SingleOrDefault();

            if (null != export)
            {
                return export.Value as IHttpController;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var exports = container.GetExports(serviceType, null, null);
            var createdObjects = new List<object>();

            if (exports.Any())
            {
                foreach (var export in exports)
                {
                    createdObjects.Add(export.Value);
                }
            }

            return createdObjects;
        }

        public void Dispose()
        {
            ;
        }
    }

    public class MefHttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration configuration;
        private readonly CompositionContainer container;

        public MefHttpControllerSelector(HttpConfiguration c, CompositionContainer compositionContainer)
            : base(c)
        {
            configuration = c;
            container = compositionContainer;
        }

        private const string ControllerSuffix = "Controller";

        private Dictionary<string, Type> apiControllerTypes;

        private Dictionary<string, Type> ApiControllerTypes
        {
            get { return apiControllerTypes ?? (apiControllerTypes = GetControllerTypes()); }
        }

        private Dictionary<string, Type> GetControllerTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies.SelectMany(a => a.GetTypes().Where(
                t => !t.IsAbstract && t.Name.EndsWith(ControllerSuffix) && typeof(IHttpController).IsAssignableFrom(t)))
                .ToDictionary(t => t.Name.ToUpper(), t => t);

            return types;
        }

        private HttpControllerDescriptor GetApiController(HttpRequestMessage request)
        {
            try
            {
                var controllerName = base.GetControllerName(request);

                Type typeFound = null;

                ApiControllerTypes.TryGetValue((controllerName + ControllerSuffix).ToUpper(), out typeFound);

                return typeFound == null ? null : new HttpControllerDescriptor(configuration, controllerName, typeFound);
            }
            catch
            {
                return null;
            }

        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var retVal = GetApiController(request);

            if (retVal == null)
            {
                try
                {
                    return base.SelectController(request);
                }
                catch { return retVal; }

            }

            return retVal;
        }
    }

    public static class MefConfig
    {
        public static void RegisterMef()
        {
            var container = ConfigureContainer();

            ControllerBuilder.Current.SetControllerFactory(new MefControllerFactory(container));

            var dependencyResolver = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver;

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new MefDependencyResolver(container);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new MefHttpControllerSelector(GlobalConfiguration.Configuration, container));
        }

        private static CompositionContainer ConfigureContainer()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            // add all assemblies found in the same folder as the executing program
            catalog.Catalogs.Add(
                new DirectoryCatalog(
                    Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location
                    )
                )
            );

            // add all assemblies located in iis extensions folder
            string defaultExtensionLocation = HostingEnvironment.MapPath("~/extensions");
            if (Directory.Exists(defaultExtensionLocation))
            {
                catalog.Catalogs.Add(new DirectoryCatalog(defaultExtensionLocation));
            }

            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }
    }
}