using System;
using System.Linq;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_munki_availablecatalogs : BasePages.Global
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

       

        EndUserMessage = updateCount > 0 ? "Successfully Updated Catalogs" : "Could Not Update Catalogs";
        
        PopulateGrid();
    }


  

    protected void showAvailable_OnClick(object sender, EventArgs e)
    {
        Available.Visible = !Available.Visible;
    }
}