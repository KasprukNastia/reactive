using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SettingsProxyAPI.Auth;
using SettingsProxyAPI.Keywords;
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
                options => options.UseSqlServer(dbConnStr));

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IKeywordRepository, KeywordRepository>();
            services.AddSingleton<IUserAuthHandler, UserAuthHandler>();
            
            services.AddSingleton<IKeywordProvider>(
                sp => new KeywordProvider(dbConnStr, sp.GetService<IKeywordRepository>()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();

            app.UseMiddleware<UserKeywordsMiddleware>();
        }
    }
}
