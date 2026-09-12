namespace Ogani.WebApp.UI.Models.Shop
{
    public class ShopFilterVM
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal AvailableMinPrice { get; set; }

        public decimal AvailableMaxPrice { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 8;
    }
}
