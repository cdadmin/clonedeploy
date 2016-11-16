using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

/// <summary>
/// Summary description for IGenericAPI
/// </summary>
public interface IGenericAPI<T>
{
    List<T> Get(int limit, string searchstring);
    T Get(int id);
    ActionResult Put(int id, T tObject);
    ActionResult Post(T tObject);
    ActionResult Delete(int id);
    ApiDTO GetCount();
}