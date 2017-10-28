using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Common;
using CloneDeploy_Common.DbUpgrades;
using CloneDeploy_Entities.DTOs;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    
    public class DbUpdater
    {
        private readonly RawSqlServices _rawSqlServices;
        private readonly ILog log = LogManager.GetLogger(typeof(DbUpdater));

        public DbUpdater()
        {
            _rawSqlServices = new RawSqlServices();
        }
        
        public ActionResultDTO Update()
        {
            var result = new ActionResultDTO();
            
            var updatesToRun = new List<int>();
            var currentDbVersion = Convert.ToInt32(new CdVersionServices().Get(1).DatabaseVersion);
            var currentAppVersion = SettingStrings.Version;
            var versionMapping = new VersionMapping().Get();
            var targetDbVersion = versionMapping[currentAppVersion];

            if (targetDbVersion != currentDbVersion)
            {
                foreach (var v in versionMapping)
                {
                    if (v.Value > currentDbVersion && v.Value <= targetDbVersion)
                    {
                        updatesToRun.Add(v.Value);
                    }
                }

            }

            var ordered = updatesToRun.OrderBy(x => x).Distinct().ToList();
      
            foreach (var version in ordered)
            {
                var type = Type.GetType("CloneDeploy_Common.DbUpgrades._" + version + ", CloneDeploy-Common");

                try
                {
                    var instance = Activator.CreateInstance(type) as IDbScript;
                    _rawSqlServices.ExecuteQuery(instance.Get());
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    log.Error("Could Not Update Database To Version " + version);
                    log.Error(ex.Message);
                    result.Success = false;
                    result.ErrorMessage = "Could Not Update Database To Version " + version + "<br>" + ex.Message;
                    return result;
                }
            }
            return result;
        }
    }
}
