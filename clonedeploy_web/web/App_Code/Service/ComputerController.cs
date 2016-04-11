using System;
using System.Activities.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using CsvHelper;
using Helpers;
using Models;


namespace Service
{
    

    public class ComputerController : ApiController
    {

        public IEnumerable<Models.Computer> Get()
        {
            return BLL.Computer.GetAll();
        }

        
        public Computer Get(int id)
        {
            return BLL.Computer.GetComputer(id);
        }

        public Models.ValidationResult Post(Models.Computer value)
        {
            return BLL.Computer.AddComputer(value);
        }

        public Models.ValidationResult Put(Models.Computer value)
        {
            return BLL.Computer.UpdateComputer(value);
        }

        public Models.ValidationResult Delete(int id)
        {
            var computer = BLL.Computer.GetComputer(id);
            return BLL.Computer.DeleteComputer(computer);
        }
    }
}
