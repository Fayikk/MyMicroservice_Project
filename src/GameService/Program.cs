using GameService.Base;
using GameService.Consumers;
using GameService.Data;
using GameService.GameRepository;
using GameService.Repositories;
using GameService.Repositories.ForCategory;
using GameService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<GameDatabaseContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(BaseResponseModel));
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IGameRepository,GameRepository>();
builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(opt => {

    opt.AddEntityFrameworkOutbox<GameDatabaseContext>(x=>{
        x.QueryDelay = TimeSpan.FromSeconds(10);

        x.UsePostgres();
        x.UseBusOutbox();
    });
    opt.AddConsumersFromNamespaceContaining<GameCreatedFaultConsumer>();

    opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("game",false));
    opt.UsingRabbitMq((context,cfg) => {
        cfg.Host(builder.Configuration["RabbitMQ:Host"],"/",host => {
            host.Username(builder.Configuration.GetValue("RabbitMQ:Username","guest"));
            host.Username(builder.Configuration.GetValue("RabbitMQ:Password","guest"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["AuthorirtyServiceUrl"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.NameClaimType = "username";
});
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcGameService>();
app.MapGrpcService<GrpcMyGameService>();
app.Run();
