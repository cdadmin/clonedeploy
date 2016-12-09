using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_munki_availablemanifests : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var availableLimit = ddlLimitAvailable.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAvailable.Text);

        var listOfManifests = new List<MunkiPackageInfoEntity>();
        var manifests = Call.FilesystemApi.GetMunkiResources("manifests");
        if (manifests != null)
        {
            foreach (var manifest in manifests)
            {
                listOfManifests.Add(new MunkiPackageInfoEntity() {Name = manifest.Name});
            }

            listOfManifests =
                listOfManifests.Where(
                    s => s.Name.IndexOf(txtSearchAvailable.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                    .Take(availableLimit)
                    .ToList();
            gvPkgInfos.DataSource = listOfManifests;
            gvPkgInfos.DataBind();

            lblTotalAvailable.Text = gvPkgInfos.Rows.Count + " Result(s) / " + manifests.Count +
                                     " Total Manifest(s)";
        }
        else
        {
            gvPkgInfos.DataBind();
        }
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);

        var updateCount = 0;
        foreach (GridViewRow row in gvPkgInfos.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            if (!enabled.Checked) continue;

            var dataKey = gvPkgInfos.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var includedManifest = new MunkiManifestIncludedManifestEntity()
            {
                Name = dataKey.Value.ToString(),
                ManifestTemplateId = ManifestTemplate.Id,
            };

            var condition = (TextBox)row.FindControl("txtCondition");
            includedManifest.Condition = condition.Text;

            if (Call.MunkiManifestTemplateApi.AddManifestToTemplate(includedManifest)) updateCount++;
        }

        if (updateCount > 0)
        {
            EndUserMessage = "Successfully Updated Included Manifests";
            ManifestTemplate.ChangesApplied = 0;
            Call.MunkiManifestTemplateApi.Put(ManifestTemplate.Id,ManifestTemplate);
        }
        else
        {
            EndUserMessage = "Could Not Update Included Manifests";
        }

        

        PopulateGrid();
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvPkgInfos);
    }

    protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}