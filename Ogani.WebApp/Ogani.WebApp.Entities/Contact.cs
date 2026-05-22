using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class Contact:BaseEntity<int>
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
