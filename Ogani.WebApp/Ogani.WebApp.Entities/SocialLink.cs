using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class SocialLink:BaseEntity<int>
    {
        public string Platform { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
