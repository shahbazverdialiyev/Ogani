using Microsoft.AspNetCore.Http;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; }= null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Info { get; set; }
        public double Weight { get; set; }
        public bool IsFeatured { get; set; } = false;
        public bool IsAvailable { get; set; }= false;
        public string? ImageUrl { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public void SetModified()
        {
            ModifiedDate = DateTime.UtcNow;
        }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Discount> Discounts { get; set; }= new List<Discount>();
    }
}
