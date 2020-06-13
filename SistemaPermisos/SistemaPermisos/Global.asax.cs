using Autofac;
using Autofac.Integration.Mvc;
using SistemaPermisos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SistemaPermisos
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            /// Add call
            //RegisterAutofac();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //private void RegisterAutofac()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterControllers(Assembly.GetExecutingAssembly());
        //    builder.RegisterSource(new ViewRegistrationSource());

        //    // manual registration of types;
        //    Repository<ROL> rol = new Repository<ROL>(() => new ApplicationDbContext());
        //    builder.Register< Repository<ROL>> (a => rol);

        //    Repository<USUARIO> user = new Repository<USUARIO>(() => new ApplicationDbContext());
        //    builder.Register<Repository<USUARIO>>(a => user);


        //    var container = builder.Build();

        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        //}

    }
}
