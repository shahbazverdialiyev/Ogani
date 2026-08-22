using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Interfaces
{
    public interface IDiscountRepository : IRepository<Discount, int>
    {
        Task<List<Discount>> GetActiveDiscountsAsync();

        Task<List<Discount>> GetExpiredDiscountsAsync();

        Task<List<Discount>> GetUpcomingDiscountsAsync();

        Task<Discount?> GetByIdWithProductsAsync(int id, bool tracking = false);
    }
}
