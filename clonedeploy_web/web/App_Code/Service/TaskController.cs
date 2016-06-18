using System.Collections.Generic;
using System.Web.Http;
using Models;

namespace Service
{
    public class TaskController : ApiController
    {
        public string StartComputerUpload(int id)
        {
             var userId = BLL.Authorize.ApiAuth();
            return new BLL.Workflows.Unicast(BLL.Computer.GetComputer(id), "pull", userId).Start();
        }

        public string StartComputerUpload(string mac)
        {
            var userId = BLL.Authorize.ApiAuth();
            return new BLL.Workflows.Unicast(BLL.Computer.GetComputerFromMac(mac), "pull", userId).Start();
        }

        public string StartComputerDeploy(int id)
        {
            var userId = BLL.Authorize.ApiAuth();
            return new BLL.Workflows.Unicast(BLL.Computer.GetComputer(id), "push", userId).Start();
        }

        public string StartComputerDeploy(string mac)
        {
            var userId = BLL.Authorize.ApiAuth();
            return new BLL.Workflows.Unicast(BLL.Computer.GetComputerFromMac(mac), "push", userId).Start();
        }

        public string StartGroupMulticast(int id)
        {
            var userId = BLL.Authorize.ApiAuth();
            var group = BLL.Group.GetGroup(id);
            return new BLL.Workflows.Multicast(group, userId).Create();
            
        }

        public string StartGroupUnicast(int id)
        {
            var userId = BLL.Authorize.ApiAuth();
            var group = BLL.Group.GetGroup(id);
            return BLL.Group.StartGroupUnicast(group, userId).ToString();
        }
       
    }
}
