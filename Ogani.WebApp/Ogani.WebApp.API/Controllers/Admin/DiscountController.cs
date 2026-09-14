using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.DiscountDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await _discountService.GetAllAsync();
            return Ok(discounts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var discount = await _discountService.GetByIdAsync(id);
                return Ok(discount);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountCreateDTO discountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _discountService.AddAsync(discountDto);
                return StatusCode(StatusCodes.Status201Created, new { message = "Discount created successfully." });
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
        public async Task<IActionResult> Update(int id, [FromBody] DiscountUpdateDTO discountDto)
        {
            if (id != discountDto.Id)
                return BadRequest(new { message = "ID mismatch in request URL and body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _discountService.UpdateAsync(discountDto);
                return Ok(new { message = $"Discount \"{discountDto.Code}\" was updated successfully." });
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
                await _discountService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{discountId:int}/products")]
        public async Task<IActionResult> GetProductsForManage([FromRoute] int discountId)
        {
            if (discountId <= 0)
            {
                return BadRequest(new { message = "Invalid discount ID provided." });
            }

            try
            {
                DiscountProductsDTO discountDto = await _discountService.GetProductsForManageAsync(discountId);
                return Ok(discountDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred while fetching discount products.",
                    detail = ex.Message
                });
            }
        }

        [HttpPut("{discountId:int}/products")]
        public async Task<IActionResult> UpdateProducts([FromRoute] int discountId, [FromBody] DiscountProductsDTO discountDto)
        {
            if (discountId <= 0)
            {
                return BadRequest(new { message = "Invalid discount ID provided." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure route ID matches DTO ID
            discountDto.DiscountId = discountId;

            try
            {
                await _discountService.UpdateProductsAsync(discountDto.DiscountId, discountDto.SelectedProductIds);

                return Ok(new { message = "Discount products updated successfully." });
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
                    message = "An unexpected error occurred while updating discount products.",
                    detail = ex.Message
                });
            }
        }
    }
}