using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class Hero : BaseEntity<int>, IImageEntity
    {
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string? Description { get; set; }

        public string ButtonText { get; set; } = null!;
        public string ButtonUrl { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
