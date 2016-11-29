using CloneDeploy_Entities;
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
            BLL.ActiveImagingTask.UpdateActiveImagingTask(activeTask);
        }
    }
}