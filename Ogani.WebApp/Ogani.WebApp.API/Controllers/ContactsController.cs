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

        public ContactsController(IContactService contactService) =>
            _contactService = contactService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _contactService.GetContactsForUIAsync());

        [HttpGet("phone")]
        public async Task<IActionResult> GetPhone() => Ok(await _contactService.GetPhoneAsync());

        [HttpGet("email")]
        public async Task<IActionResult> GetEmail() => Ok(await _contactService.GetEmailAsync());
    }
}