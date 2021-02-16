using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_FireBase_AzureCloud.Models;

namespace WebAPI_FireBase_AzureCloud.Repositories.IRepository
{
    public interface ICloudClient<Client ,T> where T :class
                                             where Client :class
    {
        Client InitalCleinttCredential { get; }
        List<T> GetData();
        List<T> GetData(string ID);
        T GetSingleData(string ID);

        StateContainer InsertData(T obj);
        StateContainer BulkInsert(IEnumerable<T> obj);
        StateContainer UpdateDAta(T obj);
        StateContainer BulkUpdate(IEnumerable<T> obj);
        StateContainer DeleteData(string ID);
        StateContainer BulkDelete(IEnumerable<T> obj);
    }
}
