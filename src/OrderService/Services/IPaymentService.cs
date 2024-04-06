using OrderService.Models;

namespace OrderService.Services
{
    public interface IPaymentService
    {
        Task<bool> PayMyGames(PaymentForm model);
    }
}