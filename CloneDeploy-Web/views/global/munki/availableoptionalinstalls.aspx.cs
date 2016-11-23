using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_global_munki_availableoptionalinstalls : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var availableLimit = ddlLimitAvailable.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAvailable.Text);


        var listOfPackages = new List<MunkiPackageInfo>();
         var pkgInfos = GetMunkiResources("pkgsinfo");
        if (pkgInfos != null)
        {
            foreach (var pkgInfoFile in pkgInfos)
            {
                var pkg = ReadPlist(pkgInfoFile.FullName);
                if (pkg != null)
                    listOfPackages.Add(pkg);
            }

            listOfPackages =
                listOfPackages.Where(
                    f => f.Name.IndexOf(txtSearchAvailable.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                    .Take(availableLimit)
                    .ToList();
            gvPkgInfos.DataSource = listOfPackages;
            gvPkgInfos.DataBind();

            lblTotalAvailable.Text = gvPkgInfos.Rows.Count + " Result(s) / " + pkgInfos.Length +
                                     " Total Packages(s)";
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

            var optionalInstall = new MunkiManifestOptionInstall()
            {
                Name = dataKey.Value.ToString(),
                ManifestTemplateId = ManifestTemplate.Id,
            };



            var cbUseVersion = (CheckBox)row.FindControl("chkUseVersion");
            if (cbUseVersion.Checked)
            {
                optionalInstall.Version = row.Cells[2].Text;
                optionalInstall.IncludeVersion = 1;
            }

            var condition = (TextBox)row.FindControl("txtCondition");
            optionalInstall.Condition = condition.Text;
            if (BLL.MunkiOptionalInstall.AddOptionalInstallToTemplate(optionalInstall)) updateCount++;
        }

        if (updateCount > 0)
        {
            EndUserMessage = "Successfully Updated Optional Installs";
            ManifestTemplate.ChangesApplied = 0;
            BLL.MunkiManifestTemplate.UpdateManifest(ManifestTemplate);
        }
        else
        {
            EndUserMessage = "Could Not Update Optional Installs";
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


    protected void showAvailable_OnClick(object sender, EventArgs e)
    {
        Available.Visible = !Available.Visible;
    }

    protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}