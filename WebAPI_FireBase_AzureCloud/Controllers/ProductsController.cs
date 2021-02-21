using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

namespace WebAPI_FireBase_AzureCloud.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private ICloudClient<FirestoreDb, Product> _products;
        private ICloudClient<CloudBlobContainer, Product> _file;
        public ProductsController(ILogger<ProductsController> logger, ICloudClient<FirestoreDb, Product> products, ICloudClient<CloudBlobContainer, Product> file)
        {
            _logger = logger;
            _products = products;
            _file = file;
        }
        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        public IEnumerable<Product> GetProducts()
        {

            var products = _products.GetData(); 
            products.ForEach(x =>
            {
                x.ProductImagePath = _file.GetSingleData(x.ProductName).ProductImagePath;
            });
            return products;
        }
    }
}
