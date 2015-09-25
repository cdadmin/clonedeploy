using System.Linq;
using Global;
using Models;

namespace Tasks
{
    public class TaskProgress
    {
        public string HostName { get; set; }
        public string Progress { get; set; }

        public void UpdateProgress()
        {
            var values = Progress.Split('*').ToList();
            var activeTask = new ActiveImagingTask
            {
                Elapsed = values[1],
                Remaining = values[2],
                Completed = values[3],
                Rate = values[4],
            };

            new BLL.ActiveImagingTask().UpdateActiveImagingTask(activeTask);
        }

        public void UpdateProgressPartition(string hostName, string partition)
        {
            var activeTask = new ActiveImagingTask
            {
                Elapsed = "",
                Remaining = "",
                Completed = "",
                Rate = "",
                Partition = partition,
            };
            new BLL.ActiveImagingTask().UpdateActiveImagingTask(activeTask);
        }
    }
}