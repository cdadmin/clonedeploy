using System;
using System.Linq;

public partial class views_computers_groups : BasePages.Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateGrid();
        }
    }

    protected void PopulateGrid()
    {
        var memberships = Call.ComputerApi.GetGroupMemberships(Computer.Id);
        var computerGroups = memberships.Select(membership => Call.GroupApi.Get(membership.GroupId)).ToList();
        gvGroups.DataSource = computerGroups;
        gvGroups.DataBind();
    }
}