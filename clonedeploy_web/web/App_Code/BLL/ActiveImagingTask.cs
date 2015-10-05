using System.Collections.Generic;
using BLL.Workflows;
using DAL;
using Helpers;
using Pxe;

namespace BLL
{


    public class ActiveImagingTask
    {
        private readonly DAL.ActiveImagingTask _da = new DAL.ActiveImagingTask();
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public bool DeleteActiveImagingTask(int activeImagingTaskId)
        {
            var activeImagingTask = _da.Read(activeImagingTaskId);
            var computer = _unitOfWork.Computer.GetById(activeImagingTask.ComputerId);

            if (_da.Delete(activeImagingTaskId))
            {
                if (new PxeFileOps().CleanPxeBoot(Utility.MacToPxeMac(computer.Mac)))
                {
                    Message.Text = "Successfully Deleted Task";
                    return true;
                }
                else
                {
                    Message.Text = "Could Not Delete Task";
                    return false;
                }
            }
            else
            {
                Message.Text = "Could Not Delete Task";
                return false;
            }
        }

        public bool AddActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            if (_da.Exists(activeImagingTask.ComputerId))
            {
                Message.Text = "A Task Is Already Running For This Computer";
                return false;
            }
            if (_da.Create(activeImagingTask))
            {
                Message.Text = "Successfully Created Task";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Task";
                return false;
            }
        }

        public void DeleteForMulticast(int multicastId)
        {
            _da.DeleteForMulticast(multicastId);
        }

        public bool UpdateActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            return _da.Update(activeImagingTask);
        }

        public List<Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
        {
            return _da.MulticastMemberStatus(multicastId);
        }

        public List<Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {
            return _da.MulticastProgress(multicastId);
        }

        public List<Models.ActiveImagingTask> ReadAll()
        {
            return _da.ReadAll();
        }

        public List<Models.ActiveImagingTask> ReadUnicasts()
        {
            return _da.ReadUnicasts();
        }

        public List<Models.Computer> GetMulticastComputers(int multicastId)
        {
            return _da.MulticastComputers(multicastId);
        }
        public void CancelAll()
        {
            CancelAllImagingTasks.Run();
        }

       
    }
}