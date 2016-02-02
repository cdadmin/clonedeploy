using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Service.Client
{
    public class OnDemand
    {
        public string ImageList(int userId = 0)
        {
            var imageList = new Services.Client.ImageList { Images = new List<string>() };

            foreach (var image in BLL.Image.GetOnDemandImageList(userId))
                imageList.Images.Add(image.Id + " " + image.Name);

            return JsonConvert.SerializeObject(imageList);
        }

        public string ImageProfileList(int imageId)
        {
            var imageProfileList = new Services.Client.ImageProfileList { ImageProfiles = new List<string>() };

            int profileCounter = 0;
            foreach (var imageProfile in BLL.ImageProfile.SearchProfiles(Convert.ToInt32(imageId)))
            {
                profileCounter++;
                imageProfileList.ImageProfiles.Add(imageProfile.Id + " " + imageProfile.Name);
                if (profileCounter == 1)
                    imageProfileList.FirstProfileId = imageProfile.Id.ToString();
            }

            imageProfileList.Count = profileCounter.ToString();

            return JsonConvert.SerializeObject(imageProfileList);
        }

        public string MulicastSessionList()
        {
            var multicastList = new Services.Client.MulticastList(){ Multicasts = new List<string>() };

            foreach (var multicast in BLL.ActiveMulticastSession.GetOnDemandList())
            {
                multicastList.Multicasts.Add(multicast.Id + " " + multicast.Name);
            }

            return JsonConvert.SerializeObject(multicastList);
        }

        public string AddImage(string imageName)
        {
            var image = new Models.Image()
            {
                Name = imageName
            };
            var result = BLL.Image.AddImage(image);
            if (result.IsValid)
                result.Message = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

       
      
    }
}