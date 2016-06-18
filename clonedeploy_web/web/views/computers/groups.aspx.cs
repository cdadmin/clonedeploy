using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        var memberships = BLL.GroupMembership.GetAllComputerMemberships(Computer.Id);
        var computerGroups = memberships.Select(membership => BLL.Group.GetGroup(membership.GroupId)).ToList();
        gvGroups.DataSource = computerGroups;
        gvGroups.DataBind();
    }
}