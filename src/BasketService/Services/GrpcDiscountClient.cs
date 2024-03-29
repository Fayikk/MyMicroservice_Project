using Grpc.Net.Client;
using BasketService.Model;

namespace DiscountService.Services;

public class GrpcDiscountClient
{
    private readonly ILogger<GrpcDiscountClient> _logger;
    private readonly IConfiguration _configuration;
    public GrpcDiscountClient(ILogger<GrpcDiscountClient> logger,IConfiguration configuration)
    {
        _configuration=configuration;
        _logger=logger;
    }

    public Discount GetDiscount(string CouponCode)
    {
        _logger.LogWarning("Calling grpc protobuf service");
        var channel = GrpcChannel.ForAddress(_configuration["GrpcDiscount"]);
        var client = new GrpcDiscount.GrpcDiscountClient(channel);
        var request = new GetDiscountRequest{
            CouponCode = CouponCode
        };
        try
        {

            var response = client.GetDiscount(request);
            Discount Discount = new Discount{
                ExpireDate = response.Discount.ExpireDate.ToDateTime(),
                CouponCode = response.Discount.CouponCode,
                DiscountAmount = response.Discount.DiscountAmount,
                GameId = response.Discount.GameId,
                UserId = response.Discount.UserId
            };
            Console.WriteLine(response);
            return Discount;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
            throw ex;
        }
    }
}