using Microsoft.EntityFrameworkCore;

namespace Order.API.Model
{
    [Owned]  // since it should be in the Order table
    public class Address
    {
       
        public string Line { get; set; }

        public string Province { get; set; }

        public string District { get; set; }
    }
}
