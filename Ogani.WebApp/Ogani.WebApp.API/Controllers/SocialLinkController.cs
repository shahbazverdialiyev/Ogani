using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialLinksController : ControllerBase
    {
        private readonly ISocialLinkService _socialLinkService;

        public SocialLinksController(ISocialLinkService socialLinkService) =>
            _socialLinkService = socialLinkService;

        [HttpGet]
        public async Task<IActionResult> GetSocialLinks() =>
            Ok(await _socialLinkService.GetSocialLinksForUIAsync());
    }
}