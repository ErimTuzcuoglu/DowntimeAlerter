using System.Collections.Generic;
using System.Threading.Tasks;
using DowntimeAlerter.Domain.Common;
using DowntimeAlerter.Infrastructure.ViewModel;
using DowntimeAlerter.Infrastructure.ViewModel.Request;
using DowntimeAlerter.Infrastructure.ViewModel.Response;

namespace DowntimeAlerter.Services.Contract
{
    public interface IUserService
    {
        public ApiResponse<List<UserModel>> GetAll();
        public ApiResponse<UserModel> Get(string id);
        public Task<ApiResponse<LoginResultModel>> Login(LoginModel model);
        public Task<ApiResponse<string>> Register(UserRegisterModel model);
        public Task<ApiResponse<string>> Update(UserUpdateModel model);
        public Task<ApiResponse<string>> Delete(string id);
    }
}