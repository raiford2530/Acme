using Acme.Shop.Application.Interfaces;
using Acme.Shop.Application.Products;
using Acme.Shop.Infrastructure.Persistence;
using Acme.Shop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("SqlServer")));

            // EF Core logging/tracing is enabled via OTel in Api; nothing else needed here.

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // Auto-apply configurations (already in AppDbContext.OnModelCreating)
            return services;
        }
    }
}
