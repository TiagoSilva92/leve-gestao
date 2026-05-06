using LeveGestao.Domain.Entities;
using LeveGestao.Domain.Interfaces.Repositories;
using LeveGestao.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LeveGestao.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Usuario>> ListarTodosAsync()
    {
        return await _context.Usuarios
            .OrderBy(u => u.NomeCompleto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarSubordinadosAsync()
    {
        return await _context.Usuarios
            .Where(u => !u.IsGestor)
            .OrderBy(u => u.NomeCompleto)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return;

        try
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("Não é possível excluir este usuário pois ele possui tarefas associadas.");
        }
    }

    public async Task<bool> EmailExisteAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }
}
