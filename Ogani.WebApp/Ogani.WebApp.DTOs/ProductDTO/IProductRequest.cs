using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ProductDTO
{
    public interface IProductRequest
    {
        string Name { get; set; }
        string? Description { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
        string? Info { get; set; }
        decimal Weight { get; set; }

        bool IsFeatured { get; set; }
        bool IsAvailable { get; set; }

        IFormFile? Image { get; set; }

        int? CategoryId { get; set; }
        ICollection<int> DiscountIds { get; set; }
    }
}
