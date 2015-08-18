using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Image = Models.Image;

namespace views.hosts
{
    public partial class HostEdit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var host = new Host { Id = Convert.ToInt16(Request["hostid"]) };
            host.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = host.Name + " | Edit";
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Host
            {
                Id = Convert.ToInt16(Request["hostid"]),
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = ddlHostImage.Text,
                Group = ddlHostGroup.Text,
                Description = txtHostDesc.Text,
                Kernel = ddlHostKernel.Text,
                BootImage = ddlHostBootImage.Text,
                Args = txtHostArguments.Text
            };

            foreach (ListItem item in lbScripts.Items)
                if (item.Selected)
                    host.Scripts += item.Value + ",";

            if (host.ValidateHostData()) host.Update();

            Master.Msgbox(Utility.Message);
        }

        protected void PopulateForm()
        {
            Master.Msgbox(Utility.Message);
            ddlHostImage.DataSource = new Image().Search("").Select(i => i.Name);
            ddlHostImage.DataBind();
            ddlHostImage.Items.Insert(0, "Select Image");

            ddlHostGroup.DataSource = new Group().Search("").Select(g => g.Name);
            ddlHostGroup.DataBind();
            ddlHostGroup.Items.Insert(0, "");

            lbScripts.DataSource = Utility.GetScripts("custom");
            lbScripts.DataBind();

            ddlHostKernel.DataSource = Utility.GetKernels();
            ddlHostKernel.DataBind();
            ddlHostKernel.Items.Insert(0, "Select Kernel");

            ddlHostBootImage.DataSource = Utility.GetBootImages();
            ddlHostBootImage.DataBind();
            ddlHostBootImage.Items.Insert(0, "Select Boot Image");

            var host = new Host {Id = Convert.ToInt16(Request["hostid"])};
            host.Read();
            txtHostName.Text = host.Name;
            txtHostMac.Text = host.Mac;
            ddlHostImage.Text = host.Image;
            ddlHostGroup.Text = host.Group;
            txtHostDesc.Text = host.Description;
            ddlHostKernel.Text = host.Kernel;
            ddlHostBootImage.Text = host.BootImage;
            txtHostArguments.Text = host.Args;

            if (string.IsNullOrEmpty(host.Scripts)) return;
            var listhostScripts = host.Scripts.Split(',').ToList();
            foreach (ListItem item in lbScripts.Items)
                foreach (var script in listhostScripts)
                    if (item.Value == script)
                        item.Selected = true;
        }
    }
}