using MassTransit;
using Order.API.Model;
using Shared;

namespace Order.API.Consumer
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly AppDbContext _context;

        
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;

        public PaymentCompletedEventConsumer(AppDbContext context, ILogger<PaymentCompletedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order!=null){
                order.Status = OrderStatus.Success;

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
