using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService) =>
            _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetCategories() => Ok(await _categoryService.GetCategoriesForUIAsync());
    }
}