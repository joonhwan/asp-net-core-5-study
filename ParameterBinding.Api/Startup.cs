using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParameterBinding.Api.Middlewares;
using ParameterBinding.Api.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace ParameterBinding.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddSingleton<ILoggerFactory>(provider =>
            // {
            //     Log.Logger = new LoggerConfiguration()
            //         .MinimumLevel.Information()
            //         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //         .Enrich.FromLogContext()
            //         .WriteTo.Console()
            //         .CreateLogger();
            //     return new SerilogLoggerFactory(Log.Logger);
            // });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ParameterBinding.Api", Version = "v1"});
            });
            services.AddDbContext<ModelBindingContexts>(options =>
            {
                options.UseInMemoryDatabase("pets.db");
            });
            services.AddScoped<IPetRepository, PetRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParameterBinding.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}