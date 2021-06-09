using System.Threading.Tasks;
using DowntimeAlerter.Domain.Common;
using DowntimeAlerter.Infrastructure.ViewModel.Request;
using DowntimeAlerter.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace DowntimeAlerter.Controllers
{
    public class UserController : ApplicationControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(_service.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            return Ok(await _service.Register(model));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {
            return Ok(await _service.Update(model));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}