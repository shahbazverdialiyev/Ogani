using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.ContactDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactService.GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var contact = await _contactService.GetByIdAsync(id);
                return Ok(contact);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("phone")]
        public async Task<IActionResult> GetPhone()
        {
            try
            {
                var phoneContact = await _contactService.GetPhoneAsync();

                if (phoneContact == null)
                {
                    return NotFound(new { message = "Phone contact information not found." });
                }

                return Ok(phoneContact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching phone contact details.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetEmail()
        {
            try
            {
                var emailContact = await _contactService.GetEmailAsync();

                if (emailContact == null)
                {
                    return NotFound(new { message = "Email contact information not found." });
                }

                return Ok(emailContact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching email contact details.",
                    detail = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactCreateDTO contactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _contactService.AddAsync(contactDto);
                return StatusCode(StatusCodes.Status201Created, new { message = $"Contact \"{contactDto.Title}\" was created successfully." });
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
        public async Task<IActionResult> Update(int id, [FromBody] ContactUpdateDTO contactDto)
        {
            if (id != contactDto.Id)
                return BadRequest(new { message = "ID mismatch in request URL and body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _contactService.UpdateAsync(contactDto);
                return Ok(new { message = $"Contact \"{contactDto.Title}\" was updated successfully." });
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
                await _contactService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}