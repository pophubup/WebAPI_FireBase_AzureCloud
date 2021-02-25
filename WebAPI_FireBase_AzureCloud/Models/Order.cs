using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_FireBase_AzureCloud.Models
{
    [FirestoreData]
    public class Order
    {
        [FirestoreProperty]
        public string productId { get; set; }
        [FirestoreProperty]
        public string orderId { get; set; }
        [FirestoreProperty]
        public int quantity { get; set; }
        [FirestoreProperty]
        public int total { get; set; }
    }
}
