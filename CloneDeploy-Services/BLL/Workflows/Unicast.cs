using CloneDeploy_App.Helpers;
using CloneDeploy_Entities;
using CloneDeploy_Services;

namespace CloneDeploy_App.BLL.Workflows
{
    public class Unicast
    {
        private readonly ComputerEntity _computer;
        private readonly string _direction;
        private ActiveImagingTaskEntity _activeTask;
        private ImageProfileEntity _imageProfile;
        private readonly int _userId;

        public Unicast(ComputerEntity computer, string direction, int userId)
        {
            _direction = direction;
            _computer = computer;
            _userId = userId;
        }

        public string Start()
        {
            if (_computer == null)
                return "The Computer Does Not Exist";

            _imageProfile = new ImageProfileServices().ReadProfile(_computer.ImageProfileId);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            if (_direction == "push" || _direction == "permanent_push")
            {
                var validation = new ImageServices().CheckApprovalAndChecksum(_imageProfile.Image,_userId);
                if (!validation.Success) return validation.ErrorMessage;
            }

            var dp = new DistributionPointServices().GetPrimaryDistributionPoint();
            if (dp == null) return "Could Not Find A Primary Distribution Point";

            if (new ComputerServices().IsComputerActive(_computer.Id))
                return "This Computer Is Already Part Of An Active Task";

            _activeTask = new ActiveImagingTaskEntity
            {
                ComputerId = _computer.Id,
                Direction = _direction,
                UserId = _userId
            };

            _activeTask.Type = _direction == "permanent_push" ? "permanent_push" : "unicast";


            var activeImagingTaskServices = new ActiveImagingTaskServices();

            if (!activeImagingTaskServices.AddActiveImagingTask(_activeTask))
                return "Could Not Create The Database Entry For This Task";

            if (!new TaskBootMenu(_computer, _imageProfile, _direction).CreatePxeBootFiles())
            {
                activeImagingTaskServices.DeleteActiveImagingTask(_activeTask.Id);
                return "Could Not Create PXE Boot File";
            }

            _activeTask.Arguments = new CreateTaskArguments(_computer, _imageProfile, _direction).Run();
            if (!activeImagingTaskServices.UpdateActiveImagingTask(_activeTask))
            {
                activeImagingTaskServices.DeleteActiveImagingTask(_activeTask.Id);
                return "Could Not Create Task Arguments";
            }

            Utility.WakeUp(_computer.Mac);

            return "Successfully Started Task For " + _computer.Name;
        }
    }
}