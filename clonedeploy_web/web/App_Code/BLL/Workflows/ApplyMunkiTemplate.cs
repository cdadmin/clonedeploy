using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ApplyMunkiTemplate
/// </summary>
public class ApplyMunkiTemplate
{
	public ApplyMunkiTemplate()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void Apply(int templateId)
    {
        var includedTemplates = new List<Models.MunkiManifestTemplate>();
        var groups = BLL.GroupMunki.GetGroupsForManifestTemplate(templateId);
        //get list of all templates that are used in these groups
        foreach (var munkiGroup in groups)
        {
            foreach (var template in BLL.GroupMunki.Get(munkiGroup.GroupId))
            {
                includedTemplates.Add(BLL.MunkiManifestTemplate.GetManifest(template.Id));
            }
        }

        var computers = BLL.ComputerMunki.GetComputersForManifestTemplate(templateId);


    }
}