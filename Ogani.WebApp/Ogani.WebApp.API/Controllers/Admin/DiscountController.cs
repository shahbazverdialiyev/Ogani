using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.DiscountDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService) =>
            _discountService = discountService;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _discountService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) =>
            Ok(await _discountService.GetByIdAsync(id));

        [HttpGet("{discountId:int}/products")]
        public async Task<IActionResult> GetProductsForManage([FromRoute] int discountId) =>
            Ok(await _discountService.GetProductsForManageAsync(discountId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountCreateDTO discountDto)
        {
            var discountId = await _discountService.AddAsync(discountDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = discountId },
                new { id = discountId, message = "Discount created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DiscountUpdateDTO discountDto)
        {
            discountDto.Id = id;
            await _discountService.UpdateAsync(discountDto);
            return NoContent();
        }

        [HttpPut("{discountId:int}/products")]
        public async Task<IActionResult> UpdateProducts([FromRoute] int discountId, [FromBody] DiscountProductsDTO discountDto)
        {
            discountDto.DiscountId = discountId;
            await _discountService.UpdateProductsAsync(discountDto.DiscountId, discountDto.SelectedProductIds);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.DeleteAsync(id);
            return NoContent();
        }
    }
}