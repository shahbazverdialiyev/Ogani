using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.UI.ViewComponents
{
    public class SocialLinksViewComponent : ViewComponent
    {
        private readonly ISocialLinkService _socialLinkService;

        public SocialLinksViewComponent(ISocialLinkService socialLinkService)
        {
            _socialLinkService = socialLinkService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
            => View(viewName, await _socialLinkService.GetSocialLinksForUIAsync());
    }
}
