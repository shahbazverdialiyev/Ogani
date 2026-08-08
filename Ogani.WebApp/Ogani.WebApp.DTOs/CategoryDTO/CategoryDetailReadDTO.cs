using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.CategoryDTO
{
    public class CategoryDetailReadDTO
    {
        public string Name { get; init; } = null!;
        public string? Description { get; init; }

        public string? ImageUrl { get; init; }

        public IReadOnlyCollection<string> ProductNames { get; init; } = [];
    }
}
