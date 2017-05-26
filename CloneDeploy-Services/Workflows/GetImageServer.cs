using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_Services.Workflows
{
    public class GetImageServer
    {
        private readonly ComputerEntity _computer;
        private readonly string _direction;
        private Random _random;


        public GetImageServer(ComputerEntity computer,string direction)
        {
            _computer = computer;
            _direction = direction;
            _random = new Random();
        }

        public int Run()
        {
            //Find best distribution point to use

            int dpId;
            var distributionPointServices = new DistributionPointServices();

            if (SettingServices.ServerIsNotClustered)
            {
                var dp = distributionPointServices.GetPrimaryDistributionPoint();
                if (dp != null)
                    return dp.Id;
                else
                {
                    return -1;
                }
            }

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

            //Something went wrong
            if (clusterGroup == null) return -1;

            var queueSizesDict = new Dictionary<int, int>();
            var toRemove = new List<DistributionPointEntity>();
            var clusterDps = clusterServices.GetClusterDps(clusterGroup.Id);
            var aDps = new List<DistributionPointEntity>();
            foreach (var clusterDp in clusterDps)
            {
                aDps.Add(distributionPointServices.GetDistributionPoint(clusterDp.DistributionPointId));
            }
            var availableDistributionPoints =
                clusterServices.GetClusterDps(clusterGroup.Id)
                    .Select(
                        clusterDp => distributionPointServices.GetDistributionPoint(clusterDp.DistributionPointId))
                    .ToList();

            //Check if any Distribution point in the cluster group is the primary dp
            foreach (var dp in availableDistributionPoints.Where(dp => dp.IsPrimary == 1))
            {
                //Cluster Group has a primary, always return the primary for an upload, not necessary but saves syncing to the primary later
                if (_direction == "pull")
                    return dp.Id;
            }

            foreach (var dp in availableDistributionPoints)
            {
                if (dp.QueueSize == 0)
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
                var freeDictionary = new Dictionary<int, int>();
                var slotsInUseList = new List<int>();
                foreach (var dp in freeDps)
                {
                    freeDictionary.Add(dp.Id, taskInUseDict[dp.Id]);
                    slotsInUseList.Add(taskInUseDict[dp.Id]);
                }

                if (slotsInUseList.All(x => x == slotsInUseList[0]))
                {
                    //all image servers have equal free slots - randomly choose one.                 
                    var index = _random.Next(0, freeDps.Count);
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

            return dpId;
        }
    }
}