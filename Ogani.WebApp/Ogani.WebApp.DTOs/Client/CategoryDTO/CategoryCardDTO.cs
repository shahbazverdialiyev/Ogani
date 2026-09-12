using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Client.CategoryDTO
{
    public class CategoryCardDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;

        public string? ImageUrl { get; init; }
    }
}
