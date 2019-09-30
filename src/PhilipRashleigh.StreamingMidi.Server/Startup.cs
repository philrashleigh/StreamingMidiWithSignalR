using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhilipRashleigh.StreamingMidi.Core;
using PhilipRashleigh.StreamingMidi.Server.Hubs;

namespace PhilipRashleigh.StreamingMidi.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new MidiFileManager(Configuration["MidiFileDirectory"]));
            
            services.AddCors();
            services.AddSignalR().AddHubOptions<MidiHub>(options =>
            {    
                options.EnableDetailedErrors = true;
            });
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
                    .WithOrigins(Configuration["ClientUrl"])
                    .AllowCredentials();
            });

            app.UseEndpoints(options =>
            {
                options.MapHub<MidiHub>("/midi");
            });
        }
    }
}