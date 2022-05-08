using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PisscordServer.Storage;

namespace PisscordServer {
    public class Program {

        public static Dictionary<string, string> Config;
        public static bool Debug = true;
        public static IStorageMethod Storage;
        public static string WwwAuthHeader = "Bearer realm=\"PisscordAuthentication\"";

        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}