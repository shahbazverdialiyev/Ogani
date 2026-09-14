using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.API.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(category);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("with-products")]
        public async Task<IActionResult> GetCategoriesWithProducts()
        {
            try
            {
                var categoriesWithProducts = await _categoryService.GetCategoriesWithProductsAsync();

                if (categoriesWithProducts == null)
                {
                    return NotFound(new { message = "No categories found with products." });
                }

                return Ok(categoriesWithProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching categories with products for admin.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("{id:int}/for-update")]
        public async Task<IActionResult> GetForUpdate([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid product ID provided." });
            }

            try
            {
                var productDto = await _categoryService.GetForUpdateAsync(id);

                if (productDto == null)
                {
                    return NotFound(new { message = $"Product not found for update with ID {id}." });
                }

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching product data for update.",
                    detail = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _categoryService.AddAsync(categoryDto);
                return StatusCode(StatusCodes.Status201Created, new { message = $"Category \"{categoryDto.Name}\" was created successfully." });
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CategoryUpdateDTO updateDto)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid product ID provided." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the ID in the route matches the DTO ID
            updateDto.Id = id;

            try
            {
                await _categoryService.UpdateAsync(updateDto);
                return NoContent(); // 204 No Content - Standard RESTful response
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred while updating the product.",
                    detail = ex.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}