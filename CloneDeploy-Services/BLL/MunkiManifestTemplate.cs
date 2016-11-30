using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class MunkiManifestTemplate
    {

        public static ActionResultEntity AddManifest(MunkiManifestTemplateEntity manifest)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateManifest(manifest, true);
                if (validationResult.Success)
                {
                    uow.MunkiManifestRepository.Insert(manifest);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }

        }

        public static  string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManifestRepository.Count();
            }
        }

        public static ActionResultEntity DeleteManifest(int manifestId)
        {
            var manifest = GetManifest(manifestId);
            if (manifest == null)
            {
                var message = string.Format("Could Not Delete Manifest Template With Id {0}.  The Manifest Template Was not Found", manifestId);
                Logger.Log(message);
                return new ActionResultEntity() { Success = false, Message = message, ObjectId = manifestId };
            }
            using (var uow = new UnitOfWork())
            {
                var actionResult = new ActionResultEntity();
                uow.MunkiManifestRepository.Delete(manifestId);
                uow.Save();
                actionResult.Success = true;
                actionResult.ObjectId = manifest.Id;
                actionResult.Object = JsonConvert.SerializeObject(manifest);
                return actionResult;
            }
        }

        public static MunkiManifestTemplateEntity GetManifest(int manifestId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManifestRepository.GetById(manifestId);
            }
        }



        public static List<MunkiManifestTemplateEntity> SearchManifests(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManifestRepository.Get(s => s.Name.Contains(searchString),
                    orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static ActionResultEntity UpdateManifest(MunkiManifestTemplateEntity manifest)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateManifest(manifest, false);
                if (validationResult.Success)
                {
                    uow.MunkiManifestRepository.Update(manifest, manifest.Id);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateManifest(MunkiManifestTemplateEntity manifest, bool isNewManifest)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(manifest.Name) || manifest.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Manifest Name Is Not Valid";
                return validationResult;
            }

            if (isNewManifest)
            {
                using (var uow = new UnitOfWork())
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
                using (var uow = new UnitOfWork())
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