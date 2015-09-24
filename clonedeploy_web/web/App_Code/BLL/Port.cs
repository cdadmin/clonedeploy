using Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class Port
    {
        private readonly DAL.Port _da = new DAL.Port();

        public bool AddPort(Models.Port port)
        {
            return _da.Create(port);
        }

        public int GetNextPort()
        {
            var lastPort = _da.GetLastUsedPort();
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