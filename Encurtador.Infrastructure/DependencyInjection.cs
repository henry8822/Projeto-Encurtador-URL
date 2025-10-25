using Encurtador.Core.Interfaces;
using Encurtador.Infrastructure.Data;
using Encurtador.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.Replication;

namespace Encurtador.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApiDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUrlRepository, UrlRepository>();

            return services;
        }
    }
}