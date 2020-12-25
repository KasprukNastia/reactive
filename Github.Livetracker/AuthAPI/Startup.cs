using AuthAPI.Tokens;
using AuthAPI.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsersLivetrackerConfigDAL;
using UsersLivetrackerConfigDAL.Repos.Impl;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace AuthAPI
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
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<ITokenHasher, TokenHasher>();
            services.AddScoped<IUserRegistrator, UserRegistrator>();

            services.AddOpenApiDocument(conf =>
            {
                conf.AllowReferencesWithProperties = true;
                conf.AlwaysAllowAdditionalObjectProperties = true;

                conf.PostProcess = doc =>
                {
                    doc.Info.Title = "Registration for github-livetracker";
                    doc.Info.Description = "Registration service for github-livetracker";
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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
