using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;
using DAL;
using Helpers;
using Pxe;

namespace BLL
{

    public class ActiveImagingTask
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public bool DeleteActiveImagingTask(int activeImagingTaskId)
        {
            var activeImagingTask = _unitOfWork.ActiveImagingTaskRepository.GetById(activeImagingTaskId);
            var computer = _unitOfWork.ComputerRepository.GetById(activeImagingTask.ComputerId);

            _unitOfWork.ActiveImagingTaskRepository.Delete(activeImagingTask.Id);
            if (_unitOfWork.Save())
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
            if (_unitOfWork.ActiveImagingTaskRepository.Exists(h => h.ComputerId == activeImagingTask.ComputerId))
            {
                Message.Text = "A Task Is Already Running For This Computer";
                return false;
            }
            _unitOfWork.ActiveImagingTaskRepository.Insert(activeImagingTask);
            if (_unitOfWork.Save())
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
            _unitOfWork.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
        }

        public bool UpdateActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            _unitOfWork.ActiveImagingTaskRepository.Update(activeImagingTask, activeImagingTask.Id);
            return _unitOfWork.Save();
        }

        public List<Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
        {
            return _unitOfWork.ActiveImagingTaskRepository.Get(t => t.MulticastId == multicastId, orderBy: q => q.OrderBy(t => t.ComputerId) );
        }

        public List<Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {
            return _unitOfWork.ActiveImagingTaskRepository.MulticastProgress(multicastId);
        }

        public List<Models.ActiveImagingTask> ReadAll()
        {
            return _unitOfWork.ActiveImagingTaskRepository.Get(orderBy: q => q.OrderBy(t => t.Id));
        }

        public List<Models.ActiveImagingTask> ReadUnicasts()
        {
            return _unitOfWork.ActiveImagingTaskRepository.Get(t => t.Type == "unicast",
                orderBy: q => q.OrderBy(t => t.ComputerId));
        }

        public List<Models.Computer> GetMulticastComputers(int multicastId)
        {
            return _unitOfWork.ActiveImagingTaskRepository.MulticastComputers(multicastId);
        }
        public void CancelAll()
        {
            CancelAllImagingTasks.Run();
        }

       
    }
}