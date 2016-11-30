using System;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class Port
    {
        public static bool AddPort(PortEntity port)
        {
            using (var uow = new UnitOfWork())
            {
                uow.PortRepository.Insert(port);
                uow.Save();
                return true;
            }
        }

        public static int GetNextPort()
        {
            var lastPort = new PortEntity();
            var nextPort = new PortEntity();
            using (var uow = new UnitOfWork())
            {
                lastPort =
                    uow.PortRepository.GetFirstOrDefault(orderBy: (q => q.OrderByDescending(p => p.Id)));

            }

            if (lastPort == null)
                nextPort.Number = Convert.ToInt32(Settings.StartPort);
            else if (nextPort.Number >= Convert.ToInt32(Settings.EndPort))
                nextPort.Number = Convert.ToInt32(Settings.StartPort);
            else
                nextPort.Number = lastPort.Number + 2;

            AddPort(nextPort);

            return nextPort.Number;
        }
    }
}