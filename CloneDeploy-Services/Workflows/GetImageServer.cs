using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services.Workflows
{
    public class GetImageServer
    {
        private readonly ComputerEntity _computer;
        public GetImageServer(ComputerEntity computer)
        {
            _computer = computer;
        }

        public int Run()
        {
            //Find best distribution point to use

            int dpId;
            var distributionPointServices = new DistributionPointServices();
           
            if (Settings.OperationMode == "Single")
                dpId = distributionPointServices.GetPrimaryDistributionPoint().Id;
            else
            {


                var clusterServices = new ClusterGroupServices();

                ClusterGroupEntity clusterGroup;

                if (_computer != null)
                {
                    clusterGroup = new ComputerServices().GetClusterGroup(_computer.Id);

                }
                else
                {
                    //on demand computer might be null
                    //use default cluster group
                    clusterGroup = clusterServices.GetDefaultClusterGroup();

                }

                var queueSizesDict = new Dictionary<int, int>();
                var toRemove = new List<DistributionPointEntity>();
                var availableDistributionPoints =
                    clusterServices.GetClusterDps(clusterGroup.Id)
                        .Select(
                            clusterDp => distributionPointServices.GetDistributionPoint(clusterDp.DistributionPointId))
                        .ToList();

                foreach (var dp in availableDistributionPoints)
                {
                    if(dp.QueueSize == 0)
                        toRemove.Add(dp);
                    else
                        queueSizesDict.Add(dp.Id, dp.QueueSize);
                }
                availableDistributionPoints = availableDistributionPoints.Except(toRemove).ToList();

                var taskInUseDict = new Dictionary<int, int>();
                foreach (var dp in availableDistributionPoints)
                {
                    var counter = 0;
                    foreach (var activeTask in new ActiveImagingTaskServices().GetAll().Where(x => x.Status != "0"))
                    {
                        if (activeTask.DpId == dp.Id)
                        {
                            counter++;
                        }
                    }

                    taskInUseDict.Add(dp.Id, counter);
                }

                var freeDps = new List<DistributionPointEntity>();
                foreach (var dp in availableDistributionPoints)
                {
                    if (taskInUseDict[dp.Id] < queueSizesDict[dp.Id])
                        freeDps.Add(dp);
                }

                if (freeDps.Count == 1)
                    dpId = freeDps.First().Id;

                else if (freeDps.Count > 1)
                {
                    var freeDictionary = new Dictionary<int,int>();
                    foreach (var dp in freeDps)
                    {
                        freeDictionary.Add(dp.Id,taskInUseDict[dp.Id]);
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
