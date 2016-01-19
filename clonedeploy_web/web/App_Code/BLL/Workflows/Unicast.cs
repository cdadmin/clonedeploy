using Helpers;


namespace BLL.Workflows
{
    public class Unicast
    {
        private readonly Models.Computer _computer;
        private readonly string _direction;
        private Models.ActiveImagingTask _activeTask;
        private Models.ImageProfile _imageProfile;

        public Unicast(Models.Computer computer, string direction)
        {
            _direction = direction;
            _computer = computer;
        }

        public string Start()
        {
            if (_computer == null)
                return "The Computer Does Not Exist";

            _imageProfile = ImageProfile.ReadProfile(_computer.ImageProfile);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            var validation = Image.CheckApprovalAndChecksum(_imageProfile.Image);
            if (!validation.IsValid) return validation.Message;

            if (ActiveImagingTask.IsComputerActive(_computer.Id))
                return "This Computer Is Already Part Of An Active Task";

            _activeTask = new Models.ActiveImagingTask
            {
                Type = "unicast",
                ComputerId = _computer.Id,
                Direction = _direction
            };

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

            return "Successfully Started Task";
        }
    }
}