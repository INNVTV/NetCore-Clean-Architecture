using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Common;
using Core.Common.Configuration;
using Core.Common.Logging;
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
using Core.Common.Persistence.DocumentDatabase;

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
            /* Default .Net Core IServiceCollection is used in this example.
             * You can switch to Autofaq, Ninject or any DI Container of your choice.
             * 
             * Autofaq allows for automatic registration of Interfaces by using "Assembly Scanning":
             *     - builder.RegisterAssemblyTypes(dataAccess)
             *         .Where(t => t.Name.EndsWith("Repository"))
             *         .AsImplementedInterfaces();
             */

            #region Create our custom dependancies

            #region Initialize our ICoreConfiguration object

            ICoreConfiguration coreConfiguration = new CoreConfiguration(Configuration);

            #endregion

            #region Initialize our ICoreLogger

            ICoreLogger coreLogger = new CoreLogger();

            #endregion

            #region Initialize our IDocumentContext

            IDocumentContext documentContext = new DocumentContext(Configuration);

            #endregion

            #endregion

            #region Register our dependencies

            // Register default WebAPI dependancies
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            /* REGISTER MEDIATR for CQRS Pattern ---------------
             * 
             * MediatR will automatically search your assemblies for IRequest and IRequestHandler implementations
             * and will build up your library of commands and queries for use throught your project. */
            services.AddMediatR();

            #region Inject our custom dependancies into the default WebAPI provider

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ICoreConfiguration>(coreConfiguration);
            services.AddSingleton<ICoreLogger>(coreLogger);

            services.AddSingleton<IDocumentContext>(documentContext);

            #endregion

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
