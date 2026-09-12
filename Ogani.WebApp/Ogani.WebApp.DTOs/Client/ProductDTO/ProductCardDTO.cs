using Ogani.WebApp.DTOs.DiscountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Client.ProductDTO
{
    public class ProductCardDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public decimal Price { get; init; }
        public string? ImageUrl { get; init; }
        public string? CategoryName { get; init; }
        public decimal DiscountPercentage { get; init; }
        public decimal DiscountedPrice => Price - (Price * DiscountPercentage / 100);
    }
}
