using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Image = Models.Image;

namespace views.images
{
    public partial class ImageHistory : BasePages.Images
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
            var history = new History()
            {
                Type = "Image",
                TypeId = Image.Id.ToString(),
                Limit = ddlLimit.Text
            };
            gvHistory.DataSource = history.Read();
            gvHistory.DataBind();
        }
    }
}