using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.WebUI.ViewComponents
{
    public class HeroViewComponent : ViewComponent
    {
        private readonly IHeroService _heroService;

        public HeroViewComponent(IHeroService heroService)
        {
            _heroService = heroService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeHero = await _heroService.GetActiveHeroAsync();
            return View(activeHero);
        }
    }
}