using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ogani.WebApp.UI.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
