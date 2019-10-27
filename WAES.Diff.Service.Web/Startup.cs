using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;
using WAES.Diff.Service.Common;
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
                config.SwaggerDoc(Constants.API_VERSION, new OpenApiInfo
                {
                    Version = Constants.API_VERSION,
                    Title = Constants.API_NAME,
                    Description = Constants.API_DESCRIPTION,
                    Contact = new OpenApiContact
                    {
                        Name = Constants.CONTACT_NAME,
                        Email = Constants.CONTACT_EMAIL,
                        Url = new Uri(Constants.CONTACT_GITHUB),
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint(Constants.API_ENDPOINT, Constants.API_NAME);
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
