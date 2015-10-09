using System;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class Port
    {
        public static bool AddPort(Models.Port port)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.PortRepository.Insert(port);
                return uow.Save();
            }
        }

        public static int GetNextPort()
        {
            var lastPort = new Models.Port();
            var nextPort = new Models.Port();
            using (var uow = new DAL.UnitOfWork())
            {
                lastPort =
                    uow.PortRepository.GetFirstOrDefault(orderBy: (q => q.OrderByDescending(p => p.Id)));

            }

            if (lastPort == null)
                nextPort.Number = Convert.ToInt16(Settings.StartPort);
            else if (nextPort.Number >= Convert.ToInt16(Settings.EndPort))
                nextPort.Number = Convert.ToInt16(Settings.StartPort);
            else
                nextPort.Number = lastPort.Number + 2;

            AddPort(nextPort);

            return nextPort.Number;
        }
    }
}