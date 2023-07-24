using MassTransit;
using Shared;
using AppDbContext = Stock.API.Model.AppDbContext;
using Microsoft.EntityFrameworkCore;
using MassTransit.Transports;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent> //mass transit'i event'i dinlediğimden haberdar et
    {
        //make Db connection
        private readonly AppDbContext _context;

        //for log
        private ILogger<OrderCreatedEventConsumer> _logger;

        //since storckReservedEvent will only be listened by PaymentAPI, we can use send method instead of publish
        private readonly ISendEndpointProvider _sendEndpointProvider;

        private readonly IPublishEndpoint _publishEndpoint; // in publich method we do not need to indicate queue name

        public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        // method that will run when a message arrives to queue
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context) 
        {
            var stockResult = new List<bool>();

            foreach (var item in context.Message.OrderItems) //Check we have enough products
            {
                stockResult.Add(
                    await _context.Stocks.AnyAsync(x => x.ProductId== item.ProductId && x.Count>= item.Count)
                    );
                
            }

            if(stockResult.All(x => x.Equals(true)))
            {

                //update remaining product amount
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($" Stock was reserved for Buyer Id: {context.Message.BuyerId}");


                //send stockReservedEvent, we created queue-> since we use send method we should give the name of the queue
                var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri
                    ($"queue:{RabbitMQSettings.StockReservedEventQueueName}"));

                StockReservedEvent stockReservedEvent = new StockReservedEvent()
                { //create a new event 
                    Payment = context.Message.Payment, // payment information coming from the orderCreatedEvent
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItem = context.Message.OrderItems,

                };

                //send to the queue
                await sendEndPoint.Send(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent()
                {//we do not give the queue name because subscriber will give the name

                    OrderId =context.Message.OrderId,
                    Message="Not enough stock",
                });

                _logger.LogInformation($" Stock was  not reserved for Buyer Id: {context.Message.BuyerId}");
            }


           
        }
    }
}
