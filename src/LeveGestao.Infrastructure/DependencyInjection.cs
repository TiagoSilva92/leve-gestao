using LeveGestao.Application.Interfaces;
using LeveGestao.Application.Services;
using LeveGestao.Domain.Interfaces.Repositories;
using LeveGestao.Infrastructure.Data;
using LeveGestao.Infrastructure.Repositories;
using LeveGestao.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeveGestao.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.Configure<EmailSettings>(configuration.GetSection("Email"));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ITarefaService, TarefaService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

        return services;
    }
}
