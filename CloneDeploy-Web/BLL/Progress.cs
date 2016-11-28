using CloneDeploy_Web.Models;

namespace Tasks
{
    public class TaskProgress
    {
        public int ComputerId { get; set; }
        public string Progress { get; set; }

     
        //moved not needed
        public void UpdateProgressPartition(string computerName, string partition)
        {
            var activeTask = new ActiveImagingTask
            {
                Elapsed = "",
                Remaining = "",
                Completed = "",
                Rate = "",
                Partition = partition,
            };
            BLL.ActiveImagingTask.UpdateActiveImagingTask(activeTask);
        }
    }
}