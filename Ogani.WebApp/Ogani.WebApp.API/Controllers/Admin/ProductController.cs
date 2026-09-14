using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
        {
            var products = categoryId.HasValue
                ? await _productService.GetProductsByCategoryIdAsync(categoryId.Value)
                : await _productService.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategoryId([FromRoute] int categoryId)
        {
            // 1. Input Validation
            if (categoryId <= 0)
            {
                return BadRequest(new { message = "Invalid category ID provided." });
            }

            try
            {
                // 2. Fetch data from business service
                var products = await _productService.GetProductsByCategoryIdAsync(categoryId);

                if (products == null)
                {
                    return NotFound(new { message = $"No products found for category ID {categoryId}." });
                }

                // 3. Return 200 OK
                return Ok(products);
            }
            catch (Exception ex)
            {
                // 4. Handle internal server errors
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching products by category.",
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
                var productDto = await _productService.GetForUpdateAsync(id);

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

        [HttpGet("discount/{discountId:int}")]
        public async Task<IActionResult> GetByDiscountId([FromRoute] int discountId)
        {
            // 1. Input Validation
            if (discountId <= 0)
            {
                return BadRequest(new { message = "Invalid discount ID provided." });
            }

            try
            {
                // 2. Fetch data from business service
                var products = await _productService.GetProductsByDiscountIdAsync(discountId);

                if (products == null)
                {
                    return NotFound(new { message = $"No products found for discount ID {discountId}." });
                }

                // 3. Return 200 OK
                return Ok(products);
            }
            catch (Exception ex)
            {
                // 4. Handle internal server errors
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching products by discount.",
                    detail = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int productId = await _productService.AddAsync(productDto);

                return CreatedAtAction(nameof(GetById), new { id = productId }, new { id = productId, message = $"Product \"{productDto.Name}\" created successfully." });
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ProductUpdateDTO updateDto)
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
                await _productService.UpdateAsync(updateDto);
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
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}