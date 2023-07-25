using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.API.Consumers;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    //inform about the consumer
    x.AddConsumer<StockReservedEventConsumer>();


    x.UsingRabbitMq((context, cfg) =>
    {

        //declare the queue 
        cfg.ReceiveEndpoint(RabbitMQSettings.StockReservedEventQueueName, e => //since StockReservedEvent used send method our queue name is already defined
        {
            // with "e" declare which consumer will listen this queue (StockReservedEventQueueName)
            e.ConfigureConsumer<StockReservedEventConsumer>(context);
        });
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
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





    app.Run();
