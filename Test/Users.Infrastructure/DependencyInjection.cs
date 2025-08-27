using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Interfaces;
using Users.Infrastructure.Data;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(connectionString));

            // Repositorio de usuarios
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
