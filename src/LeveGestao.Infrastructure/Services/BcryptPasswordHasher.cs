using LeveGestao.Application.Interfaces;

namespace LeveGestao.Infrastructure.Services;

public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);

    public bool Verify(string senha, string hash) => BCrypt.Net.BCrypt.Verify(senha, hash);
}
