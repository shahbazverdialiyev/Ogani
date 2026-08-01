using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.DTOs.DiscountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ProductDTO
{
    public class ProductReadDTO : BaseDTO<int>
    {
        public string Name { get; init; } = null!;
        public decimal Price { get; init; }
        public int Quantity { get; init; }

        public bool IsFeatured { get; init; }
        public bool IsAvailable { get; init; }

        public string? ImageUrl { get; init; }

        public DateTime CreatedDate { get; init; }

        public int? CategoryId { get; init; }
        public string? CategoryName { get; init; }
    }
}
