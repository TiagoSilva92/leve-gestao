using LeveGestao.Domain.Enums;

namespace LeveGestao.Application.DTOs;

public class TarefaDto
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = "";
    public DateTime DataLimite { get; set; }
    public StatusTarefa Status { get; set; }

    public string StatusDescricao
    {
        get
        {
            if (Status == StatusTarefa.Pendente) return "Pendente";
            if (Status == StatusTarefa.EmAndamento) return "Em andamento";
            if (Status == StatusTarefa.Concluida) return "Concluída";
            return Status.ToString();
        }
    }

    public int GestorId { get; set; }
    public string NomeGestor { get; set; } = "";
    public int SubordinadoId { get; set; }
    public string NomeSubordinado { get; set; } = "";
    public DateTime CriadaEm { get; set; }
    public DateTime? ConcluidaEm { get; set; }
}

public class CriarTarefaDto
{
    public string Mensagem { get; set; } = "";
    public DateTime DataLimite { get; set; }
    public int SubordinadoId { get; set; }
    public int GestorId { get; set; }
}
