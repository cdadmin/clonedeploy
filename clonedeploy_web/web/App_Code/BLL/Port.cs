using System;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class Port
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public Port()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddPort(Models.Port port)
        {
            _unitOfWork.PortRepository.Insert(port);
            return _unitOfWork.Save();
        }

        public int GetNextPort()
        {
            var lastPort = _unitOfWork.PortRepository.GetFirstOrDefault(orderBy: (q => q.OrderByDescending(p => p.Id)));
            var nextPort = new Models.Port();

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