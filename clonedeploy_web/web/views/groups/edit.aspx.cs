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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Security;
using Image = Models.Image;

namespace views.groups
{
    public partial class GroupEdit : Page
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var group = new Group {Id = Request.QueryString["groupid"]};
            group.Read();

            var currentMembers = new List<Host>();
            var membersToAdd = new List<Host>();
            var membersToRemove = new List<Host>();

            group.Name = txtGroupName.Text;
            group.Description = txtGroupDesc.Text;
            group.Image = ddlGroupImage.Text;
            group.Kernel = ddlGroupKernel.Text;
            group.BootImage = ddlGroupBootImage.Text;
            group.Args = txtGroupArguments.Text;
            group.SenderArgs = txtGroupSenderArgs.Text;
            group.Type = ddlGroupType.Text;
            @group.Expression = @group.Type == "smart" ? txtExpression.Text : "";
            group.Scripts = null;
            foreach (ListItem item in lbScripts.Items)
                if (item.Selected)
                    group.Scripts += item.Value + ",";
            if (!group.Update())
            {
                Master.Master.Msgbox(Utility.Message);
                return;
            }

            if (group.Type == "standard")
            {
                foreach (GridViewRow row in gvAdd.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (cb == null || !cb.Checked) continue;
                    var dataKey = gvAdd.DataKeys[row.RowIndex];
                    if (dataKey != null)
                    {
                        var host = new Host {Id = Convert.ToInt16(dataKey.Value)};
                        host.Read();
                        membersToAdd.Add(host);
                    }
                }

                foreach (GridViewRow row in gvRemove.Rows)
                {
                    var host = new Host();
                    var dataKey = gvRemove.DataKeys[row.RowIndex];
                    if (dataKey != null)
                    {
                        host.Id = Convert.ToInt16(dataKey.Value);
                        host.Read();
                        currentMembers.Add(host);
                    }
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (cb == null || !cb.Checked) continue;
                    var key = gvRemove.DataKeys[row.RowIndex];
                    if (key != null)
                        membersToRemove.Add(host);
                }

                if (currentMembers.Count > 0)
                {
                    group.Members = currentMembers;
                    group.UpdateHosts(false);
                }
                if (membersToAdd.Count > 0)
                {
                    group.Members = membersToAdd;
                    group.UpdateHosts(true);
                }
                if (membersToRemove.Count > 0)
                {
                    group.Members = membersToRemove;
                    group.RemoveGroupHosts();
                }

                gvRemove.DataSource = group.GroupMembers();
                gvRemove.DataBind();
            }
            else
            {
                foreach (var host in group.SearchSmartHosts(txtExpression.Text))
                {
                    group.Members.Add(host);
                }
                group.UpdateHosts(true);
                txtSmartSearch.Text = group.Expression;
                gvSmartHosts.DataSource = group.SearchSmartHosts(txtSmartSearch.Text);
                gvSmartHosts.DataBind();
                lblSmartTotal.Text = gvSmartHosts.Rows.Count + " Result(s) / " + new Host().GetTotalCount() +
                                     " Total Host(s)";
            }


            Master.Master.Msgbox(Utility.Message);
        }

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
            var group = new Group {Id = Request.QueryString["groupid"]};
            group.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = group.Name + " | Edit";
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            Master.Master.Msgbox(Utility.Message);
            var group = new Group {Id = Request.QueryString["groupid"]};
            group.Read();

            if (new Authorize().IsInMembership("User"))
            {
                var user = new WdsUser {Name = HttpContext.Current.User.Identity.Name};
                user.Read();
                var listManagementGroups = user.GroupManagement.Split(' ').ToList();

                var isAuthorized = false;
                foreach (var id in listManagementGroups)
                {
                    if (@group.Id != id) continue;
                    isAuthorized = true;
                    break;
                }

                if (!isAuthorized)
                    Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            }

            ddlGroupImage.DataSource = new Image().Search("").Select(i => i.Name);
            ddlGroupImage.DataBind();
            ddlGroupImage.Items.Insert(0, "Select Image");

            ddlGroupKernel.DataSource = Utility.GetKernels();
            ddlGroupKernel.DataBind();
            var itemKernel = ddlGroupKernel.Items.FindByText("kernel");
            if (itemKernel != null)
                ddlGroupKernel.SelectedValue = "speed";
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

            txtGroupName.Text = group.Name;
            txtGroupDesc.Text = group.Description;
            ddlGroupImage.Text = group.Image;
            ddlGroupKernel.Text = group.Kernel;
            ddlGroupBootImage.Text = group.BootImage;
            txtGroupArguments.Text = group.Args;
            txtGroupSenderArgs.Text = group.SenderArgs;
            ddlGroupType.Text = group.Type;
            if (group.Type == "smart")
            {
                smartParameters.Visible = true;
                txtExpression.Text = group.Expression;
            }

            if (!string.IsNullOrEmpty(group.Scripts))
            {
                var listhostScripts = group.Scripts.Split(',').ToList();
                foreach (ListItem item in lbScripts.Items)
                    foreach (var script in listhostScripts)
                        if (item.Value == script)
                            item.Selected = true;
            }

            if (ddlGroupType.Text == "standard")
            {
                gvRemove.DataSource = group.GroupMembers();
                gvRemove.DataBind();
                standardGroup.Visible = true;
                smartGroup.Visible = false;
                smartParameters.Visible = false;
            }
            else
            {
                txtSmartSearch.Text = group.Expression;
                smartParameters.Visible = true;
                standardGroup.Visible = false;
                smartGroup.Visible = true;
            }


            if (Settings.DefaultHostView == "all")
                PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var host = new Host();
            if (ddlGroupType.Text == "standard")
            {
                gvAdd.DataSource = host.Search(txtSearchHosts.Text);

                gvAdd.DataBind();

                lblTotal.Text = gvAdd.Rows.Count + " Result(s) / " + host.GetTotalCount() + " Total Host(s)";
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
            var hcb = (CheckBox) gvRemove.HeaderRow.FindControl("chkSelectAll");

            ToggleCheckState(hcb.Checked, false);
        }

        protected void SelectAllAdd_CheckedChanged(object sender, EventArgs e)
        {
            var hcb = (CheckBox) gvAdd.HeaderRow.FindControl("chkSelectAll");

            ToggleCheckState(hcb.Checked, true);
        }

        private void ToggleCheckState(bool checkState, bool isAdd)
        {
            if (isAdd)
            {
                foreach (GridViewRow row in gvAdd.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (cb != null)
                        cb.Checked = checkState;
                }
            }
            else
            {
                foreach (GridViewRow row in gvRemove.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (cb != null)
                        cb.Checked = checkState;
                }
            }
        }

        protected void txtSearchHosts_TextChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}