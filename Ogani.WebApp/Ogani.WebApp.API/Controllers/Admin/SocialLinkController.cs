using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.SocialLinkDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class SocialLinksController : ControllerBase
    {
        private readonly ISocialLinkService _socialLinkService;

        public SocialLinksController(ISocialLinkService socialLinkService) =>
            _socialLinkService = socialLinkService;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _socialLinkService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) =>
            Ok(await _socialLinkService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SocialLinkCreateDTO linkDto)
        {
            var id = await _socialLinkService.AddAsync(linkDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { id, message = $"Social Link \"{linkDto.Platform}\" was created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SocialLinkUpdateDTO linkDto)
        {
            linkDto.Id = id;
            await _socialLinkService.UpdateAsync(linkDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _socialLinkService.DeleteAsync(id);
            return NoContent();
        }
    }
}