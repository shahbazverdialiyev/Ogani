using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.UsefulLinkDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IUsefulLinkService _usefulLinkService;

        public UsefulLinksController(IUsefulLinkService usefulLinkService)
        {
            _usefulLinkService = usefulLinkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var links = await _usefulLinkService.GetAllAsync();
            return Ok(links);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var link = await _usefulLinkService.GetByIdAsync(id);
                return Ok(link);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsefulLinkCreateDTO linkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _usefulLinkService.AddAsync(linkDto);
                return StatusCode(StatusCodes.Status201Created, new { message = $"Useful link \"{linkDto.Name}\" was created successfully." });
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
        public async Task<IActionResult> Update(int id, [FromBody] UsefulLinkUpdateDTO linkDto)
        {
            if (id != linkDto.Id)
                return BadRequest(new { message = "ID mismatch in request URL and body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _usefulLinkService.UpdateAsync(linkDto);
                return Ok(new { message = $"Useful link \"{linkDto.Name}\" was updated successfully." });
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
                await _usefulLinkService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}