using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;
using WAES.Diff.Service.DependencyResolution;
using WAES.Diff.Service.Infrastructure;

namespace WAES.Diff.Service.Web
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
            services.AddControllers();

            services.RegisterServices();

            services.AddDbContext<DiffServiceDbContext>(options => options.UseInMemoryDatabase(databaseName: "DiffService"));

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            //TODO Move to a helper
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WAES Diff Service",
                    Description = "A simple diff API",
                    Contact = new OpenApiContact
                    {
                        Name = "Valeria Infer",
                        Email = "valeinfer@gmail.com",
                        Url = new Uri("https://github.com/vrinfer"),
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                //TODO Move to a helper
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "WAES Diff Service");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
