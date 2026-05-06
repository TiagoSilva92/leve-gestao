using System.Security.Claims;
using LeveGestao.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeveGestao.Web.Controllers;

[ApiController]
[Route("api/tarefas")]
[Authorize]
public class TarefasApiController : ControllerBase
{
    private readonly ITarefaService _tarefaService;

    public TarefasApiController(ITarefaService tarefaService)
    {
        _tarefaService = tarefaService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isGestor = User.IsInRole("Gestor");

        if (isGestor)
        {
            var tarefas = await _tarefaService.ListarPorGestorAsync(usuarioId);
            return Ok(tarefas);
        }
        else
        {
            var tarefas = await _tarefaService.ListarPorSubordinadoAsync(usuarioId);
            return Ok(tarefas);
        }
    }

    [HttpPut("{id}/iniciar")]
    public async Task<IActionResult> Iniciar(int id)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            await _tarefaService.IniciarAsync(id, usuarioId);
            return Ok(new { mensagem = "Tarefa iniciada." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id}/concluir")]
    public async Task<IActionResult> Concluir(int id)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            await _tarefaService.ConcluirAsync(id, usuarioId);
            return Ok(new { mensagem = "Tarefa concluída." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
