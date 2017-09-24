using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.users.groupacls
{
    public partial class views_users_groupacls_general : Users
    {
        private List<CheckBox> _listCheckBoxes;

        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            Call.UserGroupApi.DeleteRights(CloneDeployUserGroup.Id);
            var listOfRights =
                _listCheckBoxes.Where(x => x.Checked)
                    .Select(box => new UserGroupRightEntity {UserGroupId = CloneDeployUserGroup.Id, Right = box.ID})
                    .ToList();
            Call.UserGroupRightApi.Post(listOfRights);
            Call.UserGroupApi.UpdateMemberAcls(CloneDeployUserGroup.Id);
            EndUserMessage = "Updated ACLs";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.Administrator);

            _listCheckBoxes = new List<CheckBox>
            {
                ComputerCreate,
                ComputerRead,
                ComputerUpdate,
                ComputerDelete,
                ComputerSearch,
                ImageCreate,
                ImageRead,
                ImageUpdate,
                ImageDelete,
                ImageSearch,
                GroupCreate,
                GroupUpdate,
                GroupRead,
                GroupDelete,
                GroupSearch,
                ProfileCreate,
                ProfileDelete,
                ProfileRead,
                ProfileUpdate,
                ProfileSearch,
                GlobalCreate,
                GlobalDelete,
                GlobalRead,
                GlobalUpdate,
                AdminRead,
                AdminUpdate,
                ImageTaskUpload,
                ImageTaskDeploy,
                ImageTaskMulticast,
                ApproveImage,
                AllowOnd,
                AllowDebug,
                SmartCreate,
                SmartUpdate
            };
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            var listOfRights = Call.UserGroupApi.GetRights(CloneDeployUserGroup.Id);
            foreach (var right in listOfRights)
            {
                foreach (var box in _listCheckBoxes.Where(box => box.ID == right.Right))
                    box.Checked = true;
            }
        }
    }
}