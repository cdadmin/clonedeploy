using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs
{
    public class MunkiUpdateConfirmDTO
    {
        public List<MunkiManifestTemplateEntity> manifestTemplates { get; set; }
        public int groupCount { get; set; }
        public int computerCount { get; set; }
    }
}
