using System.Security.Claims;
using LeveGestao.Application.DTOs;
using LeveGestao.Application.Interfaces;
using LeveGestao.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeveGestao.Web.Controllers;

[Authorize]
public class UsuariosController : Controller
{
    private readonly IUsuarioService _usuarioService;
    private readonly IWebHostEnvironment _env;

    public UsuariosController(IUsuarioService usuarioService, IWebHostEnvironment env)
    {
        _usuarioService = usuarioService;
        _env = env;
    }

    [Authorize(Roles = "Gestor")]
    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioService.ListarTodosAsync();

        var lista = usuarios.Select(u => new UsuarioListaViewModel
        {
            Id = u.Id,
            NomeCompleto = u.NomeCompleto,
            Email = u.Email,
            TelefoneCelular = u.TelefoneCelular,
            IsGestor = u.IsGestor
        });

        return View(lista);
    }

    [HttpGet]
    [Authorize(Roles = "Gestor")]
    public IActionResult Cadastrar()
    {
        return View(new UsuarioViewModel());
    }

    [HttpPost]
    [Authorize(Roles = "Gestor")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastrar(UsuarioViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Senha))
            ModelState.AddModelError(nameof(model.Senha), "A senha é obrigatória.");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            string? fotoPath = null;
            if (model.Foto != null && model.Foto.Length > 0)
                fotoPath = await SalvarFotoAsync(model.Foto);

            var dto = new CadastrarUsuarioDto
            {
                NomeCompleto = model.NomeCompleto,
                DataNascimento = model.DataNascimento,
                TelefoneFixo = model.TelefoneFixo,
                TelefoneCelular = model.TelefoneCelular,
                Email = model.Email,
                Senha = model.Senha!,
                IsGestor = model.IsGestor,
                FotoPath = fotoPath,
                Logradouro = model.Logradouro,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Estado = model.Estado,
                Cep = model.Cep
            };

            await _usuarioService.CadastrarAsync(dto);

            TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Gestor")]
    public async Task<IActionResult> Editar(int id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        if (usuario == null)
            return NotFound();

        var model = new UsuarioViewModel
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            DataNascimento = usuario.DataNascimento,
            TelefoneFixo = usuario.TelefoneFixo,
            TelefoneCelular = usuario.TelefoneCelular,
            Email = usuario.Email,
            IsGestor = usuario.IsGestor,
            FotoPathAtual = usuario.FotoPath,
            Logradouro = usuario.Logradouro,
            Numero = usuario.Numero,
            Complemento = usuario.Complemento,
            Bairro = usuario.Bairro,
            Cidade = usuario.Cidade,
            Estado = usuario.Estado,
            Cep = usuario.Cep
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Gestor")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, UsuarioViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            string? fotoPath = model.FotoPathAtual;
            if (model.Foto != null && model.Foto.Length > 0)
            {
                fotoPath = await SalvarFotoAsync(model.Foto);
                DeletarFoto(model.FotoPathAtual);
            }

            var dto = new AtualizarUsuarioDto
            {
                NomeCompleto = model.NomeCompleto,
                DataNascimento = model.DataNascimento,
                TelefoneFixo = model.TelefoneFixo,
                TelefoneCelular = model.TelefoneCelular,
                Email = model.Email,
                IsGestor = model.IsGestor,
                FotoPath = fotoPath,
                Logradouro = model.Logradouro,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Estado = model.Estado,
                Cep = model.Cep
            };

            await _usuarioService.AtualizarAsync(id, dto);

            TempData["Sucesso"] = "Usuário atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Gestor")]
    public async Task<IActionResult> Excluir(int id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        if (usuario == null)
            return NotFound();

        var model = new UsuarioListaViewModel
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            TelefoneCelular = usuario.TelefoneCelular,
            IsGestor = usuario.IsGestor
        };

        return View(model);
    }

    [HttpPost, ActionName("Excluir")]
    [Authorize(Roles = "Gestor")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        var usuarioLogadoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (id == usuarioLogadoId)
        {
            TempData["Erro"] = "Você não pode excluir sua própria conta.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await _usuarioService.ExcluirAsync(id);
            TempData["Sucesso"] = "Usuário excluído com sucesso!";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Erro"] = ex.Message;
        }
        catch (KeyNotFoundException)
        {
            TempData["Erro"] = "Usuário não encontrado.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void DeletarFoto(string? fotoPath)
    {
        if (string.IsNullOrEmpty(fotoPath)) return;
        var caminho = Path.Combine(_env.WebRootPath, fotoPath.TrimStart('/'));
        if (System.IO.File.Exists(caminho))
            System.IO.File.Delete(caminho);
    }

    private async Task<string> SalvarFotoAsync(IFormFile foto)
    {
        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        var extensao = Path.GetExtension(foto.FileName).ToLower();

        if (!extensoesPermitidas.Contains(extensao) || !tiposPermitidos.Contains(foto.ContentType.ToLower()))
            throw new InvalidOperationException("Formato de imagem inválido. Use JPG, PNG, GIF ou WEBP.");

        if (foto.Length > 5 * 1024 * 1024)
            throw new InvalidOperationException("A foto não pode ter mais de 5MB.");

        var nomeArquivo = Guid.NewGuid().ToString() + extensao;
        var pasta = Path.Combine(_env.WebRootPath, "uploads", "usuarios");

        if (!Directory.Exists(pasta))
            Directory.CreateDirectory(pasta);

        var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            await foto.CopyToAsync(stream);
        }

        return "/uploads/usuarios/" + nomeArquivo;
    }
}
