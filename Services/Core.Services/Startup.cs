using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using MediatR;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.StorageAccount;
using Core.Infrastructure.Persistence.RedisCache;
using Core.Infrastructure.Services.Email;
using Core.Infrastructure.Pipeline;
using MediatR.Pipeline;
using System.Reflection;
using Core.Application;
using Core.Application.Accounts.Queries;
using Core.Application.Accounts.Commands;
using Serilog;

namespace Core.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Setup Serilog for Logging

            // Core is set up to use the global, statically accessible logger from Serilog.
            // It must be set up in the main entrpoint and does not require a DI container

            // Create a logger with configured sinks, enrichers, and minimum level
            // Serilog's global, statically accessible logger, is set via Log.Logger and can be invoked using the static methods on the Log class.

            // File Sink is commented out and can be replaced with Serilogs vast library of available sinks

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console() //<-- This will give us output to our Kestrel console
            //.WriteTo.File("_logs/log-.txt", rollingInterval: RollingInterval.Day) //<-- Write our logs to a local text file with rolling interval configuration
            .CreateLogger();

            Log.Information("The global logger has been configured.");
            Log.Information("Hello, Serilog!");

            #endregion

            /* -----------------------------------------------------------------
             * Default .Net Core IServiceCollection is used in this example.
             * You can switch to Autofaq, Ninject or any DI Container of your choice.
             * ------------------------------------------------------------------
             * Autofaq allows for automatic registration of Interfaces by using "Assembly Scanning":
             *     - builder.RegisterAssemblyTypes(dataAccess)
             *         .Where(t => t.Name.EndsWith("Repository"))
             *         .AsImplementedInterfaces();
             ---------------------------------------------------------------------*/


            #region Create our custom dependancies

            #region Initialize our ICoreConfiguration object

            ICoreConfiguration coreConfiguration = new CoreConfiguration(Configuration);

            #endregion

            #region Initialize our Persistence Layer objects

            IDocumentContext documentContext = new DocumentContext(Configuration);
            IStorageContext storageContext = new StorageContext(Configuration);
            IRedisContext redisContext = new RedisContext(Configuration);

            #endregion

            #region Initialize 3rd Party Service Dependencies

            IEmailService sendgridService = new SendGridEmailService(Configuration);

            #endregion

            #endregion

            #region Register our dependencies

            // Register default WebAPI dependancies
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()));


            #region Inject our custom dependancies into the default WebAPI provider

            // Configuration
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ICoreConfiguration>(coreConfiguration);

            // Persistence
            services.AddSingleton<IDocumentContext>(documentContext);
            services.AddSingleton<IStorageContext>(storageContext);
            services.AddSingleton<IRedisContext>(redisContext);

            // 3rd Party Services
            services.AddSingleton<IEmailService>(sendgridService);

            // TODO: Account/Platform Activity Logging
            // serviceCollection.AddSingleton<ICore(Account/Platform)ActivityLogger>(coreLogger);

            // MediatR Pipeline Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>)); //<-- Includes LoggingBehavior

            #endregion

            /* -----------------------------------------------------
             * REGISTER MEDIATR for CQRS Pattern 
             * ------------------------------------------------------
             * MediatR will automatically search your assemblies for IRequest and IRequestHandler implementations
             * and will build up your library of commands and queries for use throught your project.
             * 
             * Note: MediatR should be added LAST. */

            // For WebAPI we need to register each IRequest as a MediatR type into our DI Container:
            services.AddMediatR(typeof(CreateAccountCommand));
            services.AddMediatR(typeof(GetAccountListQuery));
            //services.AddMediatR();

            //Alternatively we use the entire assembly MediatR.Extensions.Microsoft.DependencyInjection package
            //services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(Core.Services.Startup).GetTypeInfo().Assembly);

            #endregion 

            // Initialize Core.Startup
            Core.Startup.Routines.Initialize();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
