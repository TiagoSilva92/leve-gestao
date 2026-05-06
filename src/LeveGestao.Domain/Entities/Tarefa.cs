using LeveGestao.Domain.Enums;

namespace LeveGestao.Domain.Entities;

public class Tarefa
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = "";
    public DateTime DataLimite { get; set; }
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    public int GestorId { get; set; }
    public int SubordinadoId { get; set; }
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
    public DateTime? ConcluidaEm { get; set; }

    public Usuario? Gestor { get; set; }
    public Usuario? Subordinado { get; set; }
}
