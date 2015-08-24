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
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Newtonsoft.Json;
using Partition;
using Security;
using Image = Models.Image;

namespace views.images
{
    public partial class ImageSearch : Page
    {
        protected void btnHds_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var row = (GridViewRow) control.Parent.Parent;
            var image = new Image {Name = row.Cells[4].Text};
            image.Read();

            var gvHDs = (GridView) row.FindControl("gvHDs");
            ImagePhysicalSpecs specs;
            try
            {
                specs = JsonConvert.DeserializeObject<ImagePhysicalSpecs>(image.ClientSize);
            }
            catch
            {
                return;
            }
            if (specs == null)
                return;
            var specslist = specs.Hd.ToList();


            var btn = (LinkButton) row.FindControl("btnHDs");
            if (gvHDs.Visible == false)
            {
                var td = row.FindControl("tdHds");
                td.Visible = true;
                gvHDs.Visible = true;

                gvHDs.DataSource = specslist;
                gvHDs.DataBind();
                btn.Text = "-";
            }
            else
            {
                var td = row.FindControl("tdHds");
                td.Visible = false;
                gvHDs.Visible = false;

                btn.Text = "+";
            }

            foreach (GridViewRow hdrow in gvHDs.Rows)
            {
                try
                {
                    var lbl = hdrow.FindControl("lblHDSize") as Label;
                    string imagePath;
                    try
                    {
                        if (hdrow.RowIndex.ToString() == "0")
                        {
                            imagePath = Settings.ImageStorePath + row.Cells[4].Text;
                        }
                        else
                        {
                            var selectedHd = (hdrow.RowIndex + 1).ToString();
                            imagePath = Settings.ImageStorePath + row.Cells[4].Text +
                                        Path.DirectorySeparatorChar + "hd" + selectedHd;
                        }
                    }
                    catch
                    {
                        return;
                    }


                    var size = new FileOps().GetDirectorySize(new DirectoryInfo(imagePath))/1024f/1024f/1024f;
                    if (Math.Abs(size) < 0.1f)
                    {
                        if (lbl != null) lbl.Text = "N/A";
                    }
                    else
                    {
                        if (lbl != null)
                        {
                            lbl.Text = size.ToString("#.##") + " GB";
                            if (lbl.Text == " GB")
                                lbl.Text = "< .01 GB";
                        }
                    }
                }
                catch
                {
                    var lbl = hdrow.FindControl("lblHDSize") as Label;
                    if (lbl != null) lbl.Text = "N/A";
                    Utility.Message = "";
                }

                try
                {
                    var lblClient = hdrow.FindControl("lblHDSizeClient") as Label;

                    var calc = new MinimumSize {Image = image};
                    var fltClientSize = calc.Hd(hdrow.RowIndex, "1")/
                                        1024f/1024f/1024f;

                    if (Math.Abs(fltClientSize) < 0.1f)
                    {
                        if (lblClient != null) lblClient.Text = "N/A";
                    }
                    else
                    {
                        if (lblClient != null)
                        {
                            lblClient.Text = fltClientSize.ToString("#.##") + " GB";
                            if (lblClient.Text == " GB")
                                lblClient.Text = "< .01 GB";
                        }
                    }
                }
                catch (Exception)
                {
                    var lblClient = hdrow.FindControl("lblHDSizeClient") as Label;
                    if (lblClient != null) lblClient.Text = "N/A";
                    Utility.Message = "";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvImages.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvImages.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var image = new Image {Id = Convert.ToInt32(dataKey.Value)};
                image.Delete();
            }


            Master.Master.Msgbox(Utility.Message);
            PopulateGrid(true);
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var hcb = (CheckBox) gvImages.HeaderRow.FindControl("chkSelectAll");
            ToggleCheckState(hcb.Checked);
        }

        public string GetSortDirection(string sortExpression)
        {
            if (ViewState[sortExpression] == null)
                ViewState[sortExpression] = "Desc";
            else
                ViewState[sortExpression] = ViewState[sortExpression].ToString() == "Desc" ? "Asc" : "Desc";

            return ViewState[sortExpression].ToString();
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid(true);
            var dataTable = gvImages.DataSource as DataTable;

            if (dataTable == null) return;
            var dataView = new DataView(dataTable) {Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression)};
            gvImages.DataSource = dataView;
            gvImages.DataBind();

            PopulateGrid(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Master.FindControl("SubNavDynamic").Visible = false;
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");

            Master.Master.Msgbox(Utility.Message); //For Redirects
            PopulateGrid(true);
        }

        protected void PopulateGrid(bool bind)
        {
            if (bind)
            {
                var image = new Image();
                gvImages.DataSource = image.Search(txtSearch.Text);
                gvImages.DataBind();
                lblTotal.Text = gvImages.Rows.Count + " Result(s) / " + image.GetTotalCount() + " Total Image(s)";
            }

            foreach (GridViewRow row in gvImages.Rows)
            {
                try
                {
                    var lbl = row.FindControl("lblSize") as Label;
                    var imagePath = Settings.ImageStorePath + row.Cells[4].Text;

                    var size = new FileOps().GetDirectorySize(new DirectoryInfo(imagePath))/1024f/1024f/1024f;
                    if (Math.Abs(size) < 0.1f)
                    {
                        if (lbl != null) lbl.Text = "N/A";
                    }
                    else if (lbl != null) lbl.Text = size.ToString("#.##") + " GB";
                }
                catch
                {
                    var lbl = row.FindControl("lblSize") as Label;
                    if (lbl != null) lbl.Text = "N/A";
                    Utility.Message = "";
                }

                try
                {
                    var lblClient = row.FindControl("lblSizeClient") as Label;
                    var imageId = ((HiddenField) row.FindControl("HiddenID")).Value;
                    var img = new Image {Id = Convert.ToInt32(imageId) };
                    img.Read();

                    var calc = new MinimumSize {Image = img};
                    var fltClientSize = calc.Hd(0, "1")/1024f/1024f/1024f;
                    if (Math.Abs(fltClientSize) < 0.1f)
                    {
                        if (lblClient != null) lblClient.Text = "N/A";
                    }
                    else if (lblClient != null) lblClient.Text = fltClientSize.ToString("#.##") + " GB";
                }
                catch
                {
                    var lblClient = row.FindControl("lblSizeClient") as Label;
                    if (lblClient != null) lblClient.Text = "N/A";
                    Utility.Message = "";
                }
            }
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid(true);
        }

        private void ToggleCheckState(bool checkState)
        {
            foreach (GridViewRow row in gvImages.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb != null)
                    cb.Checked = checkState;
            }
        }
    }
}