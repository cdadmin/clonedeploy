using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs
{
    public class MunkiUpdateConfirmDTO
    {
        public int computerCount { get; set; }
        public int groupCount { get; set; }
        public List<MunkiManifestTemplateEntity> manifestTemplates { get; set; }
    }
}