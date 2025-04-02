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
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; } = true;

        public void SetModified()
        {
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
