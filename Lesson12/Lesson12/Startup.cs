using Lesson12.Crypto_Service.Src.Service.External;
using Lesson12.Crypto_Service.Src.Service.External.Utils;
using Lesson12.Lesson12.Crypto_Service_Idl.Src.Service;
using Lesson12.Price_Service.Src.Service.Impl;
using Lesson12.Price_Service_Idl.Src.Service;
using Lesson12.Sockets;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            services.AddWebSocketManager();

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
            services.AddTransient<WSHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/stream")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            WSHandler wsHandler = serviceProvider.GetService<WSHandler>();

                            var buffer = new byte[1024 * 4];

                            webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None)
                               .ToObservable()
                               .SelectMany(result => Observable.Return(Encoding.UTF8.GetString(buffer, 0, result.Count)))
                               .Do(onNext: m => buffer = new byte[1024 * 4])
                               .Let(wsHandler.Handle)
                               .Select(m => JsonConvert.SerializeObject(m))
                               .Select(m =>
                               {
                                   byte[] output = Encoding.UTF8.GetBytes(m as string);

                                   return webSocket.SendAsync(new ArraySegment<byte>(output, 0, output.Length), WebSocketMessageType.Text, false, CancellationToken.None);
                               })
                               .Subscribe(onNext: t => Debug.WriteLine("Received"));
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });
            //app.MapWebSocketManager("/stream", serviceProvider.GetService<WSHandler>());
            app.UseStaticFiles();

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
