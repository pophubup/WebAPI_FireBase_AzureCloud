using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_FireBase_AzureCloud.Models
{
    public class OrderDetail
    {
        public Product product { get; set; }
        public IEnumerable<Order> order { get; set; }
  

    }
}
