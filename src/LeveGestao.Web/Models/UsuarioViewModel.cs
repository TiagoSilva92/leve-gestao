using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LeveGestao.Web.Models;

public class UsuarioViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [MaxLength(150)]
    [Display(Name = "Nome completo")]
    public string NomeCompleto { get; set; } = "";

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de nascimento")]
    public DateTime DataNascimento { get; set; }

    [MaxLength(20)]
    [Display(Name = "Telefone fixo")]
    public string? TelefoneFixo { get; set; }

    [Required(ErrorMessage = "O telefone celular é obrigatório.")]
    [MaxLength(20)]
    [Display(Name = "Telefone celular")]
    public string TelefoneCelular { get; set; } = "";

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [MaxLength(200)]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    [Display(Name = "Perfil de gestor")]
    public bool IsGestor { get; set; }

    [Display(Name = "Foto")]
    public IFormFile? Foto { get; set; }

    public string? FotoPathAtual { get; set; }

    [Required(ErrorMessage = "O logradouro é obrigatório.")]
    [MaxLength(200)]
    [Display(Name = "Logradouro")]
    public string Logradouro { get; set; } = "";

    [Required(ErrorMessage = "O número é obrigatório.")]
    [MaxLength(20)]
    [Display(Name = "Número")]
    public string Numero { get; set; } = "";

    [MaxLength(100)]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [MaxLength(100)]
    [Display(Name = "Bairro")]
    public string Bairro { get; set; } = "";

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [MaxLength(100)]
    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = "";

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [MaxLength(2)]
    [Display(Name = "Estado (UF)")]
    public string Estado { get; set; } = "";

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [MaxLength(10)]
    [Display(Name = "CEP")]
    public string Cep { get; set; } = "";

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }
}

public class UsuarioListaViewModel
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = "";
    public string Email { get; set; } = "";
    public string TelefoneCelular { get; set; } = "";
    public bool IsGestor { get; set; }
}
