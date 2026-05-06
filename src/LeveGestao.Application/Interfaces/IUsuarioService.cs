using LeveGestao.Application.DTOs;

namespace LeveGestao.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto?> AutenticarAsync(string email, string senha);
    Task<UsuarioDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<UsuarioDto>> ListarTodosAsync();
    Task<IEnumerable<UsuarioDto>> ListarSubordinadosAsync();
    Task CadastrarAsync(CadastrarUsuarioDto dto);
    Task AtualizarAsync(int id, AtualizarUsuarioDto dto);
    Task ExcluirAsync(int id);
}
