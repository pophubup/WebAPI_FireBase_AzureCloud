using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_FireBase_AzureCloud.Models
{
    [FirestoreData]
    public class Product
    {
        [FirestoreProperty]
        public int  CategoryID { get; set; }
        [FirestoreProperty]
        public string ProductDescription { get; set; }
        [FirestoreProperty]
        public string ProductID { get; set; }
        [FirestoreProperty]
        public string ProductImagePath { get; set; }
        [FirestoreProperty]
        public string ProductName { get; set; }
        [FirestoreProperty]
        public double ProductPrice { get; set; }
        [FirestoreProperty]
        public double Quantity { get; set; }

    }
}
