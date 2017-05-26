using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.cluster
{
    public partial class editcluster : Admin
    {
        public ClusterGroupEntity ClusterGroup
        {
            get { return Read(); }
        }

        protected void btnUpdateCluster_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var clusterGroup = new ClusterGroupEntity
            {
                Id = ClusterGroup.Id,
                Name = txtClusterName.Text,
                Default = chkDefault.Checked ? 1 : 0
            };

            var result = Call.ClusterGroupApi.Put(clusterGroup.Id, clusterGroup);
            if (result.Success)
            {
                var listOfServers = new List<ClusterGroupServerEntity>();
                foreach (GridViewRow row in gvServers.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (!cb.Checked) continue;

                    var cbImage = (CheckBox) row.FindControl("chkImage");
                    var cbTftp = (CheckBox) row.FindControl("chkTftp");
                    var cbMulticast = (CheckBox) row.FindControl("chkMulticast");
                    var dataKey = gvServers.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;

                    var clusterGroupServer = new ClusterGroupServerEntity();
                    clusterGroupServer.ClusterGroupId = result.Id;
                    clusterGroupServer.ServerId = Convert.ToInt32(dataKey.Value);

                    if (cbTftp.Checked)
                        clusterGroupServer.TftpRole = 1;
                    if (!cbTftp.Visible)
                        clusterGroupServer.TftpRole = 0;
                    if (cbMulticast.Checked)
                        clusterGroupServer.MulticastRole = 1;
                    if (!cbMulticast.Visible)
                        clusterGroupServer.MulticastRole = 0;

                    listOfServers.Add(clusterGroupServer);
                }

                if (listOfServers.Count == 0)
                    listOfServers.Add(new ClusterGroupServerEntity
                    {
                        ClusterGroupId = ClusterGroup.Id,
                        ServerId = -2
                    });
                Call.ClusterGroupServerApi.Post(listOfServers);


                var listOfDps = new List<ClusterGroupDistributionPointEntity>();
                foreach (GridViewRow row in gvDps.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (!cb.Checked) continue;


                    var dataKey = gvDps.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;

                    var clusterGroupDistributionPoint = new ClusterGroupDistributionPointEntity();
                    clusterGroupDistributionPoint.ClusterGroupId = result.Id;
                    clusterGroupDistributionPoint.DistributionPointId = Convert.ToInt32(dataKey.Value);


                    listOfDps.Add(clusterGroupDistributionPoint);
                }

                if (listOfDps.Count == 0)
                    listOfDps.Add(new ClusterGroupDistributionPointEntity
                    {
                        ClusterGroupId = ClusterGroup.Id,
                        DistributionPointId = -2
                    });
                Call.ClusterGroupDistributionPointApi.Post(listOfDps);

                EndUserMessage = "Successfully Updated Cluster Group";
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvServers);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            txtClusterName.Text = ClusterGroup.Name;
            chkDefault.Checked = ClusterGroup.Default == 1;

            var secondaryServers = Call.SecondaryServerApi.GetAll(int.MaxValue, "");
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Primary")
            {
                var primary = new SecondaryServerEntity();
                primary.Id = -1;
                primary.Name = GetSetting(SettingStrings.ServerIdentifier);

                primary.TftpRole = Convert.ToBoolean(Convert.ToInt16(GetSetting(SettingStrings.TftpServerRole))) ? 1 : 0;
                primary.MulticastRole = Convert.ToBoolean(Convert.ToInt16(GetSetting(SettingStrings.MulticastServerRole))) ? 1 : 0;
                secondaryServers.Insert(0, primary);
            }
            gvServers.DataSource = secondaryServers;
            gvServers.DataBind();

            foreach (GridViewRow row in gvServers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");

                var cbTftp = (CheckBox) row.FindControl("chkTftp");
                var cbMulticast = (CheckBox) row.FindControl("chkMulticast");
                var dataKey = gvServers.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                if (Convert.ToInt32(dataKey.Value) == -1)
                {
                    cbTftp.Visible = Convert.ToBoolean(Convert.ToInt16(GetSetting(SettingStrings.TftpServerRole)));
                    cbMulticast.Visible = Convert.ToBoolean(Convert.ToInt16(GetSetting(SettingStrings.MulticastServerRole)));
                }
                else
                {
                    var secondaryServer = Call.SecondaryServerApi.Get(Convert.ToInt32(dataKey.Value));

                    if (secondaryServer.TftpRole != 1)
                        cbTftp.Visible = false;
                    if (secondaryServer.MulticastRole != 1)
                        cbMulticast.Visible = false;
                }

                foreach (var clusterServer in Call.ClusterGroupApi.GetClusterServers(ClusterGroup.Id))
                {
                    if (clusterServer.ServerId == Convert.ToInt32(dataKey.Value))
                    {
                        cb.Checked = true;

                        cbTftp.Checked = clusterServer.TftpRole == 1;
                        cbMulticast.Checked = clusterServer.MulticastRole == 1;
                    }
                }
            }

            gvDps.DataSource = Call.DistributionPointApi.GetAll(int.MaxValue, "");
            gvDps.DataBind();

            foreach (GridViewRow row in gvDps.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                var dataKey = gvDps.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                foreach (var clusterDp in Call.ClusterGroupApi.GetClusterDistributionPoints(ClusterGroup.Id))
                {
                    if (clusterDp.DistributionPointId == Convert.ToInt32(dataKey.Value))
                    {
                        cb.Checked = true;
                    }
                }
            }
        }

        private ClusterGroupEntity Read()
        {
            return Call.ClusterGroupApi.Get(Convert.ToInt32(Request.QueryString["clusterid"]));
        }
    }
}