using System.Collections.Generic;

namespace CloneDeploy_Common.DbUpgrades
{
    public class VersionMapping
    {
        private readonly Dictionary<int, int> _mapping;

        public VersionMapping()
        {
            _mapping  = new Dictionary<int,int>();
            _mapping.Add(130, 1300);
            _mapping.Add(131, 1301);
            _mapping.Add(132, 1302);
            _mapping.Add(133, 1303);
            _mapping.Add(134, 1304);
            _mapping.Add(135, 1305);
        }

        public Dictionary<int, int> Get()
        {
            return _mapping;  
        }
    }
}
