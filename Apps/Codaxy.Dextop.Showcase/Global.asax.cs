﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;

namespace Codaxy.Dextop.Showcase
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.IgnoreRoute("rpc.ashx");
            routes.IgnoreRoute("poll.ashx");
            routes.IgnoreRoute("lpoll.ashx");
            routes.IgnoreRoute("api.ashx");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            try
            {
                //AreaRegistration.RegisterAllAreas(); //slow on startup, not neccessary if areas are not used

                RegisterGlobalFilters(GlobalFilters.Filters);
                RegisterRoutes(RouteTable.Routes);

#if DEBUG
                var debug = true;
#else
                var debug = false;
#endif
                var app = new ShowcaseApplication
                {
                    Optimize = !debug,
                    PreprocessingEnabled = !debug
                };

                using (var bm = new Codaxy.Common.Benchmarking.BenchmarkStopwatch())
                {
                    app.Initialize();
                    Debug.WriteLine("Dextop application init time: " + bm.Elapsed.TotalMilliseconds + " ms");
                }
                DextopApplication.RegisterApplication(app);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                HttpRuntime.UnloadAppDomain();
                throw;
            }
        }

        protected void Application_End()
        {
            try
            {
                var app = DextopApplication.GetApplication();
                app.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}