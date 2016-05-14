using System;
using System.Linq;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_munki_catalogs : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ManifestTemplate.TemplateAsManifest == 1)
        {
            txtSearch.Visible = false;
            na.Text = "Catalogs Are Not Used When The Manifest Template Is Overridden As Manifest";
            na.Visible = true;
            buttonUpdate.Visible = false;
            return;
        }

        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var templateCatalogs = BLL.MunkiCatalog.GetAllCatalogsForMt(ManifestTemplate.Id).Where(f => f.Name.IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) != -1).OrderBy(x => x.Priority);
        gvTemplateCatalogs.DataSource = templateCatalogs;
        gvTemplateCatalogs.DataBind();
      
        gvCatalogs.DataSource = GetMunkiResources("catalogs").Where(f => f.Name.IndexOf(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase) != -1 && f.Name != "all");
        gvCatalogs.DataBind();

        lblTotal.Text = gvCatalogs.Rows.Count + " Result(s) / " + GetMunkiResources("catalogs").Length + " Total Catalog(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
       
        var updateCount = 0;
        foreach (GridViewRow row in gvCatalogs.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            if (!enabled.Checked) continue;
           
            var dataKey = gvCatalogs.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var catalog = new Models.MunkiManifestCatalog()
            {
                Name = dataKey.Value.ToString(),
                ManifestTemplateId = ManifestTemplate.Id,
            };
            var txtPriority = row.FindControl("txtPriority") as TextBox;
            if (txtPriority != null)
                if (!string.IsNullOrEmpty(txtPriority.Text))
                    catalog.Priority = Convert.ToInt32(txtPriority.Text);

            if (BLL.MunkiCatalog.AddCatalogToTemplate(catalog)) updateCount++;
        }

        var deleteCount = 0;
        foreach (GridViewRow row in gvTemplateCatalogs.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            var dataKey = gvTemplateCatalogs.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            if (!enabled.Checked)
            {
                if(BLL.MunkiCatalog.DeleteCatalogFromTemplate(Convert.ToInt32(dataKey.Value)))
                deleteCount++;
            }
            else
            {
                var catalog = BLL.MunkiCatalog.GetCatalog(Convert.ToInt32(dataKey.Value));
              
                var txtPriority = row.FindControl("txtPriority") as TextBox;
                if (txtPriority != null)
                    if (!string.IsNullOrEmpty(txtPriority.Text))
                        catalog.Priority = Convert.ToInt32(txtPriority.Text);

                if (BLL.MunkiCatalog.AddCatalogToTemplate(catalog)) updateCount++;
            }       
        }

        if(deleteCount > 0 || updateCount > 0)
            EndUserMessage =  "Successfully Updated Catalogs";
        else    
            EndUserMessage = "Could Not Update Catalogs";
        
        PopulateGrid();
    }


    protected void showAssigned_OnClick(object sender, EventArgs e)
    {
        Assigned.Visible = !Assigned.Visible;
    }

    protected void showAvailable_OnClick(object sender, EventArgs e)
    {
        Available.Visible = !Available.Visible;
    }
}