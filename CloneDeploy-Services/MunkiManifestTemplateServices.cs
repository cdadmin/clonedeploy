using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Claunia.PropertyList;
using CloneDeploy_Common;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using log4net;

namespace CloneDeploy_Services
{
    public class MunkiManifestTemplateServices
    {
        private static readonly ILog log = LogManager.GetLogger("ApplicationLog");

        private readonly UnitOfWork _uow;

        public MunkiManifestTemplateServices()
        {
            _uow = new UnitOfWork();
        }

        public bool AddCatalogToTemplate(MunkiManifestCatalogEntity catalog)
        {
            if (
                !_uow.MunkiCatalogRepository.Exists(
                    s => s.Name == catalog.Name && s.ManifestTemplateId == catalog.ManifestTemplateId))
                _uow.MunkiCatalogRepository.Insert(catalog);
            else
            {
                catalog.Id =
                    _uow.MunkiCatalogRepository.GetFirstOrDefault(
                        s => s.Name == catalog.Name && s.ManifestTemplateId == catalog.ManifestTemplateId).Id;
                _uow.MunkiCatalogRepository.Update(catalog, catalog.Id);
            }

            _uow.Save();
            return true;
        }

        public bool AddIncludedManifestToTemplate(MunkiManifestIncludedManifestEntity includedManifest)
        {
            if (
                !_uow.MunkiIncludedManifestRepository.Exists(
                    s => s.Name == includedManifest.Name && s.ManifestTemplateId == includedManifest.ManifestTemplateId))
                _uow.MunkiIncludedManifestRepository.Insert(includedManifest);
            else
            {
                includedManifest.Id =
                    _uow.MunkiIncludedManifestRepository.GetFirstOrDefault(
                        s =>
                            s.Name == includedManifest.Name &&
                            s.ManifestTemplateId == includedManifest.ManifestTemplateId).Id;
                _uow.MunkiIncludedManifestRepository.Update(includedManifest, includedManifest.Id);
            }

            _uow.Save();
            return true;
        }

        public bool AddManagedInstallToTemplate(MunkiManifestManagedInstallEntity managedInstall)
        {
            if (
                !_uow.MunkiManagedInstallRepository.Exists(
                    s => s.Name == managedInstall.Name && s.ManifestTemplateId == managedInstall.ManifestTemplateId))
                _uow.MunkiManagedInstallRepository.Insert(managedInstall);
            else
            {
                managedInstall.Id =
                    _uow.MunkiManagedInstallRepository.GetFirstOrDefault(
                        s => s.Name == managedInstall.Name && s.ManifestTemplateId == managedInstall.ManifestTemplateId)
                        .Id;
                _uow.MunkiManagedInstallRepository.Update(managedInstall, managedInstall.Id);
            }

            _uow.Save();
            return true;
        }

        public bool AddManagedUnInstallToTemplate(MunkiManifestManagedUnInstallEntity managedUnInstall)
        {
            if (
                !_uow.MunkiManagedUnInstallRepository.Exists(
                    s => s.Name == managedUnInstall.Name && s.ManifestTemplateId == managedUnInstall.ManifestTemplateId))
                _uow.MunkiManagedUnInstallRepository.Insert(managedUnInstall);
            else
            {
                managedUnInstall.Id =
                    _uow.MunkiManagedUnInstallRepository.GetFirstOrDefault(
                        s =>
                            s.Name == managedUnInstall.Name &&
                            s.ManifestTemplateId == managedUnInstall.ManifestTemplateId).Id;
                _uow.MunkiManagedUnInstallRepository.Update(managedUnInstall, managedUnInstall.Id);
            }

            _uow.Save();
            return true;
        }

        public bool AddManagedUpdateToTemplate(MunkiManifestManagedUpdateEntity managedUpdate)
        {
            if (
                !_uow.MunkiManagedUpdateRepository.Exists(
                    s => s.Name == managedUpdate.Name && s.ManifestTemplateId == managedUpdate.ManifestTemplateId))
                _uow.MunkiManagedUpdateRepository.Insert(managedUpdate);
            else
            {
                managedUpdate.Id =
                    _uow.MunkiManagedUpdateRepository.GetFirstOrDefault(
                        s => s.Name == managedUpdate.Name && s.ManifestTemplateId == managedUpdate.ManifestTemplateId)
                        .Id;
                _uow.MunkiManagedUpdateRepository.Update(managedUpdate, managedUpdate.Id);
            }

            _uow.Save();
            return true;
        }

