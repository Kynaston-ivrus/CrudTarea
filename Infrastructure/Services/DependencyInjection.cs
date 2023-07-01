using Application.Data;
// using Domain.Customers;
using Domain.Reservations;
using Domain.Primitives;
using Domain.Places;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Packages;

namespace Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();

        return services;

    }
}