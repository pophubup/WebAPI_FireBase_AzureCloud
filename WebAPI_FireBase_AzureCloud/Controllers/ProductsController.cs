using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

namespace WebAPI_FireBase_AzureCloud.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private ICloudClient<FirestoreDb, Product> _products;
        /// <summary>
        /// 
        /// </summary>
        public ProductsController(ILogger<ProductsController> logger, ICloudClient<FirestoreDb, Product> products)
        {
            _logger = logger;
            _products = products;
        }
        /// <summary>
        /// need Authorization to get All Products
        /// </summary>
        /// <param header="Bear ...."></param>  
        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        [SwaggerResponse(statusCode: 200, Type = typeof(IEnumerable<Product>))]
        [SwaggerResponse(statusCode: 401, Type = typeof(string))]
        public IEnumerable<Product> GetProducts()
        {
            return _products.GetData();
        }
    }
}
