using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.SocialLinkDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class SocialLinksController : ControllerBase
    {
        private readonly ISocialLinkService _socialLinkService;

        public SocialLinksController(ISocialLinkService socialLinkService)
        {
            _socialLinkService = socialLinkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var links = await _socialLinkService.GetAllAsync();
            return Ok(links);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var link = await _socialLinkService.GetByIdAsync(id);
                return Ok(link);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SocialLinkCreateDTO linkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _socialLinkService.AddAsync(linkDto);
                return StatusCode(StatusCodes.Status201Created, new { message = $"Social Link \"{linkDto.Platform}\" was created successfully." });
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return BadRequest(ModelState);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SocialLinkUpdateDTO linkDto)
        {
            if (id != linkDto.Id)
                return BadRequest(new { message = "ID mismatch in request URL and body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _socialLinkService.UpdateAsync(linkDto);
                return Ok(new { message = $"Social Link \"{linkDto.Platform}\" was updated successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return BadRequest(ModelState);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _socialLinkService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}