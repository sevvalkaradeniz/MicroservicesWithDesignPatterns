using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumer;
using Order.API.Model;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<PaymentCompletedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint(RabbitMQSettings.OrderPaymentCompletedEventQueueuName, e =>
        {
            // with e declare which consumer will listen this queue (StockOrderCreatedEventQueueName)
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));// give connection
});// add reference to Order.API





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
