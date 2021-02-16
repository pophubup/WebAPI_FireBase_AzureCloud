using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_FireBase_AzureCloud.Models;
using WebAPI_FireBase_AzureCloud.Repositories.IRepository;

namespace WebAPI_FireBase_AzureCloud.Repositories
{
    public class ProductRepository_FireBase : ICloudClient<FirestoreDb, Product>
    {
        private IHostEnvironment _hostingEnvironment;

        public ProductRepository_FireBase(IHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public FirestoreDb InitalCleinttCredential 
       {
            get
            {
                
                var path = _hostingEnvironment.ContentRootFileProvider.GetFileInfo("config\\getproducts-92bee-firebase-adminsdk-744dz-3461fb2344.json").PhysicalPath;
                GoogleCredential cred = GoogleCredential.FromFile(path);
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
          
            List<Product> products = new List<Product>();
            Query allCitiesQuery = InitalCleinttCredential.Collection("Products");
            QuerySnapshot allCitiesQuerySnapshot = allCitiesQuery.GetSnapshotAsync().GetAwaiter().GetResult();

            foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
            {
                Product product = documentSnapshot.ConvertTo<Product>();
                products.Add(product);
            }
            return products;
        }

        public List<Product> GetData(string ID)
        {
            throw new NotImplementedException();
        }

        public Product GetSingleData(string ID)
        {
            throw new NotImplementedException();
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
