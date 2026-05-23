using Microsoft.AspNetCore.Mvc;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
