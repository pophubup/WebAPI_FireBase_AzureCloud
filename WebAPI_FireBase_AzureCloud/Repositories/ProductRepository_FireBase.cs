﻿using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

namespace WebAPI_FireBase_AzureCloud.Repositories
{
    public class ProductRepository_FireBase : ICloudClient<FirestoreDb, Product>
    {
        private IConfiguration _config;
        private IMemoryCache _cache;
        private ICloudClient<CloudBlobContainer, Product> _file;
        public ProductRepository_FireBase(IConfiguration config, IMemoryCache cache, ICloudClient<CloudBlobContainer, Product> file)
        {
            _config = config;
            _cache = cache;
            _file = file;
        }
        public FirestoreDb InitalCleinttCredential 
       {
            get
            {

                FirebBase data = _config.GetSection("Firebase").Get<FirebBase>();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                GoogleCredential cred = GoogleCredential.FromJson(json);
                Grpc.Core.Channel channel = new Grpc.Core.Channel(FirestoreClient.DefaultEndpoint.Host,
                             FirestoreClient.DefaultEndpoint.Port,
                               cred.ToChannelCredentials());
                FirestoreClient client = FirestoreClient.Create(channel);
                FirestoreDb db = FirestoreDb.Create("getproducts-92bee", client);
                return db;
            }
             
        }

        public StateContainer BulkDelete(IEnumerable<Product> obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer BulkInsert(IEnumerable<Product> obj)
        {
            string msg = string.Empty;
            WriteBatch batch = InitalCleinttCredential.StartBatch();
            DocumentReference nycRef = InitalCleinttCredential.Collection("orders").Document(DateTime.Now.ToString("yyyyMMddHHmmss"));
            obj.ToList().ForEach(x =>
            {
                batch.Set(nycRef, x);
                msg += $"{x.productId},";
            });
            var result = batch.CommitAsync().GetAwaiter().GetResult();
            return new StateContainer()
            {
                isActionSuccess = true,
                Message = msg + " Bulk Insert Success"
            };
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
        
            var data = _cache.Get<List<Product>>("bundleProduct");
            if (data == null)
            {
                List<Product> products = new List<Product>();
                Query allCitiesQuery = InitalCleinttCredential.Collection("products");
                QuerySnapshot allCitiesQuerySnapshot = allCitiesQuery.GetSnapshotAsync().GetAwaiter().GetResult();
                foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
                {
                    Product product = documentSnapshot.ConvertTo<Product>();
                    products.Add(product);
                }
                products.ForEach(x =>
                {
                    x.productImg = _file.GetSingleData(x.productName).productImg;
                });
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.Set("bundleProduct", products, cacheEntryOptions);
               
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
            CollectionReference citiesRef = InitalCleinttCredential.Collection("products");
            Query query = citiesRef.WhereEqualTo("productId", ID);
            QuerySnapshot querySnapshot = query.GetSnapshotAsync().GetAwaiter().GetResult();
            var result = querySnapshot.Documents[0].ConvertTo<Product>();
            result.productImg = _file.GetSingleData(result.productName).productImg;
            return result;
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
