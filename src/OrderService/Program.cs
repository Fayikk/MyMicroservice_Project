using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OrderService.Base;
using OrderService.Consumer;
using OrderService.Data;
using OrderService.Repository;
using OrderService.Services;
using OrderService.Services.GrpcFolder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>(); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(BaseResponse));
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["AuthorirtyServiceUrl"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.NameClaimType = "username";
});
builder.Services.AddMassTransit(opt => {

    opt.AddConsumersFromNamespaceContaining<CheckoutBasketConsumer>();

    opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search",false));
    opt.UsingRabbitMq((context,cfg) => {
        cfg.Host(builder.Configuration["RabbitMQ:Host"],"/",host => {
            host.Username(builder.Configuration.GetValue("RabbitMQ:Username","guest"));
            host.Username(builder.Configuration.GetValue("RabbitMQ:Password","guest"));
        });

        cfg.ReceiveEndpoint("order-created",e => {
            e.UseMessageRetry(r=>r.Interval(5,5));
            e.ConfigureConsumer<CheckoutBasketConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddScoped<GrpcMyGameClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
