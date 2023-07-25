using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Model;

namespace Stock.API.Consumers
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
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (stock != null)
                {
                    _logger.LogInformation($"Payment failed,stock updated for {stock.ProductId} prev stock is {stock.Count}");
                    stock.Count += item.Count;
                   await  _context.SaveChangesAsync();
                    _logger.LogInformation($"Payment failed,stock updated for {stock.ProductId} new stock is {stock.Count}");
                }
            }
        }
    }
}
