using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services.Workflows
{
    public class GetMulticastServer
    {
        private readonly GroupEntity _group;
        public GetMulticastServer(GroupEntity group)
        {
            _group = group;
        }

        public int Run()
        {
            //Find the best multicast server to use

            int serverId = -1;
           

            if (Settings.OperationMode == "Single")
                return serverId;
            else
            {
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
                    clusterGroup = clusterServices.GetDefaultClusterGroup();

                }

                var availableMulticastServers =
                        new ClusterGroupServices().GetClusterServers(clusterGroup.Id).Where(x => x.MulticastRole == 1);

               
                var taskInUseDict = new Dictionary<int, int>();
                foreach (var mServer in availableMulticastServers)
                {
                    var counter = new ActiveMulticastSessionServices().GetAll().Count(x => x.ServerId == mServer.SecondaryServerId);

                    taskInUseDict.Add(mServer.SecondaryServerId, counter);
                }

           

                if (taskInUseDict.Count == 1)
                    serverId = taskInUseDict.Keys.First();

                else if (taskInUseDict.Count > 1)
                {
                    var orderedInUse = taskInUseDict.OrderBy(x => x.Value);

                    if (taskInUseDict.Values.Distinct().Count() == 1)
                    {
                        //all multicast server have equal tasks - randomly choose one.
                        var random = new Random();
                        var index = random.Next(0, taskInUseDict.Count);
                        serverId = taskInUseDict[index];
                    }
                    //Just grab the first one with the smallest queue, could be a duplicate but will eventually even out on it's own               
                    serverId = orderedInUse.First().Key;

                   
                }
              

            }
            return serverId;
        }
    }
}
