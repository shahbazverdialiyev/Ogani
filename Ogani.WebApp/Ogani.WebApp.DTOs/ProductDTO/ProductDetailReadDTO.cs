using Ogani.WebApp.DTOs.DiscountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.ProductDTO
{
    public class ProductDetailReadDTO:ProductReadDTO
    {
        public string? Description { get; init; }
        public string? Info { get; init; }
        public decimal Weight { get; init; }

        public DateTime? ModifiedDate { get; init; }

        public IReadOnlyCollection<DiscountReadDTO> Discounts { get; init; } = [];
    }
}
