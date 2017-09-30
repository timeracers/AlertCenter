using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AlertCenter.Middleware;
using JwtAuthenticator;
using AlertCenter.Security;
using AlertCenter.Database;
using AlertCenter.Emails;
using AlertCenter.Subscriptions;
using AlertCenter.Alerts;
using Swashbuckle.AspNetCore.Swagger;
using AlertCenter.Controllers.Emails;
using System.Reflection;

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
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => {
                options.ModelBinderProviders.Insert(1, new BodyBinderProvider());
                options.ModelBinderProviders.Insert(0, new HeaderBinderProvider());
                options.Filters.Add(new Validator());
                options.Filters.Add(new HttpExceptionHandler());
            }).AddApplicationPart(typeof(AlertController).GetType().GetTypeInfo().Assembly)
                .AddApplicationPart(typeof(EmailController).GetType().GetTypeInfo().Assembly)
                .AddApplicationPart(typeof(SubscriptionController).GetType().GetTypeInfo().Assembly)
                .AddControllersAsServices();
            services.AddSingleton(new AuthenticationWrapper(HmacEncryptor.CreateSha256(Config.Secret)));
            var db = new SqlDatabase(new NpgsqlConnectionFactory(Config.ConnectionString));
            services.AddSingleton(new EmailGateway(db));
            services.AddSingleton(new SubscriptionGateway(db));
            services.AddSingleton(new AlertGateway(db, new EmailClient()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("alertcenter", new Info { });
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/alertcenter/swagger.json", "Alert Center");
            });
        }
    }
}
