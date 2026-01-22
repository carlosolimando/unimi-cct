using CS.ApiGateway.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CS.ApiGateway.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public IActionResult Index() => this.View();

        [AllowAnonymous]
        public IActionResult Public() => this.View();

        [AllowAnonymous]
        public IActionResult AccessDenied() => this.View();

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            this.View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
                }
            );
    }
}
