using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            try
            {
                var contacts = await _contactService.GetContactsForUIAsync();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching contact information for UI.",
                    detail = ex.Message
                });
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
    }
}