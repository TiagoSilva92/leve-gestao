using System.ComponentModel.DataAnnotations;

namespace LeveGestao.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string NomeCompleto { get; set; } = "";

    [Required]
    public DateTime DataNascimento { get; set; }

    [MaxLength(20)]
    public string? TelefoneFixo { get; set; }

    [Required]
    [MaxLength(20)]
    public string TelefoneCelular { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = "";

    [Required]
    public string SenhaHash { get; set; } = "";

    public bool IsGestor { get; set; }

    public string? FotoPath { get; set; }

    [Required]
    [MaxLength(200)]
    public string Logradouro { get; set; } = "";

    [Required]
    [MaxLength(20)]
    public string Numero { get; set; } = "";

    [MaxLength(100)]
    public string? Complemento { get; set; }

    [Required]
    [MaxLength(100)]
    public string Bairro { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Cidade { get; set; } = "";

    [Required]
    [MaxLength(2)]
    public string Estado { get; set; } = "";

    [Required]
    [MaxLength(10)]
    public string Cep { get; set; } = "";

    public ICollection<Tarefa> TarefasRecebidas { get; set; } = new List<Tarefa>();
    public ICollection<Tarefa> TarefasCriadas { get; set; } = new List<Tarefa>();
}
