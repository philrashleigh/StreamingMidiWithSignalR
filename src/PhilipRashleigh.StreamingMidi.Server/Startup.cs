using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhilipRashleigh.StreamingMidi.Server.Hubs;
using PhilipRashleigh.StreamingMidi.Server.Other;

namespace PhilipRashleigh.StreamingMidi.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {   
            services.AddCors();
            services.AddSignalR().AddHubOptions<MidiHub>(options =>
            {    
                options.EnableDetailedErrors = true;
            });

            services.AddSingleton(new AppState());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseCors(options =>
            {
                options
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:5001")
                    .AllowCredentials();
            });

            app.UseEndpoints(options =>
            {
                options.MapHub<MidiHub>("/midi");
            });
        }
    }
}