using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Claunia.PropertyList;
using Helpers;

namespace BLL.Workflows
{
    public class PreviewMunkiTemplate
    {
        public static MemoryStream Preview(int templateId)
        {
            NSDictionary root = new NSDictionary();

            var assignedCatalogs = BLL.MunkiCatalog.GetAllCatalogsForMt(templateId).OrderBy(x => x.Priority);
            NSArray catalogs = new NSArray(assignedCatalogs.Count());
            var catalogCounter = 0;
            foreach (var catalog in assignedCatalogs)
            {
                catalogs.SetValue(catalogCounter, catalog.Name);
                catalogCounter++;
            }

            var uniqueConditions = GetAllUniqueConditions(templateId);
            NSArray conditionalItems = new NSArray(uniqueConditions.Count);
            var uniqueConditionsCounter = 0;
            foreach (var uniqueCondition in uniqueConditions)
            {
                NSDictionary condition = new NSDictionary();
                condition.Add("condition", uniqueCondition);
                conditionalItems.SetValue(uniqueConditionsCounter, condition);
                uniqueConditionsCounter++;

                var conditionalIncludedManifests =
                    BLL.MunkiIncludedManifest.GetAllIncludedManifestsForMt(templateId)
                        .Where(x => x.Condition == uniqueCondition)
                        .OrderBy(x => x.Name);
                NSArray conditionalIncludedManifestsArray = new NSArray(conditionalIncludedManifests.Count());
                var conditionalIncludedManifestsCounter = 0;
                foreach (var conditionalIncludedManifest in conditionalIncludedManifests)
                {
                    conditionalIncludedManifestsArray.SetValue(conditionalIncludedManifestsCounter,
                        conditionalIncludedManifest.Name);
                    conditionalIncludedManifestsCounter++;
                }

                var conditionalManagedInstalls =
                    BLL.MunkiManagedInstall.GetAllManagedInstallsForMt(templateId)
                        .Where(x => x.Condition == uniqueCondition)
                        .OrderBy(x => x.Name);
                NSArray conditionalManagedInstallsArray = new NSArray(conditionalManagedInstalls.Count());
                var conditionalManagedInstallCounter = 0;
                foreach (var conditionalManagedInstall in conditionalManagedInstalls)
                {
                    conditionalManagedInstallsArray.SetValue(conditionalManagedInstallCounter,
                        conditionalManagedInstall.Name);
                    conditionalManagedInstallCounter++;
                }

                var conditionalManagedUnInstalls =
                    BLL.MunkiManagedUninstall.GetAllManagedUnInstallsForMt(templateId)
                        .Where(x => x.Condition == uniqueCondition)
                        .OrderBy(x => x.Name);
                NSArray conditionalManagedUnInstallsArray = new NSArray(conditionalManagedUnInstalls.Count());
                var conditionalManagedUnInstallCounter = 0;
                foreach (var conditionalManagedUnInstall in conditionalManagedUnInstalls)
                {
                    conditionalManagedUnInstallsArray.SetValue(conditionalManagedUnInstallCounter,
                        conditionalManagedUnInstall.Name);
                    conditionalManagedUnInstallCounter++;
                }

                var conditionalManagedUpdates =
                    BLL.MunkiManagedUpdate.GetAllManagedUpdatesForMt(templateId)
                        .Where(x => x.Condition == uniqueCondition)
                        .OrderBy(x => x.Name);
                NSArray conditionalManagedUpdatesArray = new NSArray(conditionalManagedUpdates.Count());
                var conditionalManagedUpdateCounter = 0;
                foreach (var conditionalManagedUpdate in conditionalManagedUpdates)
                {
                    conditionalManagedUpdatesArray.SetValue(conditionalManagedUpdateCounter,
                        conditionalManagedUpdate.Name);
                    conditionalManagedUpdateCounter++;
                }


                var conditionalOptionalInstalls =
                    BLL.MunkiOptionalInstall.GetAllOptionalInstallsForMt(templateId)
                        .Where(x => x.Condition == uniqueCondition)
                        .OrderBy(x => x.Name);
                NSArray conditionalOptionalInstallsArray = new NSArray(conditionalOptionalInstalls.Count());
                var conditionalOptionalInstallCounter = 0;
                foreach (var conditionalOptionalInstall in conditionalOptionalInstalls)
                {
                    conditionalOptionalInstallsArray.SetValue(conditionalOptionalInstallCounter,
                        conditionalOptionalInstall.Name);
                    conditionalOptionalInstallCounter++;
                }


                if (conditionalIncludedManifestsArray.Count > 0)
                    condition.Add("included_manifests", conditionalIncludedManifestsArray);
                if (conditionalManagedInstallsArray.Count > 0)
                    condition.Add("managed_installs", conditionalManagedInstallsArray);
                if (conditionalManagedUnInstallsArray.Count > 0)
                    condition.Add("managed_uninstalls", conditionalManagedUnInstallsArray);
                if (conditionalManagedUpdatesArray.Count > 0)
                    condition.Add("managed_updates", conditionalManagedUpdatesArray);
                if (conditionalOptionalInstallsArray.Count > 0)
                    condition.Add("optional_installs", conditionalOptionalInstallsArray);
            }

            var assignedIncludedManifests =
                BLL.MunkiIncludedManifest.GetAllIncludedManifestsForMt(templateId)
                    .Where(x => string.IsNullOrEmpty(x.Condition))
                    .OrderBy(x => x.Name);
            NSArray includedManifests = new NSArray(assignedIncludedManifests.Count());
            var includedManifestCounter = 0;
            foreach (var includedManifest in assignedIncludedManifests)
            {
                includedManifests.SetValue(includedManifestCounter, includedManifest.Name);
                includedManifestCounter++;
            }

            var assignedManagedInstalls =
                BLL.MunkiManagedInstall.GetAllManagedInstallsForMt(templateId)
                    .Where(x => string.IsNullOrEmpty(x.Condition))
                    .OrderBy(x => x.Name);
            NSArray managedInstalls = new NSArray(assignedManagedInstalls.Count());
            var managedInstallCounter = 0;
            foreach (var managedInstall in assignedManagedInstalls)
            {
                managedInstalls.SetValue(managedInstallCounter, managedInstall.Name);
                managedInstallCounter++;
            }

            var assignedManagedUnInstalls =
                BLL.MunkiManagedUninstall.GetAllManagedUnInstallsForMt(templateId)
                    .Where(x => string.IsNullOrEmpty(x.Condition))
                    .OrderBy(x => x.Name);
            NSArray managedUninstalls = new NSArray(assignedManagedUnInstalls.Count());
            var managedUninstallCounter = 0;
            foreach (var managedUninstall in assignedManagedUnInstalls)
            {
                managedUninstalls.SetValue(managedUninstallCounter, managedUninstall.Name);
                managedUninstallCounter++;
            }

            var assignedManagedUpdates =
                BLL.MunkiManagedUpdate.GetAllManagedUpdatesForMt(templateId)
                    .Where(x => string.IsNullOrEmpty(x.Condition))
                    .OrderBy(x => x.Name);
            NSArray managedUpdates = new NSArray(assignedManagedUpdates.Count());
            var managedUpdateCounter = 0;
            foreach (var managedUpdate in assignedManagedUpdates)
            {
                managedUpdates.SetValue(managedUpdateCounter, managedUpdate.Name);
                managedUpdateCounter++;
            }

            var assignedOptionalInstalls =
                BLL.MunkiOptionalInstall.GetAllOptionalInstallsForMt(templateId)
                    .Where(x => string.IsNullOrEmpty(x.Condition))
                    .OrderBy(x => x.Name);
            NSArray optionalInstalls = new NSArray(assignedOptionalInstalls.Count());
            var optionalInstallCounter = 0;
            foreach (var optionalInstall in assignedOptionalInstalls)
            {
                optionalInstalls.SetValue(optionalInstallCounter, optionalInstall.Name);
                optionalInstallCounter++;
            }

            if (catalogs.Count > 0) root.Add("catalogs", catalogs);
            if (conditionalItems.Count > 0) root.Add("conditional_items", conditionalItems);
            if (includedManifests.Count > 0) root.Add("included_manifests", includedManifests);
            if (managedInstalls.Count > 0) root.Add("managed_installs", managedInstalls);
            if (managedUninstalls.Count > 0) root.Add("managed_uninstalls", managedUninstalls);
            if (managedUpdates.Count > 0) root.Add("managed_updates", managedUpdates);
            if (optionalInstalls.Count > 0) root.Add("optional_installs", optionalInstalls);

            //Save the propery list

            var rdr = new MemoryStream();
            try
            {
            
                PropertyListParser.SaveAsXml(root, rdr);
           
                //File.WriteAllBytes("C:\\intel\\my.plist", rdr.ToArray());
                //PropertyListParser.SaveAsXml(root, new FileInfo("C:\\intel\\my.plist"));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);

            }

            return rdr;
        }

