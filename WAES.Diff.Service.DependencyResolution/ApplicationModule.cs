using Microsoft.Extensions.DependencyInjection;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Domain.Interfaces.Validators;
using WAES.Diff.Service.Domain.Services;
using WAES.Diff.Service.Domain.Validators;
using WAES.Diff.Service.Infrastructure.Repositories;

namespace WAES.Diff.Service.DependencyResolution
{
    public static class ApplicationModule
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IDiffService, DiffService>();
            services.AddTransient<IEntryService, EntryService>();
            services.AddTransient<IDiffCalculator, DiffCalculator>();

            services.AddTransient<IBase64Validator, Base64Validator>();
            services.AddTransient<IEntryValidator, EntryValidator>();

            services.AddTransient<IEntryRepository, EntryRepository>();
        }
    }
}
