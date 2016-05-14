using System.Collections.Generic;
using System.Web.Http;
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
            return BLL.Computer.GetComputer(id);
        }

        public Computer GetFromMac(string mac)
        {
            return BLL.Computer.GetComputerFromMac(mac);
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
