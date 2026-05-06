using System.Security.Claims;
using LeveGestao.Application.DTOs;
using LeveGestao.Application.Interfaces;
using LeveGestao.Domain.Enums;
using LeveGestao.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeveGestao.Web.Controllers;

[Authorize]
public class TarefasController : Controller
{
    private readonly ITarefaService _tarefaService;
    private readonly IUsuarioService _usuarioService;

    public TarefasController(ITarefaService tarefaService, IUsuarioService usuarioService)
    {
        _tarefaService = tarefaService;
        _usuarioService = usuarioService;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isGestor = User.IsInRole("Gestor");

        IEnumerable<TarefaDto> tarefas;

        if (isGestor)
            tarefas = await _tarefaService.ListarPorGestorAsync(usuarioId);
        else
            tarefas = await _tarefaService.ListarPorSubordinadoAsync(usuarioId);

        var viewModels = tarefas.Select(t => new TarefaListaViewModel
        {
            Id = t.Id,
            Mensagem = t.Mensagem,
            DataLimite = t.DataLimite,
            StatusDescricao = t.StatusDescricao,
            NomeResponsavel = isGestor ? t.NomeSubordinado : t.NomeGestor,
            Atrasada = t.Status != StatusTarefa.Concluida && t.DataLimite.Date < DateTime.Today
        });

        ViewBag.IsGestor = isGestor;
        return View(viewModels);
    }

    [HttpGet]
    [Authorize(Roles = "Gestor")]
    public async Task<IActionResult> Criar()
    {
        var model = new TarefaViewModel
        {
            SubordinadosDisponiveis = await ObterSubordinadosSelectListAsync()
        };
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Gestor")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(TarefaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.SubordinadosDisponiveis = await ObterSubordinadosSelectListAsync();
            return View(model);
        }

        try
        {
            var gestorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var dto = new CriarTarefaDto
            {
                Mensagem = model.Mensagem,
                DataLimite = model.DataLimite,
                SubordinadoId = model.SubordinadoId,
                GestorId = gestorId
            };

            await _tarefaService.CriarAsync(dto);
            TempData["Sucesso"] = "Tarefa criada e colaborador notificado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            model.SubordinadosDisponiveis = await ObterSubordinadosSelectListAsync();
            return View(model);
        }
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var tarefa = await _tarefaService.ObterPorIdAsync(id);
        if (tarefa == null)
            return NotFound();

        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isGestor = User.IsInRole("Gestor");

        bool temAcesso;
        if (isGestor)
            temAcesso = tarefa.GestorId == usuarioId;
        else
            temAcesso = tarefa.SubordinadoId == usuarioId;

        if (!temAcesso)
            return NotFound();

        var viewModel = new TarefaDetalhesViewModel
        {
            Id = tarefa.Id,
            Mensagem = tarefa.Mensagem,
            DataLimite = tarefa.DataLimite,
            Status = tarefa.Status,
            StatusDescricao = tarefa.StatusDescricao,
            NomeGestor = tarefa.NomeGestor,
            NomeSubordinado = tarefa.NomeSubordinado,
            CriadaEm = tarefa.CriadaEm,
            ConcluidaEm = tarefa.ConcluidaEm,
            Atrasada = tarefa.Status != StatusTarefa.Concluida && tarefa.DataLimite.Date < DateTime.Today,
            PodeAlterar = !isGestor && tarefa.SubordinadoId == usuarioId && tarefa.Status != StatusTarefa.Concluida
        };

        return View(viewModel);
    }

    private async Task<IEnumerable<SelectListItem>> ObterSubordinadosSelectListAsync()
    {
        var subordinados = await _usuarioService.ListarSubordinadosAsync();
        return subordinados.Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = $"{u.NomeCompleto} ({u.Email})"
        });
    }
}
