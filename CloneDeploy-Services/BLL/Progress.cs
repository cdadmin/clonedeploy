using CloneDeploy_Entities;
using CloneDeploy_Services;

namespace CloneDeploy_App.BLL
{
    public class TaskProgress
    {
        public int ComputerId { get; set; }
        public string Progress { get; set; }

     

        public void UpdateProgressPartition(string computerName, string partition)
        {
            var activeTask = new ActiveImagingTaskEntity
            {
                Elapsed = "",
                Remaining = "",
                Completed = "",
                Rate = "",
                Partition = partition,
            };
            new ActiveImagingTaskServices().UpdateActiveImagingTask(activeTask);
        }
    }
}