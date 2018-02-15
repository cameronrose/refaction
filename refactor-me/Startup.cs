using System.Web.Http;
using Owin;
using Microsoft.Owin;
using log4net.Config;
using log4net;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Linq;
using refactor_me.Interfaces;
using refactor_me.Concrete;
using refactor_me.Domain;
using refactor_me.Formatters;
using AutoMapper;
using System;

[assembly: OwinStartup(typeof(refactor_me.Startup))]
namespace refactor_me
{
    public class Startup
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            // Configure, initialise log4net;
            XmlConfigurator.Configure();

            _log.Info("Entered Startup()");

            // Configure Web API for self-host
            var config = new HttpConfiguration();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Enable Cross-Origin Resource Sharing
            config.EnableCors();

            // Configure Autofac IoC
            var builder = new ContainerBuilder();

            RegisterTypes(builder);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            ConfigureWebApi(config);

            app.UseWebApi(config);

            _log.Info("Leaving Startup()");
        }

        /// <summary>
        /// Configure Routes and other WebApi settings
        /// </summary>
        /// <param name="config"></param>
        public void ConfigureWebApi(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;
            config.Formatters.Insert(0, new JsonRootFormatter());           

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                  name: "DefaultApi",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { id = RouteParameter.Optional }
              );
        }

        /// <summary>
        /// Configures all Dependency Injection type mappings
        /// </summary>
        /// <param name="builder"></param>
        public virtual void RegisterTypes(ContainerBuilder builder)
        {
            // Get all Profiles
            var profiles = from t in Assembly.GetExecutingAssembly().GetTypes()
                           where typeof(Profile).IsAssignableFrom(t)
                           select (Profile)Activator.CreateInstance(t);

            // For each Profile, include that profile in the MapperConfiguration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            // Create a mapper that will be used by the DI container
            var mapper = config.CreateMapper();

            // Register the DI interfaces with their implementation
            builder.RegisterInstance(mapper);

            builder.RegisterType<EntitiesDbContext>()
                .As<IEntitiesDbContext>();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>();
        }
    }
}