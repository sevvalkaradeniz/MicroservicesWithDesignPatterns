using MassTransit;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {

        private readonly ILogger<StockReservedEventConsumer> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        // method that will run when a message arrives to queue
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {

            var balance = 3000m; // m for make it decimal

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} was withdrawn from credit card for user id: {context.Message.BuyerId}");
               

                await _publishEndpoint.Publish(new PaymentSuccessedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId=context.Message.OrderId,
                });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} was not withdrawn from credit card for user id: {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymetFailedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message="not enough balance",
                });
            }
            //throw new NotImplementedException();
        }
    }
}
