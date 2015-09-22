using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.groups
{
    public partial class GroupHistory : Page
    {
        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack) PopulateHistory();
        }

        protected void PopulateHistory()
        {
           

            if (!IsPostBack)
                ddlLimit.SelectedValue = "10";
            var history = new History
            {
                Type = "Group",
                TypeId = Master.Group.Id.ToString(),
                Limit = ddlLimit.Text
            };
            gvHistory.DataSource = history.Read();
            gvHistory.DataBind();
            Master.Master.Msgbox(Utility.Message);
        }
    }
}