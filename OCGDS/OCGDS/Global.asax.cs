using OCGDS.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace OCGDS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MefConfig.RegisterMef();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
