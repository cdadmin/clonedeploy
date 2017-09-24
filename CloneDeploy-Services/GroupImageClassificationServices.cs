using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class GroupImageClassificationServices
    {
        private readonly UnitOfWork _uow;

        public GroupImageClassificationServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddClassifications(List<GroupImageClassificationEntity> listOfClassifications)
        {
            foreach (var classification in listOfClassifications)
                _uow.GroupImageClassificationRepository.Insert(classification);

            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;

            var firstClass = listOfClassifications.FirstOrDefault();
            if (firstClass != null)
            {
                var groupProperties = new GroupServices().GetGroupProperty(firstClass.GroupId);
                if(groupProperties == null) return actionResult;
                
                if (Convert.ToBoolean(groupProperties.ImageClassificationsEnabled))
                {
                    foreach (var computer in new GroupServices().GetGroupMembersWithImages(groupProperties.GroupId))
                    {
                        var computerImageClassifications = new List<ComputerImageClassificationEntity>();
                        if (new ComputerServices().DeleteComputerImageClassifications(computer.Id))
                        {
                            foreach (var imageClass in listOfClassifications)
                            {
                                computerImageClassifications.Add(
                                    new ComputerImageClassificationEntity
                                    {
                                        ComputerId = computer.Id,
                                        ImageClassificationId = imageClass.ImageClassificationId
                                    });
                            }
                        }
                        new ComputerImageClassificationServices().AddClassifications(computerImageClassifications);
                    }
                }
            }

            return actionResult;
        }

        public List<ImageWithDate> FilterClassifications(int groupId, List<ImageWithDate> listImages)
        {
            var filteredImageList = new List<ImageWithDate>();
            var imageClassifications = new GroupServices().GetGroupImageClassifications(groupId);
            if (imageClassifications == null) return listImages;
            if (imageClassifications.Count == 0) return listImages;
            foreach (var image in listImages)
            {
                if (image.ClassificationId == -1)
                {
                    //Image has no classification, add it
                    filteredImageList.Add(image);
                    continue;
                }

                foreach (var classification in imageClassifications)
                {
                    if (image.ClassificationId == classification.ImageClassificationId)
                    {
                        filteredImageList.Add(image);
                        break;
                    }
                }
            }

            return filteredImageList;
        }
    }
}