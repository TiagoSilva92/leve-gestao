namespace LeveGestao.Application.DTOs;

public class UsuarioDto
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = "";
    public DateTime DataNascimento { get; set; }
    public string? TelefoneFixo { get; set; }
    public string TelefoneCelular { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsGestor { get; set; }
    public string? FotoPath { get; set; }
    public string Logradouro { get; set; } = "";
    public string Numero { get; set; } = "";
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Estado { get; set; } = "";
    public string Cep { get; set; } = "";
}

public class CadastrarUsuarioDto
{
    public string NomeCompleto { get; set; } = "";
    public DateTime DataNascimento { get; set; }
    public string? TelefoneFixo { get; set; }
    public string TelefoneCelular { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
    public bool IsGestor { get; set; }
    public string? FotoPath { get; set; }
    public string Logradouro { get; set; } = "";
    public string Numero { get; set; } = "";
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Estado { get; set; } = "";
    public string Cep { get; set; } = "";
}

public class AtualizarUsuarioDto
{
    public string NomeCompleto { get; set; } = "";
    public DateTime DataNascimento { get; set; }
    public string? TelefoneFixo { get; set; }
    public string TelefoneCelular { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsGestor { get; set; }
    public string? FotoPath { get; set; }
    public string Logradouro { get; set; } = "";
    public string Numero { get; set; } = "";
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Estado { get; set; } = "";
    public string Cep { get; set; } = "";
}
