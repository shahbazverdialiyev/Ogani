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

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDTO? filter)
        {
            filter ??= new ProductFilterDTO();

            if (filter.Page <= 0 || filter.PageSize <= 0)
            {
                return BadRequest(new { message = "Page and PageSize must be greater than zero." });
            }

            try
            {
                var result = await _productService.GetProductsForShopAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching products.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            try
            {
                var featuredProducts = await _productService.GetFeaturedProductsAsync();
                return Ok(featuredProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching featured products.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestProducts()
        {
            try
            {
                var latestProducts = await _productService.GetLatestProductsAsync();
                return Ok(latestProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching latest products.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductDetail([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid product ID provided." });
            }

            try
            {
                var productDetail = await _productService.GetProductDetailAsync(id);

                if (productDetail == null)
                {
                    return NotFound(new { message = $"Product detail not found for ID {id}." });
                }

                return Ok(productDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching product details.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("category/{id:int}")]
        public async Task<IActionResult> GetProductsByCategoryForUI([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid category ID provided." });
            }

            try
            {
                var products = await _productService.GetProductsByCategoryForUIAsync(id);

                if (products == null)
                {
                    return NotFound(new { message = $"No products found for category ID {id}." });
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching products by category.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedProducts()
        {
            try
            {
                var discountedProducts = await _productService.GetDiscountedProductsAsync();
                return Ok(discountedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching discounted products.",
                    detail = ex.Message
                });
            }
        }
    }
}