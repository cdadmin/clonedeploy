using System;
using System.Linq;
using Models;

namespace Tasks
{
    public class TaskProgress
    {
        public int ComputerId { get; set; }
        public string Progress { get; set; }

     

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