using System;
using System.Web.UI.WebControls;
using CloneDeploy_Web;

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
        
    }

    protected void btnTestQuery_OnClick(object sender, EventArgs e)
    {
        gvComputers.DataSource = Call.ComputerApi.GetAll(Int32.MaxValue, txtContains.Text);
        gvComputers.DataBind();
        lblTotal.Text = gvComputers.Rows.Count + " Result(s)";
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedGroup(Authorizations.UpdateGroup,Group.Id); 
        RequiresAuthorization(Authorizations.UpdateSmart);
        var group = Group;
        group.SmartCriteria = txtContains.Text;
        var result = Call.GroupApi.Put(group.Id,group);
        EndUserMessage = result.Success ? "Successfully Updated Smart Criteria" : result.ErrorMessage;
        Call.GroupApi.UpdateSmartMembership(group.Id);
    }
}