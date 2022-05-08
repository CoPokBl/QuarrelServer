using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PisscordServer.Storage;
using RayKeys.Misc;

namespace PisscordServer {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
            
            // Custom config because ASP.NETs config system is stupid
            try {
                // Don't bother creating a default config because they get it when they build
                if (!File.Exists("config.json")) {
                    throw new FileNotFoundException("config.json not found, please create it or rebuild the project");
                }
                // Get config data
                string data = File.ReadAllText("config.json");
                Dictionary<string, string> configDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
                Program.Config = configDict;
                Logger.Info("Loaded config");
            }
            catch (Exception e) {
                // Failed to load config
                Logger.Error($"Failed to load config {e.Message}");
                throw new Exception("Failed to load config");
            }
            
            Logger.Init((LogLevel) int.Parse(Program.Config["LoggingLevel"]));
            
            // Load storage
            Program.Storage = Program.Config["StorageMethod"] switch {
                "RAM" => new RamStorage(),
                _ => throw new ArgumentException("Invalid storage method in config")
            };

            // Init storage
            try {
                Program.Storage.Initialize();
            }
            catch (Exception e) {
                Logger.Error("Failed to initialize storage");
                Logger.Error(e.Message);
                throw;
            }
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}