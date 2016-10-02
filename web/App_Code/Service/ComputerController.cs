using System.Collections.Generic;
using System.Web.Http;
using Helpers;
using Models;

namespace Service
{
    public class ComputerController : ApiController
    {
        public IEnumerable<Models.Computer> Get()
        {
            BLL.Authorize.ApiAuth();
            return BLL.Computer.GetAll();
        }
    
        public Computer Get(int id)
        {
            BLL.Authorize.ApiAuth();
            return BLL.Computer.GetComputer(id);
        }

        public Computer GetFromMac(string mac)
        {
            BLL.Authorize.ApiAuth();
            return BLL.Computer.GetComputerFromMac(mac);
        }

        public Models.ValidationResult Post(Models.Computer value)
        {
            BLL.Authorize.ApiAuth();
            return BLL.Computer.AddComputer(value);
        }

        public Models.ValidationResult Put(Models.Computer value)
        {
            BLL.Authorize.ApiAuth();
            return BLL.Computer.UpdateComputer(value);
        }

        public Models.ValidationResult Delete(int id)
        {
            BLL.Authorize.ApiAuth();
            var computer = BLL.Computer.GetComputer(id);
            return BLL.Computer.DeleteComputer(computer);
        }
    }
}
