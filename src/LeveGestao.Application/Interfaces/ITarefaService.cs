using LeveGestao.Application.DTOs;

namespace LeveGestao.Application.Interfaces;

public interface ITarefaService
{
    Task<TarefaDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<TarefaDto>> ListarPorGestorAsync(int gestorId);
    Task<IEnumerable<TarefaDto>> ListarPorSubordinadoAsync(int subordinadoId);
    Task CriarAsync(CriarTarefaDto dto);
    Task IniciarAsync(int tarefaId, int subordinadoId);
    Task ConcluirAsync(int tarefaId, int subordinadoId);
}
