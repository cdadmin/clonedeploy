using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ImageClassificationServices
    {
        private readonly UnitOfWork _uow;

        public ImageClassificationServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddImageClassification(ImageClassificationEntity imageClassification)
        {
            var validationResult = ValidateImageClassification(imageClassification, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.ImageClassificationRepository.Insert(imageClassification);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = imageClassification.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteImageClassification(int imageClassificationId)
        {
            var imageClassification = GetImageClassification(imageClassificationId);
            if (imageClassification == null)
                return new ActionResultDTO {ErrorMessage = "Image Classification Not Found", Id = 0};
            _uow.ImageClassificationRepository.Delete(imageClassificationId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = imageClassification.Id;
            return actionResult;
        }

        public List<ImageClassificationEntity> GetAll()
        {
            return _uow.ImageClassificationRepository.Get();
        }

        public ImageClassificationEntity GetImageClassification(int imageClassificationId)
        {
            return _uow.ImageClassificationRepository.GetById(imageClassificationId);
        }

        public string TotalCount()
        {
            return _uow.ImageClassificationRepository.Count();
        }

        public ActionResultDTO UpdateImageClassification(ImageClassificationEntity imageClassification)
        {
            var r = GetImageClassification(imageClassification.Id);
            if (r == null) return new ActionResultDTO {ErrorMessage = "Image Classification Not Found", Id = 0};

            var validationResult = ValidateImageClassification(imageClassification, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.ImageClassificationRepository.Update(imageClassification, imageClassification.Id);
                _uow.Save();

                actionResult.Success = true;
                actionResult.Id = imageClassification.Id;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateImageClassification(ImageClassificationEntity imageClassification,
            bool isNewImageClassification)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(imageClassification.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Image Classification Name Is Not Valid";
                return validationResult;
            }

            if (isNewImageClassification)
            {
                if (_uow.ImageClassificationRepository.Exists(h => h.Name == imageClassification.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Image Classification Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalImageClassification = _uow.ImageClassificationRepository.GetById(imageClassification.Id);
                if (originalImageClassification.Name != imageClassification.Name)
                {
                    if (_uow.ImageClassificationRepository.Exists(h => h.Name == imageClassification.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Image Classification Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}