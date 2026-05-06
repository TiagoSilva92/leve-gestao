using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeveGestao.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();

    [AllowAnonymous]
    [Route("Home/Erro")]
    public IActionResult Erro() => View();
}
