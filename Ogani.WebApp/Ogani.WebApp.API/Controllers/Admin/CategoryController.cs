using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.CategoryDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService) =>
            _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _categoryService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) =>
            Ok(await _categoryService.GetByIdAsync(id));

        [HttpGet("with-products")]
        public async Task<IActionResult> GetCategoriesWithProducts() =>
            Ok(await _categoryService.GetCategoriesWithProductsAsync());

        [HttpGet("{id:int}/for-update")]
        public async Task<IActionResult> GetForUpdate([FromRoute] int id) =>
            Ok(await _categoryService.GetForUpdateAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO categoryDto)
        {
            var categoryId = await _categoryService.AddAsync(categoryDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = categoryId },
                new { id = categoryId, message = $"Category \"{categoryDto.Name}\" was created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryUpdateDTO updateDto)
        {
            updateDto.Id = id;
            await _categoryService.UpdateAsync(updateDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}