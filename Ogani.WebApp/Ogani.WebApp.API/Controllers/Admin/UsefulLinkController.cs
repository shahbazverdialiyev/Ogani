using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.UsefulLinkDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IUsefulLinkService _usefulLinkService;

        public UsefulLinksController(IUsefulLinkService usefulLinkService) =>
            _usefulLinkService = usefulLinkService;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _usefulLinkService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) =>
            Ok(await _usefulLinkService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsefulLinkCreateDTO linkDto)
        {
            var id = await _usefulLinkService.AddAsync(linkDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { id, message = $"Useful link \"{linkDto.Name}\" was created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UsefulLinkUpdateDTO linkDto)
        {
            linkDto.Id = id;
            await _usefulLinkService.UpdateAsync(linkDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _usefulLinkService.DeleteAsync(id);
            return NoContent();
        }
    }
}