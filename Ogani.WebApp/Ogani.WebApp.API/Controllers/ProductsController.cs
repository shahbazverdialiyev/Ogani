using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.Client;
using Ogani.WebApp.DTOs.Client.ProductDTO;

namespace Ogani.WebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) =>
            _productService = productService;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDTO? filter)
        {
            filter ??= new ProductFilterDTO();

            if (filter.Page <= 0 || filter.PageSize <= 0)
            {
                return BadRequest(new { message = "Page and PageSize must be greater than zero." });
            }

            var result = await _productService.GetProductsForShopAsync(filter);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts() => Ok(await _productService.GetFeaturedProductsAsync());

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestProducts() => Ok(await _productService.GetLatestProductsAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductDetail([FromRoute] int id) => Ok(await _productService.GetProductDetailAsync(id));

        [HttpGet("category/{id:int}")]
        public async Task<IActionResult> GetProductsByCategoryForUI([FromRoute] int id) => Ok(await _productService.GetProductsByCategoryForUIAsync(id));

        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedProducts() => Ok(await _productService.GetDiscountedProductsAsync());
    }
}