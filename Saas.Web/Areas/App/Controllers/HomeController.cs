using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Web.Models;

namespace Saas.Web.Areas.App.Controllers;

[Area("App")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
