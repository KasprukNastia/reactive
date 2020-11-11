using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lesson12.Crypto_Service.Src.Service.External;
using Lesson12.Crypto_Service.Src.Service.External.Utils;
using Lesson12.Lesson12.Crypto_Service_Idl.Src.Service;
using Lesson12.Price_Service.Src.Service.Impl;
using Lesson12.Price_Service_Idl.Src.Service;
using Lesson12.Trade_Service.Src.Repository;
using Lesson12.Trade_Service.Src.Repository.impl;
using Lesson12.Trade_Service.Src.Service.impl;
using Lesson12.Trade_Service_Idl.Src.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Lesson12
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<CryptoCompareClient>();
            services.AddTransient<ICryptoService, CryptoCompareService>();
            services.AddTransient<IPriceService, DefaultPriceService>();
            services.AddTransient<IMessageUnpacker, PriceMessageUnpacker>();
            services.AddTransient<IMessageUnpacker, TradeMessageUnpacker>();
            services.AddTransient<ITradeRepository>(sp => 
                new H2TradeRepository(sp.GetService<ILogger<H2TradeRepository>>(), Configuration.GetConnectionString("TradesContext"))); // !!!
            services.AddTransient<ITradeRepository>(sp =>
                new MongoTradeRepository(sp.GetService<ILogger<MongoTradeRepository>>(), new MongoClient("mongodb://localhost:27017"))); // !!!
            services.AddTransient<ITradeService, DefaultTradeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
