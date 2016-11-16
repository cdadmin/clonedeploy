using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public interface IAPICall
{
    IGenericAPI<Models.Computer> ComputerApi { get; }
    TokenApi TokenApi { get; }
    User CloneDeployUserApi { get; }
}