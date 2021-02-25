using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

namespace WebAPI_FireBase_AzureCloud.Repositories
{
    public class OrderRepository_FireBase : ICloudClient<FirestoreDb, Order>
    {
        private IConfiguration _config;
        private IMemoryCache _cache;

        public OrderRepository_FireBase(IConfiguration config, IMemoryCache cache)
        {
            _config = config;
            _cache = cache;
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
        public StateContainer BulkDelete(IEnumerable<Order> obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer BulkInsert(IEnumerable<Order> obj)
        {
            WriteBatch batch = InitalCleinttCredential.StartBatch();
            DocumentReference nycRef = InitalCleinttCredential.Collection("Orders").Document(DateTime.Now.ToString("yyyyMMddHHmmss"));
            string msg = string.Empty;
            obj.ToList().ForEach(x =>
            {
                batch.Set(nycRef, x);
                msg += $"ProductId : {x.productId},";
            });

            
            // Commit the batch
             batch.CommitAsync().GetAwaiter().GetResult();
            return new StateContainer()
            {
                isActionSuccess = true,
                Message = $"{msg} Insert Success"
            };
        }

        public StateContainer BulkUpdate(IEnumerable<Order> obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer DeleteData(string ID)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetData()
        {
            var data = _cache.Get<List<Order>>("bundleOrder");
            if (data == null)
            {
                List<Order> Orders = new List<Order>();
                Query allCitiesQuery = InitalCleinttCredential.Collection("Orders");
                QuerySnapshot allCitiesQuerySnapshot = allCitiesQuery.GetSnapshotAsync().GetAwaiter().GetResult();
                foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
                {
                    Order order = documentSnapshot.ConvertTo<Order>();
                    Orders.Add(order);
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.Set("bundleOrder", Orders, cacheEntryOptions);
                return Orders;
            }
            else
            {

                return data;
            }

        }

        public List<Order> GetData(string ID)
        {
            throw new NotImplementedException();
        }

        public Order GetSingleData(string ID)
        {
            Query allCitiesQuery = InitalCleinttCredential.Collection("Orders");
            Query query = allCitiesQuery.WhereEqualTo("orderId", ID);
            QuerySnapshot querySnapshot =  query.GetSnapshotAsync().GetAwaiter().GetResult();
            
            return querySnapshot.Documents[0].ConvertTo<Order>();
        }

        public StateContainer InsertData(Order obj)
        {
            throw new NotImplementedException();
        }

        public StateContainer UpdateDAta(Order obj)
        {
            throw new NotImplementedException();
        }
    }
}
