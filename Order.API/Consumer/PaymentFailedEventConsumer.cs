using MassTransit;
using Order.API.Model;
using Shared;

namespace Order.API.Consumer
{
    public class PaymentFailedEventConsumer : IConsumer<PaymetFailedEvent>
    {
        private readonly AppDbContext _context;

        //for log
        private readonly ILogger<PaymentFailedEventConsumer> _logger;

        public PaymentFailedEventConsumer(AppDbContext context, ILogger<PaymentFailedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymetFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.Message = context.Message.Message;

                await _context.SaveChangesAsync();

                _logger.LogInformation($" Order Id {context.Message.OrderId} status changed to {order.Status}");
            }
            else
            {
                _logger.LogError($"Order Id {context.Message.OrderId} not found");
            }
        }
    }
}
