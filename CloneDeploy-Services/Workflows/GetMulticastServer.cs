using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_Services.Workflows
{
    public class GetMulticastServer
    {
        private readonly GroupEntity _group;
        private readonly Random _random;
        private readonly int _clusterId;

        public GetMulticastServer(GroupEntity group)
        {
            _group = group;
            _random = new Random();
        }

        public GetMulticastServer(int clusterId)
        {
            //For on demand no group
            _clusterId = clusterId;
        }

        public int Run()
        {
            //Find the best multicast server to use

            var serverId = -1;

            if (SettingServices.ServerIsNotClustered)
                return serverId;
            var clusterServices = new ClusterGroupServices();

            ClusterGroupEntity clusterGroup;

            if (_group != null)
            {
                clusterGroup = new GroupServices().GetClusterGroup(_group.Id);
            }
            else
            {
                //on demand group might be null
                //use default cluster group
                if(_clusterId == -1)
                clusterGroup = clusterServices.GetDefaultClusterGroup();
                else
                {
                    clusterGroup = new ClusterGroupServices().GetClusterGroup(_clusterId);
                }
            }

            var availableMulticastServers =
                new ClusterGroupServices().GetClusterMulticastServers(clusterGroup.Id);

            if (!availableMulticastServers.Any())
                return -2;

            var taskInUseDict = new Dictionary<int, int>();
            foreach (var mServer in availableMulticastServers)
            {
                var counter =
                    new ActiveMulticastSessionServices().GetAll()
                        .Count(x => x.ServerId == mServer.ServerId);

                taskInUseDict.Add(mServer.ServerId, counter);
            }

            if (taskInUseDict.Count == 1)
                serverId = taskInUseDict.Keys.First();

            else if (taskInUseDict.Count > 1)
            {
                var orderedInUse = taskInUseDict.OrderBy(x => x.Value);

                if (taskInUseDict.Values.Distinct().Count() == 1)
                {
                    //all multicast server have equal tasks - randomly choose one.

                    var index = _random.Next(0, taskInUseDict.Count);
                    serverId = taskInUseDict[index];
                }
                else
                {
                    //Just grab the first one with the smallest queue, could be a duplicate but will eventually even out on it's own               
                    serverId = orderedInUse.First().Key;
                }
            }
            return serverId;
        }
    }
}