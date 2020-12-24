using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SettingsProxyAPI.AppCode.Auth;
using SettingsProxyAPI.AppCode.Swagger;
using SettingsProxyAPI.Business.Impl;
using SettingsProxyAPI.Business.Interfaces;
using SettingsProxyAPI.Models;
using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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
            services.AddDbContext<UsersLivetrackerContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("UsersLivetrackerConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserKeywordsManager, UserKeywordsManager>();
            services.AddSingleton<IKeywordProvider>(
                sp => new KeywordProvider(remoteKeywordServiceUri: Configuration["RemoteKeywordServiceUri"]));

            services.AddAuthentication(opts =>
            {
                opts.AddScheme<SettingsAuthenticationHandler>(
                    SettingsAuthenticationScheme.SchemeName, SettingsAuthenticationScheme.SchemeName);
            });

            services.AddOpenApiDocument(conf =>
            {
                conf.AllowReferencesWithProperties = true;
                conf.AlwaysAllowAdditionalObjectProperties = true;

                conf.OperationProcessors.Add(new AuthHeaderOperationProcessor());
                conf.PostProcess = doc =>
                {
                    doc.Info.Title = "Settings for github-livetracker";
                    doc.Info.Description = "Settings service for github-livetracker";
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

            app.UseMiddleware<WebSocketAuthMiddleware>();

            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/keywords")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                        IUserKeywordsManager userKeywordsManager = app.ApplicationServices.GetService<IUserKeywordsManager>();
                        IKeywordProvider keywordProvider = app.ApplicationServices.GetService<IKeywordProvider>();
                        
                        await Observable.Merge(
                            userKeywordsManager.OnUserConnected(int.Parse(context.User.Identity.Name)),
                            Observable.Create<string>(async observer =>
                            {

                                var buffer = new byte[1024 * 4];
                                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                KeywordRequest receivedRequest;
                                while (!result.EndOfMessage)
                                {
                                    buffer = new byte[1024 * 4];
                                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                    receivedRequest = JsonConvert.DeserializeObject<KeywordRequest>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                                    observer.OnNext(receivedRequest.Keyword);
                                }
                            })
                            .SelectMany(k => keywordProvider.GetOneKeyword(k)))
                            .LastAsync();
                    }
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUi3(conf =>
            {
                conf.DocumentTitle = "Registration for github-livetracker";
            });
            app.UseOpenApi();
        }
    }
}
