using System.Collections.Generic;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class Multicast
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");
        private readonly string _clientCount;
        private readonly bool _isOnDemand;
        private readonly ActiveMulticastSessionEntity _multicastSession;
        private List<ComputerEntity> _computers;
        private readonly GroupEntity _group;
        private ImageProfileWithImage _imageProfile;
        private readonly int _userId;
        private int _multicastServerId;

        //Constructor For Starting Multicast For Group
        public Multicast(int groupId, int userId)
        {
            _computers = new List<ComputerEntity>();
            _multicastSession = new ActiveMulticastSessionEntity();
            _isOnDemand = false;
            _group = new GroupServices().GetGroup(groupId);
            _userId = userId;

        }

        //Constructor For Starting Multicast For On Demand
        public Multicast(int imageProfileId, string clientCount, int userId)
        {
            _multicastSession = new ActiveMulticastSessionEntity();
            _isOnDemand = true;
            _imageProfile = new ImageProfileServices().ReadProfile(imageProfileId);
            _clientCount = clientCount;
            _group = new GroupEntity { ImageProfileId = _imageProfile.Id };
            _userId = userId;
            _multicastSession.ImageProfileId = _imageProfile.Id;
        }

        public string Create()
        {
            _imageProfile = new ImageProfileServices().ReadProfile(_group.ImageProfileId);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            var validation = new ImageServices().CheckApprovalAndChecksum(_imageProfile.Image,_userId);
            if (!validation.Success) return validation.ErrorMessage;

            _multicastSession.Port = new PortServices().GetNextPort();
            if (_multicastSession.Port == 0)
            {
                return "Could Not Determine Current Port Base";
            }

            _multicastServerId = _isOnDemand
                ? new Workflows.GetMulticastServer().Run()
                : new Workflows.GetMulticastServer(_group).Run();

            if (_multicastServerId == -2)
                return "Could Not Find Any Available Multicast Servers";

      
            _multicastSession.UserId = _userId;
            //End of the road for starting an on demand multicast
            if (_isOnDemand)
            {
                _multicastSession.Name = _group.Name;
                _group.Name =_multicastSession.Port.ToString();
                var onDemandprocessArguments = GenerateProcessArguments();
                if (onDemandprocessArguments == 0)
                    return "Could Not Start The Multicast Application";
                else
                    return "Successfully Started Multicast " + _group.Name;
            }

            //Continue On If multicast is for a group
            _multicastSession.Name = _group.Name;
            _computers = new GroupServices().GetGroupMembers(_group.Id);
            if (_computers.Count < 1)
            {
                return "The group Does Not Have Any Members";
            }

            var activeMulticastSessionServices = new ActiveMulticastSessionServices();
            if (!activeMulticastSessionServices.AddActiveMulticastSession(_multicastSession))
            {
                return "Could Not Create Multicast Database Task.  An Existing Task May Be Running.";
            }

            if (!CreateComputerTasks())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Create Computer Database Tasks.  A Computer May Have An Existing Task.";
            }

            if (!CreatePxeFiles())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Create Computer Boot Files";
            }

            if (!CreateTaskArguments())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Create Computer Task Arguments";
            }

            var processArguments = GenerateProcessArguments();
            if (processArguments == 0)
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Start The Multicast Application";
            }
            
            foreach (var computer in _computers)
                Utility.WakeUp(computer.Mac);

            return "Successfully Started Multicast " + _group.Name;
        }

        private bool CreateComputerTasks()
        {
            var error = false;
            var activeTaskIds = new List<int>();
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            foreach (var computer in _computers)
            {
                if (new ComputerServices().IsComputerActive(computer.Id)) return false;
                var activeTask = new ActiveImagingTaskEntity
                {
                    Type = "multicast",
                    ComputerId = computer.Id,
                    Direction = "push",
                    MulticastId = _multicastSession.Id,
                    UserId = _userId
                    
                };

                if (activeImagingTaskServices.AddActiveImagingTask(activeTask))
                {
                    activeTaskIds.Add(activeTask.Id);
                    //computer.ActiveImagingTask = activeTask;
                }
                else
                {
                    error = true;
                    break;
                }
            }
            if (error)
            {
                foreach (var taskId in activeTaskIds)
                    activeImagingTaskServices.DeleteActiveImagingTask(taskId);

                return false;
            }
            return true;
        }

        private bool CreatePxeFiles()
        {
            foreach (var computer in _computers)
            {
                if (!new TaskBootMenu(computer, _imageProfile, "push").CreatePxeBootFiles())
                    return false;
            }
            return true;
        }

        private bool CreateTaskArguments()
        {
            foreach (var computer in _computers)
            {
                var activeTask = new ComputerServices().GetTaskForComputer(computer.Id);
                activeTask.Arguments =
                    new CreateTaskArguments(computer, _imageProfile, "multicast").Run(_multicastSession.Port.ToString());
                if (!new ActiveImagingTaskServices().UpdateActiveImagingTask(activeTask))
                    return false;
            }
            return true;
        }

        private int GenerateProcessArguments()
        {
            var multicastArgs = new MulticastArgsDTO();
            multicastArgs.schema = new ClientPartitionHelper(_imageProfile).GetImageSchema();
            multicastArgs.Environment = _imageProfile.Image.Environment;
            multicastArgs.ImageName = _imageProfile.Image.Name;
            multicastArgs.Port = _multicastSession.Port.ToString();
            if (_isOnDemand)
            {
                multicastArgs.ExtraArgs = Settings.SenderArgs;
                if (!string.IsNullOrEmpty(_clientCount))
                    multicastArgs.clientCount = _clientCount;
            }
            else
            {
                multicastArgs.ExtraArgs = string.IsNullOrEmpty(_imageProfile.SenderArguments)
                    ? Settings.SenderArgs
                    : _imageProfile.SenderArguments;
                multicastArgs.clientCount = _computers.Count.ToString();
            }

            var pid = 0;
            if (_multicastServerId == -1)
                pid = new Workflows.MulticastArguments().GenerateProcessArguments(multicastArgs);
            else
            {
                var secondaryServer =
                                    new SecondaryServerServices().GetSecondaryServer(_multicastServerId);
                pid =
                    new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                        .ServiceAccountApi.GetMulticastSenderArgs(multicastArgs);             
            }

            if (pid == 0) return pid;

            var activeMulticastSessionServices = new ActiveMulticastSessionServices();
            if (_isOnDemand)
            {
                _multicastSession.Pid = pid;
                _multicastSession.Name = _group.Name;
                activeMulticastSessionServices.AddActiveMulticastSession(_multicastSession);
            }
            else
            {
                _multicastSession.Pid = pid;
                activeMulticastSessionServices.UpdateActiveMulticastSession(_multicastSession);
            }

            return pid;
        }

        

      
    }
}