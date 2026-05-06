using LeveGestao.Application.DTOs;
using LeveGestao.Application.Interfaces;
using LeveGestao.Domain.Entities;
using LeveGestao.Domain.Enums;
using LeveGestao.Domain.Interfaces.Repositories;

namespace LeveGestao.Application.Services;

public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmailService _emailService;

    public TarefaService(ITarefaRepository tarefaRepository,
                         IUsuarioRepository usuarioRepository,
                         IEmailService emailService)
    {
        _tarefaRepository = tarefaRepository;
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
    }

    public async Task<TarefaDto?> ObterPorIdAsync(int id)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(id);
        if (tarefa == null) return null;
        return MapearParaDto(tarefa);
    }

    public async Task<IEnumerable<TarefaDto>> ListarPorGestorAsync(int gestorId)
    {
        var tarefas = await _tarefaRepository.ListarPorGestorAsync(gestorId);
        return tarefas.Select(t => MapearParaDto(t));
    }

    public async Task<IEnumerable<TarefaDto>> ListarPorSubordinadoAsync(int subordinadoId)
    {
        var tarefas = await _tarefaRepository.ListarPorSubordinadoAsync(subordinadoId);
        return tarefas.Select(t => MapearParaDto(t));
    }

    public async Task CriarAsync(CriarTarefaDto dto)
    {
        var subordinado = await _usuarioRepository.ObterPorIdAsync(dto.SubordinadoId);
        if (subordinado == null)
            throw new KeyNotFoundException("Colaborador não encontrado.");

        if (subordinado.IsGestor)
            throw new InvalidOperationException("Não é possível atribuir tarefas a um gestor.");

        var tarefa = new Tarefa
        {
            Mensagem = dto.Mensagem,
            DataLimite = dto.DataLimite,
            GestorId = dto.GestorId,
            SubordinadoId = dto.SubordinadoId,
            Status = StatusTarefa.Pendente
        };

        await _tarefaRepository.AdicionarAsync(tarefa);

        try
        {
            await _emailService.EnviarAsync(
                subordinado.Email,
                subordinado.NomeCompleto,
                "Nova tarefa atribuída a você",
                $"Olá, {subordinado.NomeCompleto}!\n\nUma nova tarefa foi atribuída a você:\n\n\"{dto.Mensagem}\"\n\nPrazo: {dto.DataLimite:dd/MM/yyyy}."
            );
        }
        catch
        {
            // e-mail é best-effort; falha não reverte a criação
        }
    }

    public async Task IniciarAsync(int tarefaId, int subordinadoId)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(tarefaId);
        if (tarefa == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        if (tarefa.SubordinadoId != subordinadoId)
            throw new InvalidOperationException("Você não tem permissão para alterar esta tarefa.");

        if (tarefa.Status != StatusTarefa.Pendente)
            throw new InvalidOperationException("Apenas tarefas pendentes podem ser iniciadas.");

        tarefa.Status = StatusTarefa.EmAndamento;
        await _tarefaRepository.AtualizarAsync(tarefa);
    }

    public async Task ConcluirAsync(int tarefaId, int subordinadoId)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(tarefaId);
        if (tarefa == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        if (tarefa.SubordinadoId != subordinadoId)
            throw new InvalidOperationException("Você não tem permissão para alterar esta tarefa.");

        if (tarefa.Status == StatusTarefa.Concluida)
            throw new InvalidOperationException("Esta tarefa já foi concluída.");

        var nomeColaborador = tarefa.Subordinado?.NomeCompleto ?? "";
        tarefa.Status = StatusTarefa.Concluida;
        tarefa.ConcluidaEm = DateTime.UtcNow;
        await _tarefaRepository.AtualizarAsync(tarefa);

        var gestor = await _usuarioRepository.ObterPorIdAsync(tarefa.GestorId);
        if (gestor != null)
        {
            try
            {
                await _emailService.EnviarAsync(
                    gestor.Email,
                    gestor.NomeCompleto,
                    "Tarefa concluída",
                    $"Olá, {gestor.NomeCompleto}!\n\nA tarefa \"{tarefa.Mensagem}\" foi concluída por {nomeColaborador}."
                );
            }
            catch
            {
                // notificação é best-effort
            }
        }
    }

    private TarefaDto MapearParaDto(Tarefa t)
    {
        return new TarefaDto
        {
            Id = t.Id,
            Mensagem = t.Mensagem,
            DataLimite = t.DataLimite,
            Status = t.Status,
            GestorId = t.GestorId,
            NomeGestor = t.Gestor?.NomeCompleto ?? "",
            SubordinadoId = t.SubordinadoId,
            NomeSubordinado = t.Subordinado?.NomeCompleto ?? "",
            CriadaEm = t.CriadaEm,
            ConcluidaEm = t.ConcluidaEm
        };
    }
}
