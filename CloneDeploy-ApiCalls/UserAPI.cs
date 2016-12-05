using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class UserAPI: GenericAPI<CloneDeployUserEntity>
    {
        public UserAPI(string resource):base(resource)
        {
		
        }

        public CloneDeployUserEntity GetByName(string username)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/", _resource);
            _request.AddParameter("username", username);
            return new ApiRequest().Execute<CloneDeployUserEntity>(_request);
        }

        public ApiObjectResponseDTO GetForLogin(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetForLogin/{1}", _resource,id);
            var response = new ApiRequest().Execute<ApiObjectResponseDTO>(_request);

            if (response.Id == 0)
                response.Success = false;
            return response;

        }


        [UserAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetAdminCount()
        {
            return new ApiStringResponseDTO() { Value = _userServices.GetAdminCount().ToString() };
        }



        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO IsAdmin(int id)
        {
            return new ApiBoolResponseDTO() { Value = _userServices.IsAdmin(id) };
        }





        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserRightEntity> GetRights(int id)
        {
            return _userServices.GetUserRights(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteRights(int id)
        {
            return new ApiBoolResponseDTO() { Value = _userServices.DeleteUserRights(id) };

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserImageManagementEntity> GetImageManagements(int id)
        {
            return _userServices.GetUserImageManagements(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteImageManagements(int id)
        {
            return new ApiBoolResponseDTO() { Value = _userServices.DeleteUserImageManagements(id) };

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupManagementEntity> GetGroupManagements(int id)
        {
            return _userServices.GetUserGroupManagements(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteGroupManagements(int id)
        {
            return new ApiBoolResponseDTO() { Value = _userServices.DeleteUserGroupManagements(id) };
        }
    }
}