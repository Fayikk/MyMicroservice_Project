using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Repository;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository; 
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository; 
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetMyOrder()
        {
            var response = await _orderRepository.GetOrderByUserId();
            return Ok(response);    
        }
    }
}