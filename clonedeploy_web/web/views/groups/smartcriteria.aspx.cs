using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_groups_smartcriteria : BasePages.Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        if (Group.Type == "smart")
            txtContains.Text = Group.SmartCriteria;
    }

    protected void gvComputers_OnSorting(object sender, GridViewSortEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void btnTestQuery_OnClick(object sender, EventArgs e)
    {
        gvComputers.DataSource = BLL.Computer.SearchComputersForUser(CloneDeployCurrentUser.Id, txtContains.Text);
        gvComputers.DataBind();
        lblTotal.Text = gvComputers.Rows.Count + " Result(s)";
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateSmart);
        var group = Group;
        group.SmartCriteria = txtContains.Text;
        BLL.Group.UpdateGroup(group);
        BLL.Group.UpdateSmartMembership(group);
    }
}