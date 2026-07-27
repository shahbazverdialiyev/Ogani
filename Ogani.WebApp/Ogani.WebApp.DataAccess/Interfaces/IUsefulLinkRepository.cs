using Ogani.WebApp.Entities;
using Ogani.WebApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Interfaces
{
    public interface IUsefulLinkRepository : IRepository<UsefulLink, int>
    {
        Task<List<UsefulLink>> GetBySectionAsync(UsefulLinkSection section);
    }
}
