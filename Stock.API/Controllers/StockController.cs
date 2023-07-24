using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Model;
using System.Runtime.CompilerServices;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        //get method
        private readonly AppDbContext _context; // readonly used to set variables just in the constructor, we cannot set them somewhere else

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet] //getting remainig stock from the database
        public async Task<IActionResult> Get() {
            return Ok(await _context.Stocks.ToListAsync());
        }
       
    }
}
