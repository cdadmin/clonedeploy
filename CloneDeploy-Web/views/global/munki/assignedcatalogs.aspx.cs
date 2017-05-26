using System;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.munki
{
    public partial class views_global_munki_catalogs : Global
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateGlobal);

            var updateCount = 0;
            foreach (GridViewRow row in gvTemplateCatalogs.Rows)
            {
                var enabled = (CheckBox) row.FindControl("chkSelector");
                if (enabled == null) continue;
                var dataKey = gvTemplateCatalogs.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                if (!enabled.Checked)
                {
                    if (Call.MunkiManifestTemplateApi.DeleteCatalogsFromTemplate(Convert.ToInt32(dataKey.Value)))
                        updateCount++;
                }
                else
                {
                    var catalog = Call.MunkiManifestCatalogApi.Get(Convert.ToInt32(dataKey.Value));

                    var txtPriority = row.FindControl("txtPriority") as TextBox;
                    if (txtPriority != null)
                        if (!string.IsNullOrEmpty(txtPriority.Text))
                            catalog.Priority = Convert.ToInt32(txtPriority.Text);

                    if (Call.MunkiManifestTemplateApi.AddCatalogToTemplate(catalog)) updateCount++;
                }
            }

            if (updateCount > 0)
            {
                EndUserMessage = "Successfully Updated Catalogs";
                ManifestTemplate.ChangesApplied = 0;
                Call.MunkiManifestTemplateApi.Put(ManifestTemplate.Id, ManifestTemplate);
            }
            else
            {
                EndUserMessage = "Could Not Update Catalogs";
            }


            PopulateGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var templateCatalogs =
                Call.MunkiManifestTemplateApi.GetManifestCatalogs(ManifestTemplate.Id)
                    .Where(f => f.Name.IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                    .OrderBy(x => x.Priority);
            gvTemplateCatalogs.DataSource = templateCatalogs;
            gvTemplateCatalogs.DataBind();
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }


        protected void showAssigned_OnClick(object sender, EventArgs e)
        {
            Assigned.Visible = !Assigned.Visible;
        }
    }
}