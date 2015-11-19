﻿using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;
using DAL;
using Helpers;
using Pxe;

namespace BLL
{

    public class ActiveImagingTask
    {

        public static bool IsComputerActive(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Exists(a => a.ComputerId == computerId);
            }
        }

        public static bool DeleteActiveImagingTask(int activeImagingTaskId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var activeImagingTask = uow.ActiveImagingTaskRepository.GetById(activeImagingTaskId);
                var computer = uow.ComputerRepository.GetById(activeImagingTask.ComputerId);

                uow.ActiveImagingTaskRepository.Delete(activeImagingTask.Id);
                if (uow.Save())
                {
                    return new PxeFileOps().CleanPxeBoot(Utility.MacToPxeMac(computer.Mac));
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool AddActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.Insert(activeImagingTask);
                return uow.Save();
            }
        }

        public static void DeleteForMulticast(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
                uow.Save();
            }
        }

        public static bool UpdateActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.Update(activeImagingTask, activeImagingTask.Id);
                return uow.Save();
            }
        }

        public static List<Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Get(t => t.MulticastId == multicastId,
                    orderBy: q => q.OrderBy(t => t.ComputerId));
            }
        }

        public static List<Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.MulticastProgress(multicastId);
            }
        }

        public static List<Models.ActiveImagingTask> ReadAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Get(orderBy: q => q.OrderBy(t => t.Id));
            }
        }

        public static List<Models.ActiveImagingTask> ReadUnicasts()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Get(t => t.Type == "unicast",
                    orderBy: q => q.OrderBy(t => t.ComputerId));
            }
        }

        public static List<Models.Computer> GetMulticastComputers(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);
            }
        }

        public static Models.ActiveImagingTask GetTask(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.GetFirstOrDefault(x => x.ComputerId == computerId);
            }
        }
        public static void DeleteAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.DeleteRange();
                uow.Save();
            }
        } 
    }
}