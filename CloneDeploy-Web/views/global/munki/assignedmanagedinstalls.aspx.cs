using System;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.munki
{
    public partial class views_global_munki_assignedmanagedinstalls : Global
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateGlobal);

            var updateCount = 0;
            foreach (GridViewRow row in gvTemplateInstalls.Rows)
            {
                var enabled = (CheckBox) row.FindControl("chkSelector");
                if (enabled == null) continue;
                var dataKey = gvTemplateInstalls.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                if (!enabled.Checked)
                {
                    if (Call.MunkiManifestTemplateApi.DeleteManagedInstallsFromTemplate(Convert.ToInt32(dataKey.Value)))
                        updateCount++;
                }
                else
                {
                    var managedInstall = Call.MunkiManifestManagedInstallApi.Get(Convert.ToInt32(dataKey.Value));

                    var txtCondition = row.FindControl("txtCondition") as TextBox;
                    if (txtCondition != null)
                        if (txtCondition.Text != managedInstall.Condition)
                            managedInstall.Condition = txtCondition.Text;

                    if (Call.MunkiManifestTemplateApi.AddManagedInstallToTemplate(managedInstall)) updateCount++;
                }
            }

            if (updateCount > 0)
            {
                EndUserMessage = "Successfully Updated Managed Installs";
                ManifestTemplate.ChangesApplied = 0;
                Call.MunkiManifestTemplateApi.Put(ManifestTemplate.Id, ManifestTemplate);
            }
            else
            {
                EndUserMessage = "Could Not Update Managed Installs";
            }


            PopulateGrid();
        }

        protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var assignedLimit = ddlLimitAssigned.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlLimitAssigned.Text);

            var templateInstalls =
                Call.MunkiManifestTemplateApi.GetManifestManagedInstalls(ManifestTemplate.Id)
                    .Where(s => s.Name.IndexOf(txtSearchAssigned.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                    .OrderBy(x => x.Name)
                    .Take(assignedLimit);
            gvTemplateInstalls.DataSource = templateInstalls;
            gvTemplateInstalls.DataBind();

            lblTotalAssigned.Text = gvTemplateInstalls.Rows.Count + " Result(s) / " +
                                    Call.MunkiManifestTemplateApi.GetManagedInstallCount(ManifestTemplate.Id) +
                                    " Total Managed Install(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}