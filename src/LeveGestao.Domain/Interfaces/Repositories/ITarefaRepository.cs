using LeveGestao.Domain.Entities;

namespace LeveGestao.Domain.Interfaces.Repositories;

public interface ITarefaRepository
{
    Task<Tarefa?> ObterPorIdAsync(int id);
    Task<IEnumerable<Tarefa>> ListarPorGestorAsync(int gestorId);
    Task<IEnumerable<Tarefa>> ListarPorSubordinadoAsync(int subordinadoId);
    Task AdicionarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
}
