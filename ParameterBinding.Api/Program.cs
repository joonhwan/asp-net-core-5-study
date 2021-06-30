using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParameterBinding.Api.Repositories;
using Serilog;

namespace ParameterBinding.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var contexts = scope.ServiceProvider.GetRequiredService<ModelBindingContexts>();
                contexts.Database.EnsureCreated();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .UseSerilog((context, configuration) =>
                // {
                //     configuration
                //         .Enrich.FromLogContext()
                //         .Enrich.WithThreadId()
                //
                //         .WriteTo.Console(
                //             outputTemplate:
                //             "[{Timestamp:HH:mm:ss.fff} {Level:u4}] [{ThreadId:02}] {Message:lj} {NewLine}{Exception} ");
                //         ;
                // })
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}