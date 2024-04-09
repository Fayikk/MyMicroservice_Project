using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Entities;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrderByUserId();
    }
}