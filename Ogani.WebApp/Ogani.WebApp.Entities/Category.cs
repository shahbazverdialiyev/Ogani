using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class Category:BaseEntity<int>
    {
        public string Name { get; set; }=null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<Product>? Products { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
