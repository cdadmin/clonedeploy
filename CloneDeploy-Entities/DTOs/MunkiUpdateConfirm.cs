using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class MunkiUpdateConfirmDTO
    {
        public List<MunkiManifestTemplateEntity> manifestTemplates { get; set; }
        public int groupCount { get; set; }
        public int computerCount { get; set; }
    }
}
