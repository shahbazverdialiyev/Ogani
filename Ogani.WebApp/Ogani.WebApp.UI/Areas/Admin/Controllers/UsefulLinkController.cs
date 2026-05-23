using Microsoft.AspNetCore.Mvc;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsefulLinkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
