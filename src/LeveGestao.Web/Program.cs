using LeveGestao.Application.Interfaces;
using LeveGestao.Domain.Entities;
using LeveGestao.Infrastructure;
using LeveGestao.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.HttpOnly = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Erro");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await db.Database.MigrateAsync();

    if (!await db.Usuarios.AnyAsync())
    {
        db.Usuarios.Add(new Usuario
        {
            NomeCompleto = "Administrador TI",
            DataNascimento = new DateTime(1985, 1, 1),
            TelefoneCelular = "(11) 99999-9999",
            TelefoneFixo = "(11) 2537-7777",
            Email = "ti@leveinvestimentos.com.br",
            SenhaHash = hasher.Hash("teste123"),
            IsGestor = true,
            Logradouro = "Praça Maastricht",
            Numero = "200",
            Complemento = "7º andar, sala 704",
            Bairro = "Euroville",
            Cidade = "Bragança Paulista",
            Estado = "SP",
            Cep = "12917-021"
        });

        await db.SaveChangesAsync();
    }
}

app.Run();
