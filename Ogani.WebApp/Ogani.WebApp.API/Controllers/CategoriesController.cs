using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesForUIAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching categories.",
                    detail = ex.Message
                });
            }
        }
    }
}