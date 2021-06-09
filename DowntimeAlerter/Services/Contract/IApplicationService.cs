using System.Collections.Generic;
using System.Threading.Tasks;
using DowntimeAlerter.Infrastructure.ViewModel.Request.Application;
using ApplicationModel = DowntimeAlerter.Infrastructure.ViewModel.Response.Application.ApplicationModel;

namespace DowntimeAlerter.Services.Contract
{
    public interface IApplicationService
    {
        public Task<List<ApplicationModel>> GetAll();
        public Task<ApplicationModel> Get(string id);
        public string Add(ApplicationAddModel model);
        public Task<string> Update(ApplicationUpdateModel model);
        public string Delete(string id);
    }
}