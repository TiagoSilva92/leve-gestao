using LeveGestao.Application.DTOs;
using LeveGestao.Application.Interfaces;
using LeveGestao.Domain.Entities;
using LeveGestao.Domain.Interfaces.Repositories;

namespace LeveGestao.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UsuarioDto?> AutenticarAsync(string email, string senha)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
        if (usuario == null)
            return null;

        if (!_passwordHasher.Verify(senha, usuario.SenhaHash))
            return null;

        return MapearParaDto(usuario);
    }

    public async Task<UsuarioDto?> ObterPorIdAsync(int id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);
        if (usuario == null)
            return null;
        return MapearParaDto(usuario);
    }

    public async Task<IEnumerable<UsuarioDto>> ListarTodosAsync()
    {
        var lista = await _usuarioRepository.ListarTodosAsync();
        return lista.Select(u => MapearParaDto(u));
    }

    public async Task<IEnumerable<UsuarioDto>> ListarSubordinadosAsync()
    {
        var lista = await _usuarioRepository.ListarSubordinadosAsync();
        return lista.Select(u => MapearParaDto(u));
    }

    public async Task CadastrarAsync(CadastrarUsuarioDto dto)
    {
        if (await _usuarioRepository.EmailExisteAsync(dto.Email))
            throw new InvalidOperationException("Este e-mail já está cadastrado no sistema.");

        var usuario = new Usuario
        {
            NomeCompleto = dto.NomeCompleto,
            DataNascimento = dto.DataNascimento,
            TelefoneCelular = dto.TelefoneCelular,
            TelefoneFixo = dto.TelefoneFixo,
            Email = dto.Email,
            SenhaHash = _passwordHasher.Hash(dto.Senha),
            IsGestor = dto.IsGestor,
            FotoPath = dto.FotoPath,
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Cep = dto.Cep
        };

        await _usuarioRepository.AdicionarAsync(usuario);
    }

    public async Task ExcluirAsync(int id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);
        if (usuario == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        await _usuarioRepository.ExcluirAsync(id);
    }

    public async Task AtualizarAsync(int id, AtualizarUsuarioDto dto)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);
        if (usuario == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        if (dto.Email != usuario.Email && await _usuarioRepository.EmailExisteAsync(dto.Email))
            throw new InvalidOperationException("Este e-mail já está sendo usado por outro usuário.");

        usuario.NomeCompleto = dto.NomeCompleto;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.TelefoneCelular = dto.TelefoneCelular;
        usuario.TelefoneFixo = dto.TelefoneFixo;
        usuario.Email = dto.Email;
        usuario.IsGestor = dto.IsGestor;
        usuario.Logradouro = dto.Logradouro;
        usuario.Numero = dto.Numero;
        usuario.Complemento = dto.Complemento;
        usuario.Bairro = dto.Bairro;
        usuario.Cidade = dto.Cidade;
        usuario.Estado = dto.Estado;
        usuario.Cep = dto.Cep;

        if (!string.IsNullOrWhiteSpace(dto.FotoPath))
            usuario.FotoPath = dto.FotoPath;

        await _usuarioRepository.AtualizarAsync(usuario);
    }

    private UsuarioDto MapearParaDto(Usuario u)
    {
        return new UsuarioDto
        {
            Id = u.Id,
            NomeCompleto = u.NomeCompleto,
            DataNascimento = u.DataNascimento,
            TelefoneFixo = u.TelefoneFixo,
            TelefoneCelular = u.TelefoneCelular,
            Email = u.Email,
            IsGestor = u.IsGestor,
            FotoPath = u.FotoPath,
            Logradouro = u.Logradouro,
            Numero = u.Numero,
            Complemento = u.Complemento,
            Bairro = u.Bairro,
            Cidade = u.Cidade,
            Estado = u.Estado,
            Cep = u.Cep
        };
    }
}
