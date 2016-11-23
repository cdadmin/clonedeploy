using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace CloneDeploy_Web.APICalls
{
    /// <summary>
    /// Summary description for IGenericAPI
    /// </summary>
    public interface IGenericAPI<T>
    {
        List<T> Get(int limit, string searchstring);
        List<T> Get(string searchstring);
        T Get(int id);
        ActionResult Put(int id, T tObject);
        ActionResult Post(T tObject);
        ActionResult Delete(int id);
        ApiDTO GetCount();
    }
}