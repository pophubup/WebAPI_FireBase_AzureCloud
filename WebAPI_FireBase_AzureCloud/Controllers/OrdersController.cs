using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_FireBase_AzureCloud.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
     
        private ICloudClient<FirestoreDb, Product> _products;
        private ICloudClient<CloudBlobContainer, Product> _file;
        private ICloudClient<FirestoreDb, Order> _order;
        /// <summary>
        /// set Repositories 
        /// </summary>
        /// <param name="products"></param>
        /// <param name="file"></param>
        /// <param name="order"></param>
        public OrdersController( ICloudClient<FirestoreDb, Product> products, ICloudClient<CloudBlobContainer, Product> file, ICloudClient<FirestoreDb, Order> order)
        {
            _products = products;
            _file = file;
            _order = order;
        }
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param></param>
        /// <returns>OrderDetail</returns>
        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        [SwaggerResponse(statusCode: 200, Type = typeof(List<OrderDetail>))]
        [SwaggerResponse(statusCode: 401, Type = typeof(string))]
        public IEnumerable<OrderDetail> GetOrders()
        {
            var orders = _order.GetData();
            var product = _products.GetData();
            List<OrderDetail> contain = new List<OrderDetail>();
            product.ForEach(x => {
                if(orders.Any(g=>g.productId == x.productId))
                {
                    contain.Add(new OrderDetail()
                    {
                        product = x,
                        order = orders.Where(g=>g.productId == x.productId)
                    });
                }
                
            });
            return contain;
        }
        /// <summary>
        /// Get single order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>OrderDetail</returns>
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        [SwaggerResponse(statusCode: 200, Type = typeof(OrderDetail))]
        [SwaggerResponse(statusCode: 401, Type = typeof(string))]
        public OrderDetail GetOrder([FromForm] string orderId)
        {
            var targetOrder = _order.GetSingleData(orderId);
            var targetProduct = _products.GetSingleData(targetOrder.productId);
            return new OrderDetail()
            {
                order = new List<Order> { targetOrder },
                product = targetProduct
              
            };
        }
        /// <summary>
        /// Insert bulk orders then rturn what detail with product detail and List orders
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        ///  [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        [SwaggerResponse(statusCode: 200, Type = typeof(List<OrderDetail>))]
        [SwaggerResponse(statusCode: 401, Type = typeof(string))]
        [HttpPost]
        public List<OrderDetail> InsertOrder([FromBody] IEnumerable<Order> orders)
        {
           var result = _order.BulkInsert(orders);
           var data = new List<OrderDetail>();
            
            orders.GroupBy(g=>g.productId).ToList().ForEach(g =>
            {
                var targetProduct = _products.GetSingleData(g.Key);
                data.Add(new OrderDetail() { product = targetProduct, order = g.Select(f=>f) });
            });
            
            return data;
            
        }

    }
}
