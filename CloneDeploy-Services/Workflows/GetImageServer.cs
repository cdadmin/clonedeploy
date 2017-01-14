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

        public string Run()
        {
            //Find best image server to use
            var imageServerIdentifier = "";
            if (Settings.OperationMode == "Single")
                imageServerIdentifier = Settings.ServerIdentifier;
            else
            {
                var availableImageShareServers = new List<string>();

                var clusterServices = new ClusterGroupServices();
                var secondaryServerServices = new SecondaryServerServices();

                List<ClusterGroupServerEntity> clusterServers;
                if (_computer != null)
                {
                    var clusterGroup = new ComputerServices().GetClusterGroup(_computer.Id);
                    clusterServers = clusterServices.GetClusterServers(clusterGroup.Id);
                }
                else
                {
                    //on demand computer might be null
                    //use default cluster group
                    var clusterGroup = clusterServices.GetDefaultClusterGroup();
                    clusterServers = clusterServices.GetClusterServers(clusterGroup.Id);
                }


                foreach (var clusterServer in clusterServers.Where(x => x.ImageRole == 1))
                {
                    availableImageShareServers.Add(clusterServer.SecondaryServerId == -1
                        ? Settings.ServerIdentifier
                        : secondaryServerServices.GetSecondaryServer(clusterServer.SecondaryServerId).Name);
                }

                var queueSizesDict = new Dictionary<string, int>();
                var toRemove = new List<string>();
                foreach (var imageShareServer in availableImageShareServers)
                {
                    string queueSize;
                    if (imageShareServer == Settings.ServerIdentifier)
                        queueSize = Settings.QueueSize;
                    else
                        queueSize = new APICall(new SecondaryServerServices().GetApiToken(imageShareServer)).SettingApi.GetImageShareSettings().QueueSize;
                    if (string.IsNullOrEmpty(queueSize))
                    {
                        toRemove.Add(imageShareServer);
                        continue;
                    }
                    queueSizesDict.Add(imageShareServer, Convert.ToInt32(queueSize));
                }
                availableImageShareServers = availableImageShareServers.Except(toRemove).ToList();
                var taskInUseDict = new Dictionary<string, int>();
                foreach (var imageshareServer in availableImageShareServers)
                {
                    var counter = 0;
                    foreach (var activeTask in new ActiveImagingTaskServices().GetAll().Where(x => x.Status != "0"))
                    {
                        if (activeTask.ImageServer == imageshareServer)
                        {
                            counter++;
                        }
                    }

                    taskInUseDict.Add(imageshareServer, counter);
                }

                var freeImageServers = new List<string>();
                foreach (var imageShareServer in availableImageShareServers)
                {
                    if (taskInUseDict[imageShareServer] < queueSizesDict[imageShareServer])
                        freeImageServers.Add(imageShareServer);
                }

                if (freeImageServers.Count == 1)
                    imageServerIdentifier = freeImageServers.First();
                else if (freeImageServers.Count > 1)
                {
                    var freeDictionary = new Dictionary<string,int>();
                    foreach (var freeImageServer in freeImageServers)
                    {
                        freeDictionary.Add(freeImageServer,taskInUseDict[freeImageServer]);
                    }

                    var duplicateFreeDict = freeDictionary.GroupBy(x => x.Value).Where(x => x.Count() > 1);

                    if (duplicateFreeDict.Count() == freeDictionary.Count)
                    {
                        //all image servers have equal free slots - randomly choose one.
                        var random = new Random();
                        var index = random.Next(0, freeImageServers.Count);
                        imageServerIdentifier = freeImageServers[index];
                    }
                    else
                    {
                        //Just grab the first one with the smallest queue, could be a duplicate but will eventually even out on it's own
                        var orderedInUse = freeDictionary.OrderBy(x => x.Value);
                        imageServerIdentifier = orderedInUse.First().Key;
                    }


                   
                }
                else
                {
                    //Free image servers count is 0, pick the one with the lowest number of tasks
                    var orderedInUse = taskInUseDict.OrderBy(x => x.Value);
                    imageServerIdentifier = orderedInUse.First().Key;

                }

            }
            return imageServerIdentifier;
        }
    }
}
