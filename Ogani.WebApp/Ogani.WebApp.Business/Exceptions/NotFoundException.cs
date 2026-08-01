using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string entityName, object id)
        : base($"{entityName} with ID '{id}' was not found.") { }

        public NotFoundException(string ex):base(ex) { }
    }
}
