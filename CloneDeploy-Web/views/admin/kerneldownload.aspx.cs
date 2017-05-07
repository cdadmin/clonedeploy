using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin
{
    public partial class kerneldownload : Admin
    {
        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var onlineKernel = new OnlineKernel();
                var gvRow = (GridViewRow) control.Parent.Parent;
                onlineKernel.BaseVersion = gvRow.Cells[2].Text;
                onlineKernel.FileName = gvRow.Cells[0].Text;
                if (onlineKernel.BaseVersion != null)
                {
                    var result = Call.OnlineKernelApi.Download(onlineKernel);
                    EndUserMessage = result ? "Successfully Downloaded Kernel" : "Could Not Download Kernel";
                }
            }
            PopulateKernels();
        }

        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateKernels();
        }

        private void PopulateKernels()
        {
            gvKernels.DataSource = Call.OnlineKernelApi.GetAll();
            gvKernels.DataBind();

            var installedKernels = Call.FilesystemApi.GetKernels();
            foreach (GridViewRow row in gvKernels.Rows)
            {
                var lbl = row.FindControl("lblInstalled") as Label;
                foreach (var kernel in installedKernels)
                {
                    if (kernel == row.Cells[0].Text && lbl != null)
                        lbl.Text = "Yes";
                }
            }
        }
    }
}