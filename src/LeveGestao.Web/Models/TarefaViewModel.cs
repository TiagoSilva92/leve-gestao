using System.ComponentModel.DataAnnotations;
using LeveGestao.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeveGestao.Web.Models;

public class TarefaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [MaxLength(1000, ErrorMessage = "A mensagem não pode exceder 1000 caracteres.")]
    [Display(Name = "Descrição da tarefa")]
    public string Mensagem { get; set; } = "";

    [Required(ErrorMessage = "A data limite é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data limite")]
    public DateTime DataLimite { get; set; } = DateTime.Today.AddDays(7);

    [Required(ErrorMessage = "Selecione um colaborador.")]
    [Display(Name = "Colaborador responsável")]
    public int SubordinadoId { get; set; }

    public IEnumerable<SelectListItem> SubordinadosDisponiveis { get; set; } = new List<SelectListItem>();
}

public class TarefaDetalhesViewModel
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = "";
    public DateTime DataLimite { get; set; }
    public StatusTarefa Status { get; set; }
    public string StatusDescricao { get; set; } = "";
    public string NomeGestor { get; set; } = "";
    public string NomeSubordinado { get; set; } = "";
    public DateTime CriadaEm { get; set; }
    public DateTime? ConcluidaEm { get; set; }
    public bool Atrasada { get; set; }
    public bool PodeAlterar { get; set; }
}

public class TarefaListaViewModel
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = "";
    public DateTime DataLimite { get; set; }
    public string StatusDescricao { get; set; } = "";
    public string NomeResponsavel { get; set; } = "";
    public bool Atrasada { get; set; }
}
