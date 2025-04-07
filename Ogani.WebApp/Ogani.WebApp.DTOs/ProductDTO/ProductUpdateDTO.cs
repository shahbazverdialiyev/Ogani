using Microsoft.AspNetCore.Http;
using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ProductDTO
{
    public class ProductUpdateDTO : BaseDTO<int>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Info { get; set; }
        public double Weight { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsAvailable { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
