using System;
using System.Linq;
using CloneDeploy_Common;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class PortServices
    {
        private readonly UnitOfWork _uow;

        public PortServices()
        {
            _uow = new UnitOfWork();
        }

        public bool AddPort(PortEntity port)
        {
            _uow.PortRepository.Insert(port);
            _uow.Save();
            return true;
        }

        public int GetNextPort()
        {
            var lastPort = new PortEntity();
            var nextPort = new PortEntity();

            lastPort =
                _uow.PortRepository.GetFirstOrDefault(orderBy: q => q.OrderByDescending(p => p.Id));

            if (lastPort == null)
                nextPort.Number = Convert.ToInt32(SettingServices.GetSettingValue(SettingStrings.StartPort));
            else if (nextPort.Number >= Convert.ToInt32(SettingServices.GetSettingValue(SettingStrings.EndPort)))
                nextPort.Number = Convert.ToInt32(SettingServices.GetSettingValue(SettingStrings.StartPort));
            else
                nextPort.Number = lastPort.Number + 2;

            AddPort(nextPort);

            return nextPort.Number;
        }
    }
}