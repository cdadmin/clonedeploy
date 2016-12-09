using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Claunia.PropertyList;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    

    public class EffectiveMunkiTemplate
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");
        private MunkiManifestTemplateServices _munkiManifestTemplateServices;
        private GroupServices _groupServices;
        private GroupMunkiServices _groupMunkiServices;
        private ComputerServices _computerServices;
        private ComputerMunkiServices _computerMunkiServices;
        public EffectiveMunkiTemplate()
        {
            _munkiManifestTemplateServices = new MunkiManifestTemplateServices();
            _groupServices = new GroupServices();
            _groupMunkiServices = new GroupMunkiServices();
            _computerServices = new ComputerServices();
            _computerMunkiServices = new ComputerMunkiServices();
        }
        private List<int> _templateIds;

        public MunkiUpdateConfirmDTO GetUpdateStats(int templateId)
        {
            var includedTemplates = new List<MunkiManifestTemplateEntity>();
            var groups = _groupMunkiServices.GetGroupsForManifestTemplate(templateId);
            //get list of all templates that are used in these groups

            int totalComputerCount = 0;
            foreach (var munkiGroup in groups)
            {
                totalComputerCount += Convert.ToInt32(_groupServices.GetGroupMemberCount(munkiGroup.GroupId));
                foreach (var template in _groupServices.GetGroupMunkiTemplates(munkiGroup.GroupId))
                {
                    includedTemplates.Add(_munkiManifestTemplateServices.GetManifest(template.MunkiTemplateId));
                }
            }

            var computers = _computerMunkiServices.GetComputersForManifestTemplate(templateId);
            foreach (var computer in computers)
            {
                foreach (var template in _computerServices.GetMunkiTemplates(computer.ComputerId))
                {
                    includedTemplates.Add(_munkiManifestTemplateServices.GetManifest(template.MunkiTemplateId));
                }
            }
            totalComputerCount += computers.Count;
            var distinctList = includedTemplates.GroupBy(x => x.Name).Select(s => s.First()).ToList();
            var munkiConfirm = new MunkiUpdateConfirmDTO();
            munkiConfirm.manifestTemplates = distinctList;
            munkiConfirm.groupCount = groups.Count;
            munkiConfirm.computerCount = totalComputerCount;

            return munkiConfirm;


        }

        public int Apply(int templateId)
        {
            var errorCount = 0;
            string basePath = Settings.MunkiBasePath + Path.DirectorySeparatorChar + "manifests" + Path.DirectorySeparatorChar;

            var groups = _groupMunkiServices.GetGroupsForManifestTemplate(templateId);
            if (Settings.MunkiPathType == "Local")
            {
                foreach (var munkiGroup in groups)
                {
                    var effectiveManifest = new EffectiveMunkiTemplate().Group(munkiGroup.GroupId);
                    var computersInGroup = _groupServices.GetGroupMembers(munkiGroup.GroupId);
                    foreach (var computer in computersInGroup)
                    {
                        if (!WritePath(basePath + computer.Name, Encoding.UTF8.GetString(effectiveManifest.ToArray())))
                            errorCount++;
                    }
                }
            }
            else
            {
                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                {
                    var smbPassword = new Helpers.Encryption().DecryptText(Settings.MunkiSMBPassword);
                    var smbDomain = string.IsNullOrEmpty(Settings.MunkiSMBDomain) ? "" : Settings.MunkiSMBDomain;
                    if (unc.NetUseWithCredentials(Settings.MunkiBasePath, Settings.MunkiSMBUsername, smbDomain, smbPassword) || unc.LastError == 1219)
                    {
                        foreach (var munkiGroup in groups)
                        {
                            var effectiveManifest = new EffectiveMunkiTemplate().Group(munkiGroup.GroupId);
                            var computersInGroup = _groupServices.GetGroupMembers(munkiGroup.GroupId);
                            foreach (var computer in computersInGroup)
                            {
                                if (!WritePath(basePath + computer.Name, Encoding.UTF8.GetString(effectiveManifest.ToArray())))
                                    errorCount++;
                            }
                        }
                    }
                    else
                    {
                        log.Debug("Failed to connect to " + Settings.MunkiBasePath + "\r\nLastError = " + unc.LastError);
                        foreach (var munkiGroup in groups)
                        {
                            var computersInGroup = _groupServices.GetGroupMembers(munkiGroup.GroupId);
                            errorCount += computersInGroup.Count();
                        }
                    }
                }
            }
            var computers = _computerMunkiServices.GetComputersForManifestTemplate(templateId);
            if (Settings.MunkiPathType == "Local")
            {
                foreach (var munkiComputer in computers)
                {
                    var effectiveManifest = new EffectiveMunkiTemplate().Computer(munkiComputer.ComputerId);
                    var computer = new ComputerServices().GetComputer(munkiComputer.ComputerId);
                    if (!WritePath(basePath + computer.Name, Encoding.UTF8.GetString(effectiveManifest.ToArray())))
                        errorCount++;
                }
            }
            else
            {
                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                {
                    var smbPassword = new Helpers.Encryption().DecryptText(Settings.MunkiSMBPassword);
                    var smbDomain = string.IsNullOrEmpty(Settings.MunkiSMBDomain) ? "" : Settings.MunkiSMBDomain;
                    if (
                        unc.NetUseWithCredentials(Settings.MunkiBasePath, Settings.MunkiSMBUsername, smbDomain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        foreach (var munkiComputer in computers)
                        {
                            var effectiveManifest =
                                new EffectiveMunkiTemplate().Computer(munkiComputer.ComputerId);
                            var computer = new ComputerServices().GetComputer(munkiComputer.ComputerId);


                            if (
                                !WritePath(basePath + computer.Name,
                                    Encoding.UTF8.GetString(effectiveManifest.ToArray())))
                                errorCount++;
                        }
                    }
                    else
                    {
                        log.Debug("Failed to connect to " + Settings.MunkiBasePath + "\r\nLastError = " +
                                   unc.LastError);
                        errorCount += computers.Count();
                    }
                }
            }

            if (errorCount > 0)
                return errorCount;

            var includedTemplates = new List<MunkiManifestTemplateEntity>();
            foreach (var munkiGroup in groups)
            {
                foreach (var template in _groupServices.GetGroupMunkiTemplates(munkiGroup.GroupId))
                {
                    includedTemplates.Add(_munkiManifestTemplateServices.GetManifest(template.MunkiTemplateId));
                }
            }

            foreach (var computer in computers)
            {
                foreach (var template in _computerServices.GetMunkiTemplates(computer.ComputerId))
                {
                    includedTemplates.Add(_munkiManifestTemplateServices.GetManifest(template.MunkiTemplateId));
                }
            }

            foreach (var template in includedTemplates)
            {
                template.ChangesApplied = 1;
               _munkiManifestTemplateServices.UpdateManifest(template);
            }

            return 0;
        }

        public MemoryStream Group(int groupId)
        {
            _templateIds = new List<int>();

            var groupTemplates = _groupServices.GetGroupMunkiTemplates(groupId);
            foreach (var template in groupTemplates)
            {
                _templateIds.Add(template.MunkiTemplateId);
            }

            return GeneratePlist();
        }

        public MemoryStream Computer(int computerId)
        {
            _templateIds = new List<int>();

            var computerTemplates = _computerServices.GetMunkiTemplates(computerId);
            foreach (var template in computerTemplates)
            {
                _templateIds.Add(template.MunkiTemplateId);
            }
            var memberships = _computerServices.GetAllComputerMemberships(computerId);
            foreach (var membership in memberships)
            {
                var groupTemplates = _groupServices.GetGroupMunkiTemplates(membership.GroupId);
                foreach (var template in groupTemplates)
                {
                    _templateIds.Add(template.MunkiTemplateId);
                }
            }

            return GeneratePlist();
        }

        public MemoryStream MunkiTemplate(int templateId)
        {
            _templateIds = new List<int>();
            _templateIds.Add(templateId);

            return GeneratePlist();
        }

        private MemoryStream GeneratePlist()
        {
            NSDictionary root = new NSDictionary();
            NSArray plCatalogs = GetCatalogs();
            NSArray plConditionals = GetConditionals();
            NSArray plIncludedManifests = GetIncludedManifests();
            NSArray plManagedInstalls = GetManagedInstalls();
            NSArray plManagedUninstalls = GetManagedUninstalls();
            NSArray plManagedUpdates = GetManagedUpdates();
            NSArray plOptionalInstalls = GetOptionlInstalls();

            if (plCatalogs.Count > 0) root.Add("catalogs", plCatalogs);
            if (plConditionals.Count > 0) root.Add("conditional_items", plConditionals);
            if (plIncludedManifests.Count > 0) root.Add("included_manifests", plIncludedManifests);
            if (plManagedInstalls.Count > 0) root.Add("managed_installs", plManagedInstalls);
            if (plManagedUninstalls.Count > 0) root.Add("managed_uninstalls", plManagedUninstalls);
            if (plManagedUpdates.Count > 0) root.Add("managed_updates", plManagedUpdates);
            if (plOptionalInstalls.Count > 0) root.Add("optional_installs", plOptionalInstalls);

            var rdr = new MemoryStream();
            try
            {

                PropertyListParser.SaveAsXml(root, rdr);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }

            return rdr;
        }

        private NSArray GetCatalogs()
        {
            var catalogs = new List<MunkiManifestCatalogEntity>();
            foreach (var templateId in _templateIds)
            {
                catalogs.AddRange(_munkiManifestTemplateServices.GetAllCatalogsForMt(templateId));
            }

            var orderedCatalogs = catalogs.Distinct().OrderBy(x => x.Priority).ThenBy(x => x.Name).ToList();
            orderedCatalogs = orderedCatalogs.GroupBy(x => x.Name).Select(s => s.First()).ToList();
            NSArray plCatalogs = new NSArray(orderedCatalogs.Count);
            var counter = 0;
            foreach (var catalog in orderedCatalogs)
            {
                plCatalogs.SetValue(counter, catalog.Name);
                counter++;
            }

            return plCatalogs;
        }

        private NSArray GetConditionals()
        {
            var uniqueConditions = GetAllUniqueConditions();
            NSArray conditionalItems = new NSArray(uniqueConditions.Count);
            var uniqueConditionsCounter = 0;
            foreach (var uniqueCondition in uniqueConditions)
            {
                NSDictionary condition = new NSDictionary();
                condition.Add("condition", uniqueCondition);
                conditionalItems.SetValue(uniqueConditionsCounter, condition);
                uniqueConditionsCounter++;

                NSArray plIncludedManifests = GetIncludedManifests(uniqueCondition);
                NSArray plManagedInstalls = GetManagedInstalls(uniqueCondition);
                NSArray plManagedUninstalls = GetManagedUninstalls(uniqueCondition);
                NSArray plManagedUpdates = GetManagedUpdates(uniqueCondition);
                NSArray plOptionalInstalls = GetOptionlInstalls(uniqueCondition);

                if (plIncludedManifests.Count > 0)
                    condition.Add("included_manifests", plIncludedManifests);
                if (plManagedInstalls.Count > 0)
                    condition.Add("managed_installs", plManagedInstalls);
                if (plManagedUninstalls.Count > 0)
                    condition.Add("managed_uninstalls", plManagedUninstalls);
                if (plManagedUpdates.Count > 0)
                    condition.Add("managed_updates", plManagedUpdates);
                if (plOptionalInstalls.Count > 0)
                    condition.Add("optional_installs", plOptionalInstalls);
            }

            return conditionalItems;
        }

        private NSArray GetIncludedManifests (string condition = null)
        {
            var includedManifests = new List<MunkiManifestIncludedManifestEntity>();
            foreach (var templateId in _templateIds)
            {
                if(!string.IsNullOrEmpty(condition))
                includedManifests.AddRange(_munkiManifestTemplateServices.GetAllIncludedManifestsForMt(templateId)
                    .Where(x => x.Condition == condition));
                else
                {
                    includedManifests.AddRange(_munkiManifestTemplateServices.GetAllIncludedManifestsForMt(templateId)
                   .Where(x => string.IsNullOrEmpty(x.Condition)));
                }

            }

            var orderedManifests = includedManifests.GroupBy(x => x.Name).Select(s => s.First()).OrderBy(x => x.Name);

            NSArray plIncludedManifests = new NSArray(orderedManifests.Count());
            var counter = 0;
            foreach (var includedManifest in orderedManifests)
            {
                plIncludedManifests.SetValue(counter, includedManifest.Name);
                counter++;
            }

            return plIncludedManifests;
        }

        private NSArray GetManagedInstalls(string condition = null)
        {
            var managedInstalls = new List<MunkiManifestManagedInstallEntity>();
            foreach (var templateId in _templateIds)
            {
                if (!string.IsNullOrEmpty(condition))
                    managedInstalls.AddRange(
                        _munkiManifestTemplateServices.GetAllManagedInstallsForMt(templateId)
                            .Where(x => x.Condition == condition));
                else
                {
                    managedInstalls.AddRange(_munkiManifestTemplateServices.GetAllManagedInstallsForMt(templateId)
                        .Where(x => string.IsNullOrEmpty(x.Condition)));
                }
            }

            var orderedManagedInstalls = managedInstalls.GroupBy(x => x.Name).Select(g => g.OrderByDescending(x => x.Version).First()).OrderBy(x => x.Name);

            NSArray plManagedInstalls = new NSArray(orderedManagedInstalls.Count());
            var counter = 0;
            foreach (var managedInstall in orderedManagedInstalls)
            {
                if(managedInstall.IncludeVersion == 1)
                plManagedInstalls.SetValue(counter, managedInstall.Name + "-" + managedInstall.Version);
                else
                {
                    plManagedInstalls.SetValue(counter, managedInstall.Name);
                }
                counter++;
            }

            return plManagedInstalls;
        }

        private NSArray GetManagedUninstalls(string condition = null)
        {
            var managedUninstalls = new List<MunkiManifestManagedUnInstallEntity>();
            foreach (var templateId in _templateIds)
            {
                if (!string.IsNullOrEmpty(condition))
                    managedUninstalls.AddRange(
                        _munkiManifestTemplateServices.GetAllManagedUnInstallsForMt(templateId)
                            .Where(x => x.Condition == condition));
                else
                {
                    managedUninstalls.AddRange(_munkiManifestTemplateServices.GetAllManagedUnInstallsForMt(templateId)
                        .Where(x => string.IsNullOrEmpty(x.Condition)));
                }
            }

            var orderedManagedUninstalls = managedUninstalls.GroupBy(x => x.Name).Select(g => g.OrderByDescending(x => x.Version).First()).OrderBy(x => x.Name);

            NSArray plManagedUninstalls = new NSArray(orderedManagedUninstalls.Count());
            var counter = 0;
            foreach (var managedUninstall in orderedManagedUninstalls)
            {
                if (managedUninstall.IncludeVersion == 1)
                    plManagedUninstalls.SetValue(counter, managedUninstall.Name + "-" + managedUninstall.Version);
                else
                {
                    plManagedUninstalls.SetValue(counter, managedUninstall.Name);
                }
                counter++;
            }

            return plManagedUninstalls;
        }

        private NSArray GetManagedUpdates(string condition = null)
        {
            var managedUpdates = new List<MunkiManifestManagedUpdateEntity>();
            foreach (var templateId in _templateIds)
            {
                if (!string.IsNullOrEmpty(condition))
                    managedUpdates.AddRange(
                        _munkiManifestTemplateServices.GetAllManagedUpdatesForMt(templateId)
                            .Where(x => x.Condition == condition));
                else
                {
                    managedUpdates.AddRange(_munkiManifestTemplateServices.GetAllManagedUpdatesForMt(templateId)
                        .Where(x => string.IsNullOrEmpty(x.Condition)));
                }
            }

            var orderedManagedUpdates = managedUpdates.GroupBy(x => x.Name).Select(g => g.First()).OrderBy(x => x.Name);

            NSArray plManagedUpdates = new NSArray(orderedManagedUpdates.Count());
            var counter = 0;
            foreach (var managedUninstall in orderedManagedUpdates)
            {
                plManagedUpdates.SetValue(counter, managedUninstall.Name);
                counter++;
            }

            return plManagedUpdates;
        }

        private NSArray GetOptionlInstalls(string condition = null)
        {
            var optionalInstalls = new List<MunkiManifestOptionInstallEntity>();
            foreach (var templateId in _templateIds)
            {
                if (!string.IsNullOrEmpty(condition))
                    optionalInstalls.AddRange(
                        _munkiManifestTemplateServices.GetAllOptionalInstallsForMt(templateId)
                            .Where(x => x.Condition == condition));
                else
                {
                    optionalInstalls.AddRange(_munkiManifestTemplateServices.GetAllOptionalInstallsForMt(templateId)
                        .Where(x => string.IsNullOrEmpty(x.Condition)));
                }
            }

            var orderedOptionalInstalls = optionalInstalls.GroupBy(x => x.Name).Select(g => g.OrderByDescending(x => x.Version).First()).OrderBy(x => x.Name);

            NSArray plOptionalInstalls = new NSArray(orderedOptionalInstalls.Count());
            var counter = 0;
            foreach (var optionalInstall in orderedOptionalInstalls)
            {
                if (optionalInstall.IncludeVersion == 1)
                    plOptionalInstalls.SetValue(counter, optionalInstall.Name + "-" + optionalInstall.Version);
                else
                {
                    plOptionalInstalls.SetValue(counter, optionalInstall.Name);
                }
                counter++;
            }

            return plOptionalInstalls;
        }

        private List<string> GetAllUniqueConditions()
        {
            var allConditions = new List<string>();

            foreach (var templateId in _templateIds)
            {

                allConditions.AddRange(
                    _munkiManifestTemplateServices.GetAllManagedInstallsForMt(templateId)
                        .Where(x => !string.IsNullOrEmpty(x.Condition))
                        .Select(x => x.Condition)
                        .ToList());

                allConditions.AddRange(
                    _munkiManifestTemplateServices.GetAllManagedUnInstallsForMt(templateId)
                        .Where(x => !string.IsNullOrEmpty(x.Condition))
                        .Select(x => x.Condition)
                        .ToList());

                allConditions.AddRange(
                    _munkiManifestTemplateServices.GetAllOptionalInstallsForMt(templateId)
                        .Where(x => !string.IsNullOrEmpty(x.Condition))
                        .Select(x => x.Condition)
                        .ToList());

                allConditions.AddRange(
                    _munkiManifestTemplateServices.GetAllManagedUpdatesForMt(templateId)
                        .Where(x => !string.IsNullOrEmpty(x.Condition))
                        .Select(x => x.Condition)
                        .ToList());

                allConditions.AddRange(
                    _munkiManifestTemplateServices.GetAllIncludedManifestsForMt(templateId)
                        .Where(x => !string.IsNullOrEmpty(x.Condition))
                        .Select(x => x.Condition)
                        .ToList());
            }

            return allConditions.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
        }

        public bool WritePath(string path, string contents)
        {
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(contents);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Debug("Could Not Write " + path + " " + ex.Message);
                return false;
            }
        }
    }


}