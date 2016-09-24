using Helpers;


namespace BLL.Workflows
{
    public class Unicast
    {
        private readonly Models.Computer _computer;
        private readonly string _direction;
        private Models.ActiveImagingTask _activeTask;
        private Models.ImageProfile _imageProfile;
        private readonly int _userId;

        public Unicast(Models.Computer computer, string direction, int userId)
        {
            _direction = direction;
            _computer = computer;
            _userId = userId;
        }

        public string Start()
        {
            if (_computer == null)
                return "The Computer Does Not Exist";

            _imageProfile = ImageProfile.ReadProfile(_computer.ImageProfileId);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            if (_direction == "push" || _direction == "permanent_push")
            {
                var validation = Image.CheckApprovalAndChecksum(_imageProfile.Image,_userId);
                if (!validation.IsValid) return validation.Message;
            }

            var dp = BLL.DistributionPoint.GetPrimaryDistributionPoint();
            if (dp == null) return "Could Not Find A Primary Distribution Point";

            if (ActiveImagingTask.IsComputerActive(_computer.Id))
                return "This Computer Is Already Part Of An Active Task";

            _activeTask = new Models.ActiveImagingTask
            {
                ComputerId = _computer.Id,
                Direction = _direction,
                UserId = _userId
            };

            _activeTask.Type = _direction == "permanent_push" ? "permanent_push" : "unicast";

            if (!ActiveImagingTask.AddActiveImagingTask(_activeTask))
                return "Could Not Create The Database Entry For This Task";

            if (!new TaskBootMenu(_computer, _imageProfile, _direction).CreatePxeBootFiles())
            {
                ActiveImagingTask.DeleteActiveImagingTask(_activeTask.Id);
                return "Could Not Create PXE Boot File";
            }

            _activeTask.Arguments = new CreateTaskArguments(_computer, _imageProfile, _direction).Run();
            if (!ActiveImagingTask.UpdateActiveImagingTask(_activeTask))
            {
                ActiveImagingTask.DeleteActiveImagingTask(_activeTask.Id);
                return "Could Not Create Task Arguments";
            }

            Utility.WakeUp(_computer.Mac);

            return "Successfully Started Task For " + _computer.Name;
        }
    }
}