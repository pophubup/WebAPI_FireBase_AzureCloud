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
        public int categoryId { get; set; }
        [FirestoreProperty]
        public int productPrice { get; set; }
        [FirestoreProperty]
        public int Stock { get; set; }
        [FirestoreProperty]
        public string productDes { get; set; }
        [FirestoreProperty]
        public string productId { get; set; }
        [FirestoreProperty]
        public string productImg { get; set; }
        [FirestoreProperty]
        public string productName { get; set; }
       
        [FirestoreProperty]
        public DateTime createDate { get; set; }
        [FirestoreProperty]
        public string createID { get; set; }
        [FirestoreProperty]
        public string productVido { get; set; }

    }
}
