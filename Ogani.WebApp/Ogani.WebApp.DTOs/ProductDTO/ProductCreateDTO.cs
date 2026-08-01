using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ProductDTO
{
    public class ProductCreateDTO : IProductRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Info { get; set; }
        public decimal Weight { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsAvailable { get; set; }

        public IFormFile? Image { get; set; }

        public int? CategoryId { get; set; }
        public ICollection<int> DiscountIds { get; set; } = [];
    }
}
