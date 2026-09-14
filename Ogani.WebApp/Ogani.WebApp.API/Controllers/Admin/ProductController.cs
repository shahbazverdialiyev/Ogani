using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) =>
            _productService = productService;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId) =>
            Ok(categoryId.HasValue
                ? await _productService.GetProductsByCategoryIdAsync(categoryId.Value)
                : await _productService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) =>
            Ok(await _productService.GetByIdAsync(id));

        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategoryId([FromRoute] int categoryId) =>
            Ok(await _productService.GetProductsByCategoryIdAsync(categoryId));

        [HttpGet("{id:int}/for-update")]
        public async Task<IActionResult> GetForUpdate([FromRoute] int id) =>
            Ok(await _productService.GetForUpdateAsync(id));

        [HttpGet("discount/{discountId:int}")]
        public async Task<IActionResult> GetByDiscountId([FromRoute] int discountId) =>
            Ok(await _productService.GetProductsByDiscountIdAsync(discountId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO productDto)
        {
            int productId = await _productService.AddAsync(productDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = productId },
                new { id = productId, message = $"Product \"{productDto.Name}\" created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductUpdateDTO updateDto)
        {
            updateDto.Id = id;
            await _productService.UpdateAsync(updateDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}