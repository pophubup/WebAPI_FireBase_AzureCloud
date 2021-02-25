using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

namespace WebAPI_FireBase_AzureCloud.Repositories
{
    public class FileRepository_Azure : ICloudClient<CloudBlobContainer, Product>
    {
        private IConfiguration _config;
        private IMemoryCache _cache;

        public FileRepository_Azure(IConfiguration config, IMemoryCache cache)
        {
            _config = config;
            _cache = cache;
        }
        public CloudBlobContainer InitalCleinttCredential
        {
            get
            {
                AzureBlobContainer info = _config.GetSection("BlobStorageAccount").Get<AzureBlobContainer>();
                StorageCredentials storageCredentials = new StorageCredentials(info.AccountName, info.AccountKey);
                CloudStorageAccount account = new CloudStorageAccount(storageCredentials, true);
                CloudBlobClient serviceClient = account.CreateCloudBlobClient();
                CloudBlobContainer container = serviceClient.GetContainerReference("products");
                return container;
                
            }
        }

        public StateContainer BulkDelete(IEnumerable<Product> obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer BulkInsert(IEnumerable<Product> obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer BulkUpdate(IEnumerable<Product> obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer DeleteData(string ID)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetData()
        {
            BlobContinuationToken continuationToken = null;
            var data = _cache.Get<List<Product>>("bundleFiles");
            if(data == null)
            {
                BlobResultSegment resultSegment = InitalCleinttCredential.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.Metadata, null, continuationToken, null, null).GetAwaiter().GetResult();
                List<Product> products = new List<Product>();
                resultSegment.Results.ToList().ForEach(x => {
                    CloudBlob blob = (CloudBlob)x;
                    products.Add(new Product()
                    {
                        productName = blob.Name.Split('.')[0],
                        productImg = blob.Uri.ToString()

                    });
                });
               
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.Set("bundleFiles", products, cacheEntryOptions);
                return products;
            }
            else
            {
                return data;
            }
          
        }

        public List<Product> GetData(string ID)
        {
            throw new NotImplementedException();
        }

        public Product GetSingleData(string ID)
        {
        
            return GetData().FirstOrDefault(x => x.productName == ID);
        }

        public StateContainer InsertData(Product obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer UpdateDAta(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