        private static List<string> GetAllUniqueConditions(int templateId)
        {
            var managedInstallsConditions =
                BLL.MunkiManagedInstall.GetAllManagedInstallsForMt(templateId)
                    .Where(x => !string.IsNullOrEmpty(x.Condition))
                    .Select(x => x.Condition)
                    .ToList();

            var managedUnInstallsConditions =
                BLL.MunkiManagedUninstall.GetAllManagedUnInstallsForMt(templateId)
                    .Where(x => !string.IsNullOrEmpty(x.Condition))
                    .Select(x => x.Condition)
                    .ToList();

            var optionalInstallsConditions =
                BLL.MunkiOptionalInstall.GetAllOptionalInstallsForMt(templateId)
                    .Where(x => !string.IsNullOrEmpty(x.Condition))
                    .Select(x => x.Condition)
                    .ToList();

            var managedUpdatesConditions =
                BLL.MunkiManagedUpdate.GetAllManagedUpdatesForMt(templateId)
                    .Where(x => !string.IsNullOrEmpty(x.Condition))
                    .Select(x => x.Condition)
                    .ToList();

            var includedManifestsConditions =
                BLL.MunkiIncludedManifest.GetAllIncludedManifestsForMt(templateId)
                    .Where(x => !string.IsNullOrEmpty(x.Condition))
                    .Select(x => x.Condition)
                    .ToList();

            var allConditions = new List<string>();
            allConditions.AddRange(managedInstallsConditions);
            allConditions.AddRange(managedUnInstallsConditions);
            allConditions.AddRange(optionalInstallsConditions);
            allConditions.AddRange(managedUpdatesConditions);
            allConditions.AddRange(includedManifestsConditions);
        
            return allConditions.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
        }
    }
}