using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class Discount:BaseEntity<int>
    {
        public string Code { get; set; } = null!;
        public decimal DiscountPercentage {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
