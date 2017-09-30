using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ComputerImageClassificationServices
    {
        private readonly UnitOfWork _uow;

        public ComputerImageClassificationServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddClassifications(List<ComputerImageClassificationEntity> listOfClassifications)
        {
            foreach (var classification in listOfClassifications)
                _uow.ComputerImageClassificationRepository.Insert(classification);

            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;
        }

        public List<ImageWithDate> FilterClassifications(int computerId, List<ImageWithDate> listImages)
        {
            var filteredImageList = new List<ImageWithDate>();
            var imageClassifications = new ComputerServices().GetComputerImageClassifications(computerId);
            if (imageClassifications == null) return listImages;
            if (imageClassifications.Count == 0) return listImages;

            foreach (var image in listImages)
            {
                //Debatable - Should computers with image classifications be able to see unclassified images - Currently no
                /*if (image.ClassificationId == -1 || image.ClassificationId == 0)
                {
                    //Image has no classification, add it
                    filteredImageList.Add(image);
                    continue;
                }*/

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

        public List<ImageEntity> FilterForOnDemandList(int computerId, List<ImageEntity> listImages)
        {
            if (computerId == 0) return listImages;

            var filteredImageList = new List<ImageEntity>();
            var imageClassifications = new ComputerServices().GetComputerImageClassifications(computerId);
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