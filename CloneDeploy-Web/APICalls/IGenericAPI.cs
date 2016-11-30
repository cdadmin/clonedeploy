using System.Collections.Generic;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web.Models;

namespace CloneDeploy_Web.APICalls
{
    /// <summary>
    /// Summary description for IGenericAPI
    /// </summary>
    public interface IGenericAPI<T>
    {
        List<T> GetAll(int limit, string searchstring);
        List<T> Get(string searchstring);
        T Get(int id);
        ActionResultDTO Put(int id, T tObject);
        ActionResultDTO Post(T tObject);
        ActionResultDTO Delete(int id);
        string GetCount();
    }
}