using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Order.API.DTOs;
using Order.API.Model;
using Shared;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly AppDbContext _context;

        // for sending our event to the rabbitMQ
        private readonly IPublishEndpoint _publishEndpoint;


        public OrdersController(AppDbContext context, IPublishEndpoint publishEndpoint) // send both database variable and rabbitMQ variable to the constructor
        {
            _context = context;
            _publishEndpoint = publishEndpoint;

        }

        [HttpPost] //end point, post since we record and order
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)  
        {
            //The IActionResult return type is appropriate when multiple ActionResult return types are possible in an action.
            //The ActionResult types represent various HTTP status codes. 

            var newOrder = new Model.Order //creating an order
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address { Line = orderCreate.Address.Line, 
                    Province = orderCreate.Address.Province,
                    District = orderCreate.Address.District,
                },
                CreatedDate= DateTime.Now,
                // order Id will be given by the system when we succefully add the order to the database
                
            };

            orderCreate.OrderItems.ForEach(item =>
            {
                newOrder.Items.Add(new OrderItem(){
                    Price= item.Price,
                    ProductId= item.ProductId,
                    Count= item.Count,
                });
            });

            await _context.AddAsync(newOrder); //adding new order
            await _context.SaveChangesAsync();

            //if we succesfully save the order, we can create the event

            var OrderCreatedEvent = new OrderCreatedEvent {
                BuyerId = orderCreate.BuyerId,
                OrderId= newOrder.Id, // if we can succefully save the order, OrderId given automatically
                Payment= new PaymentMessage
                {
                    CardName= orderCreate.Payment.CardName,
                    CardNumber = orderCreate.Payment.CardNumber,
                    Expiration = orderCreate.Payment.Expiration,
                    CVV = orderCreate.Payment.CVV,
                    //TotalPrice= orderCreate.Payment.TotalPrice,
                    TotalPrice= orderCreate.OrderItems.Sum(item => item.Price * item.Count),

                }

               
            };

            orderCreate.OrderItems.ForEach(item =>
            {
                OrderCreatedEvent.OrderItems.Add(new OrderItemMessage
                {
                    Count= item.Count,
                    ProductId= item.ProductId,
                });
            });

            //since we created our event, now we can send it to the queue

            await _publishEndpoint.Publish(OrderCreatedEvent);

            return Ok();
        }
    }
}


//Differences between MVC (Model, View, Controller) and ASP.NET Web API are:
//MVC is for developing applications that return both data and views,
//while Web API only returns data using the HTTP services. 