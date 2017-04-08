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

            int serverId;
           

            if (Settings.OperationMode == "Single")
                serverId = -1;
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

                var freeDps = new List<DistributionPointEntity>();
                foreach (var dp in availableDistributionPoints)
                {
                    if (taskInUseDict[dp.Id] < queueSizesDict[dp.Id])
                        freeDps.Add(dp);
                }

                if (taskInUseDict.Count == 1)
                    serverId = taskInUseDict.Keys.First();

                else if (taskInUseDict.Count > 1)
                {
                    var freeDictionary = new Dictionary<int, int>();
                    foreach (var dp in freeDps)
                    {
                        freeDictionary.Add(dp.Id, taskInUseDict[dp.Id]);
                    }

                    var duplicateFreeDict = freeDictionary.GroupBy(x => x.Value).Where(x => x.Count() > 1);

                    if (duplicateFreeDict.Count() == freeDictionary.Count)
                    {
                        //all image servers have equal free slots - randomly choose one.
                        var random = new Random();
                        var index = random.Next(0, freeDps.Count);
                        dpId = freeDps[index].Id;
                    }
                    else
                    {
                        //Just grab the first one with the smallest queue, could be a duplicate but will eventually even out on it's own
                        var orderedInUse = freeDictionary.OrderBy(x => x.Value);
                        dpId = orderedInUse.First().Key;
                    }
                }
                else
                {
                    //Free image servers count is 0, pick the one with the lowest number of tasks to be added to the queue
                    var orderedInUse = taskInUseDict.OrderBy(x => x.Value);
                    dpId = orderedInUse.First().Key;

                }

            }
            return dpId;
        }
    }
}
