using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.UI.ViewComponents
{
    public class UsefulLinksViewComponent : ViewComponent
    {
        private readonly IUsefulLinkService _UsefulLinkService;

        public UsefulLinksViewComponent(IUsefulLinkService UsefulLinkService)
        {
            _UsefulLinkService = UsefulLinkService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
            => View(viewName, await _UsefulLinkService.GetUsefulLinksForUIAsync());
    }
}
