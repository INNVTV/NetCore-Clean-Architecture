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
            #region Create our dependancies

            #region Initialize our ICoreConfiguration object

            ICoreConfiguration coreConfiguration;
            coreConfiguration = Core.Common.Configuration.Initialize.InitializeCoreConfiguration(Configuration);

            #endregion

            #region Initialize our ICoreLogger

            ICoreLogger coreLogger = new CoreLogger();

            #endregion

            #endregion

            // Register default WebAPI dependancies (ServiceCollection is our DI container)
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            // Register Mediatr
            services.AddMediatR();

            #region Inject our custom dependancies into the default WebAPI provider

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ICoreConfiguration>(coreConfiguration);
            services.AddSingleton<ICoreLogger>(coreLogger);

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
