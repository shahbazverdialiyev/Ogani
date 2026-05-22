using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.DataAccess.Abstracts;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Concretes.EFCore
{
    public class EFCoreContactRepository : EFCoreRepository<Contact, int>, IContactRepository
    {
        public EFCoreContactRepository(OganiDbContext context) : base(context) { }

    }
}
