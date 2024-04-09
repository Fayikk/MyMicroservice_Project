using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Entities;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private string UserId;
        public OrderRepository(ApplicationDbContext context,IHttpContextAccessor contextAccessor)
        {
              UserId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context = context; 
        }

        public async Task<List<Order>> GetOrderByUserId()
        {
            var result = await _context.Orders.Where(x=>x.UserId == Guid.Parse(UserId) && !x.IsPaid).ToListAsync();
            if (result is not null)
            {
                return result;
            }
            return null;
        }
    }
}