        public ActionResultDTO AddManifest(MunkiManifestTemplateEntity manifest)
        {
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateManifest(manifest, true);
            if (validationResult.Success)
            {
                _uow.MunkiManifestRepository.Insert(manifest);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = manifest.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public bool AddOptionalInstallToTemplate(MunkiManifestOptionInstallEntity optionalInstall)
        {
            if (
                !_uow.MunkiOptionalInstallRepository.Exists(
                    s => s.Name == optionalInstall.Name && s.ManifestTemplateId == optionalInstall.ManifestTemplateId))
                _uow.MunkiOptionalInstallRepository.Insert(optionalInstall);
            else
            {
                optionalInstall.Id =
                    _uow.MunkiOptionalInstallRepository.GetFirstOrDefault(
                        s =>
                            s.Name == optionalInstall.Name && s.ManifestTemplateId == optionalInstall.ManifestTemplateId)
                        .Id;
                _uow.MunkiOptionalInstallRepository.Update(optionalInstall, optionalInstall.Id);
            }

            _uow.Save();
            return true;
        }

        public bool DeleteCatalogFromTemplate(int catalogId)
        {
            _uow.MunkiCatalogRepository.Delete(catalogId);
            _uow.Save();
            return true;
        }

        public bool DeleteIncludedManifestFromTemplate(int includedManifestId)
        {
            _uow.MunkiIncludedManifestRepository.Delete(includedManifestId);
            _uow.Save();
            return true;
        }

        public bool DeleteManagedInstallFromTemplate(int managedInstallId)
        {
            _uow.MunkiManagedInstallRepository.Delete(managedInstallId);
            _uow.Save();
            return true;
        }

        public bool DeleteManagedUnInstallFromTemplate(int managedUnInstallId)
        {
            _uow.MunkiManagedUnInstallRepository.Delete(managedUnInstallId);
            _uow.Save();
            return true;
        }

        public bool DeleteManagedUpdateFromTemplate(int managedUpdateId)
        {
            _uow.MunkiManagedUpdateRepository.Delete(managedUpdateId);
            _uow.Save();
            return true;
        }

        public ActionResultDTO DeleteManifest(int manifestId)
        {
            var manifest = GetManifest(manifestId);
            if (manifest == null)
                return new ActionResultDTO {ErrorMessage = "Manifest Template Not Found", Id = 0};

            var actionResult = new ActionResultDTO();
            _uow.MunkiManifestRepository.Delete(manifestId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = manifest.Id;
            return actionResult;
        }

        public bool DeleteOptionalInstallFromTemplate(int optionalInstallId)
        {
            _uow.MunkiOptionalInstallRepository.Delete(optionalInstallId);
            _uow.Save();
            return true;
        }

        public List<MunkiManifestCatalogEntity> GetAllCatalogsForMt(int manifestTemplateId)
        {
            return _uow.MunkiCatalogRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
        }

        public List<MunkiManifestIncludedManifestEntity> GetAllIncludedManifestsForMt(int manifestTemplateId)
        {
            return _uow.MunkiIncludedManifestRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
        }

        public List<MunkiManifestManagedInstallEntity> GetAllManagedInstallsForMt(int manifestTemplateId)
        {
            return _uow.MunkiManagedInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
        }

        public List<MunkiManifestManagedUnInstallEntity> GetAllManagedUnInstallsForMt(int manifestTemplateId)
        {
            return _uow.MunkiManagedUnInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
        }

        public List<MunkiManifestManagedUpdateEntity> GetAllManagedUpdatesForMt(int manifestTemplateId)
        {
            return _uow.MunkiManagedUpdateRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
        }

        public List<MunkiManifestOptionInstallEntity> GetAllOptionalInstallsForMt(int manifestTemplateId)
        {
            return _uow.MunkiOptionalInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
        }

        public string GetCatalogTotalCount(int manifestTemplateId)
        {
            return _uow.MunkiCatalogRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
        }

        public string GetIncludedManifestTotalCount(int manifestTemplateId)
        {
            return _uow.MunkiIncludedManifestRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
        }

        public string GetManagedInstallTotalCount(int manifestTemplateId)
        {
            return _uow.MunkiManagedInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
        }

        public string GetManagedUninstallTotalCount(int manifestTemplateId)
        {
            return _uow.MunkiManagedUnInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
        }

        public string GetManagedUpdateTotalCount(int manifestTemplateId)
        {
            return _uow.MunkiManagedUpdateRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
        }

        public MunkiManifestTemplateEntity GetManifest(int manifestId)
        {
            return _uow.MunkiManifestRepository.GetById(manifestId);
        }

        public List<FileInfo> GetMunkiResources(string type)
        {
            FileInfo[] directoryFiles = null;
            var pkgInfoFiles = SettingServices.GetSettingValue(SettingStrings.MunkiBasePath) +
                               Path.DirectorySeparatorChar + type + Path.DirectorySeparatorChar;
            if (SettingServices.GetSettingValue(SettingStrings.MunkiPathType) == "Local")
            {
                var di = new DirectoryInfo(pkgInfoFiles);
                try
                {
                    directoryFiles = di.GetFiles("*.*");
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                }
            }

            else
            {
                using (var unc = new UncServices())
                {
                    var smbPassword =
                        new EncryptionServices().DecryptText(
                            SettingServices.GetSettingValue(SettingStrings.MunkiSMBPassword));
                    var smbDomain = string.IsNullOrEmpty(SettingServices.GetSettingValue(SettingStrings.MunkiSMBDomain))
                        ? ""
                        : SettingServices.GetSettingValue(SettingStrings.MunkiSMBDomain);
                    if (
                        unc.NetUseWithCredentials(SettingServices.GetSettingValue(SettingStrings.MunkiBasePath),
                            SettingServices.GetSettingValue(SettingStrings.MunkiSMBUsername), smbDomain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        var di = new DirectoryInfo(pkgInfoFiles);
                        try
                        {
                            directoryFiles = di.GetFiles("*.*");
                        }
                        catch (Exception ex)
                        {
                            log.Debug(ex.Message);
                        }
                    }
                    else
                    {
                        log.Debug("Failed to connect to " +
                                  SettingServices.GetSettingValue(SettingStrings.MunkiBasePath) + "\r\nLastError = " +
                                  unc.LastError);
                    }
                }
            }

            return directoryFiles.ToList();
        }

        public string GetOptionalInstallTotalCount(int manifestTemplateId)
        {
            return _uow.MunkiOptionalInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
        }

        public MunkiPackageInfoEntity ReadPlist(string fileName)
        {
            try
            {
                var rootDict = (NSDictionary) PropertyListParser.Parse(fileName);
                var plist = new MunkiPackageInfoEntity();
                plist.Name = rootDict.ObjectForKey("name").ToString();
                plist.Version = rootDict.ObjectForKey("version").ToString();
                return plist;
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return null;
            }
        }

        public List<MunkiManifestTemplateEntity> SearchManifests(string searchString = "")
        {
            return _uow.MunkiManifestRepository.Get(s => s.Name.Contains(searchString), q => q.OrderBy(t => t.Name));
        }

        public string TotalCount()
        {
            return _uow.MunkiManifestRepository.Count();
        }

        public ActionResultDTO UpdateManifest(MunkiManifestTemplateEntity manifest)
        {
            var m = GetManifest(manifest.Id);
            if (m == null)
                return new ActionResultDTO {ErrorMessage = "Manifest Template Not Found", Id = 0};
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateManifest(manifest, false);
            if (validationResult.Success)
            {
                _uow.MunkiManifestRepository.Update(manifest, manifest.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = manifest.Id;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateManifest(MunkiManifestTemplateEntity manifest, bool isNewManifest)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(manifest.Name) || manifest.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Manifest Name Is Not Valid";
                return validationResult;
            }

            if (isNewManifest)
            {
                if (_uow.MunkiManifestRepository.Exists(h => h.Name == manifest.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Manifest Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalManifest = _uow.MunkiManifestRepository.GetById(manifest.Id);
                if (originalManifest.Name != manifest.Name)
                {
                    if (_uow.MunkiManifestRepository.Exists(h => h.Name == manifest.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Manifest Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}