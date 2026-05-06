using LeveGestao.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeveGestao.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Tarefa>()
            .HasOne(t => t.Gestor)
            .WithMany(u => u.TarefasCriadas)
            .HasForeignKey(t => t.GestorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tarefa>()
            .HasOne(t => t.Subordinado)
            .WithMany(u => u.TarefasRecebidas)
            .HasForeignKey(t => t.SubordinadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
