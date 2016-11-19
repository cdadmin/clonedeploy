namespace CloneDeploy_App.BLL
{
    public class TaskProgress
    {
        public int ComputerId { get; set; }
        public string Progress { get; set; }

     

        public void UpdateProgressPartition(string computerName, string partition)
        {
            var activeTask = new Models.ActiveImagingTask
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