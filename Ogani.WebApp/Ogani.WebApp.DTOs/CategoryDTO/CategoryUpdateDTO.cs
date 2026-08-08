using Microsoft.AspNetCore.Http;
using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.CategoryDTO
{
    public class CategoryUpdateDTO : BaseDTO<int>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public bool RemoveExistingImage { get; set; }
    }
}
