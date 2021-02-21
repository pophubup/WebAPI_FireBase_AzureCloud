using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_FireBase_AzureCloud.Models
{
    public class AzureBlobContainer
    {
        public string DefaultEndpointsProtocol { get; set; }  
         public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string EndpointSuffix { get; set; }
  
    }
}
