using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Consumers;
using Stock.API.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    //inform about the consumer
    x.AddConsumer<OrderCreatedEventConsumer>();


    x.UsingRabbitMq((context, cfg) =>
    {

        //declare the queue 
        cfg.ReceiveEndpoint(RabbitMQSettings.StockOrderCreatedEvenetQueueName,e =>
        {
            // with e declare which consumer will listen this queue (StockOrderCreatedEventQueueName)
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("StockDb"); //since it is in memory we should create seeds 
});

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

using (var scope = app.Services.CreateScope())
{
    //we need a db context
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    //adding stocks

    context.Stocks.Add(new Stock.API.Model.Stock() { Id = 1, ProductId = 1, Count = 100 });
    context.Stocks.Add(new Stock.API.Model.Stock() { Id = 2, ProductId = 2, Count = 100 });
    context.SaveChanges(); // since it will run just once we do not need to make it async
}

app.Run();
