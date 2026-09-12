namespace Ogani.WebApp.DTOs.Client
{
    public class PagedResultDTO<T>
    {
        public IReadOnlyCollection<T> Items { get; set; } = [];
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
