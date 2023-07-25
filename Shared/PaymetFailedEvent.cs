using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaymetFailedEvent
    {
        public int OrderId { get; set; }

        public string BuyerId { get; set; }

        public String? Message { get; set; }
    }
}
