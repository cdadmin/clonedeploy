using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.BLL
{
    public class MunkiManifestTemplate
    {

        public static Models.ActionResult AddManifest(Models.MunkiManifestTemplate manifest)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateManifest(manifest, true);
                if (validationResult.Success)
                {
                    uow.MunkiManifestRepository.Insert(manifest);
                    validationResult.Success = uow.Save();
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

        public static  Models.ActionResult UpdateManifest(Models.MunkiManifestTemplate manifest)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateManifest(manifest, false);
                if (validationResult.Success)
                {
                    uow.MunkiManifestRepository.Update(manifest, manifest.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static  Models.ActionResult ValidateManifest(Models.MunkiManifestTemplate manifest, bool isNewManifest)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(manifest.Name) || manifest.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Manifest Name Is Not Valid";
                return validationResult;
            }

            if (isNewManifest)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.MunkiManifestRepository.Exists(h => h.Name == manifest.Name))
                    {
                        validationResult.Success = false;
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
                            validationResult.Success = false;
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