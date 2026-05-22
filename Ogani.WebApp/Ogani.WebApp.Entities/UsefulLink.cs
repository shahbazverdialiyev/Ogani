using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class UsefulLink:BaseEntity<int>
    {
        public int Section {  get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
