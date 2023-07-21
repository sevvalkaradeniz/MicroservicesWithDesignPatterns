using Microsoft.EntityFrameworkCore;

namespace Stock.API.Model
{
    public class AppDbContext : DbContext //include base
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // we will fill this opiton in the Program.cs file
        {

        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
