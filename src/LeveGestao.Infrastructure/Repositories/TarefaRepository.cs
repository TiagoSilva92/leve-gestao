using LeveGestao.Domain.Entities;
using LeveGestao.Domain.Interfaces.Repositories;
using LeveGestao.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LeveGestao.Infrastructure.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tarefa?> ObterPorIdAsync(int id)
    {
        return await _context.Tarefas
            .Include(t => t.Gestor)
            .Include(t => t.Subordinado)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tarefa>> ListarPorGestorAsync(int gestorId)
    {
        return await _context.Tarefas
            .Include(t => t.Gestor)
            .Include(t => t.Subordinado)
            .Where(t => t.GestorId == gestorId)
            .OrderByDescending(t => t.CriadaEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> ListarPorSubordinadoAsync(int subordinadoId)
    {
        return await _context.Tarefas
            .Include(t => t.Gestor)
            .Include(t => t.Subordinado)
            .Where(t => t.SubordinadoId == subordinadoId)
            .OrderByDescending(t => t.CriadaEm)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }
}
