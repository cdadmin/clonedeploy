using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_munki_availablemanagedinstalls : BasePages.Global
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var availableLimit = ddlLimitAvailable.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAvailable.Text);
 
        var listOfPackages = new List<Models.MunkiPackageInfo>();
        foreach (var pkgInfoFile in GetMunkiResources("pkgsinfo"))
        {
            var pkg = ReadPlist(pkgInfoFile.FullName);
            if(pkg != null)
            listOfPackages.Add(pkg);    
        }

        listOfPackages = listOfPackages.Where(s => s.Name.IndexOf(txtSearchAvailable.Text, StringComparison.CurrentCultureIgnoreCase) != -1).Take(availableLimit).ToList();
        gvPkgInfos.DataSource = listOfPackages;
        gvPkgInfos.DataBind();
 
        lblTotalAvailable.Text = gvPkgInfos.Rows.Count + " Result(s) / " + GetMunkiResources("pkgsinfo").Length + " Total Packages(s)";
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

            var managedInstall = new Models.MunkiManifestManagedInstall
            {
                Name = dataKey.Value.ToString(),
                ManifestTemplateId = ManifestTemplate.Id,
            };

            var cbUseVersion = (CheckBox)row.FindControl("chkUseVersion");
            if (cbUseVersion.Checked)
            {
                managedInstall.Version = row.Cells[2].Text;
                managedInstall.IncludeVersion = 1;
            }

            var condition = (TextBox)row.FindControl("txtCondition");
            managedInstall.Condition = condition.Text;

            if (BLL.MunkiManagedInstall.AddManagedInstallToTemplate(managedInstall)) updateCount++;
        }

        EndUserMessage = updateCount > 0 ? "Successfully Updated Managed Installs" : "Could Not Update Managed Installs";

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