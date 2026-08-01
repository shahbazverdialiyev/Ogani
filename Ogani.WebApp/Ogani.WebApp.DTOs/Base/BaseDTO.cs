using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DTOs.Base
{
    public abstract class BaseDTO<TKey>
        where TKey : notnull
    {
        public TKey Id { get; set; } = default!;
        public bool Status { get; set; }
    }
}
