using System;
using BasePages;
using Helpers;
using Models;

namespace views.computers
{
    public partial class ComputerHistory : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateHistory();
        }

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void PopulateHistory()
        {
            if (!IsPostBack) ddlLimit.SelectedValue = "10";
            var history = new History {Type = "Computer", TypeId = Computer.Id.ToString(), Limit = ddlLimit.Text};
            gvHistory.DataSource = history.Read();
            gvHistory.DataBind();
          
        }
    }
}