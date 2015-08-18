using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Image = Models.Image;

namespace views.images
{
    public partial class ImageHistory : Page
    {
        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var image = new Image {Id = Request.QueryString["imageid"]};
            image.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = image.Name + " | History";
            if (!IsPostBack) PopulateHistory();
        }

        protected void PopulateHistory()
        {
            var image = new Image {Id = Request.QueryString["imageid"]};
            image.Read();
            if (!IsPostBack)
                ddlLimit.SelectedValue = "10";
            var history = new History()
            {
                Type = "Image",
                TypeId = image.Id,
                Limit = ddlLimit.Text
            };
            gvHistory.DataSource = history.Read();
            gvHistory.DataBind();
            Master.Master.Msgbox(Utility.Message);
        }
    }
}