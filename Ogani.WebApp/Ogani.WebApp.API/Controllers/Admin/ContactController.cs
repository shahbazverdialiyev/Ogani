using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.ContactDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService) => 
            _contactService = contactService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => 
            Ok(await _contactService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => 
            Ok(await _contactService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactCreateDTO contactDto)
        {
            var contactId = await _contactService.AddAsync(contactDto);

            return CreatedAtAction(
                nameof(GetById), 
                new { id = contactId }, 
                new { id = contactId, message = $"Contact \"{contactDto.Title}\" was created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ContactUpdateDTO contactDto)
        {
            contactDto.Id = id;
            await _contactService.UpdateAsync(contactDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contactService.DeleteAsync(id);
            return NoContent();
        }
    }
}