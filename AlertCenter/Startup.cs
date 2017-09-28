using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Routing;
using AlertCenter.Middleware;
using JwtAuthenticator;

namespace AlertCenter
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options => {
                options.ModelBinderProviders.Insert(1, new BodyBinderProvider());
                options.ModelBinderProviders.Insert(0, new HeaderBinderProvider());
                options.Filters.Add(new Validator());
                options.Filters.Add(new HttpExceptionHandler());
            });
            services.AddSingleton(new AuthenticationWrapper(HmacEncryptor.CreateSha256(Config.Secret)));
            var db = new SqlDatabase(new NpgsqlConnectionFactory(Config.ConnectionString));
            services.AddSingleton(new UnnamedClass1(db));
            services.AddSingleton(new UnnamedClass2(db));
            services.AddSingleton(new UnnamedClass3(db));
            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Alert Center", Version = "v1" });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMvc();
            /*app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });*/
        }
    }
}
