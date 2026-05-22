using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Entities
{
    public class BaseEntity<TKey>
        where TKey : notnull
    {
        public TKey Id { get; set; }= default!;
        public bool Status { get; set; } = true;

        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    }
}
