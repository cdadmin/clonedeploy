/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Security;
using Image = Models.Image;

namespace views.groups
{
    public partial class GroupCreate : Page
    {
        protected void groupType_ddlChanged(object sender, EventArgs e)
        {
            if (ddlGroupType.Text == "standard")
            {
                standardGroup.Visible = true;
                smartGroup.Visible = false;
                smartParameters.Visible = false;
            }
            else
            {
                smartParameters.Visible = true;
                standardGroup.Visible = false;
                smartGroup.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Master.FindControl("SubNav").Visible = false;
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            PopulateForm();
        }

        protected void PopulateForm()
        {
            ddlGroupImage.DataSource = new Image().Search("").Select(i => i.Name);
            ddlGroupImage.DataBind();
            ddlGroupImage.Items.Insert(0, "Select Image");

            ddlGroupKernel.DataSource = Utility.GetKernels();
            ddlGroupKernel.DataBind();
            var itemKernel = ddlGroupKernel.Items.FindByText(Settings.DefaultKernel32);
            if (itemKernel != null)
                ddlGroupKernel.SelectedValue = Settings.DefaultKernel32;
            else
                ddlGroupKernel.Items.Insert(0, "Select Kernel");

            ddlGroupBootImage.DataSource = Utility.GetBootImages();
            ddlGroupBootImage.DataBind();
            var itemBootImage = ddlGroupBootImage.Items.FindByText("initrd.gz");
            if (itemBootImage != null)
                ddlGroupBootImage.SelectedValue = "initrd.gz";
            else
                ddlGroupBootImage.Items.Insert(0, "Select Boot Image");

            lbScripts.DataSource = Utility.GetScripts("custom");
            lbScripts.DataBind();

            standardGroup.Visible = true;
            if (Settings.DefaultHostView == "all")
                PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var host = new Computer();
            if (ddlGroupType.Text == "standard")
            {
                gvHosts.DataSource = host.Search(txtSearchHosts.Text);
                gvHosts.DataBind();
                lblTotal.Text = gvHosts.Rows.Count + " Result(s) / " + host.GetTotalCount() + " Total Host(s)";
            }
            else
            {
                var group = new Group();
                gvSmartHosts.DataSource = group.SearchSmartHosts(txtSmartSearch.Text);
                gvSmartHosts.DataBind();
                lblSmartTotal.Text = gvSmartHosts.Rows.Count + " Result(s) / " + host.GetTotalCount() + " Total Host(s)";
            }
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var hcb = (CheckBox) gvHosts.HeaderRow.FindControl("chkSelectAll");
            ToggleCheckState(hcb.Checked);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var group = new Group();

            if (ddlGroupType.Text == "standard")
            {
                foreach (GridViewRow row in gvHosts.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (cb == null || !cb.Checked) continue;
                    var dataKey = gvHosts.DataKeys[row.RowIndex];
                    if (dataKey != null)
                    {
                        var host = new Computer {Id = Convert.ToInt16(dataKey.Value)};
                        host.Read();
                        @group.Members.Add(host);
                    }
                }
            }
            else
            {
                foreach (var host in group.SearchSmartHosts(txtExpression.Text))
                {
                    group.Members.Add(host);
                }
            }

            group.Name = txtGroupName.Text;
            group.Description = txtGroupDesc.Text;
            group.Image = ddlGroupImage.Text;
            group.Kernel = ddlGroupKernel.Text;
            group.BootImage = ddlGroupBootImage.Text;
            group.Args = txtGroupArguments.Text;
            group.SenderArgs = txtGroupSenderArgs.Text;
            group.Type = ddlGroupType.Text;
            @group.Expression = @group.Type == "smart" ? txtExpression.Text : "";
            foreach (ListItem item in lbScripts.Items)
                if (item.Selected)
                    group.Scripts += item.Value + ",";

            if (group.ValidateGroupData()) group.Create();

            if (Utility.Message.Contains("Successfully"))
                Response.Redirect("~/views/groups/edit.aspx?groupid=" + group.Id);

            Master.Master.Msgbox(Utility.Message);
        }

        private void ToggleCheckState(bool checkState)
        {
            foreach (GridViewRow row in gvHosts.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb != null)
                    cb.Checked = checkState;
            }
        }

        protected void txtSearchHosts_TextChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}