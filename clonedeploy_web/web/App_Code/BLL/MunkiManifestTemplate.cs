using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class MunkiManifestTemplate
    {

        public static Models.ValidationResult AddManifest(Models.MunkiManifestTemplate manifest)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateManifest(manifest, true);
                if (validationResult.IsValid)
                {
                    uow.MunkiManifestRepository.Insert(manifest);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }

        }

        public static  string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManifestRepository.Count();
            }
        }

        public static  bool DeleteManifest(int manifestId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiManifestRepository.Delete(manifestId);
                return uow.Save();
            }
        }

        public static  Models.MunkiManifestTemplate GetManifest(int manifestId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManifestRepository.GetById(manifestId);
            }
        }

        public static  List<Models.MunkiManifestTemplate> SearchManifests(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManifestRepository.Get(s => s.Name.Contains(searchString),
                    orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static  Models.ValidationResult UpdateManifest(Models.MunkiManifestTemplate manifest)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateManifest(manifest, false);
                if (validationResult.IsValid)
                {
                    uow.MunkiManifestRepository.Update(manifest, manifest.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static  Models.ValidationResult ValidateManifest(Models.MunkiManifestTemplate manifest, bool isNewManifest)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(manifest.Name) || manifest.Name.Contains(" "))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Manifest Name Is Not Valid";
                return validationResult;
            }

            if (isNewManifest)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.MunkiManifestRepository.Exists(h => h.Name == manifest.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Manifest Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalManifest = uow.MunkiManifestRepository.GetById(manifest.Id);
                    if (originalManifest.Name != manifest.Name)
                    {
                        if (uow.MunkiManifestRepository.Exists(h => h.Name == manifest.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Manifest Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
      
    }
}