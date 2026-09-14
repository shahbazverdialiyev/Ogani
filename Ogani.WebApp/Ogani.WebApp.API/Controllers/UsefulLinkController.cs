using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IUsefulLinkService _usefulLinkService;

        public UsefulLinksController(IUsefulLinkService usefulLinkService) =>
            _usefulLinkService = usefulLinkService;

        [HttpGet]
        public async Task<IActionResult> GetUsefulLinks() =>
            Ok(await _usefulLinkService.GetUsefulLinksForUIAsync());
    }
}