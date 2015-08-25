using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Image = Models.Image;

namespace views.hosts
{
    public partial class HostOptions : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var host = new Computer { Id = Convert.ToInt16(Request["hostid"]) };
            host.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = host.Name + " | Options";
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Computer { Id = Convert.ToInt16(Request["hostid"]) };
            host.Read();
            var fixedLineEnding = scriptEditorText.Value.Replace("\r\n", "\n");
            host.PartitionScript = fixedLineEnding;
            host.Update();
           
            Master.Msgbox(Utility.Message);
        }

        protected void PopulateForm()
        {
            var host = new Computer { Id = Convert.ToInt16(Request["hostid"]) };
            host.Read();
            scriptEditorText.Value = host.PartitionScript;
            Master.Msgbox(Utility.Message);
           
        }
    }
}