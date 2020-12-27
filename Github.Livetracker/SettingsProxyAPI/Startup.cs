using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SettingsProxyAPI.Auth.Http;
using SettingsProxyAPI.Auth.WebSockets;
using SettingsProxyAPI.Keywords;
using SettingsProxyAPI.Swagger;
using UsersLivetrackerConfigDAL;
using UsersLivetrackerConfigDAL.Repos.Impl;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("UsersLivetrackerConnection");
            services.AddDbContext<UsersLivetrackerContext>(
                options => options.UseSqlServer(dbConnStr, sqlOptions => sqlOptions.EnableRetryOnFailure()));

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUserKeywordsRepository, UserKeywordsRepository>();
            services.AddSingleton<IKeywordInfoRepository, KeywordInfoRepository>();
            services.AddSingleton<IWebSocketsAuthHandler, WebSocketsAuthHandler>();
            
            services.AddSingleton<IKeywordUpdatesProvider>(sp => 
                new KeywordUpdatesProvider(sp.GetService<IKeywordInfoRepository>(), dbConnStr));

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = HttpAuthScheme.SchemeName;
                opts.AddScheme<HttpAuthHandler>(
                    HttpAuthScheme.SchemeName, HttpAuthScheme.SchemeName);
            });

            services.AddOpenApiDocument(conf =>
            {
                conf.AllowReferencesWithProperties = true;
                conf.AlwaysAllowAdditionalObjectProperties = true;

                conf.OperationProcessors.Add(new AuthHeaderOperationProcessor());
                conf.PostProcess = doc =>
                {
                    doc.Info.Title = "Keywords info from github-livetracker";
                    doc.Info.Description = "Keywords info service from github-livetracker";
                };
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();

            app.UseMiddleware<UserKeywordsMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUi3(conf =>
            {
                conf.DocumentTitle = "Keywords info from github-livetracker";
            });
            app.UseOpenApi();
        }
    }
}
