using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Client.ProductDTO
{
    public class ProductDetailDTO : ProductCardDTO
    {
        public string? Description { get; init; }
        public string? Info { get; init; }
        public decimal Weight { get; init; }
        public bool IsAvailable { get; init; }
    }
}
