using LeveGestao.Domain.Entities;

namespace LeveGestao.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ListarTodosAsync();
    Task<IEnumerable<Usuario>> ListarSubordinadosAsync();
    Task AdicionarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task ExcluirAsync(int id);
    Task<bool> EmailExisteAsync(string email);
}
