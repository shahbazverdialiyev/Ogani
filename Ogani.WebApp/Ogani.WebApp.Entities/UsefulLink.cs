using Ogani.WebApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class UsefulLink:BaseEntity<int>
    {
        public UsefulLinkSection Section {  get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
